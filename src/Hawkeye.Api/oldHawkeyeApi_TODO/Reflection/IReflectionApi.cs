////TODO: Plugins API
//using System;

//namespace Hawkeye.Reflection
//{
//    /// <summary>
//    /// Gives plugins access to Hawkeye's Reflection API.
//    /// </summary>
//    public interface IReflectionApi
//    {
//        // TODO: Static accessors

//        /***

//        /// <summary>
//        /// Creates an object allowing to access a field named <paramref name="fieldName"/> 
//        /// on objects of type <paramref name="targetType"/>.
//        /// </summary>
//        /// <param name="targetType">Type of the target.</param>
//        /// <param name="fieldName">Name of the field.</param>
//        /// <returns>A field accessor.</returns>
//        IFieldAccessor CreateStaticFieldAccessor(Type targetType, string fieldName);

//        /// <summary>
//        /// Creates an object allowing to access a property named <paramref name="propertyName"/> 
//        /// on objects of type <paramref name="targetType"/>.
//        /// </summary>
//        /// <param name="targetType">Type of the target.</param>
//        /// <param name="propertyName">Name of the property.</param>
//        /// <returns>A property accessor.</returns>
//        IPropertyAccessor CreateStaticPropertyAccessor(Type targetType, string propertyName);

//        /// <summary>
//        /// Creates an object allowing to access a method named <paramref name="methodName"/>
//        /// on objects of type <paramref name="targetType"/>.
//        /// </summary>
//        /// <param name="targetType">Type of the target.</param>
//        /// <param name="methodName">Name of the method.</param>
//        /// <returns>A method accessor.</returns>
//        IMethodAccessor CreateStaticMethodAccessor(Type targetType, string methodName);

//        ***/ 
          
//        /// <summary>
//        /// Creates an object allowing to access a field named <paramref name="fieldName"/>
//        /// on objects of type <paramref name="targetObject"/>.
//        /// </summary>
//        /// <param name="targetObject">The target object.</param>
//        /// <param name="fieldName">Name of the field.</param>
//        /// <returns>A field accessor.</returns>
//        IFieldAccessor CreateFieldAccessor(object targetObject, string fieldName);

//        /// <summary>
//        /// Creates an object allowing to access a property named <paramref name="propertyName"/>
//        /// on objects of type <paramref name="targetObject"/>.
//        /// </summary>
//        /// <param name="targetObject">The target object.</param>
//        /// <param name="propertyName">Name of the property.</param>
//        /// <returns>A property accessor.</returns>
//        IPropertyAccessor CreatePropertyAccessor(object targetObject, string propertyName);

//        /// <summary>
//        /// Creates an object allowing to access a method named <paramref name="methodName"/>
//        /// on objects of type <paramref name="targetObject"/>.
//        /// </summary>
//        /// <param name="targetObject">The target object.</param>
//        /// <param name="methodName">Name of the method.</param>
//        /// <returns>A method accessor.</returns>
//        IMethodAccessor CreateMethodAccessor(object targetObject, string methodName);
//    }
//}
