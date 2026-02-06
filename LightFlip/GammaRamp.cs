using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace LightFlip
{
    internal static class GammaRamp
    {

        // Baseline cache (captured once per run)
        private static bool _baselineCaptured = false;
        private static readonly Dictionary<string, RAMP> _baselineByDevice = new(StringComparer.OrdinalIgnoreCase);
        private static RAMP? _baselineDesktopRamp = null;

        /// <summary>
        /// Capture the current gamma ramps as the "baseline" so we can restore exactly what the user had.
        /// Call once at startup (safe to call multiple times; it only captures once).
        /// </summary>
        public static void CaptureBaselineIfNeeded()
        {
            if (_baselineCaptured) return;

            // Per active display device
            foreach (string devName in EnumerateActiveDisplayDeviceNames())
            {
                IntPtr hdc = CreateDC("DISPLAY", devName, null, IntPtr.Zero);
                if (hdc == IntPtr.Zero)
                    continue;

                try
                {
                    if (TryGetDeviceGammaRamp(hdc, out var ramp))
                    {
                        _baselineByDevice[devName] = ramp;
                    }
                }
                finally
                {
                    DeleteDC(hdc);
                }
            }

            // Also capture the desktop DC ramp (some systems only respect this)
            {
                IntPtr hdc = GetDC(IntPtr.Zero);
                if (hdc != IntPtr.Zero)
                {
                    try
                    {
                        if (TryGetDeviceGammaRamp(hdc, out var ramp))
                            _baselineDesktopRamp = ramp;
                    }
                    finally
                    {
                        ReleaseDC(IntPtr.Zero, hdc);
                    }
                }
            }

            _baselineCaptured = true;
        }

        public static void ApplyForWindow(IntPtr hwnd, ColorProfile p)
        {

            string? dev = NativeMethods.TryGetMonitorDeviceNameForWindow(hwnd);
            if (!string.IsNullOrWhiteSpace(dev))
            {
                if (ApplyToDevice(dev!, p))
                    return;
            }

            ApplyToAllActiveDevices(p);
        }

        /// <summary>
        /// Restore the gamma ramps to the captured baseline. If baseline was not captured,
        /// falls back to Neutral (50/50/1.0).
        /// </summary>
        public static void RestoreBaselineAll()
        {
            if (!_baselineCaptured)
            {
                ApplyToAllActiveDevices(ColorProfile.Neutral());
                return;
            }

            // Restore per-device ramps where we have baselines
            foreach (string devName in EnumerateActiveDisplayDeviceNames())
            {
                if (!_baselineByDevice.TryGetValue(devName, out var ramp))
                    continue;

                IntPtr hdc = CreateDC("DISPLAY", devName, null, IntPtr.Zero);
                if (hdc == IntPtr.Zero)
                    continue;

                try
                {
                    SetDeviceGammaRamp(hdc, ref ramp);
                }
                finally
                {
                    DeleteDC(hdc);
                }
            }

            // Restore desktop DC too (best-effort)
            if (_baselineDesktopRamp != null)
            {
                IntPtr hdc = GetDC(IntPtr.Zero);
                if (hdc != IntPtr.Zero)
                {
                    try
                    {
                        var ramp = _baselineDesktopRamp.Value;
                        SetDeviceGammaRamp(hdc, ref ramp);
                    }
                    finally
                    {
                        ReleaseDC(IntPtr.Zero, hdc);
                    }
                }
            }
        }

        // Keep your existing method name for compatibility, but now it restores baseline.
        public static void ApplyToNeutralAll()
        {
            RestoreBaselineAll();
        }

        private static bool ApplyToDevice(string deviceName, ColorProfile p)
        {
            var ramp = BuildRamp(p);

            IntPtr hdc = CreateDC("DISPLAY", deviceName, null, IntPtr.Zero);
            if (hdc == IntPtr.Zero)
                return false;

            try
            {
                return SetDeviceGammaRamp(hdc, ref ramp);
            }
            finally
            {
                DeleteDC(hdc);
            }
        }

        private static void ApplyToAllActiveDevices(ColorProfile p)
        {
            var ramp = BuildRamp(p);

            foreach (string devName in EnumerateActiveDisplayDeviceNames())
            {
                IntPtr hdc = CreateDC("DISPLAY", devName, null, IntPtr.Zero);
                if (hdc == IntPtr.Zero)
                    continue;

                try
                {
                    SetDeviceGammaRamp(hdc, ref ramp);
                }
                finally
                {
                    DeleteDC(hdc);
                }
            }

            {
                IntPtr hdc = GetDC(IntPtr.Zero);
                if (hdc != IntPtr.Zero)
                {
                    try { SetDeviceGammaRamp(hdc, ref ramp); } finally { ReleaseDC(IntPtr.Zero, hdc); }
                }
            }
        }

        private static bool TryGetDeviceGammaRamp(IntPtr hdc, out RAMP ramp)
        {
            ramp = new RAMP
            {
                Red = new ushort[256],
                Green = new ushort[256],
                Blue = new ushort[256]
            };

            bool ok = GetDeviceGammaRamp(hdc, ref ramp);
            if (!ok)
            {
                ramp = default;
                return false;
            }

            ramp.Red ??= new ushort[256];
            ramp.Green ??= new ushort[256];
            ramp.Blue ??= new ushort[256];
            return true;
        }

        // NEW: applies gamma + brightness/contrast + temperature tint (cool/warm)
        private static RAMP BuildRamp(ColorProfile p)
        {
            float brightness = Clamp(p.Brightness, 0f, 100f);
            float contrast = Clamp(p.Contrast, 0f, 100f);
            float gamma = Clamp(p.Gamma, 0.2f, 5.0f);

            // NEW: -100..+100 cool/warm
            float temp = Clamp(p.Temperature, -100f, 100f);
            (double rMul, double gMul, double bMul) = TemperatureToRgbMultipliers(temp);

            double bOffset = ((brightness - 50.0) / 50.0) * 0.25;
            double cFactor = 0.5 + (contrast / 100.0);

            var ramp = new RAMP
            {
                Red = new ushort[256],
                Green = new ushort[256],
                Blue = new ushort[256]
            };

            for (int i = 0; i < 256; i++)
            {
                double x = i / 255.0;

                // contrast + brightness
                x = (x - 0.5) * cFactor + 0.5 + bOffset;

                if (x < 0) x = 0;
                if (x > 1) x = 1;

                // gamma
                x = Math.Pow(x, 1.0 / gamma);

                // base value
                double baseValue = x * 65535.0;

                // NEW: apply temperature tint per channel
                ramp.Red[i] = ToUShort(baseValue * rMul);
                ramp.Green[i] = ToUShort(baseValue * gMul);
                ramp.Blue[i] = ToUShort(baseValue * bMul);
            }

            return ramp;
        }

        private static ushort ToUShort(double v)
        {
            if (v < 0) v = 0;
            if (v > 65535) v = 65535;
            return (ushort)(v + 0.5);
        }

        // Simple warm/cool model:
        // temp > 0 warms (boost red, slightly reduce blue)
        // temp < 0 cools (boost blue, slightly reduce red)
        private static (double r, double g, double b) TemperatureToRgbMultipliers(float temp)
        {
            double t = temp / 100.0; // -1..+1

            // Caps keep it subtle and reduce clipping
            double r = 1.0 + (t > 0 ? 0.35 * t : 0.15 * t);
            double b = 1.0 + (t < 0 ? -0.35 * t : -0.15 * t);
            double g = 1.0;

            r = Math.Max(0.5, Math.Min(1.5, r));
            g = Math.Max(0.5, Math.Min(1.5, g));
            b = Math.Max(0.5, Math.Min(1.5, b));

            return (r, g, b);
        }

        private static float Clamp(float v, float min, float max)
        {
            if (v < min) return min;
            if (v > max) return max;
            return v;
        }

        private static IEnumerable<string> EnumerateActiveDisplayDeviceNames()
        {
            int i = 0;
            DISPLAY_DEVICE dd = new DISPLAY_DEVICE();
            dd.cb = Marshal.SizeOf(typeof(DISPLAY_DEVICE));

            while (EnumDisplayDevices(null, i, ref dd, 0))
            {
                bool active = (dd.StateFlags & DISPLAY_DEVICE_ACTIVE) != 0;
                bool mirroring = (dd.StateFlags & DISPLAY_DEVICE_MIRRORING_DRIVER) != 0;

                if (active && !mirroring && !string.IsNullOrWhiteSpace(dd.DeviceName))
                    yield return dd.DeviceName;

                i++;
                dd = new DISPLAY_DEVICE();
                dd.cb = Marshal.SizeOf(typeof(DISPLAY_DEVICE));
            }
        }

        private const int DISPLAY_DEVICE_ACTIVE = 0x00000001;
        private const int DISPLAY_DEVICE_MIRRORING_DRIVER = 0x00000008;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct DISPLAY_DEVICE
        {
            public int cb;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string DeviceName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceString;
            public int StateFlags;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceKey;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct RAMP
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public ushort[] Red;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public ushort[] Green;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public ushort[] Blue;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr CreateDC(string lpszDriver, string lpszDevice, string? lpszOutput, IntPtr lpInitData);

        [DllImport("gdi32.dll")]
        private static extern bool DeleteDC(IntPtr hdc);

        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern bool SetDeviceGammaRamp(IntPtr hDC, ref RAMP lpRamp);

        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern bool GetDeviceGammaRamp(IntPtr hDC, ref RAMP lpRamp);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern bool EnumDisplayDevices(string? lpDevice, int iDevNum, ref DISPLAY_DEVICE lpDisplayDevice, int dwFlags);
    }
}
