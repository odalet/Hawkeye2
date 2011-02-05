using System;
using System.Linq;
using System.Reflection;
using System.ComponentModel;

namespace FxDetector
{
    /// <summary>
    /// A <see cref="System.ComponentModel.TypeConverter"/> allowing to display objects of type
    /// <see cref="FxDetector.NativeMethods.MODULEENTRY32"/> in a <see cref="System.Windows.Forms.PropertyGrid"/>.
    /// </summary>
    internal class ModuleEntryConverter : ExpandableObjectConverter
    {
        private class ModuleEntryFieldDescriptor : TypeConverter.SimplePropertyDescriptor
        {
            private string fieldname = string.Empty;
            private Type fieldtype = null;

            public ModuleEntryFieldDescriptor(string name, Attribute[] attributes, Type type)
                : base(typeof(NativeMethods.MODULEENTRY32), name, type, attributes)
            {
                fieldname = name;
                fieldtype = type;
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

                var field = type.GetField(fieldname, bindingFlags);
                if (field == null) return null;

                return field.GetValue(component);
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
            var type = typeof(NativeMethods.MODULEENTRY32);
            var descriptors = type.GetFields(bindingFlags)
                .Select(info =>
                    new ModuleEntryFieldDescriptor(
                        info.Name,
                        info.GetCustomAttributes(false)
                            .Cast<Attribute>()
                            .ToArray(),
                        info.FieldType))
                .ToArray();

            return new PropertyDescriptorCollection(descriptors);
        }

        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
    }
}
