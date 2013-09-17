using System;
using System.Reflection;
using System.Collections;
using System.ComponentModel;

namespace Hawkeye.ComponentModel
{
    internal class InstanceEventPropertyDescriptor : BaseEventPropertyDescriptor
    {
        private readonly object component;

        /// <summary>
        /// Initializes a new instance of the <see cref="InstanceEventPropertyDescriptor" /> class.
        /// </summary>
        /// <param name="instance">The component instance.</param>
        /// <param name="ownerType">Type of the owner.</param>
        /// <param name="eventInfo">The event information.</param>
        public InstanceEventPropertyDescriptor(object instance, Type ownerType, EventInfo eventInfo)
            : base(ownerType, eventInfo)
        {
            component = instance;
        }

        /// <summary>
        /// Adds the attributes of the <see cref="T:System.ComponentModel.PropertyDescriptor" /> to the specified list of attributes in the parent class.
        /// </summary>
        /// <param name="attributeList">An <see cref="T:System.Collections.IList" /> that lists the attributes in the parent class. Initially, this is empty.</param>
        protected override void FillAttributes(IList attributeList)
        {
            base.FillAttributes(attributeList);
            attributeList.Add(new CategoryAttribute("(instance: " + base.ComponentType.Name + ")"));
        }
    }
}
