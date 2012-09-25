using System;
using System.Reflection;
using System.Collections;
using System.ComponentModel;

namespace Hawkeye.ComponentModel
{
    internal class StaticPropertyDescriptor : PropertyDescriptor
    {
        private readonly Type type;
        private readonly PropertyInfo pinfo;
        private string criticalGetError = null;
        private string criticalSetError = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="StaticPropertyDescriptor" /> class.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="propertyInfo">The property info.</param>
        public StaticPropertyDescriptor(Type objectType, PropertyInfo propertyInfo)
            : base(propertyInfo.Name, null)
        {
            type = objectType;
            pinfo = propertyInfo;
        }

        public override bool CanResetValue(object component) { return false; } //TODO: why should this be false?

        public override Type ComponentType
        {
            get { return type; }
        }

        public override object GetValue(object component)
        {
            return pinfo.Get(null, ref criticalGetError);
        }

        public override bool IsReadOnly
        {
            get { return !pinfo.CanWrite; }
        }

        public override Type PropertyType
        {
			get { return pinfo.PropertyType; }
        }

        public override void ResetValue(object component) { }

        public override void SetValue(object component, object value)
        {
            value = value.GetInnerObject(); // Make sure we are affecting a real object.
            var result = pinfo.Set(component, value, ref criticalSetError);

            //if ( WarningsHelper.Instance.SetPropertyWarning(propInfo.DeclaringType.Name, value) )
            //{
            //    CodeChangeLoggingSystem.Instance.LogSet(propInfo.DeclaringType.Name, propInfo.Name, value);
            //    propInfo.SetValue(null, value, new object[] {});
            //}
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }

        protected override void FillAttributes(IList attributeList)
        {
            base.FillAttributes(attributeList);

            attributeList.Add(new CategoryAttribute("(static: " + type.Name + ")"));
            attributeList.Add(new RefreshPropertiesAttribute(RefreshProperties.Repaint)); ;
        }
    }
}
