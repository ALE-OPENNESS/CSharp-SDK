/*
* Copyright 2021 ALE International
*
* Permission is hereby granted, free of charge, to any person obtaining a copy of this 
* software and associated documentation files (the "Software"), to deal in the Software 
* without restriction, including without limitation the rights to use, copy, modify, merge, 
* publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons 
* to whom the Software is furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in all copies or 
* substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING 
* BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
* NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
* DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/
using System;
using System.Collections.Generic;
using System.Reflection;

namespace o2g.Internal.Utility
{
    internal class DependancyResolver
    {
        private static DependancyResolver _instance = null;

        private static DependancyResolver GetInstance()
        {
            if (_instance == null)
            {
                _instance = new();
            }
            return _instance;
        }

        private DependancyResolver()
        {

        }


        private readonly List<Object> singletons = new();

        private Object GetAssignable(Type objType)
        {
            foreach (Object o in singletons)
            {
                if (o.GetType().IsAssignableFrom(objType))
                {
                    return o;
                }
            }
            return null;
        }

        private bool Exists(Type objType)
        {
            foreach (Object o in singletons)
            {
                Type oType = o.GetType();
                if (oType.IsAssignableFrom(objType) || oType.IsAssignableTo(objType))
                {
                    return true;
                }
            }
            return false;
        }


        private void _RegisterService(Object anObject)
        {
            AssertUtil.NotNull(anObject, "anObject");

            if (Exists(anObject.GetType()))
            {
                throw new InjectionException(string.Format("Injection exception: Given type {1} is already registered", anObject.GetType()));
            }

            singletons.Add(anObject);
        }

        internal static void RegisterService(Object anObject) => GetInstance()._RegisterService(anObject);


        private T _Resolve<T>(T anObject)
        {
            AssertUtil.NotNull(anObject, "anObject");

            // loop on fields
            FieldInfo[] fields = anObject.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (FieldInfo field in fields)
            {
                InjectionAttribute attr = field.GetCustomAttribute<InjectionAttribute>();
                if (attr != null)
                {
                    Type fieldType = field.FieldType;

                    // Get the singleton for this type
                    Object single = GetAssignable(fieldType);
                    if (single == null)
                    {
                        throw new InjectionException(string.Format("Injection exception: Missing object of type {1}", fieldType));
                    }
                    field.SetValue(anObject, single);
                }
            }

            return anObject;
        }

        internal static T Resolve<T>(T anObject) => GetInstance()._Resolve(anObject);
        internal static void Reset()
        {
            if (_instance != null)
            {
                _instance.singletons.Clear();
            }
        }
    }
}
