using System;
//using System.Linq;
using System.Drawing;
using System.Windows.Forms;

using Hawkeye.Logging;
using Hawkeye.Extensibility;

namespace Hawkeye.DecompilePlugin
{
    internal class ReflectorPluginCore : BaseDecompilerPluginCore
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReflectorPluginCore"/> class.
        /// </summary>
        /// <param name="descriptor">The descriptor.</param>
        public ReflectorPluginCore(ReflectorPluginDescriptor descriptor) :
            base(descriptor) { }

        /// <summary>
        /// Gets the label displayed on the menu (or toolbar button) for this command.
        /// </summary>
        public override string Label
        {
            get { return "Decompile with &Reflector"; }
        }

        /// <summary>
        /// Gets the image displayed on the menu (or toolbar button) for this command.
        /// </summary>
        public override Bitmap Image
        {
            get { return Properties.Resources.Reflector; }
        }

        protected override string DecompilerNotAvailable
        {
            get
            {
                return
@"Lutz Roeder's .NET Reflector is not started or it is a version older than 4.0. 
Hawkeye can not show you the source code for the selected item.

Please open .NET Reflector to use this feature.";
            }
        }

        protected override IDecompilerController CreateDecompilerController()
        {
            return new ReflectorController();
        }
    }
}
