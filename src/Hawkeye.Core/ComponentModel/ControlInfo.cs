using System;
using System.Windows.Forms;

using log4net.Util.TypeConverters;

namespace Hawkeye.ComponentModel
{
    [TypeConverter(typeof(DotNetInfoConverter))]
    internal class ControlInfo : IControlInfo, IProxy
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ControlInfo"/> class.
        /// </summary>
        /// <param name="hwnd">The Window handle of the control.</param>
        public ControlInfo(IntPtr hwnd)
        {
            Control = Control.FromHandle(hwnd);
        }

        #region IControlInfo Members

        public Control Control
        {
            get;
#if DEBUG
            set; // Needed for tests purpose
#else
            private set;
#endif
        }

        #endregion     
        
        #region IProxy Members

        public object Value
        {
            get { return Control; }
        }

        #endregion
    }
}
