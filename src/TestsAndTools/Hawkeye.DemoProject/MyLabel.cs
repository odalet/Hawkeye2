using System;
using System.Windows.Forms;

namespace Hawkeye.DemoProject
{
    public class MyLabel : Label
    {
        public MyLabel() : base() { }

        public string TheText
        {
            get { return base.Text; }
            set { base.Text = value; }
        }
    }
}
