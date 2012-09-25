using Hawkeye.ComponentModel;

namespace Hawkeye
{
    internal static class ObjectExtensions
    {
        /// <summary>
        /// Recursively inspect the provided object in case it is a <see cref="IProxy"/> to return its inner value.
        /// </summary>
        /// <param name="proxy">The potential proxy object.</param>
        /// <returns>The specified item or its inner value.</returns>
        public static object GetInnerObject(this object proxy)
        {
            if (proxy == null) return null;
            if (proxy is IProxy)
                return GetInnerObject(((IProxy)proxy).Value);
            else return proxy;
        }
    }
}
