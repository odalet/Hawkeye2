using System;
using System.Windows.Forms;
using System.ComponentModel;

using Hawkeye.Logging;
using Hawkeye.UI;

namespace Hawkeye.ComponentModel
{
    [TypeConverter(typeof(DotNetInfoConverter))]
    internal class ControlInfo : IControlInfo, IProxy
    {
        private static ILogService log = LogManager.GetLogger<ControlInfo>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlInfo"/> class.
        /// </summary>
        /// <param name="hwnd">The Window handle of the control.</param>
        public ControlInfo(IntPtr hwnd)
        {
            Control = GetControlFromHandle(hwnd);
            InitializeTypeDescriptor(Control);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlInfo"/> class.
        /// </summary>
        /// <param name="control">The control.</param>
        public ControlInfo(Control control)
        {
            if (control == null)
                log.Warning("Specified control is null.");

            Control = control;
            InitializeTypeDescriptor(Control);
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

        public string Name
        {
            get
            {
                if (Control == null) return string.Empty;
                var name = Control.Name;
                if (!string.IsNullOrEmpty(name))
                    return name;
                return Control.GetType().Name;
            }
        }

        #endregion     
        
        #region IProxy Members

        public object Value
        {
            get { return Control; }
        }

        #endregion

        private void InitializeTypeDescriptor(object instance)
        {
            if (instance == null) return;
            var type = instance.GetType();
            CustomTypeDescriptors.AddGenericProviderToType(type);
        }

        private Control GetControlFromHandle(IntPtr hwnd)
        {
            var control = Control.FromHandle(hwnd); // Usually this is enough
            if (control == null)
            {
                // But in some cases (when inspecting most Visual Studio controls for instance), it is not...
                // Is there a workaround? It seems that only the VS property grid & the windows forms designer
                // can be inpected
                log.Warning(string.Format(
                    "Handle {0} is not associated with a .NET control: 'Control.FromHandle(hwnd)' returns null.", hwnd));
            }

            return control;
        }
    }
}
