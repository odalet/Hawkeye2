using System;
using System.Reflection;

namespace Hawkeye.Reflection
{
    internal abstract class BaseMemberAccessor : IMemberAccessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseMemberAccessor"/> class.
        /// </summary>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="targetObject">The target object.</param>
        protected BaseMemberAccessor(Type targetType, object targetObject)
        {
            if (targetType == null) throw new ArgumentNullException("targetType");
            TargetType = targetType;
            Target = targetObject;
        }

        #region IMemberAccessor Members

        public bool IsValid
        {
            get { return MemberInfo != null; }
        }

        public abstract MemberInfo MemberInfo
        {
            get;
        }

        public object Target { get; set; }

        public Type TargetType
        {
            get;
            private set;
        }

        #endregion

        protected void SetTargetType(Type targetType)
        {
            TargetType = targetType;
        }
    }
}
