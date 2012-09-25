using System;
using System.Drawing;

namespace Hawkeye.WinApi
{
    internal static class WindowHelper
    {
        public static IntPtr FindWindow(Point point)
        {
            return NativeMethods.WindowFromPoint(POINT.FromPoint(point));            
        }

        public static IntPtr FindChildWindow(IntPtr parentWindowHandle, Point point)
        {
            return NativeMethods.ChildWindowFromPoint(parentWindowHandle, POINT.FromPoint(point));
        }

        public static Point ScreenToClient(IntPtr windowHandle, Point point)
        {
            var pt = POINT.FromPoint(point);
            if (NativeMethods.ScreenToClient(windowHandle, ref pt))
                return pt.ToPoint();
            return Point.Empty;
        }

        public static void RemoveAdorner(IntPtr hwnd)
        {
            var toUpdate = hwnd;
            var parentWindow = NativeMethods.GetParent(hwnd);
            if (parentWindow != IntPtr.Zero)
                toUpdate = parentWindow; // using parent

            NativeMethods.InvalidateRect(toUpdate, IntPtr.Zero, true);
            NativeMethods.UpdateWindow(toUpdate);
            bool result = NativeMethods.RedrawWindow(toUpdate, IntPtr.Zero, IntPtr.Zero, (uint)(
                RDW.RDW_FRAME | RDW.RDW_INVALIDATE | RDW.RDW_UPDATENOW | RDW.RDW_ERASENOW | RDW.RDW_ALLCHILDREN));
	
        }

        public static void DrawAdorner(IntPtr hwnd, Action<Graphics, Size> drawMethod = null)
        {
            var windowRect = new RECT(0, 0, 0, 0);
            NativeMethods.GetWindowRect(hwnd, ref windowRect);

            var parentWindow = NativeMethods.GetParent(hwnd);
            IntPtr windowDC;
            windowDC = NativeMethods.GetWindowDC(hwnd);
            if (windowDC == IntPtr.Zero) return;

            if (drawMethod == null) drawMethod = (g, s) =>
            {
                using (var pen = new Pen(Color.Red, 2))
                    g.DrawRectangle(pen, 1, 1, s.Width - 2, s.Height - 2);
            };

            try
            {
                using (var g = Graphics.FromHdc(windowDC, hwnd))
                    drawMethod(g, windowRect.Size);
            }
            finally { NativeMethods.ReleaseDC(hwnd, windowDC); }
        }
    }
}
