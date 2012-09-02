using System.Windows.Forms;
using System.ComponentModel;

namespace Hawkeye
{
    internal class TestControl : Control
    {
        private static string staticFoo = "";
        private string foo = "";

        public static string StaticStringProperty { get; set; }
        public static string StaticReadOnlyStringProperty { get { return "Static Readonly String Property"; } }
        public static string StaticWriteOnlyStringProperty { set { staticFoo = "Writeonly String Property"; } }

        private static string PrivateStaticStringProperty { get; set; }
        private static string PrivateStaticReadOnlyStringProperty { get { return "Private Static Readonly String Property"; } }
        private static string PrivateStaticWriteOnlyStringProperty { set { staticFoo = "Private Static Writeonly String Property"; } }

        public string StringProperty { get; set; }
        [Browsable(false)]public string NonBrowsableStringProperty { get; set; }
        public string ReadOnlyStringProperty { get { return "Readonly String Property"; } }
        public string WriteOnlyStringProperty { set { foo = "Writeonly String Property"; } }

        private string PrivateStringProperty { get; set; }
        [Browsable(false)]private string PrivateNonBrowsableStringProperty { get; set; }
        private string PrivateReadOnlyStringProperty { get { return "Private Readonly String Property"; } }
        private string PrivateWriteOnlyStringProperty { set { foo = "Private Writeonly String Property"; } }



    }
}
