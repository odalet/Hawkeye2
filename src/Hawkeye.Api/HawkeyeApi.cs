using System;
using Hawkeye.Api.Reflection;

namespace Hawkeye.Api
{
    /// <summary>
    /// Gives plugins access to various API exposed by Hawkeye.
    /// </summary>
    public class HawkeyeApi
    {
        internal HawkeyeApi(IHawkeyeApiProvider provider)
        {
            ReflectionApi = provider.CreateReflectionApi();
        }

        /// <summary>
        /// Getsa reference to Hawkeye's reflection API.
        /// </summary>
        /// <value>The reflection API.</value>
        public IReflectionApi ReflectionApi
        {
            get;
            private set;
        }
    }
}
