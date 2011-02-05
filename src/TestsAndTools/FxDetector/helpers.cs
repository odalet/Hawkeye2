using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace FxDetector
{
    internal static class This
    {
        public static bool IsX64
        {
            get { return IntPtr.Size == 8; }
        }
    }

    internal class ErrorBox
    {
        public static DialogResult Show(string text) { return Show(null, text); }
        public static DialogResult Show(IWin32Window owner, string text)
        {
            return MessageBox.Show(
                owner, text, "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error,
                MessageBoxDefaultButton.Button1);
        }
    }

    internal static class StringBuilderExtensions
    {
        public static void AppendFormattedLine(
            this StringBuilder builder,
            string format,
            params object[] args)
        {
            AppendFormattedLine(builder, 0, format, args);
        }

        public static void AppendFormattedLine(
            this StringBuilder builder,
            int tabCount,
            string format,
            params object[] args)
        {
            var tabs = new string('\t', tabCount);
            var text = tabs + format + Environment.NewLine;
            builder.AppendFormat(text, args);
        }
    }

    internal static class ModuleEntryExtensions
    {
        public static void DumpTo(this NativeMethods.MODULEENTRY32 module, StringBuilder builder)
        {
            DumpTo(module, builder, 0);
        }

        public static void DumpTo(this NativeMethods.MODULEENTRY32 module, StringBuilder builder, int tabCount)
        {
            var converter = new ModuleEntryConverter();
            converter.GetProperties(module)
                .Cast<PropertyDescriptor>()
                .OrderBy(pd => pd.Name)
                .Select(pd => new
                {
                    Name = pd.Name,
                    Value = pd.GetValue(module).ToString()
                })
                .Aggregate((acc, pair) =>
                {
                    builder.AppendFormattedLine(tabCount, "{0} = {1}",
                        pair.Name, pair.Value);
                    return pair;
                });
        }
    }
}
