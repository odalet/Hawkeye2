using System;
using System.Text;

namespace Hawkeye
{
    /// <summary>
    /// Exception formatting extension method.
    /// </summary>
    public static class ExceptionExtensions
    {
        #region Exception

        /// <summary>
        /// Returns Exception information intoa formatted string.
        /// </summary>
        /// <param name="exception">The exception to describe.</param>
        /// <returns>Formated (and indented) string giving information about <paramref name="exception"/>.</returns>
        public static string ToFormattedString(this Exception exception)
        {
            if (exception == null) return string.Empty;

            const string tab = "   ";
            const string leafEx = " + ";
            const string leafTr = " | ";
            string indent = string.Empty;

            var builder = new StringBuilder();
            for (var currentException = exception; currentException != null; currentException = currentException.InnerException)
            {
                builder.Append(indent);
                builder.Append(leafEx);
                builder.Append("[");
                builder.Append(currentException.GetType().ToString());
                builder.Append("] ");
                builder.Append(currentException.Message);
                builder.Append(Environment.NewLine);

                indent += tab;

                if (currentException.StackTrace == null)
                    continue;

                var stackTrace = currentException.StackTrace
                    .Replace(Environment.NewLine, "\n").Split('\n');

                for (int i = 0; i < stackTrace.Length; i++)
                {
                    builder.Append(indent);
                    builder.Append(leafTr);
                    builder.Append(stackTrace[i].Trim());
                    builder.Append(Environment.NewLine);
                }
            }

            return builder.ToString();
        }

        #endregion
    }
}
