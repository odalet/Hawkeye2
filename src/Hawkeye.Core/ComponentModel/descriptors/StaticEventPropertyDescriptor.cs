using System;
using System.Reflection;
using System.Collections;
using System.ComponentModel;

namespace Hawkeye.ComponentModel
{
    internal class StaticEventPropertyDescriptor : BaseEventPropertyDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StaticEventPropertyDescriptor"/> class.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="eventInfo">The event information.</param>
        public StaticEventPropertyDescriptor(Type objectType, EventInfo eventInfo)
            : base(objectType, eventInfo) { }

        /// <summary>
        /// Adds the attributes of the <see cref="T:System.ComponentModel.PropertyDescriptor" /> to the specified list of attributes in the parent class.
        /// </summary>
        /// <param name="attributeList">An <see cref="T:System.Collections.IList" /> that lists the attributes in the parent class. Initially, this is empty.</param>
        protected override void FillAttributes(IList attributeList)
        {
            base.FillAttributes(attributeList);

            attributeList.Add(new CategoryAttribute("(static: " + base.ComponentType.Name + ")"));
            attributeList.Add(new RefreshPropertiesAttribute(RefreshProperties.Repaint)); ;
        }
    }
}
