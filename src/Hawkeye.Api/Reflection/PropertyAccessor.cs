using System;
using System.Reflection;

namespace Hawkeye.Reflection
{
    internal class PropertyAccessor
    {
        private static readonly BindingFlags[] flagsToExamine;

        private readonly string name;
        private readonly Type targetType;
        private readonly object target;
        private readonly PropertyInfo info;
        
        /// <summary>
        /// Initializes the <see cref="PropertyAccessor"/> class.
        /// </summary>
        static PropertyAccessor()
        {
            flagsToExamine = new[]
            {
                BindingFlags.Default,
                BindingFlags.Instance | BindingFlags.FlattenHierarchy,
                BindingFlags.Static | BindingFlags.FlattenHierarchy,
                
                BindingFlags.NonPublic | BindingFlags.Instance,

                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy,
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.GetProperty,
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetProperty,
                
				BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.GetProperty | BindingFlags.IgnoreCase | BindingFlags.IgnoreReturn | BindingFlags.Instance | BindingFlags.PutDispProperty | BindingFlags.PutRefDispProperty | BindingFlags.SetProperty | BindingFlags.Static,

                BindingFlags.NonPublic | BindingFlags.Static,
                BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy | BindingFlags.GetProperty,
                BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetProperty                
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyAccessor"/> class.
        /// </summary>
        /// <param name="propertyTarget">The property target.</param>
        /// <param name="propertyName">Name of the property.</param>
        public PropertyAccessor(object propertyTarget, string propertyName) :
            this(propertyTarget.GetType(), propertyTarget, propertyName) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyAccessor"/> class.
        /// </summary>
        /// <param name="propertyTargetType">Type of the property target.</param>
        /// <param name="propertyName">Name of the property.</param>
        public PropertyAccessor(Type propertyTargetType, string propertyName) :
            this(propertyTargetType, null, propertyName) { }

        /// <summary>
        /// Prevents a default instance of the <see cref="PropertyAccessor"/> class from being created.
        /// </summary>
        /// <param name="propertyTargetType">Type of the property target.</param>
        /// <param name="propertyTarget">The property target.</param>
        /// <param name="propertyName">Name of the property.</param>
        private PropertyAccessor(Type propertyTargetType, object propertyTarget, string propertyName)
        {
            target = propertyTarget;
            targetType = propertyTargetType;
            name = propertyName;

            do
            {
                foreach (var flagToExamine in flagsToExamine)
                {
                    var candidate = FindProperty(flagToExamine);
                    if (candidate != null)
                    {
                        info = candidate;
                        break;
                    }
                }

                if (info == null)
                {
                    targetType = targetType.BaseType;
                    if (targetType == typeof(object))
                        break;
                }

            } while (info == null);
        }

        public object Target
        {
            get { return target; }
        }

        public bool IsValid
        {
            get { return info != null; }
        }
        
        public object Get(object operationTarget = null)
        {
            return info.GetValue(operationTarget ?? target, null);
        }

        public void Set(object newValue, object operationTarget = null)
        {
            info.SetValue(operationTarget ?? target, newValue, null);
        }

        private PropertyInfo FindProperty(BindingFlags flags)
        {
            var propertyInfo = targetType.GetProperty(name, flags);
            if (propertyInfo != null)
                return propertyInfo;

            var allProperties = targetType.GetProperties(flags);
            foreach (var pinfo in allProperties)
            {
                if (pinfo.Name == name)
                    return pinfo;
            }

            return null;
        }
    }
}
