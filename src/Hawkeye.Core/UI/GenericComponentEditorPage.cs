using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Hawkeye.UI
{
    internal class GenericComponentEditorPage : ComponentEditorPage
    {
        public GenericComponentEditorPage() : this("Page 1") { }
        public GenericComponentEditorPage(string text)
            : base()
        {
            base.Text = text;
            
            // This must not be too small otherwise, the "OK" button 
            // gets hidden in ComponentEditorForm...
            base.Size = new Size(400, 300); 
            base.Controls.Add(new Label() { Text = text });
        }

        protected override void LoadComponent()
        {
            var component = base.Component;

            base.Text = component.ToString();
        }

        protected override void SaveComponent()
        {
            // Nothing to do here
        }
    }

    internal class GenericComponentEditorPage2 : GenericComponentEditorPage
    {
        public GenericComponentEditorPage2() : base("Page 2") { }
    }

    internal class GenericComponentEditorPage3 : GenericComponentEditorPage
    {
        public GenericComponentEditorPage3() : base("Page 3") { }
    }
}
