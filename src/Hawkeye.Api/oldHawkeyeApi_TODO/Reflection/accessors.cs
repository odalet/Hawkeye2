////TODO: Plugins API
//using System;
//using System.Reflection;

//namespace Hawkeye.Reflection
//{
//    /// <summary>
//    /// Represents a static member.
//    /// </summary>
//    public interface IStaticMemberAccessor
//    {
//        /// <summary>
//        /// Gets a value indicating whether this instance is valid.
//        /// </summary>
//        /// <value><c>true</c> if this instance is valid; otherwise, <c>false</c>.</value>
//        bool IsValid { get; }

//        /// <summary>
//        /// Gets the <see cref="MemberInfo"/> object associated with this instance.
//        /// </summary>
//        /// <value>The <see cref="MemberInfo"/> object associated with this instance or 
//        /// <c>null</c> if this instance is not valid.</value>
//        MemberInfo MemberInfo { get; }
        
//        /// <summary>
//        /// Gets the real target type this instance is associated with.
//        /// </summary>
//        /// <value>The type of objects owning this member.</value>
//        Type TargetType { get; }
//    }

//    /// <summary>
//    /// Represents an object member.
//    /// </summary>
//    public interface IMemberAccessor
//    {
//        /// <summary>
//        /// Gets a value indicating whether this instance is valid.
//        /// </summary>
//        /// <value><c>true</c> if this instance is valid; otherwise, <c>false</c>.</value>
//        bool IsValid { get; }

//        /// <summary>
//        /// Gets the <see cref="MemberInfo"/> object associated with this instance.
//        /// </summary>
//        /// <value>The <see cref="MemberInfo"/> object associated with this instance or 
//        /// <c>null</c> if this instance is not valid.</value>
//        MemberInfo MemberInfo { get; }

//        /// <summary>
//        /// Gets or sets the target object to use when <i>invoking</i> the member.
//        /// </summary>
//        /// <value>The target.</value>
//        object Target { get; set; }

//        /// <summary>
//        /// Gets the real target type this instance is associated with.
//        /// </summary>
//        /// <value>The type of objects owning this member.</value>
//        Type TargetType { get; }
//    }

//    /// <summary>
//    /// Represents an object field.
//    /// </summary>
//    public interface IFieldAccessor : IMemberAccessor
//    {
//        /// <summary>
//        /// Gets the name of the field represented by this instance.
//        /// </summary>
//        /// <value>The name of the field.</value>
//        string FieldName { get; }

//        /// <summary>
//        /// Gets the <see cref="FieldInfo"/> object associated with this instance.
//        /// </summary>
//        /// <value>The <see cref="FieldInfo"/> object associated with this instance or 
//        /// <c>null</c> if this instance is not valid.</value>
//        FieldInfo FieldInfo { get; }

//        /// <summary>
//        /// Gets the last affected or retrieved value on <see cref="IMemberAccessor.Target"/>.
//        /// </summary>
//        /// <value>The value.</value>
//        object Value { get; }

//        /// <summary>
//        /// Gets the value of the field represented by this instance on the 
//        /// default <see cref="IMemberAccessor.Target"/>.
//        /// </summary>
//        /// <returns>The field value.</returns>
//        object Get();

//        /// <summary>
//        /// Sets the value of the field represented by this instance on the 
//        /// default <see cref="IMemberAccessor.Target"/>.
//        /// </summary>
//        /// <param name="value">The value.</param>
//        void Set(object value);
//    }

//    /// <summary>
//    /// Represents an object property.
//    /// </summary>
//    public interface IPropertyAccessor : IMemberAccessor
//    {
//        /// <summary>
//        /// Gets the name of the property represented by this instance.
//        /// </summary>
//        /// <value>The name of the property.</value>
//        string PropertyName { get; }

//        /// <summary>
//        /// Gets the <see cref="PropertyInfo"/> object associated with this instance.
//        /// </summary>
//        /// <value>The <see cref="PropertyInfo"/> object associated with this instance or 
//        /// <c>null</c> if this instance is not valid.</value>
//        PropertyInfo PropertyInfo { get; }

//        /// <summary>
//        /// Gets the last affected or retrieved value on <see cref="IMemberAccessor.Target"/>.
//        /// </summary>
//        /// <value>The value.</value>
//        object Value { get; }

//        /// <summary>
//        /// Gets the value of the field represented by this instance on the 
//        /// default <see cref="IMemberAccessor.Target"/>.
//        /// </summary>
//        /// <returns>The property value.</returns>
//        object Get();

//        /// <summary>
//        /// Sets the value of the property represented by this instance on the 
//        /// default <see cref="IMemberAccessor.Target"/>.
//        /// </summary>
//        /// <param name="value">The value.</param>
//        void Set(object value);
//    }

//    /// <summary>
//    /// Represents an object method.
//    /// </summary>
//    public interface IMethodAccessor : IMemberAccessor
//    {
//        /// <summary>
//        /// Gets the name of the method represented by this instance.
//        /// </summary>
//        /// <value>The name of the method.</value>
//        string MethodName { get; }

//        /// <summary>
//        /// Gets the <see cref="MethodInfo"/> object associated with this instance.
//        /// </summary>
//        /// <value>The <see cref="MethodInfo"/> object associated with this instance or 
//        /// <c>null</c> if this instance is not valid.</value>
//        MethodInfo MethodInfo { get; }

//        /// <summary>
//        /// Invokes the method represented by this instance on the 
//        /// default <see cref="IMemberAccessor.Target"/>.
//        /// </summary>
//        /// <param name="parameters">The parameters to pass to the method.</param>
//        /// <returns>The method invocation result.</returns>
//        object Invoke(params object[] parameters);
//    }
//}