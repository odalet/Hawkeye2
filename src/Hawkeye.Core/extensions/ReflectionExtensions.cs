//using System;
//using System.Linq;
//using System.Reflection;
//using System.Collections.Generic;

//namespace Hawkeye
//{
//    internal static class ReflectionExtensions
//    {
//        private static readonly Dictionary<Type, object> relevantAttributes;

//        static ReflectionExtensions()
//        {
//            relevantAttributes = new Dictionary<Type, object>();
//            relevantAttributes.Add(typeof(System.ComponentModel.CategoryAttribute), null);
//            relevantAttributes.Add(typeof(System.ComponentModel.DesignerAttribute), null);
//            relevantAttributes.Add(typeof(System.ComponentModel.DefaultValueAttribute), null);
//            relevantAttributes.Add(typeof(System.ComponentModel.Design.Serialization.DesignerSerializerAttribute), null);
//            relevantAttributes.Add(typeof(System.Runtime.InteropServices.DispIdAttribute), null);
//            relevantAttributes.Add(typeof(System.ComponentModel.DesignerSerializationVisibility), null);
//            relevantAttributes.Add(typeof(System.ComponentModel.DescriptionAttribute), null);

//            relevantAttributes.Add(typeof(System.ComponentModel.EditorAttribute), null);
//            relevantAttributes.Add(typeof(System.ComponentModel.DesignerCategoryAttribute), null);
//            relevantAttributes.Add(typeof(System.ComponentModel.ParenthesizePropertyNameAttribute), null);
//            relevantAttributes.Add(typeof(System.ComponentModel.RefreshPropertiesAttribute), null);
//            relevantAttributes.Add(typeof(System.ComponentModel.TypeConverterAttribute), null);
//        }

//        /// <summary>
//        /// Adds all the attributes of the given <c>MemberInfo</c>to the specified <paramref name="attributeList"/>.
//        /// </summary>
//        /// <param name="memberInfo">The member info.</param>
//        /// <param name="attributeList">The attribute list to fill.</param>
//        /// <param name="inherit">if set to <c>true</c> search for inherited attributes.</param>
//        public static void AddAllAttributesTo(this MemberInfo memberInfo, IList<Attribute> attributeList, bool inherit)
//        {
//            attributeList.AddRange(
//                memberInfo.GetCustomAttributes(inherit).Cast<Attribute>());
//        }

//        /// <summary>
//        /// Deletes the non relevant attributes in the specified <see cref="attributeList"/>.
//        /// </summary>
//        /// <param name="attributeList">The attribute list.</param>
//        public static void DeleteNonRelevantAttributes(this IList<Attribute> attributeList)
//        {            
//            int i = 0;
//            while (i < attributeList.Count)
//            {
//                if (attributeList[i].IsRelevant()) i++;
//                else attributeList.RemoveAt(i);
//            }
//        }

//        private static bool IsRelevant(this Attribute attribute)
//        {
//            return relevantAttributes.ContainsKey(attribute.GetType()) ||
//                relevantAttributes.ContainsKey(attribute.GetType().BaseType);
//        }
//    }
//}
