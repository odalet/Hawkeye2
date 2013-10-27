using System.Reflection;

namespace Hawkeye.ComponentModel
{
    internal abstract class BasePropertyPropertyDescriptor : BaseMemberPropertyDescriptor
    {
        private readonly PropertyInfo pinfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasePropertyPropertyDescriptor"/> class.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        public BasePropertyPropertyDescriptor(PropertyInfo propertyInfo) : base(propertyInfo, propertyInfo.Name)
        {
            pinfo = propertyInfo;
        }

        protected PropertyInfo PropertyInfo
        {
            get { return pinfo; }
        }
    }
}
