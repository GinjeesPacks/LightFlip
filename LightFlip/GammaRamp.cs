using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace LightFlip {
  internal static class GammaRamp {
    public static void ApplyForWindow(IntPtr hwnd, ColorProfile p) {
      
      string? dev = NativeMethods.TryGetMonitorDeviceNameForWindow(hwnd);
      if (!string.IsNullOrWhiteSpace(dev)) {
        if (ApplyToDevice(dev!, p))
          return;
      }

      
      ApplyToAllActiveDevices(p);
    }

    public static void ApplyToNeutralAll() {
      ApplyToAllActiveDevices(ColorProfile.Neutral());
    }

    private static bool ApplyToDevice(string deviceName, ColorProfile p) {
      var ramp = BuildRamp(p);

      IntPtr hdc = CreateDC("DISPLAY", deviceName, null, IntPtr.Zero);
      if (hdc == IntPtr.Zero)
        return false;

      try {
        return SetDeviceGammaRamp(hdc, ref ramp);
      } finally {
        DeleteDC(hdc);
      }
    }

    private static void ApplyToAllActiveDevices(ColorProfile p) {
      var ramp = BuildRamp(p);

      foreach (string devName in EnumerateActiveDisplayDeviceNames()) {
        IntPtr hdc = CreateDC("DISPLAY", devName, null, IntPtr.Zero);
        if (hdc == IntPtr.Zero)
          continue;

        try {
          SetDeviceGammaRamp(hdc, ref ramp);
        } finally {
          DeleteDC(hdc);
        }
      }

      
      {
        IntPtr hdc = GetDC(IntPtr.Zero);
        if (hdc != IntPtr.Zero) {
          try { SetDeviceGammaRamp(hdc, ref ramp); } finally { ReleaseDC(IntPtr.Zero, hdc); }
        }
      }
    }

    private static RAMP BuildRamp(ColorProfile p) {
      float brightness = Clamp(p.Brightness, 0f, 100f);
      float contrast = Clamp(p.Contrast, 0f, 100f);
      float gamma = Clamp(p.Gamma, 0.2f, 5.0f);

      double bOffset = ((brightness - 50.0) / 50.0) * 0.25; 
      double cFactor = 0.5 + (contrast / 100.0);            

      var ramp = new RAMP {
        Red = new ushort[256],
        Green = new ushort[256],
        Blue = new ushort[256]
      };

      for (int i = 0; i < 256; i++) {
        double x = i / 255.0;

        x = (x - 0.5) * cFactor + 0.5 + bOffset;

        if (x < 0) x = 0;
        if (x > 1) x = 1;

        x = Math.Pow(x, 1.0 / gamma);

        int value = (int)(x * 65535.0 + 0.5);
        if (value < 0) value = 0;
        if (value > 65535) value = 65535;

        ramp.Red[i] = (ushort)value;
        ramp.Green[i] = (ushort)value;
        ramp.Blue[i] = (ushort)value;
      }

      return ramp;
    }

    private static float Clamp(float v, float min, float max) {
      if (v < min) return min;
      if (v > max) return max;
      return v;
    }

    private static IEnumerable<string> EnumerateActiveDisplayDeviceNames() {
      int i = 0;
      DISPLAY_DEVICE dd = new DISPLAY_DEVICE();
      dd.cb = Marshal.SizeOf(typeof(DISPLAY_DEVICE));

      while (EnumDisplayDevices(null, i, ref dd, 0)) {
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
    private struct DISPLAY_DEVICE {
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
    private struct RAMP {
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

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern bool EnumDisplayDevices(string? lpDevice, int iDevNum, ref DISPLAY_DEVICE lpDisplayDevice, int dwFlags);
  }
}
