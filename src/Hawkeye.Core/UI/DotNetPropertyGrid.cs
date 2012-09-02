using Hawkeye.UI.Controls;
using Hawkeye.UI.PropertyTabs;

namespace Hawkeye.UI
{
    /// <summary>
    /// The property grid used to display .NET related information about a window.
    /// </summary>
    internal class DotNetPropertyGrid : PropertyGridEx
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetPropertyGrid"/> class.
        /// </summary>
        public DotNetPropertyGrid() : base() { }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            base.PropertyTabs.AddTabType(typeof(AllPropertiesTab));
        }
    }
}
