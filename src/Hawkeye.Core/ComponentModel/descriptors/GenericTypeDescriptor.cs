using System;
using System.ComponentModel;

using Hawkeye.UI;

namespace Hawkeye.ComponentModel
{
    internal class GenericTypeDescriptor : ICustomTypeDescriptor
    {
        private Type type = null;
        private ICustomTypeDescriptor parentDescriptor = null;

        public GenericTypeDescriptor(Type objectType, ICustomTypeDescriptor parent)
        {
            if (objectType == null) throw new ArgumentNullException("objectType");
            if (parent == null) throw new ArgumentNullException("parent");
            type = objectType;
            parentDescriptor = parent;
        }

        #region ICustomTypeDescriptor Members

        public AttributeCollection GetAttributes()
        {
            return parentDescriptor.GetAttributes();
        }

        public string GetClassName()
        {
            return parentDescriptor.GetClassName();
        }

        public string GetComponentName()
        {
            return parentDescriptor.GetComponentName();
        }

        public TypeConverter GetConverter()
        {
            return parentDescriptor.GetConverter();
        }

        public EventDescriptor GetDefaultEvent()
        {
            return parentDescriptor.GetDefaultEvent();
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            return parentDescriptor.GetDefaultProperty();
        }

        public object GetEditor(Type editorBaseType)
        {
            // Return nothing while we have nothing to show in UI.GenericComponentEditor
            return null; 

            ////var editor = parentDescriptor.GetEditor(editorBaseType);
            ////if (editor == null && editorBaseType == typeof(ComponentEditor)) 
            ////    editor = new GenericComponentEditor();
            //////else if (editor == null && editorBaseType == typeof(UITypeEditor)) 
            //////    editor = new GenericUITypeEditor();
            ////return editor;
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return parentDescriptor.GetEvents(attributes);
        }

        public EventDescriptorCollection GetEvents()
        {
            return parentDescriptor.GetEvents();
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            return parentDescriptor.GetProperties(attributes);
        }

        public PropertyDescriptorCollection GetProperties()
        {
            return parentDescriptor.GetProperties();
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return parentDescriptor.GetPropertyOwner(pd);
        }

        #endregion
    }
}
