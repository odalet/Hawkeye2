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
	internal class MethodAccessor : BaseMemberAccessor, IMethodAccessor
	{
        public MethodAccessor(Type targetType, string methodName) :
            this(targetType, null, methodName) { }

        public MethodAccessor(object targetObject, string methodName) :
            this(targetObject.GetType(), targetObject, methodName) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodAccessor"/> class.
        /// </summary>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="targetObject">The target object.</param>
        /// <param name="methodName">Name of the method.</param>
        public MethodAccessor(Type targetType, object targetObject, string methodName) : base(targetType, targetObject)
        {
            if (string.IsNullOrEmpty(methodName)) throw new ArgumentNullException("methodName");
            MethodName = methodName;

            do
            {
                TryFindMethod(BindingFlags.Public | BindingFlags.Instance);
                TryFindMethod(BindingFlags.NonPublic | BindingFlags.Instance);
                TryFindMethod(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

                if (MethodInfo == null)
                {
                    base.SetTargetType(base.TargetType.BaseType);
                    if (TargetType == typeof(object)) break; // no chance
                }

            } while (MethodInfo == null);
        }

        #region IMethodAccessor Members

        public string MethodName
        {
            get;
            private set;
        }

        public MethodInfo MethodInfo
        {
            get;
            private set;
        }

        public object Invoke(object target)
        {
            return MethodInfo.Invoke(target, new object[0]);
        }

        public object Invoke(object target, object parameter)
        {
            return MethodInfo.Invoke(target, new[] { parameter });
        }

        public object Invoke(object target, object[] parameters)
        {
            return MethodInfo.Invoke(target, parameters);
        }

        public object Invoke(object[] parameters)
        {
            return Invoke(Target, parameters);
        }

        #endregion

        public override MemberInfo MemberInfo
        {
            get { return MethodInfo; }
        }

        private void TryFindMethod(BindingFlags bindingFlags)
        {
            if (MethodInfo != null) return;

            MethodInfo = base.TargetType.GetMethod(MethodName, bindingFlags);
        }        
    }
}