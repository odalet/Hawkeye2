using System;
using System.Reflection;
using System.Collections;
using System.ComponentModel;

namespace Hawkeye.ComponentModel
{
    internal class InstancePropertyPropertyDescriptor : BasePropertyPropertyDescriptor
    {
        private readonly Type type;
        private readonly object component;
        private string criticalGetError = null;
        private string criticalSetError = null;
        private bool keepOriginalCategory = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="InstancePropertyPropertyDescriptor" /> class.
        /// </summary>
        /// <param name="instance">The component instance.</param>
        /// <param name="ownerType">Type of the owner.</param>
        /// <param name="propertyInfo">The property information.</param>
        /// <param name="keepOriginalCategoryAttribute">if set to <c>true</c> [keep original category attribute].</param>
        public InstancePropertyPropertyDescriptor(object instance, Type ownerType, PropertyInfo propertyInfo, bool keepOriginalCategoryAttribute = true)
            : base(propertyInfo)
        {
            type = ownerType;
            component = instance;
            keepOriginalCategory = keepOriginalCategoryAttribute;
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether this property is read-only.
        /// </summary>
        /// <returns>true if the property is read-only; otherwise, false.
        ///   </returns>
        public override bool IsReadOnly
        {
            get { return !base.PropertyInfo.CanWrite; }
        }

        /// <summary>
        /// When overridden in a derived class, gets the type of the component this property is bound to.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Type" /> that represents the type of component this property is bound to. When the <see cref="M:System.ComponentModel.PropertyDescriptor.GetValue(System.Object)" /> or <see cref="M:System.ComponentModel.PropertyDescriptor.SetValue(System.Object,System.Object)" /> methods are invoked, the object specified might be an instance of this type.
        ///   </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override Type ComponentType
        {
            get { return base.PropertyInfo.PropertyType; }
        }

        /// <summary>
        /// When overridden in a derived class, gets the type of the property.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Type" /> that represents the type of the property.
        ///   </returns>
        public override Type PropertyType
        {
            get
            {
                return base.PropertyInfo.PropertyType == typeof(object) ?
                    typeof(string) : base.PropertyInfo.PropertyType;
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets the current value of the property on a component.
        /// </summary>
        /// <param name="component">The component with the property for which to retrieve the value.</param>
        /// <returns>
        /// The value of a property for a given component.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override object GetValue(object component)
        {
            component = component.GetInnerObject(); // Make sure we are working on a real object.
            if (!base.PropertyInfo.CanRead) return null;

            return base.PropertyInfo.Get(component, ref criticalGetError);
        }

        public override void SetValue(object component, object value)
        {
            component = component.GetInnerObject(); // Make sure we are working on a real object0
            value = value.GetInnerObject(); // Make sure we are affecting a real object.
            var result = base.PropertyInfo.Set(component, value, ref criticalSetError);

            //if (WarningsHelper.Instance.SetPropertyWarning(propInfo.DeclaringType.Name, value))
            //{
            //    //LoggingSystem.Instance.LogSet(propInfo.DeclaringType.Name, propInfo.Name, value);
            //    CodeChangeLoggingSystem.Instance.LogSet(HawkeyeUtils.GetControlName(component), propInfo.Name, value);
            //    propInfo.SetValue(component, value, new object[] { });
            //}
        }

        protected override void FillAttributes(IList attributeList)
        {
            base.FillAttributes(attributeList);
            if (!keepOriginalCategory)
                attributeList.Add(new CategoryAttribute("(instance: " + type.Name + ")"));
        }

        protected override bool IsFiltered(Attribute attribute)
        {
            return !keepOriginalCategory && attribute is CategoryAttribute;
        }
    }
}
