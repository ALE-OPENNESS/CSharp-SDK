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
using o2g.Internal.Types.Management;
using o2g.Utility;
using System;
using System.Collections.Generic;

namespace o2g.Types.ManagementNS
{
    /// <summary>
    /// <c>PbxObject</c> represents an object of the OmniPCX Enterprise object model.
    /// </summary>
    /// <remarks>
    /// A <c>PbxObject</c> is referenced by it's object instance definition, a hierarchical path from the root object, and a unique instance id.<br/>
    /// For exemple:
    /// <list type="table">
    /// <listheader><term>objectInstanceDefinition</term><description>Description</description></listheader>
    /// <item>
    ///     <term>"Subscriber"</term><description>A Subscriber object.</description>
    /// </item>
    /// <item>
    ///     <term>"Application_Configuration/1/ACD2/1/ACD2_Operator/1/ACD2_Operator_data"</term><description>A CCD operator data object.</description>
    /// </item>
    /// </list>
    /// </remarks>
    public class PbxObject
    {
        /// <summary>
        /// Return this object attributes.
        /// </summary>
        /// <value>
        /// A <see cref="MutableStringMap&lt;PbxAttribute&gt;"/> object that represents this object attributes.
        /// </value>
        public MutableStringMap<PbxAttribute> Attributes { get; set; }

        /// <summary>
        /// Return this object name.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that represents this object name. Exemple "Subscriber".
        /// </value>
        public string ObjectName { get; init; }

        /// <summary>
        /// Return this object instance Identifier.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that represents this object instance unique identifier.
        /// </value>
        public string Id { get; init; }

        /// <summary>
        /// Return the list of sub-objects.
        /// </summary>
        /// <value>
        /// A list of <see langword="string"/> that represents the name of the sub-objects.
        /// </value>
        public List<String> ObjectNames { get; init; }

        /// <summary>
        /// Return wheter this object can be deleted.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the object can be deleted; <see langword="false"/> otherwise.
        /// </value>
        /// <remarks>
        /// <c>Delete</c> equal to <see langword="true"/> means the Delete operation can be invoked, but not that it will succeed.
        /// </remarks>
        /// <seealso cref="IPbxManagement.DeleteObjectAsync(int, string, string, bool)"/>
        public bool Delete { get; init; }

        /// <summary>
        /// Return wheter this object can be modified.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the object can be modified; <see langword="false"/> otherwise.
        /// </value>
        /// <remarks>
        /// <c>Delete</c> equal to <see langword="true"/> means the Set operation can be invoked, but not that it will succeed.
        /// </remarks>
        /// <seealso cref="IPbxManagement.SetObjectAsync(int, string, string, List{PbxAttribute})"/>
        public bool Set { get; init; }

        static internal PbxObject Build(O2GPbxObject o2GPbxObject)
        {
            Dictionary<string, PbxAttribute> mapAttributes = new();

            if (o2GPbxObject.Attributes != null)
            {
                foreach (O2GPbxAttribute attr in o2GPbxObject.Attributes)
                {
                    string[] names = attr.Name.Split('.');
                    if (names.Length == 1)
                    {
                        mapAttributes.Add(attr.Name, PbxAttribute.Build(attr));
                    }
                    else
                    {
                        // sequence !
                        if (!mapAttributes.ContainsKey(names[0]))
                        {
                            mapAttributes.Add(names[0], new()
                            {
                                Name = names[0]
                            });
                        }

                        PbxAttribute.AddSequenceAttribute(mapAttributes[names[0]], names[1], attr);
                    }
                }
            }

            return new()
            {
                ObjectName = o2GPbxObject.ObjectName,
                Id = o2GPbxObject.ObjectId,
                ObjectNames = o2GPbxObject.ObjectNames,
//                Create = o2GPbxObject.Create,
                Delete = o2GPbxObject.Delete,
                Set = o2GPbxObject.Set,
//                Get = o2GPbxObject.Get,
//                OtherActions = o2GPbxObject.OtherActions,
                Attributes = new MutableStringMap<PbxAttribute>()
                {
                    Map = mapAttributes
                }
            };
        }
    }
}
