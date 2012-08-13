using System;
using System.IO;

namespace Hawkeye.Logging.log4net
{
    public class SpecialFolderPatternConverter : global::log4net.Util.PatternConverter
    {
        /// <summary>
        /// Evaluate this pattern converter and write the output to a writer.
        /// </summary>
        /// <param name="writer"><see cref="T:System.IO.TextWriter"/> that will receive the formatted result.</param>
        /// <param name="state">The state object on which the pattern converter should be executed.</param>
        protected override void Convert(TextWriter writer, object state)
        {
            var specialFolder = (Environment.SpecialFolder)Enum.Parse(
                typeof(Environment.SpecialFolder), base.Option, true);
            writer.Write(Environment.GetFolderPath(specialFolder));
        }
    }
}
