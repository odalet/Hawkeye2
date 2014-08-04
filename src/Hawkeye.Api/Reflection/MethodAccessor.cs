using System;
using System.Reflection;

namespace Hawkeye.Reflection
{
    internal class MethodAccessor
    {
        private static readonly BindingFlags[] flagsToExamine;

        private readonly string name;
        private readonly Type targetType;
        private readonly MethodInfo info;

        /// <summary>
        /// Initializes the <see cref="MethodAccessor"/> class.
        /// </summary>
        static MethodAccessor()
        {
            flagsToExamine = new[]
            {
                BindingFlags.NonPublic | BindingFlags.Instance,
                BindingFlags.Public | BindingFlags.Instance,
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodAccessor"/> class.
        /// </summary>
        /// <param name="methodTargetType">Type of the method target.</param>
        /// <param name="methodName">Name of the method.</param>
        public MethodAccessor(Type methodTargetType, string methodName)
        {
            name = methodName;
            targetType = methodTargetType;

            do
            {
                foreach (var flagToExamine in flagsToExamine)
                {
                    var candidate = FindMethod(flagToExamine);
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

        public bool IsValid
        {
            get { return info != null; }
        }
        
        public object Invoke(object target, object[] param)
        {
            return info.Invoke(target, param);
        }

        public object Invoke(object target)
        {
            return Invoke(target, new object[] { });
        }

        public object Invoke(object target, object oneParam)
        {
            return Invoke(target, new object[] { oneParam });
        }

        private MethodInfo FindMethod(BindingFlags flags)
        {
            return targetType.GetMethod(name, flags);
        }
    }
}
