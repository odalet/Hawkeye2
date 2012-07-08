using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.ComponentModel;

namespace Hawkeye.WinApi
{
    internal static class CursorHelper
    {
        //TODO: check this doesn't lead to a memory leak.
        public static Cursor LoadFrom(Bitmap image)
        {
            var handle = image.GetHicon();
            var hcur = NativeMethods.CopyImage(handle, 2, 32, 32, 0);
            var cursor = new Cursor(hcur);
            
            // Note: force the cursor to own the handle so it gets released properly
            var fieldInfo = typeof(Cursor).GetField("ownHandle", BindingFlags.NonPublic | BindingFlags.Instance);
            fieldInfo.SetValue(cursor, true);
            return cursor;
        }

        public static Cursor LoadFrom(string path)
        {
            var handle = NativeMethods.LoadCursorFromFile(path);
            if (handle == IntPtr.Zero) throw new Win32Exception(string.Format(
                "Invalid Cursor handle; could not load cursor from {0}", path));

            var cursor = new Cursor(handle);

            // Note: force the cursor to own the handle so it gets released properly
            var fieldInfo = typeof(Cursor).GetField("ownHandle", BindingFlags.NonPublic | BindingFlags.Instance);
            fieldInfo.SetValue(cursor, true);
            return cursor;
        }
    }
}
