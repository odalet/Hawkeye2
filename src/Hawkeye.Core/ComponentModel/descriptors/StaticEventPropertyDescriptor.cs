using System;
using System.Reflection;
using System.Collections;
using System.ComponentModel;

namespace Hawkeye.ComponentModel
{
    internal class StaticEventPropertyDescriptor : BaseEventPropertyDescriptor
    {
        private bool keepOriginalCategory = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="StaticEventPropertyDescriptor"/> class.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="eventInfo">The event information.</param>
        public StaticEventPropertyDescriptor(Type objectType, EventInfo eventInfo, bool keepOriginalCategoryAttribute = true)
            : base(objectType, eventInfo)
        {
            keepOriginalCategory = keepOriginalCategoryAttribute;
        }

        protected override void FillAttributes(IList attributeList)
        {
            base.FillAttributes(attributeList);
            if (!keepOriginalCategory)
                attributeList.Add(new CategoryAttribute("(static: " + base.ComponentType.Name + ")"));
        }

        protected override bool IsFiltered(Attribute attribute)
        {
            return !keepOriginalCategory && attribute is CategoryAttribute;
        }
    }
}
