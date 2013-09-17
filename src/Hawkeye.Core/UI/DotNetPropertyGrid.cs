using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Design;

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
        public DotNetPropertyGrid() : base()
        {
        }

        /// <summary>
        /// Called when the control is created.
        /// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            // Clear the standard tabs
            var existingTabTypes = base.PropertyTabs.Cast<PropertyTab>().Select(t => t.GetType()).Distinct().ToArray();
            foreach (var tabType in existingTabTypes)
                base.PropertyTabs.RemoveTabType(tabType);

            base.PropertyTabs.AddTabType(typeof(AllPropertiesTab));
            base.PropertyTabs.AddTabType(typeof(AllEventsTab));

            //base.RemoveDefaultPropertyPage();
            //base.ToolStrip.Items[3].Select();
            //base.SelectedTab = base.PropertyTabs[0];
            
            // Don't activate property page now because there's nothing in GenericComponentEditor
            EnablePropertyPageButton();
        }
        
        private void EnablePropertyPageButton()
        {
            //Yet another reflection hack...
            var method = typeof(PropertyGrid).GetMethod(
                "EnablePropPageButton", BindingFlags.Instance | BindingFlags.NonPublic);

            var result = (bool)method.Invoke(this, new[] { base.SelectedObject });
            if (result)
            {
                // The pg may fail to refresh its toolstrip. Let's force it.
                ToolStrip.Items[ToolStrip.Items.Count - 1].Enabled = true;
            }

            ToolStrip.Invalidate();
        }

        /// <summary>
        /// Initializes the tool strip.
        /// </summary>
        protected override void InitializeToolStrip()
        {
            // do nothing
        }
    }
}