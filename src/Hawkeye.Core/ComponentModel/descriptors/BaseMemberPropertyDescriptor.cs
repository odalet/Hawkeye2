using System.Reflection;
using System.Collections;
using System.ComponentModel;
using System;

namespace Hawkeye.ComponentModel
{
    internal abstract class BaseMemberPropertyDescriptor : PropertyDescriptor
    {
        private readonly MemberInfo minfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseMemberPropertyDescriptor" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        protected BaseMemberPropertyDescriptor(MemberInfo info, string name) :
			base(name, null) 
        {
            minfo = info;
        }

        protected MemberInfo MemberInfo
        {
            get { return minfo; }
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether this property is read-only.
        /// </summary>
        /// <returns>true if the property is read-only; otherwise, false.
        ///   </returns>
        public override bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// When overridden in a derived class, returns whether resetting an object changes its value.
        /// </summary>
        /// <param name="component">The component to test for reset capability.</param>
        /// <returns>
        /// true if resetting the component changes its value; otherwise, false.
        /// </returns>
        public override bool CanResetValue(object component) //TODO: why should this be false?
        {
            return false;
        }

        /// <summary>
        /// When overridden in a derived class, resets the value for this property of the component to the default value.
        /// </summary>
        /// <param name="component">The component with the property value that is to be reset to the default value.</param>
        public override void ResetValue(object component) { }

        /// <summary>
        /// When overridden in a derived class, sets the value of the component to a different value.
        /// </summary>
        /// <param name="component">The component with the property value that is to be set.</param>
        /// <param name="value">The new value.</param>
        public override void SetValue(object component, object value) { }

        /// <summary>
        /// When overridden in a derived class, determines a value indicating whether the value of this property needs to be persisted.
        /// </summary>
        /// <param name="component">The component with the property to be examined for persistence.</param>
        /// <returns>
        /// true if the property should be persisted; otherwise, false.
        /// </returns>
        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }

        protected override void FillAttributes(IList attributeList)
        {
            base.FillAttributes(attributeList);

            var customAttributes = MemberInfo.GetCustomAttributes(true);
            foreach (var attribute in customAttributes)
            {
                if (!(attribute is Attribute)) continue;
                if (!IsFiltered((Attribute)attribute))
                    attributeList.Add(attribute);
            }
        }
        
        protected virtual bool IsFiltered(Attribute attribute)
        {
            return false;
        }
    }
}
