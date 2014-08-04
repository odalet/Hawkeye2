using System;
using System.Reflection;

namespace Hawkeye.Reflection
{
    internal class FieldAccessor
    {
        private static readonly BindingFlags[] flagsToExamine;

        private readonly string name;
        private readonly Type targetType;
        private readonly object target;
        private readonly FieldInfo info;
        
        /// <summary>
        /// Initializes the <see cref="FieldAccessor"/> class.
        /// </summary>
        static FieldAccessor()
        {
            flagsToExamine = new[]
            {
                BindingFlags.Default,
                BindingFlags.Instance | BindingFlags.FlattenHierarchy,
                BindingFlags.Static | BindingFlags.FlattenHierarchy,

                BindingFlags.NonPublic | BindingFlags.Instance,

                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy,
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.GetField,
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField,
                
                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.IgnoreCase | BindingFlags.IgnoreReturn | BindingFlags.Instance | BindingFlags.PutDispProperty | BindingFlags.PutRefDispProperty | BindingFlags.SetField | BindingFlags.Static,
                
                BindingFlags.NonPublic | BindingFlags.Static,
                BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy | BindingFlags.GetField,
                BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField                
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldAccessor"/> class.
        /// </summary>
        /// <param name="fieldTarget">The target.</param>
        /// <param name="fieldName">Name of the field.</param>
        public FieldAccessor(object fieldTarget, string fieldName) :
            this(fieldTarget.GetType(), fieldTarget, fieldName) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldAccessor"/> class.
        /// </summary>
        /// <param name="fieldTargetType">Type of the target.</param>
        /// <param name="fieldName">Name of the field.</param>
        public FieldAccessor(Type fieldTargetType, string fieldName) :
            this(fieldTargetType, null, fieldName) { }

        /// <summary>
        /// Prevents a default instance of the <see cref="FieldAccessor"/> class from being created.
        /// </summary>
        /// <param name="fieldTargetType">Type of the target.</param>
        /// <param name="fieldTarget">The target.</param>
        /// <param name="fieldName">Name of the field.</param>
        private FieldAccessor(Type fieldTargetType, object fieldTarget, string fieldName)
        {
            target = fieldTarget;
            targetType = fieldTargetType;
            name = fieldName;

            do
            {
                foreach (var flagToExamine in flagsToExamine)
                {
                    var candidate = FindField(flagToExamine);
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
            return info.GetValue(operationTarget ?? target);
        }

        public void Set(object newValue, object operationTarget = null)
        {
            info.SetValue(operationTarget ?? target, newValue);
        }

        private FieldInfo FindField(BindingFlags flags)
        {
            var fieldInfo = targetType.GetField(name, flags);
            if (fieldInfo != null)
                return fieldInfo;

            var allFields = targetType.GetFields(flags);
            foreach (var field in allFields)
            {
                if (field.Name == name)
                    return field;
            }

            return null;
        }
    }
}
