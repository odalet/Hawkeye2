using System;
using System.Reflection;

namespace Hawkeye
{
    /// <summary>
    /// Extends the <see cref="System.Type"/> class.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Determines whether the specified object is a descendant of <paramref name="type" />.
        /// </summary>
        /// <param name="value">The object to test.</param>
        /// <param name="type">Type of the supposed parent.</param>
        /// <returns>
        ///   <c>true</c> if the specified object is a descendant of <paramref name="type" />;
        /// otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">value</exception>
        public static bool IsA(this object value, Type type)
        {
            if (value == null) throw new ArgumentNullException("value");
            return value.GetType().IsA(type);
        }

        /// <summary>
        /// Determines whether the specified object is a descendant of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of the supposed parent.</typeparam>
        /// <param name="value">The object to test.</param>
        /// <returns>
        /// 	<c>true</c> if the specified object is a descendant of <typeparamref name="T"/>;
        /// otherwise, <c>false</c>.
        /// </returns>
        public static bool IsA<T>(this object value)
        {
            return IsA(value, typeof(T));
        }

        /// <summary>
        /// Determines whether the specified type is a descendant of <paramref name="parentType"/>.
        /// </summary>
        /// <param name="type">The type to test.</param>
        /// <param name="parentType">Type of the supposed parent.</param>
        /// <returns>
        /// 	<c>true</c> if the specified type is a descendant of <paramref name="parentType"/>;
        /// otherwise, <c>false</c>.
        /// </returns>
        public static bool IsA(this Type type, Type parentType)
        {
            if (parentType == null) return type == null;
            return parentType.IsAssignableFrom(type);
        }

        /// <summary>
        /// Determines whether the specified type is a descendant of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of the supposed parent.</typeparam>
        /// <param name="type">The type to test.</param>
        /// <returns>
        /// 	<c>true</c> if the specified type is a descendant of <typeparamref name="T"/>;
        /// otherwise, <c>false</c>.
        /// </returns>
        public static bool IsA<T>(this Type type)
        {
            return IsA(type, typeof(T));
        }

        /// <summary>
        /// Returns the default value for the given <paramref name="type"/>.
        /// </summary>
        /// <remarks>
        /// The returned value is equivalent to <c>default(T)</c>.
        /// </remarks>
        /// <param name="type">The type for which to return the default value.</param>
        /// <returns>The default value for <paramref name="type"/>.</returns>
        public static object CreateDefaultValue(this Type type)
        {
            if (type == null) return null;
            if (type.IsValueType) return Activator.CreateInstance(type);
            return null; // in all other cases.
        }

        /// <summary>
        /// Creates an instance of the given <paramref name="type"/>.
        /// </summary>
        /// <remarks>
        /// Type <paramref name="type"/> must provide a parameterless constructor.
        /// The constructor needn't be public.
        /// </remarks>
        /// <param name="type">The type for which to create an instance.</param>
        /// <returns>An instance of <paramref name="type"/>.</returns>
        public static object CreateInstance(this Type type)
        {
            if (type == null) return null;
            var flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            var ci = type.GetConstructor(flags, null, Type.EmptyTypes, null);
            if (ci == null) throw new InvalidOperationException(string.Format("Type {0} must define a parameterless constructor.", type));
            return ci.Invoke(null);
        }

        /// <summary>
        /// Determines whether the specified type is a nullable type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// 	<c>true</c> if the specified type is nullable; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullable(this Type type)
        {
            if (type == null) return false;
            if (!type.IsGenericType) return false;
            return type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
    }
}
