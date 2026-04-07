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
using System.Collections;
using System.Collections.Generic;

namespace o2g.Types.ManagementNS
{
    /// <summary>
    /// <c>PbxAttributeMap</c> represents a sequence of named attributes.
    /// </summary>
    /// <seealso cref="PbxAttribute"/>
    public class PbxAttributeMap : IEnumerable<PbxAttribute>
    {
        private MutableStringMap<PbxAttribute> _attributes = new();

        /// <summary>
        /// Return the list of attribute name in this sequence.
        /// </summary>
        /// <value>
        /// A list of <see langword="string"/> that represents the name.
        /// </value>
        public List<string> Names
        {
            get
            {
                return _attributes.Names;
            }
        }


        /// <summary>
        /// Return the Attribute with the given name.
        /// </summary>
        /// <param name="name">The attribute name</param>
        /// <value>
        /// A <see cref="PbxAttribute"/> that corresponds to the attribute with the given name or <see langword="null"/> if there is
        /// no attribute with this name.
        /// </value>
        /// <example>
        /// <code>
        ///     PbxAttributeMap skillSequence = ...
        ///    
        ///     PbxAttribute attr = skillSequence["Skill_nb"]
        /// </code>
        /// </example>
        public PbxAttribute this[string name]
        {
            get
            {
                return _attributes[name];
            }
        }

        /// <summary>
        /// Add an attribute to this sequence.
        /// </summary>
        /// <param name="value">The <see cref="PbxAttribute"/> object value to add to the sequence.</param>
        /// <returns>
        /// The sequence this attribute has been added to.
        /// </returns>
        /// <remarks>
        /// It's possible to chain the <c>Add</c> to create a complete sequence.
        /// <exemple>
        /// <code>
        ///     attrList.Add(PbxAttribute.Create("Name", PbxAttributeMap.Create()
        ///                                                 .Add(PbxAttribute.Create("Elem1", true))
        ///                                                 .Add(PbxAttribute.Create("Elem2", 23))));
        /// </code>
        /// </exemple>
        /// </remarks>
        public PbxAttributeMap Add(PbxAttribute value)
        {
            _attributes.Add(value.Name, value);
            return this;
        }

        /// <summary>
        /// Create a new empty PbxAttributeMap.
        /// </summary>
        /// <returns>
        /// The new created <see cref="PbxAttributeMap"/> object.
        /// </returns>
        /// <remarks>
        /// Use this method to create a new sequence of attribute. For exemple:
        /// <pre>
        ///    Sequence {
        ///       Param1 := Integer,
        ///       Param2 := Boolean
        ///    }
        /// </pre>
        /// </remarks>
        /// <example>
        /// <code>
        ///     PbxAttributeMap sequence = 
        ///         PbxAttributeMap.Create()
        ///                 .Add(PbxAttribute.Create("Param1", 1))
        ///                 .Add(PbxAttribute.Create("Param2", true));
        /// </code>
        /// </example>
        public static PbxAttributeMap Create()
        {
            return new();
        }


        /// <summary>
        /// Create a new PbxAttributeMap with the specified attribute list.
        /// </summary>
        /// <param name="attributes">the list of attributes</param>
        /// <returns>
        /// The new created <see cref="PbxAttributeMap"/> object.
        /// </returns>
        /// <remarks>
        /// Use this method to create a new sequence of attribute. For exemple:
        /// <pre>
        ///    Sequence {
        ///       Param1 := Integer,
        ///       Param2 := Boolean
        ///    }
        /// </pre>
        /// </remarks>
        /// <example>
        /// <code>
        ///     PbxAttributeMap sequence = 
        ///         PbxAttributeMap.Create(new List&lt;PbxAttribute&gt;
        ///         {
        ///             PbxAttribute.Create("Param1", 1),
        ///             PbxAttribute.Create("Param2", true)
        ///         });
        /// </code>
        /// </example>
        public static PbxAttributeMap Create(List<PbxAttribute> attributes)
        {
            PbxAttributeMap map = new();
            attributes.ForEach((a) => map.Add(a));
            return map;
        }

        internal static PbxAttributeMap Build(O2GPbxComplexAttribute cv)
        {
            Dictionary<string, PbxAttribute> mapAttributes = new();

            if (cv.Attributes != null)
            {
                cv.Attributes.ForEach((a) => mapAttributes.Add(a.Name, PbxAttribute.Build(a)));
            }

            return new()
            {
                _attributes = new()
                {
                    Map = mapAttributes
                }
            };
        }


        internal static O2GPbxComplexAttribute From(PbxAttributeMap attributeMaps, string attrName)
        {
            List<O2GPbxAttribute> o2gPbxAttributes = new();
            attributeMaps.Names.ForEach((name) => o2gPbxAttributes.AddRange(PbxAttribute.From(attributeMaps[name])));

            return new()
            {
                Name = attrName,
                Attributes = o2gPbxAttributes
            };
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<PbxAttribute> GetEnumerator()
        {
            return _attributes.Map.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _attributes.Map.Values.GetEnumerator();
        }
    }
}
