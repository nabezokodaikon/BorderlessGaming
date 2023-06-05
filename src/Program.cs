using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace BorderlessGaming
{
    public static class Program
    {
        private const int GWL_STYLE = -16;

        private const int WS_CAPTION = 0x00C00000;
        private const int WS_THICKFRAME = 0x00040000;
        private const int WS_MINIMIZEBOX = 0x00020000;
        private const int WS_MAXIMIZEBOX = 0x00010000;
        private const int WS_SYSMENU = 0x00080000;

        private const int SWP_FRAMECHANGED = 0x0020;
        private const int SWP_NOSIZE = 0x0001;
        private const int SWP_NOMOVE = 0x0002;
        private const int SWP_NOOWNERZORDER = 0x0200;
        private const int SWP_NOZORDER = 0x0004;

        private static readonly int WINDOW_WIDTH = (int)(2560 / 1.25);
        private static readonly int WINDOW_HEIGHT = (int)(1440 / 1.25);

        public enum SystemMetric : int
        {
            SM_CXVIRTUALSCREEN = 78,
            SM_CYVIRTUALSCREEN = 79,
        }

        [DllImport("user32.dll")]
        private static extern int GetSystemMetrics(SystemMetric smIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int cx, int cy, int uFlags);

        [DllImport("user32.dll")]
        private static extern int MoveWindow(IntPtr hwnd, int x, int y, int nWidth, int nHeight, int bRepaint);

        private static void RemoveFrame(IntPtr hWnd)
        {
            var lStyle = GetWindowLong(hWnd, GWL_STYLE);
            lStyle &= ~(WS_CAPTION | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX | WS_SYSMENU);
            SetWindowLong(hWnd, GWL_STYLE, lStyle);

            SetWindowPos(hWnd, 0, 0, 0, 0, 0,
                SWP_FRAMECHANGED | SWP_NOSIZE | SWP_NOZORDER | SWP_NOOWNERZORDER);

            var screenWidth = GetSystemMetrics(SystemMetric.SM_CXVIRTUALSCREEN);
            Console.WriteLine(screenWidth);
            MoveWindow(hWnd, screenWidth / 4, 0, WINDOW_WIDTH, WINDOW_HEIGHT, 1);
        }

        public static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                foreach (Process p in Process.GetProcesses())
                {
                    if (p.MainWindowTitle.Length > 0)
                    {
                        Console.WriteLine(p.MainWindowTitle);
                    }
                }

                Console.ReadLine();
            }
            else
            {
                foreach (var p in Process.GetProcesses())
                {
                    if (p.MainWindowTitle == args[0])
                    {
                        RemoveFrame(p.MainWindowHandle);
                        return;
                    }
                }
            }
        }
    }
}