using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace Hawkeye.ComponentModel
{
    public static class CustomTypeDescriptors
    {
        private class GenericTypeDescriptionProvider : TypeDescriptionProvider
        {
            private ICustomTypeDescriptor typeDescriptor = null;

            /// <summary>
            /// Initializes a new instance of the <see cref="GenericTypeDescriptionProvider" /> class.
            /// </summary>
            /// <param name="customTypeDescriptor">
            /// The custom type descriptor returned by <see cref="GetTypeDescriptor"/>.
            /// </param>
            /// <exception cref="System.ArgumentNullException">customTypeDescriptor</exception>
            public GenericTypeDescriptionProvider(ICustomTypeDescriptor customTypeDescriptor)
                : base() 
            {
                if (customTypeDescriptor == null) throw new ArgumentNullException("customTypeDescriptor");
                typeDescriptor = customTypeDescriptor;
            }

            /// <summary>
            /// Gets an extended custom type descriptor for the given object.
            /// </summary>
            /// <param name="instance">The object for which to get the extended type descriptor.</param>
            /// <returns>
            /// An <see cref="T:System.ComponentModel.ICustomTypeDescriptor" /> that can provide extended metadata for the object.
            /// </returns>
            public override ICustomTypeDescriptor GetExtendedTypeDescriptor(object instance)
            {
                //TODO: return typeDescriptor?
                return base.GetExtendedTypeDescriptor(instance);
            }
            
            /// <summary>
            /// Gets a custom type descriptor for the given type and object.
            /// </summary>
            /// <param name="objectType">The type of object for which to retrieve the type descriptor.</param>
            /// <param name="instance">
            /// An instance of the type. Can be null if no instance was passed to the 
            /// <see cref="T:System.ComponentModel.TypeDescriptor" />.</param>
            /// <returns>
            /// An <see cref="T:System.ComponentModel.ICustomTypeDescriptor" /> that can provide metadata for the type.
            /// </returns>
            public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
            {
                return typeDescriptor;
            }
        }

        private static Dictionary<Type, TypeDescriptionProvider> providers = 
            new Dictionary<Type, TypeDescriptionProvider>();

        static CustomTypeDescriptors()
        {
            //var type = typeof(IProxy);
            //var proxyProvider = new ProxyDescriptionProvider();
            //TypeDescriptor.AddProvider(proxyProvider, type);
            //providers.Add(type, proxyProvider);
        }

        public static void AddGenericProviderToType(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (providers.ContainsKey(type))
                return;

            var previousProvider = TypeDescriptor.GetProvider(type);            
            var typeDescriptor = new GenericTypeDescriptor(type, previousProvider.GetTypeDescriptor(type));
            var newProvider = new GenericTypeDescriptionProvider(typeDescriptor);

            TypeDescriptor.RemoveProvider(previousProvider, type);
            TypeDescriptor.AddProvider(newProvider, type);
        }
    }
}
