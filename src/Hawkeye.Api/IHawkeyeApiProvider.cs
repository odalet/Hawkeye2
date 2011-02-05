using System;
using Hawkeye.Api.Reflection;

namespace Hawkeye.Api
{
    internal interface IHawkeyeApiProvider
    {
        IReflectionApi CreateReflectionApi();
    }
}
