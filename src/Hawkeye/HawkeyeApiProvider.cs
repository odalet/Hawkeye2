using System;

using Hawkeye.Api;
using Hawkeye.Api.Reflection;
using Hawkeye.Reflection;

namespace Hawkeye
{
    internal class HawkeyeApiProvider : IHawkeyeApiProvider
    {
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

        #region IHawkeyeApiProvider Members

        public IReflectionApi CreateReflectionApi()
        {
            return new ReflectionApi();
        }

        #endregion
    }
}
