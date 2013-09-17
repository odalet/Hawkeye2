using System;
using System.Reflection;
using System.ComponentModel;

namespace Hawkeye.ComponentModel
{
    internal abstract class BaseEventPropertyDescriptor : PropertyDescriptor
    {
        private readonly Type type;
        private readonly EventInfo einfo;

        public BaseEventPropertyDescriptor(Type objectType, EventInfo eventInfo)
            : base(eventInfo.Name, null)
        {
            type = objectType;
            einfo = eventInfo;
        }

        public override bool CanResetValue(object component) { return false; }

        public override Type ComponentType
        {
            get { return type; }
        }

        public override object GetValue(object component)
        {
            return null;
        }

        public override bool IsReadOnly
        {
            get { return true; }
        }

        public override Type PropertyType
        {
            get { return einfo.EventHandlerType; }
        }

        public override void ResetValue(object component) { }

        public override void SetValue(object component, object value)
        {
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }
    }
}
