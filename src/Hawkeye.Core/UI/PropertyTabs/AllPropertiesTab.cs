using System;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms.Design;

namespace Hawkeye.UI.PropertyTabs
{
    internal class AllPropertiesTab : PropertyTab
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
            get { return Properties.Resources.Properties; }
        }

        /// <summary>
        /// Gets the name for the property tab.
        /// </summary>
        /// <returns>
        /// The name for the property tab.
        ///   </returns>
        public override string TabName
        {
            get { return "All Properties"; }
        }

        /// <summary>
        /// Gets the properties of the specified component that match the specified attributes.
        /// </summary>
        /// <param name="component">The component to retrieve properties from.</param>
        /// <param name="attributes">An array of type <see cref="T:System.Attribute" /> that indicates the attributes of the properties to retrieve.</param>
        /// <returns>
        /// A <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> that contains the properties.
        /// </returns>
        public override PropertyDescriptorCollection GetProperties(object component, Attribute[] attributes)
        {
            return GetProperties(null, component, attributes);
        }

        /// <summary>
        /// Gets the properties of the specified component that match the specified attributes and context.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that indicates the context to retrieve properties from.</param>
        /// <param name="component">The component to retrieve properties from.</param>
        /// <param name="attributes">An array of type <see cref="T:System.Attribute" /> that indicates the attributes of the properties to retrieve.</param>
        /// <returns>
        /// A <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> that contains the properties matching the specified context and attributes.
        /// </returns>
        public override PropertyDescriptorCollection GetProperties(
            ITypeDescriptorContext context, object component, Attribute[] attributes)
        {
            var collection = context.GetAllProperties(component, new Attribute[] { });
            //PropertyDescriptorCollection propsCollection = DescriptorUtils.GetAllProperties(context, component, attributes);
            //return DescriptorUtils.RemapComponent(propsCollection, component, component, null, null);
            return collection;
        }
    }
}
