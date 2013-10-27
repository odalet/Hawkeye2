using System;
using System.Drawing;
using System.ComponentModel;

using Hawkeye.ComponentModel;

namespace Hawkeye.UI.PropertyTabs
{
    internal class AllPropertiesTab : BasePropertyTab
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AllPropertiesTab" /> class.
        /// </summary>
        public AllPropertiesTab() : base() { }
        
        /// <summary>
        /// Gets the bitmap that is displayed for the <see cref="T:System.Windows.Forms.Design.PropertyTab" />.
        /// </summary>
        /// <returns>
        /// The <see cref="T:System.Drawing.Bitmap" /> to display for the <see cref="T:System.Windows.Forms.Design.PropertyTab" />.
        ///   </returns>
        ///   <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   </PermissionSet>
        public override Bitmap Bitmap
        {
            get { return Properties.Resources.AllProperties; }
        }

        /// <summary>
        /// Gets the name for the property tab.
        /// </summary>
        /// <returns>
        /// The name for the property tab.
        ///   </returns>
        public override string TabName
        {
            get { return "3. All Properties"; }
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
        protected override PropertyDescriptorCollection GetAllProperties(
            ITypeDescriptorContext context, object component, Attribute[] attributes)
        {
            return context.GetAllProperties(
                component,
                inspectBaseClasses: true,
                retrieveStaticMembers: true,
                keepOriginalCategoryAttribute: false);
        }

        /// <summary>
        /// Adds pseudo-properties for the specified component.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" />
        /// that indicates the context to retrieve properties from.</param>
        /// <param name="component">The component to retrieve properties from.</param>
        /// <param name="attributes">An array of type <see cref="T:System.Attribute" /> that indicates the
        /// attributes of the properties to retrieve.</param>
        /// <param name="originalCollection">The original property descriptor collection.</param>
        /// <returns>
        /// A <see cref="T:System.ComponentModel.PropertyDescriptorCollection" />
        /// that contains the properties matching the specified context and attributes.
        /// </returns>
        protected override PropertyDescriptorCollection AddPseudoProperties(
            ITypeDescriptorContext context, object component, Attribute[] attributes, 
            PropertyDescriptorCollection originalCollection)
        {
            originalCollection.Add(new TypePropertyDescriptor(component));
            return originalCollection;
        }
    }   
}
