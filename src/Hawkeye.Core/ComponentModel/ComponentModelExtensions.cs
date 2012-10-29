using System;
using System.Linq;
using System.Security;
using System.Reflection;
using System.ComponentModel;
using System.Collections.Generic;

using Hawkeye.Logging;

namespace Hawkeye.ComponentModel
{
    internal static class ComponentModelExtensions
    {
        private static ILogService log = LogManager.GetLogger(typeof(ComponentModelExtensions));

        private static readonly string[] excludedProperties = new[]
        {
            "System.Windows.Forms.Control.ShowParams",
            "System.Windows.Forms.Control.ActiveXAmbientBackColor",
            "System.Windows.Forms.Control.ActiveXAmbientFont",
            "System.Windows.Forms.Control.ActiveXAmbientForeColor",
            "System.Windows.Forms.Control.ActiveXEventsFrozen",
            "System.Windows.Forms.Control.ActiveXHWNDParent",
            "System.Windows.Forms.Control.ActiveXInstance",
            "System.Windows.Forms.Form.ShowParams",
        };

        private static readonly BindingFlags instanceFlags =
            BindingFlags.Instance | BindingFlags.InvokeMethod |
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;

        private static readonly BindingFlags staticFlags =
            BindingFlags.Static | BindingFlags.InvokeMethod |
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;

        #region ITypeDescriptorContext extensions

        public static PropertyDescriptorCollection GetAllProperties(
            this ITypeDescriptorContext context,
            object component,
            Attribute[] attributes,
            bool inspectBaseClasses = true)
        {
            if (component == null || component.GetType().IsPrimitive || component is string)
                return new PropertyDescriptorCollection(new PropertyDescriptor[] { });
            
            // Make sure we are inspecting the real component
            component = component.GetInnerObject();
            
            var type = component.GetType();

            var allprops = new Dictionary<string, PropertyDescriptor>();

            Action<PropertyInfo, bool> addPropertyDescriptor = (pi, isStatic) =>
            {
                try
                {
                    if (allprops.ContainsKey(pi.Name)) return;

                    var fullName = pi.DeclaringType.FullName + "." + pi.Name;
                    if (excludedProperties.Contains(fullName)) return;

                    if (isStatic) allprops.Add(pi.Name, new StaticPropertyDescriptor(type, pi));
                    else allprops.Add(pi.Name, new RealPropertyDescriptor(component, type, pi));
                }
                catch (Exception ex)
                {
                    log.Error(string.Format(
                        "Could not convert a property info into a property descriptor: {0}", ex.Message), ex);
                }
            };

            int depth = 1;
            do
            {
                foreach (var pi in type.GetProperties(instanceFlags))
                    addPropertyDescriptor(pi, false);

                foreach (var pi in type.GetProperties(staticFlags))
                    addPropertyDescriptor(pi, true);

                if (type == typeof(object) || !inspectBaseClasses)
                    break;
                type = type.BaseType;
                depth++;
            }
            while (true);
            
            return new PropertyDescriptorCollection(allprops.Values.ToArray());

            //PropertyDescriptorCollection properties;
            //PropertyDescriptorCollection childElements = null;

            ////Attribute[] attributeArray1 = new Attribute[] { };
            ////attributes = attributeArray1; // replace the attribute array to allow all properties to be visible.

            //if (component is IEnumerable)
            //    childElements = ConverterUtils.GetEnumerableChildsAsProperties(context, component, attributes);

            //if (context == null)
            //{
            //    //properties = TypeDescriptor.GetProperties(component, attributes);
            //    properties = GetAllPropertiesCustom(component, attributes);
            //}
            //else
            //{
            //    RemapPropertyDescriptor remapDescriptor = context.PropertyDescriptor as RemapPropertyDescriptor;
            //    if (remapDescriptor != null)
            //    {
            //        component = remapDescriptor.OriginalComponent;
            //    }

            //    TypeConverter converter1 = (context.PropertyDescriptor == null) ? 
            //        TypeDescriptor.GetConverter(component) : context.PropertyDescriptor.Converter;
            //    if ((converter1 != null) && converter1.GetPropertiesSupported(context))
            //    {
            //        //					properties = converter1.GetProperties(context, component, attributes);
            //        //					if ( properties == null )
            //        properties = GetAllPropertiesCustom(component, attributes);
            //    }
            //    else
            //    {
            //        properties = GetAllPropertiesCustom(component, attributes);
            //    }
            //}

            //if (childElements != null && childElements.Count > 0)
            //    properties = MergeProperties(properties, childElements);

            //return properties;
        }

        #endregion

        #region PropertyInfo extensions

        public static object Get(this PropertyInfo pinfo, object target, ref string criticalError)
        {
            if (!string.IsNullOrEmpty(criticalError))
                return criticalError;

            try
            {
                var get = pinfo.GetGetMethod(true);
                if (get != null)
                    return get.Invoke(target, new object[] { });
                else criticalError = "No Get Method.";
            }
            catch (SecurityException ex)
            {
                criticalError = ex.Message;
            }
            catch (TargetParameterCountException ex)
            {
                criticalError = ex.Message;
            }
            catch (TargetInvocationException ex)
            {
                criticalError = ex.InnerException.Message;
            }
            return criticalError;
        }

        public static object Set(this PropertyInfo pinfo, object target, object value, ref string criticalError)
        {
            if (!string.IsNullOrEmpty(criticalError))
                return criticalError;

            try
            {
                var set = pinfo.GetSetMethod(true);
                if (set != null)
                    return set.Invoke(target, new object[] { value });
                else criticalError = "No Set Method.";
            }
            catch (SecurityException ex)
            {
                criticalError = ex.Message;
            }
            catch (TargetParameterCountException ex)
            {
                criticalError = ex.Message;
            }
            catch (TargetInvocationException ex)
            {
                criticalError = ex.InnerException.Message;
            }
            return criticalError;
        }

        #endregion
    }
}
