using System;
using System.Xml;
using System.Windows.Forms;

namespace Hawkeye.Configuration
{
    internal static class LayoutManager
    {
        private static LayoutService service = null;

        public static void Load(XmlNode rootNode)
        {
            if (service == null)
                service = new LayoutService(() => rootNode);
            else throw new ApplicationException("LayoutManager is already initialized.");
        }

        public static void RegisterForm(string key, Form form)
        {
            if (service == null)
                throw new ApplicationException("LayoutManager is not initialized.");
            service.RegisterForm(key, form);
        }
    }
}
