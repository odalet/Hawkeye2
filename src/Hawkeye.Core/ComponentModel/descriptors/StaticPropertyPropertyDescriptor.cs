using System;
using System.Reflection;
using System.Collections;
using System.ComponentModel;

namespace Hawkeye.ComponentModel
{
    internal class StaticPropertyPropertyDescriptor : BasePropertyPropertyDescriptor
    {
        private readonly Type type;
        private string criticalGetError = null;
        private string criticalSetError = null;
        private bool keepOriginalCategory = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="StaticPropertyPropertyDescriptor" /> class.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="propertyInfo">The property info.</param>
        /// <param name="keepOriginalCategoryAttribute">if set to <c>true</c> [keep original category attribute].</param>
        public StaticPropertyPropertyDescriptor(Type objectType, PropertyInfo propertyInfo, bool keepOriginalCategoryAttribute = true)
            : base(propertyInfo)
        {
            type = objectType;
            keepOriginalCategory = keepOriginalCategoryAttribute;
        }

        public override bool CanResetValue(object component) { return false; } //TODO: why should this be false?

        public override Type ComponentType
        {
            get { return type; }
        }

        public override object GetValue(object component)
        {
            if (!base.PropertyInfo.CanRead) return null;
            return base.PropertyInfo.Get(null, ref criticalGetError);
        }

        public override bool IsReadOnly
        {
            get { return !base.PropertyInfo.CanWrite; }
        }

        public override Type PropertyType
        {
            get { return base.PropertyInfo.PropertyType; }
        }

        public override void ResetValue(object component) { }

        public override void SetValue(object component, object value)
        {
            value = value.GetInnerObject(); // Make sure we are affecting a real object.
            var result = base.PropertyInfo.Set(component, value, ref criticalSetError);

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
            if (!keepOriginalCategory)
                attributeList.Add(new CategoryAttribute("(static: " + type.Name + ")"));
        }

        protected override bool IsFiltered(Attribute attribute)
        {
            return !keepOriginalCategory && attribute is CategoryAttribute;
        }
    }
}
