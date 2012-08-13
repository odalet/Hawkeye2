using System;
using System.Collections.Generic;
using System.Text;
using log4net.Util.TypeConverters;
using System.Windows.Forms;

namespace Hawkeye.WinApi
{
    [TypeConverter(typeof(DotNetInfoConverter))]
    internal class DotNetInfo : IDotNetInfo
    {
        #region IDotNetInfo Members

        public string Foo
        {
            get { return "Hello World!"; }
        }

        #endregion

        public Control Control
        {
            get;
            private set;
        }

        public DotNetInfo(IntPtr hwnd)
        {
            Control = Control.FromHandle(hwnd);
        }
    }
}
