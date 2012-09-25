using System;
using System.Linq;
using System.Reflection;
using System.ComponentModel;

namespace Hawkeye.ComponentModel
{
    internal class ModuleInfoConverter : ExpandableObjectConverter
    {
        private class ModuleInfoPropertyDescriptor : TypeConverter.SimplePropertyDescriptor
        {
            private string propname = string.Empty;
            private Type proptype = null;

            /// <summary>
            /// Initializes a new instance of the <see cref="ModuleInfoPropertyDescriptor" /> class.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <param name="attributes">The attributes.</param>
            /// <param name="type">The type.</param>
            public ModuleInfoPropertyDescriptor(string name, Attribute[] attributes, Type type)
                : base(typeof(IModuleInfo), name, type, attributes)
            {
                propname = name;
                proptype = type;
            }

            /// <summary>
            /// Always read-only!
            /// </summary>
            public override bool IsReadOnly
            {
                get { return true; }
            }

            /// <summary>
            /// When overridden in a derived class, gets the current value of the property on a component.
            /// </summary>
            /// <param name="component">The component with the property for which to retrieve the value.</param>
            /// <returns>
            /// The value of a property for a given component.
            /// </returns>
            public override object GetValue(object component)
            {
                if (component == null) return null;
                var type = component.GetType();

                var prop = type.GetProperty(propname, bindingFlags);
                if (prop == null) return null;

                var m = prop.GetGetMethod();
                if (m == null) return null;

                return m.Invoke(component, null);
            }

            /// <summary>
            /// Not supported.
            /// </summary>
            /// <param name="component"></param>
            /// <param name="value"></param>
            public override void SetValue(object component, object value)
            {
                throw new NotSupportedException();
            }
        }

        private const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            var type = typeof(IModuleInfo);
            var descriptors = type.GetProperties(bindingFlags)
                .Select(info =>
                    new ModuleInfoPropertyDescriptor(
                        info.Name,
                        info.GetCustomAttributes(false)
                            .Cast<Attribute>()
                            .ToArray(),
                        info.PropertyType))
                .ToArray();

            return new PropertyDescriptorCollection(descriptors);
        }

        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
    }
}
