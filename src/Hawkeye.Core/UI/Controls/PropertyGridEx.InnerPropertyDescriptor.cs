//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.ComponentModel;
//using System.Resources;
//using System.Reflection;

//namespace Hawkeye.UI.Controls
//{
//    partial class PropertyGridEx
//    {
//        protected class InnerPropertyDescriptor : PropertyDescriptor
//        {
//            private bool shouldProtectConnectionStrings = true;
//            private const string connectionStringPropertyName = "ConnectionString";

//            private static Dictionary<string, ResourceManager> resourceManagerCache =
//                new Dictionary<string, ResourceManager>();

//            private Assembly ownerObjectAssembly = null;
//            private PropertyDescriptor property = null;

//            private string name = string.Empty;
//            private string description = string.Empty;
//            private string category = string.Empty;

//            public InnerPropertyDescriptor(object ownerObject, PropertyDescriptor baseProperty)
//                : this(ownerObject, baseProperty, true) { }

//            public InnerPropertyDescriptor(object ownerObject, PropertyDescriptor baseProperty, bool protectConnectionStrings)
//                : base(baseProperty)
//            {
//                if (ownerObject == null) throw new ArgumentNullException("ownerObject");
//                if (baseProperty == null) throw new ArgumentNullException("baseProperty");

//                property = baseProperty;
//                ownerObjectAssembly = ownerObject.GetType().Assembly;
//            }

//            #region Overrides

//            /// <summary>
//            /// When overridden in a derived class, returns whether resetting an object changes its value.
//            /// </summary>
//            /// <param name="component">The component to test for reset capability.</param>
//            /// <returns>
//            /// true if resetting the component changes its value; otherwise, false.
//            /// </returns>
//            public override bool CanResetValue(object component) { return property.CanResetValue(component); }

//            /// <summary>
//            /// When overridden in a derived class, gets the type of the component this property is bound to.
//            /// </summary>
//            /// <value></value>
//            /// <returns>
//            /// A <see cref="T:System.Type"/> that represents the type of component this property is bound to. When the <see cref="M:System.ComponentModel.PropertyDescriptor.GetValue(System.Object)"/> or <see cref="M:System.ComponentModel.PropertyDescriptor.SetValue(System.Object,System.Object)"/> methods are invoked, the object specified might be an instance of this type.
//            /// </returns>
//            public override Type ComponentType { get { return property.ComponentType; } }

//            /// <summary>
//            /// When overridden in a derived class, gets the current value of the property on a component.
//            /// </summary>
//            /// <param name="component">The component with the property for which to retrieve the value.</param>
//            /// <returns>
//            /// The value of a property for a given component.
//            /// </returns>
//            public override object GetValue(object component)
//            {
//                if (shouldProtectConnectionStrings &&
//                    BasePropertyDescriptor.Name == connectionStringPropertyName)
//                {
//                    var original = BaseGetValue(component);
//                    if ((original != null) && (original is string))
//                        return ((string)original).ToStringNoPassword(true);
//                    else return original;
//                }
//                else return BaseGetValue(component);
//            }

//            /// <summary>
//            /// When overridden in a derived class, gets a value indicating whether this property is read-only.
//            /// </summary>
//            /// <value></value>
//            /// <returns>true if the property is read-only; otherwise, false.
//            /// </returns>
//            public override bool IsReadOnly
//            {
//                get
//                {
//                    if (shouldProtectConnectionStrings &&
//                        BasePropertyDescriptor.Name == connectionStringPropertyName) return true;
//                    else return property.IsReadOnly;
//                }
//            }

//            /// <summary>
//            /// When overridden in a derived class, gets the type of the property.
//            /// </summary>
//            /// <value></value>
//            /// <returns>
//            /// A <see cref="T:System.Type"/> that represents the type of the property.
//            /// </returns>
//            public override Type PropertyType 
//            {
//                get { return property.PropertyType; } 
//            }

//            /// <summary>
//            /// When overridden in a derived class, resets the value for this property of the component to the default value.
//            /// </summary>
//            /// <param name="component">The component with the property value that is to be reset to the default value.</param>
//            public override void ResetValue(object component)
//            {
//                property.ResetValue(component); 
//            }

//            /// <summary>
//            /// When overridden in a derived class, sets the value of the component to a different value.
//            /// </summary>
//            /// <param name="component">The component with the property value that is to be set.</param>
//            /// <param name="value">The new value.</param>
//            public override void SetValue(object component, object value)
//            {
//                property.SetValue(component, value); 
//            }

//            /// <summary>
//            /// When overridden in a derived class, determines a value indicating whether the value of this property needs to be persisted.
//            /// </summary>
//            /// <param name="component">The component with the property to be examined for persistence.</param>
//            /// <returns>
//            /// true if the property should be persisted; otherwise, false.
//            /// </returns>
//            public override bool ShouldSerializeValue(object component) 
//            {
//                return property.ShouldSerializeValue(component); 
//            }

//            ///// <summary>
//            ///// Gets the description of the member, as specified in the <see cref="T:System.ComponentModel.DescriptionAttribute"/>.
//            ///// </summary>
//            ///// <value></value>
//            ///// <returns>
//            ///// The description of the member. If there is no <see cref="T:System.ComponentModel.DescriptionAttribute"/>, the property value is set to the default, which is an empty string ("").
//            ///// </returns>
//            //public override string Description
//            //{
//            //    get
//            //    {
//            //        if (string.IsNullOrEmpty(description)) description = BuildDescription();
//            //        return description;
//            //    }
//            //}

//            ///// <summary>
//            ///// Gets the name of the category to which the member belongs, as specified in the <see cref="T:System.ComponentModel.CategoryAttribute"/>.
//            ///// </summary>
//            ///// <value></value>
//            ///// <returns>
//            ///// The name of the category to which the member belongs. If there is no <see cref="T:System.ComponentModel.CategoryAttribute"/>, the category name is set to the default category, Misc.
//            ///// </returns>
//            ///// <PermissionSet>
//            ///// 	<IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/>
//            ///// </PermissionSet>
//            //public override string Category
//            //{
//            //    get
//            //    {
//            //        if (string.IsNullOrEmpty(category)) category = BuildCategory();
//            //        return category;
//            //    }
//            //}

//            ///// <summary>
//            ///// Gets the name that can be displayed in a window, such as a Properties window.
//            ///// </summary>
//            ///// <value></value>
//            ///// <returns>
//            ///// The name to display for the member.
//            ///// </returns>
//            //public override string DisplayName
//            //{
//            //    get
//            //    {
//            //        if (string.IsNullOrEmpty(name)) name = BuildName();
//            //        return name;
//            //    }
//            //}

//            #endregion

//            /// <summary>
//            /// Gets the base property descriptor.
//            /// </summary>
//            /// <value>The base property descriptor.</value>
//            protected PropertyDescriptor BasePropertyDescriptor
//            {
//                get { return property; }
//            }

//            ///// <summary>Builds the display name.</summary>
//            ///// <returns>Display name.</returns>
//            //protected virtual string BuildName()
//            //{
//            //    var name = property.DisplayName;
//            //    foreach (Attribute attribute in property.Attributes)
//            //    {
//            //        // If we find a SRName attribute, we don't want to continue searching. 
//            //        // We also override an existing Name attribute.
//            //        if (attribute is SRNameAttribute)
//            //        {
//            //            string resName = ((SRNameAttribute)attribute).ResourceName;
//            //            var result = ResourceLocator.GetString(resName, ownerObjectAssembly);
//            //            if (!string.IsNullOrEmpty(result))
//            //            {
//            //                name = result;
//            //                break;
//            //            }
//            //        }

//            //        if (attribute is NameAttribute)
//            //            name = ((NameAttribute)attribute).Name;
//            //    }

//            //    return name;
//            //}

//            ///// <summary>Builds the description.</summary>
//            ///// <returns>Description.</returns>
//            //protected virtual string BuildDescription()
//            //{
//            //    var description = property.Description;
//            //    foreach (Attribute attribute in property.Attributes)
//            //    {
//            //        // If we find a SRName attribute, we override an existing Description attribute.
//            //        if (attribute is SRDescriptionAttribute)
//            //        {
//            //            string resName = ((SRDescriptionAttribute)attribute).ResourceName;
//            //            description = ResourceLocator.GetString(resName, ownerObjectAssembly);
//            //            if (!string.IsNullOrEmpty(description)) break;
//            //        }
//            //    }

//            //    return description;
//            //}

//            ///// <summary>Builds the category.</summary>
//            ///// <returns>Category.</returns>
//            //protected virtual string BuildCategory()
//            //{
//            //    var category = property.Category;
//            //    foreach (Attribute attribute in property.Attributes)
//            //    {
//            //        // If we find a SRName attribute, we override an existing Category attribute.
//            //        if (attribute is SRCategoryAttribute)
//            //        {
//            //            string resName = ((SRCategoryAttribute)attribute).ResourceName;
//            //            category = ResourceLocator.GetString(resName, ownerObjectAssembly);
//            //            if (!string.IsNullOrEmpty(category)) break;
//            //        }
//            //    }

//            //    return category;
//            //}

//            private object BaseGetValue(object component)
//            {
//                return property.GetValue(component);
//            }
//        }
//    }
//}
