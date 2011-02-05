#region Original Copyright notice
/* ****************************************************************************
 *  Hawkeye - The .Net Runtime Object Editor
 * 
 * Copyright (c) 2005 Corneliu I. Tusnea
 * 
 * This software is provided 'as-is', without any express or implied warranty.
 * In no event will the author be held liable for any damages arising from 
 * the use of this software.
 * Permission to use, copy, modify, distribute and sell this software for any 
 * purpose is hereby granted without fee, provided that the above copyright 
 * notice appear in all copies and that both that copyright notice and this 
 * permission notice appear in supporting documentation.
 * 
 * Corneliu I. Tusnea (corneliutusnea@yahoo.com.au)
 * http://www.acorns.com.au/hawkeye/
 * ****************************************************************************/
#endregion

using System;
using System.Reflection;
using Hawkeye.Api.Reflection;

namespace Hawkeye.Reflection
{
    internal class PropertyAccessor : BaseMemberAccessor, IPropertyAccessor
    {
        public PropertyAccessor(Type targetType, string propertyName) :
            this(targetType, null, propertyName) { }

        public PropertyAccessor(object targetObject, string propertyName) :
            this(targetObject.GetType(), targetObject, propertyName) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyAccesor{T}"/> class.
        /// </summary>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="propertyName">Name of the property.</param>
        public PropertyAccessor(Type targetType, object targetObject, string propertyName) :
            base(targetType, targetObject)
        {
            PropertyName = propertyName;

            do
            {
                TryFindProperty(BindingFlags.Default);
                TryFindProperty(BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                TryFindProperty(BindingFlags.Static | BindingFlags.FlattenHierarchy);

                TryFindProperty(BindingFlags.NonPublic | BindingFlags.Instance);

                TryFindProperty(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                TryFindProperty(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.GetProperty);
                TryFindProperty(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetProperty);

                TryFindProperty(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.GetProperty | BindingFlags.GetProperty | BindingFlags.IgnoreCase | BindingFlags.IgnoreReturn | BindingFlags.Instance | BindingFlags.PutDispProperty | BindingFlags.PutRefDispProperty | BindingFlags.SetProperty | BindingFlags.Static);

                TryFindProperty(BindingFlags.NonPublic | BindingFlags.Static);
                TryFindProperty(BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy | BindingFlags.GetProperty);
                TryFindProperty(BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetProperty);

                if (PropertyInfo == null)
                {
                    base.SetTargetType(base.TargetType.BaseType);
                    if (targetType == typeof(object)) break; // no chance
                }

            } while (PropertyInfo == null);
        }

        #region IPropertyAccessor<T> Members

        public string PropertyName
        {
            get;
            private set;
        }

        public PropertyInfo PropertyInfo
        {
            get;
            private set;
        }

        public object Value
        {
            get;
            private set;
        }

        public object Get(object target)
        {
            return PropertyInfo.GetValue(target, null);
        }

        public object Get()
        {
            Value = Get(Target);
            return Value;
        }

        public void Set(object target, object value)
        {
            PropertyInfo.SetValue(target, value, null);
        }

        public void Set(object value)
        {
            Set(Target, value);
            Value = value;
        }

        #endregion

        public override MemberInfo MemberInfo
        {
            get { return PropertyInfo; }
        }

        private void TryFindProperty(BindingFlags bindingFlags)
        {
            if (PropertyInfo != null) return;

            PropertyInfo = base.TargetType.GetProperty(PropertyName, bindingFlags);
            if (PropertyInfo != null) return;

            var allProperties = base.TargetType.GetProperties(bindingFlags);
            foreach (var property in allProperties)
            {
                if (property.Name == PropertyName)
                {
                    PropertyInfo = property;
                    return;
                }
            }
        }
    }
}