using System;
using System.Collections.Generic;
using System.Text;

namespace Hawkeye.Api
{
    public interface IHawkeyeApplication
    {
        IHawkeyeEditor Editor { get; }
        HawkeyeApi Api { get; }
    }
}
