using System;

using Hawkeye.Api;
using Hawkeye.Api.Reflection;
using Hawkeye.Reflection;

namespace Hawkeye
{
    internal static class HawkeyeApiBuilder
    {
        private class HawkeyeApi : IHawkeyeApi
        {
            #region IHawkeyeApi Members

            public IHawkeyeEditor Editor { get; set; }

            public IReflectionApi ReflectionApi { get; set; }
            
            #endregion
        }

        private class ReflectionApi : IReflectionApi
        {
            #region IReflectionApi Members

            public IFieldAccessor CreateFieldAccessor(Type targetType, string fieldName)
            {
                return new FieldAccessor(targetType, fieldName);
            }

            public IPropertyAccessor CreatePropertyAccessor(Type targetType, string propertyName)
            {
                return new PropertyAccessor(targetType, propertyName);
            }

            public IMethodAccessor CreateMethodAccessor(Type targetType, string methodName)
            {
                return new MethodAccessor(targetType, methodName);
            }
            
            public IFieldAccessor CreateFieldAccessor(object targetObject, string fieldName)
            {
                return new FieldAccessor(targetObject, fieldName);
            }

            public IPropertyAccessor CreatePropertyAccessor(object targetObject, string propertyName)
            {
                return new PropertyAccessor(targetObject, propertyName);
            }

            public IMethodAccessor CreateMethodAccessor(object targetObject, string methodName)
            {
                return new MethodAccessor(targetObject, methodName);
            }

            #endregion
        }
        
        public static IHawkeyeApi CreateHawkeyeApi()
        {
            return new HawkeyeApi()
            {
                Editor = null, // TODO
                ReflectionApi = new ReflectionApi()
            };
        }
    }
}
