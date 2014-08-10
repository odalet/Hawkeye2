using System.Drawing;
using Hawkeye.Extensibility;

namespace Hawkeye.DecompilePlugin
{
    internal class ILSpyPluginCore : BaseDecompilerPluginCore
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ILSpyPluginCore"/> class.
        /// </summary>
        /// <param name="descriptor">The descriptor.</param>
        public ILSpyPluginCore(ILSpyPluginDescriptor descriptor) : 
            base(descriptor) { }

        /// <summary>
        /// Gets the label displayed on the menu (or toolbar button) for this command.
        /// </summary>
        public override string Label
        {
            get { return "Decompile with &ILSpy"; }
        }

        /// <summary>
        /// Gets the image displayed on the menu (or toolbar button) for this command.
        /// </summary>
        public override Bitmap Image
        {
            get { return Properties.Resources.ILSpy; }
        }

        protected override string DecompilerNotAvailable
        {
            get
            {
                return
@"ILSpy is not started. 
Hawkeye can not show you the source code for the selected item.

Please open ILSpy to use this feature.";
            }
        }

        protected override IDecompilerController CreateDecompilerController()
        {
            return new ILSpyController();
        }
    }
}
