using System;
using System.IO;

namespace Hawkeye.Logging.log4net
{
    public class SpecialFolderPatternConverter : global::log4net.Util.PatternConverter
    {
        protected override void Convert(TextWriter writer, object state)
        {
            var specialFolder = (Environment.SpecialFolder)Enum.Parse(
                typeof(Environment.SpecialFolder), base.Option, true);
            writer.Write(Environment.GetFolderPath(specialFolder));
        }
    }
}
