using System;
using System.Reflection;
using System.ComponentModel;

namespace Hawkeye.ComponentModel
{
    internal abstract class BaseEventPropertyDescriptor : BaseMemberPropertyDescriptor
    {
        private readonly Type type;
        private readonly EventInfo einfo;

        public BaseEventPropertyDescriptor(Type objectType, EventInfo eventInfo)
            : base(eventInfo, eventInfo.Name)
        {
            type = objectType;
            einfo = eventInfo;
        }

        protected EventInfo EventInfo
        {
            get { return einfo; }
        }

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
    }
}
