using System;
using System.Collections.Generic;
using System.Text;

namespace Hawkeye.Api
{
    public interface IPlugin
    {
        void Initialize(IHawkeyeApplication application);
    }
}
