using System;
using System.Collections.Generic;
using System.Text;
using Hawkeye.Extensibility;
using Hawkeye.Logging;
using System.Windows.Forms;

namespace Hawkeye.DecompilePlugin
{
    internal abstract class BaseDecompilerPluginCore : BaseCommandPlugin
    {
        private ILogService log = null;
        private IWindowInfo windowInfo = null;
        private IDecompilerController controller = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReflectorPluginCore"/> class.
        /// </summary>
        /// <param name="descriptor">The descriptor.</param>
        public BaseDecompilerPluginCore(IPluginDescriptor descriptor) : 
            base(descriptor) { }

        public override string Label
        {
            get { return "&Decompile"; }
        }

        protected abstract IDecompilerController CreateDecompilerController();

        protected virtual string DecompilerNotAvailable
        {
            get 
            { 
                return
@"A running instance of the decompiler could not be found. 
Hawkeye can not show you the source code for the selected item.

Make sure it is running"; 
            }
        }

        /// <summary>
        /// Called when the plugin has just been initialized.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            controller = CreateDecompilerController();

            log = base.Host.GetLogger<ReflectorPluginCore>();
            base.Host.CurrentWindowInfoChanged += (s, e) =>
            {
                windowInfo = base.Host.CurrentWindowInfo;
                base.RaiseCanExecuteChanged(this);
            };

            windowInfo = base.Host.CurrentWindowInfo;
            base.RaiseCanExecuteChanged(this);

            log.Info(string.Format("'{0}' was initialized.", base.Descriptor.Name));
        }

        /// <summary>
        /// Determines whether this plugin command can be executed.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the command can be executed; otherwise, <c>false</c>.
        /// </returns>
        protected override bool CanExecuteCore()
        {
            return
                windowInfo != null &&
                windowInfo.ControlInfo != null &&
                windowInfo.ControlInfo.Control != null;
        }

        /// <summary>
        /// Executes this plugin command.
        /// </summary>
        protected override void ExecuteCore()
        {
            if (!CanExecuteCore()) return;

            var savedCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                OpenInDecompiler();
            }
            finally
            {
                Cursor.Current = savedCursor;
            }
        }

        private void OpenInDecompiler()
        {
            if (!controller.IsRunning)
            {
                MessageBox.Show(DecompilerNotAvailable,
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var control = windowInfo.ControlInfo.Control;
            var type = control.GetType();

            // Remark: the logic here is really simplified comapred with the Hawkeye 1 Reflector facility:
            // In Hawkeye 1, we tried to get Reflector to point to the exact property, member, event or method
            // that was curently selected.
            // Here, we only open the current type in Reflector.
            // Indeed this makes more sense, because often times, in the previous version, what was selected 
            // was some member inherited from Control and Reflector was not loading the really inspected 
            // control but some member of the System.Windows.Forms.Control class.

            controller.GotoType(type);
        }
    }
}
