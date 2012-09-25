using System;
using System.Reflection;
using System.ComponentModel;

namespace Hawkeye.ComponentModel
{
    internal class RealPropertyDescriptor : BasePropertyDescriptor
    {
        private readonly PropertyInfo pinfo;
        private readonly Type type;
        private readonly object component;
        private string criticalGetError = null;
        private string criticalSetError = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="RealPropertyDescriptor" /> class.
        /// </summary>
        /// <param name="theObject">The component.</param>
        /// <param name="pinfo">The property info.</param>
        /// <param name="owner">The owner.</param>
        public RealPropertyDescriptor(object theObject, Type ownerType, PropertyInfo propertyInfo)
            : base(propertyInfo.Name)
        {
            pinfo = propertyInfo;
            type = ownerType;
            component = theObject;
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether this property is read-only.
        /// </summary>
        /// <returns>true if the property is read-only; otherwise, false.
        ///   </returns>
        public override bool IsReadOnly
        {
            get { return !pinfo.CanWrite; }
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
            get { return pinfo.PropertyType; }
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
                return pinfo.PropertyType == typeof(object) ?
                    typeof(string) : pinfo.PropertyType;
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
            return pinfo.Get(component, ref criticalGetError);
        }

        public override void SetValue(object component, object value)
        {
            value = value.GetInnerObject(); // Make sure we are affecting a real object.
            var result = pinfo.Set(component, value, ref criticalSetError);

            //if (WarningsHelper.Instance.SetPropertyWarning(propInfo.DeclaringType.Name, value))
            //{
            //    //LoggingSystem.Instance.LogSet(propInfo.DeclaringType.Name, propInfo.Name, value);
            //    CodeChangeLoggingSystem.Instance.LogSet(HawkeyeUtils.GetControlName(component), propInfo.Name, value);
            //    propInfo.SetValue(component, value, new object[] { });
            //}
        }

        /// <summary>
        /// Adds the attributes of the <see cref="T:System.ComponentModel.PropertyDescriptor" /> 
        /// to the specified list of attributes in the parent class.
        /// </summary>
        /// <param name="attributeList">An <see cref="T:System.Collections.IList" /> 
        /// that lists the attributes in the parent class. Initially, this is empty.</param>
        protected override void FillAttributes(System.Collections.IList attributeList)
        {
            base.FillAttributes(attributeList);
            attributeList.Add(new CategoryAttribute("(instance: " + type.Name + ")"));
            //AttributeUtils.AddAllAttributes(attributeList, propertyInfo, true);
            //AttributeUtils.DeleteNonRelevatAttributes(attributeList);
        }
    }
}
