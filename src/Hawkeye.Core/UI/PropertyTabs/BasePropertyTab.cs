using System;
using System.ComponentModel;
using System.Windows.Forms.Design;

namespace Hawkeye.UI.PropertyTabs
{
    internal abstract class BasePropertyTab : PropertyTab
    {
        public override PropertyDescriptorCollection GetProperties(object component, Attribute[] attributes)
        {
            return base.GetProperties(null, component, attributes);
        }

        /// <summary>
        /// Gets all the properties of the specified component that match the specified attributes and context.
        /// </summary>
        /// An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> 
        /// that indicates the context to retrieve properties from.</param>
        /// <param name="component">The component to retrieve properties from.</param>
        /// <param name="attributes">
        /// An array of type <see cref="T:System.Attribute" /> that indicates the 
        /// attributes of the properties to retrieve.</param>
        /// <returns>
        /// A <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> 
        /// that contains the properties matching the specified context and attributes.
        /// </returns>
        protected abstract PropertyDescriptorCollection GetAllProperties(
            ITypeDescriptorContext context, object component, Attribute[] attributes);

        /// <summary>
        /// Gets the properties of the specified component that match the specified attributes and context.
        /// </summary>
        /// <param name="context">
        /// An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> 
        /// that indicates the context to retrieve properties from.</param>
        /// <param name="component">The component to retrieve properties from.</param>
        /// <param name="attributes">
        /// An array of type <see cref="T:System.Attribute" /> that indicates the 
        /// attributes of the properties to retrieve.</param>
        /// <returns>
        /// A <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> 
        /// that contains the properties matching the specified context and attributes.
        /// </returns>
        public override PropertyDescriptorCollection GetProperties(
            ITypeDescriptorContext context, object component, Attribute[] attributes)
        {
            if (attributes == null) attributes = new Attribute[] { };
            var collection = GetAllProperties(context, component, attributes);

            collection = AddPseudoProperties(context, component, attributes, collection);

            return collection;
        }

        /// <summary>
        /// Adds pseudo-properties for the specified component.
        /// </summary>
        /// <param name="context">
        /// An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> 
        /// that indicates the context to retrieve properties from.</param>
        /// <param name="component">The component to retrieve properties from.</param>
        /// <param name="attributes">
        /// An array of type <see cref="T:System.Attribute" /> that indicates the 
        /// attributes of the properties to retrieve.</param>
        /// <param name="originalCollection">The original property descriptor collection.</param>
        /// <returns>
        /// A <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> 
        /// that contains the properties matching the specified context and attributes.
        /// </returns>
        protected virtual PropertyDescriptorCollection AddPseudoProperties(
            ITypeDescriptorContext context, object component, Attribute[] attributes,
            PropertyDescriptorCollection originalCollection)
        {
            return originalCollection;
        }
    }
}
