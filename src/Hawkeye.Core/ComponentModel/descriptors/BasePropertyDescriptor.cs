using System.ComponentModel;

namespace Hawkeye.ComponentModel
{
    internal abstract class BasePropertyDescriptor : PropertyDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BasePropertyDescriptor" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        protected BasePropertyDescriptor(string name) :
			base(name, null) { }

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
        public override bool CanResetValue(object component)//TODO
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
    }
}
