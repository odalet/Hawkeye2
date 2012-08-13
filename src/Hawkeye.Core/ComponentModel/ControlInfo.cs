using System;
using System.Windows.Forms;

using log4net.Util.TypeConverters;

namespace Hawkeye.ComponentModel
{
    [TypeConverter(typeof(DotNetInfoConverter))]
    internal class ControlInfo : IControlInfo
    {
        #region IControlInfo Members

        public Control Control
        {
            get;
            private set;
        }

        #endregion     

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlInfo"/> class.
        /// </summary>
        /// <param name="hwnd">The Window handle of the control.</param>
        public ControlInfo(IntPtr hwnd)
        {
            Control = Control.FromHandle(hwnd);
        }
    }
}
