using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace LightFlip
{
    internal static class NativeMethods
    {
        // GetAncestor() flags
        // GA_ROOT returns the root window by walking up the parent chain.
        private const uint GA_ROOT = 2;

        public static IntPtr GetForegroundWindowSafe()
        {
            try { return GetForegroundWindow(); } catch { return IntPtr.Zero; }
        }

        // Returns the top-level (root) ancestor window for the current foreground window.
        // This helps when the foreground window belongs to a child/overlay window.
        public static IntPtr GetForegroundRootWindowSafe()
        {
            IntPtr hwnd = GetForegroundWindowSafe();
            if (hwnd == IntPtr.Zero) return IntPtr.Zero;

            try
            {
                IntPtr root = GetAncestor(hwnd, GA_ROOT);
                return root != IntPtr.Zero ? root : hwnd;
            }
            catch
            {
                return hwnd;
            }
        }

        public static uint GetForegroundRootProcessId()
        {
            IntPtr hwnd = GetForegroundRootWindowSafe();
            if (hwnd == IntPtr.Zero) return 0;
            GetWindowThreadProcessId(hwnd, out uint pid);
            return pid;
        }


        public static uint GetForegroundProcessId()
        {
            IntPtr hwnd = GetForegroundWindowSafe();
            if (hwnd == IntPtr.Zero) return 0;
            GetWindowThreadProcessId(hwnd, out uint pid);
            return pid;
        }

        public static string? TryGetProcessImagePath(uint pid)
        {
            if (pid == 0) return null;

            IntPtr hProcess = OpenProcess(PROCESS_QUERY_LIMITED_INFORMATION, false, pid);
            if (hProcess == IntPtr.Zero) return null;

            try
            {
                var sb = new StringBuilder(1024);
                int size = sb.Capacity;
                if (QueryFullProcessImageName(hProcess, 0, sb, ref size))
                    return sb.ToString();
            }
            finally
            {
                CloseHandle(hProcess);
            }

            return null;
        }

        public static string TryGetProcessName(uint pid)
        {
            if (pid == 0) return string.Empty;
            try
            {
                using (var p = Process.GetProcessById((int)pid))
                    return p.ProcessName ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static bool AnyProcessByNameExists(string processName)
        {
            if (string.IsNullOrWhiteSpace(processName)) return false;
            try
            {
                return Process.GetProcessesByName(processName).Length > 0;
            }
            catch
            {
                return false;
            }
        }

        public static bool ProcessExists(uint pid)
        {
            if (pid == 0) return false;
            try
            {
                var p = Process.GetProcessById((int)pid);
                return !p.HasExited;
            }
            catch { return false; }
        }

        
        public static string? TryGetMonitorDeviceNameForWindow(IntPtr hwnd)
        {
            if (hwnd == IntPtr.Zero) return null;

            IntPtr hMon = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);
            if (hMon == IntPtr.Zero) return null;

            MONITORINFOEX info = new MONITORINFOEX();
            info.cbSize = Marshal.SizeOf(typeof(MONITORINFOEX));

            if (!GetMonitorInfo(hMon, ref info))
                return null;

            
            return info.szDevice;
        }

        private const uint PROCESS_QUERY_LIMITED_INFORMATION = 0x1000;

        private const uint MONITOR_DEFAULTTONEAREST = 2;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct MONITORINFOEX
        {
            public int cbSize;
            public RECT rcMonitor;
            public RECT rcWork;
            public uint dwFlags;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string szDevice;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int left, top, right, bottom;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern IntPtr GetAncestor(IntPtr hwnd, uint gaFlags);

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, uint dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool QueryFullProcessImageName(IntPtr hProcess, int dwFlags, StringBuilder lpExeName, ref int lpdwSize);

        [DllImport("user32.dll")]
        private static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFOEX lpmi);
    }
}
