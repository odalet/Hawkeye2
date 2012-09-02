//#region Copyright notices
///* ****************************************************************************
// *  Hawkeye - The .Net Runtime Object Editor
// * 
// * Copyright (c) 2005 Corneliu I. Tusnea
// * 
// * This software is provided 'as-is', without any express or implied warranty.
// * In no event will the author be held liable for any damages arising from 
// * the use of this software.
// * Permission to use, copy, modify, distribute and sell this software for any 
// * purpose is hereby granted without fee, provided that the above copyright 
// * notice appear in all copies and that both that copyright notice and this 
// * permission notice appear in supporting documentation.
// * 
// * Corneliu I. Tusnea (corneliutusnea@yahoo.com.au)
// * http://www.acorns.com.au/hawkeye/
// * ****************************************************************************/
//#endregion

//using System;
//using System.Reflection;

//namespace Hawkeye.Reflection
//{
//    internal class FieldAccessor : BaseMemberAccessor, IFieldAccessor
//    {
//        public FieldAccessor(object targetObject, string fieldName) :
//            this(targetObject.GetType(), targetObject, fieldName) { }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="FieldAccessor{T}"/> class.
//        /// </summary>
//        /// <param name="targetType">Type of the target.</param>
//        /// <param name="fieldName">The field name.</param>
//        public FieldAccessor(Type targetType, object targetObject, string fieldName) : base(targetType, targetObject)
//        {
//            if (string.IsNullOrEmpty(fieldName)) throw new ArgumentNullException("fieldName");
//            FieldName = fieldName;

//            do
//            {
//                TryFindField(BindingFlags.Default);
//                TryFindField(BindingFlags.Instance | BindingFlags.FlattenHierarchy);
//                TryFindField(BindingFlags.Static | BindingFlags.FlattenHierarchy);

//                TryFindField(BindingFlags.NonPublic | BindingFlags.Instance);

//                TryFindField(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
//                TryFindField(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.GetField);
//                TryFindField(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField);

//                TryFindField(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.IgnoreCase | BindingFlags.IgnoreReturn | BindingFlags.Instance | BindingFlags.PutDispProperty | BindingFlags.PutRefDispProperty | BindingFlags.SetField | BindingFlags.Static);

//                TryFindField(BindingFlags.NonPublic | BindingFlags.Static);
//                TryFindField(BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy | BindingFlags.GetField);
//                TryFindField(BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField);

//                if (FieldInfo == null)
//                {
//                    base.SetTargetType(base.TargetType.BaseType);
//                    if (targetType == typeof(object)) break; // no chance
//                }

//            } while (FieldInfo == null);
//        }

//        #region IFieldAccessor<T> Members

//        public string FieldName
//        {
//            get;
//            private set;
//        }

//        public FieldInfo FieldInfo
//        {
//            get;
//            private set;
//        }

//        public object Value
//        {
//            get;
//            private set;
//        }
        
//        public object Get()
//        {
//            Value = FieldInfo.GetValue(Target);
//            return Value;
//        }

//        public void Set(object value)
//        {
//            FieldInfo.SetValue(Target, value);
//            Value = value;
//        }

//        #endregion

//        public override MemberInfo MemberInfo
//        {
//            get { return FieldInfo; }
//        }
        
//        private void TryFindField(BindingFlags bindingFlags)
//        {
//            if (FieldInfo != null) return;

//            FieldInfo = base.TargetType.GetField(FieldName, bindingFlags);
//            if (FieldInfo != null) return;

//            var allFields = base.TargetType.GetFields(bindingFlags);
//            foreach (var field in allFields)
//            {
//                if (field.Name == FieldName)
//                {
//                    FieldInfo = field;
//                    return;
//                }
//            }
//        }
//    }
//}