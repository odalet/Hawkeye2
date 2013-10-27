using System;
using System.Reflection;
using System.Collections;
using System.ComponentModel;

namespace Hawkeye.ComponentModel
{
    internal class InstanceEventPropertyDescriptor : BaseEventPropertyDescriptor
    {
        private readonly object component;
        private bool keepOriginalCategory = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="InstanceEventPropertyDescriptor" /> class.
        /// </summary>
        /// <param name="instance">The component instance.</param>
        /// <param name="ownerType">Type of the owner.</param>
        /// <param name="eventInfo">The event information.</param>
        /// <param name="keepOriginalCategoryAttribute">if set to <c>true</c> [keep original category attribute].</param>
        public InstanceEventPropertyDescriptor(object instance, Type ownerType, EventInfo eventInfo, bool keepOriginalCategoryAttribute = true)
            : base(ownerType, eventInfo)
        {
            component = instance;
            keepOriginalCategory = keepOriginalCategoryAttribute;
        }

        protected override void FillAttributes(IList attributeList)
        {
            base.FillAttributes(attributeList);
            if (!keepOriginalCategory)
                attributeList.Add(new CategoryAttribute("(instance: " + base.ComponentType.Name + ")"));
        }

        protected override bool IsFiltered(Attribute attribute)
        {
            return !keepOriginalCategory && attribute is CategoryAttribute;
        }
    }
}
