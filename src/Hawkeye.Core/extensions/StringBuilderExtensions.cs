using System;
using System.Text;

namespace Hawkeye
{
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
}
