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
using o2g.Internal.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace o2g.Types.ManagementNS
{
    /// <summary>
    /// <c>PbxAttribute</c> class represents an attribute in a <see cref="PbxObject"/>.
    /// <para>
    /// A PbxAttribute can be of the following type:
    /// <h3>Integer</h3>
    /// An Integer value is equivalent to an <see langword="int"/> value.
    /// 
    /// <h3>Boolean</h3>
    /// A Boolean value is equivalent to an <see langword="bool"/> value.
    /// 
    /// <h3>Enumerated</h3>
    /// An enumerated value can have a limited set of possible values. <c>PbxAttribute</c> treats enumerated value as <see langword="string"/> value.
    /// 
    /// <h3>OctetString, ByteString</h3>
    /// An OctetString or a ByteString are equivalent to a <see langword="string"/>.
    /// 
    /// <h3>Sequence</h3>
    /// A Sequence is a structured data whose attribute member have a specific name and type: For exemple
    /// <pre>
    ///    Skill := Sequence {
    ///       Skill_Nb := Integer,
    ///       Skill_Level := Integer,
    ///       Skill_Activate := Boolean
    ///    }
    /// </pre>
    /// 
    /// <h3>Set</h3>
    /// A Set value is a list of attributes of the same type. It can be a list of simple value like:
    /// <pre>
    ///    SimpleSet := Set {
    ///       Item := OctetString
    ///    }
    /// </pre>
    /// or a list of sequences:
    /// <pre>
    ///    SkillSet := Set { 
    ///       Item := Sequence {
    ///          Skill_Nb := Integer,
    ///          Skill_Level := Integer,
    ///          Skill_Activate := Boolean
    ///       }
    ///    }
    /// </pre>
    /// </para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// <c>PbxAttribute</c> provide conversion method to usual C# types. The conversion is 
    /// controlled "at the best", but it can not check all the possible error case. An Enumerated value and an OctetString value can
    /// both be converted to a <see langword="string"/>.
    /// </para>
    /// <para>
    /// When a conversion error is detected, a <see cref="FormatException"/> exception is raised.
    /// </para>
    /// </remarks>
    public class PbxAttribute
    {
        private List<string> _values;
        private List<PbxAttributeMap> _attributeMaps;
        private PbxAttributeMap _sequenceMap;


        /// <summary>
        /// Return the name of this attribute.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> value that represents the attribute name.
        /// </value>
        public string Name { get; init; }


        /// <summary>
        /// Create a new <c>PbxAttribute</c> of type sequence with the specified strings.
        /// </summary>
        /// <param name="attrName">The attribute name.</param>
        /// <param name="values">The list of string</param>
        /// <returns>The created <see cref="PbxAttribute"/> object.</returns>
        public static PbxAttribute Create(string attrName, List<string> values)
        {
            return new()
            {
                Name = attrName,
                _values = values
            };
        }


        /// <summary>
        /// Create a new <c>PbxAttribute</c> of the <see langword="string"/> type.
        /// </summary>
        /// <param name="attrName">The attribute name.</param>
        /// <param name="value">The string value.</param>
        /// <returns>The created <see cref="PbxAttribute"/> object.</returns>
        /// <example>
        /// <code>
        ///     PbxAttribute simple = PbxAttribute.Create("AttrName", "a string value");
        /// </code>
        /// </example>
        public static PbxAttribute Create(string attrName, string value)
        {
            return new()
            {
                Name = attrName,
                _values = ValueConverter.FromNative<string>(value, ValueConverter.FromStringConverter)
            };
        }

        /// <summary>
        /// Create a new <c>PbxAttribute</c> of the <see langword="bool"/> type.
        /// </summary>
        /// <param name="attrName">The attribute name.</param>
        /// <param name="value">The boolean value.</param>
        /// <returns>The created <see cref="PbxAttribute"/> object.</returns>
        /// <example>
        /// <code>
        ///     PbxAttribute simple = PbxAttribute.Create("AttrName", true);
        /// </code>
        /// </example>
        public static PbxAttribute Create(string attrName, bool value)
        {
            return new()
            {
                Name = attrName,
                _values = ValueConverter.FromNative<bool>(value, ValueConverter.FromBooleanConverter)
            };
        }

        /// <summary>
        /// Create a new <c>PbxAttribute</c> with the specified sequence.
        /// </summary>
        /// <param name="attrName">The attribute name.</param>
        /// <param name="sequence">The sequence value.</param>
        /// <returns>The created <see cref="PbxAttribute"/> object.</returns>
        /// <example>
        /// <code>
        ///     PbxAttribute sequence = PbxAttribute.Create("sequence", 
        ///             PbxAttributeMap.Create(new List&lt;PbxAttribute&gt;() 
        ///             {
        ///                 PbxAttribute.Create("Elem1", true),
        ///                 PbxAttribute.Create("Elem2", 23)
        ///             }); 
        /// </code>
        /// </example>
        public static PbxAttribute Create(string attrName, PbxAttributeMap sequence)
        {
            return new()
            {
                Name = attrName,
                _sequenceMap = sequence
            };
        }

        /// <summary>
        /// Create a new <c>PbxAttribute</c> with the specified list of sequences.
        /// </summary>
        /// <param name="attrName">The attribute name.</param>
        /// <param name="sequences">The list of sequences</param>
        /// <returns>The created <see cref="PbxAttribute"/> object.</returns>
        /// <exemple>
        /// <code>
        ///     PbxAttribute skillSet = PbxAttribute.Create("skillSet", new List&lt;PbxAttributeMap&gt;() 
        ///     {
        ///         PbxAttributeMap.Create(new List&lt;PbxAttribute&gt;()
        ///         {
        ///             PbxAttribute.Create("Skill_nb", 21),
        ///             PbxAttribute.Create("Skill_Level", 2),
        ///             PbxAttribute.Create("Skill_Activate", true)
        ///         },
        ///         PbxAttributeMap.Create(new List&lt;PbxAttribute&gt;()
        ///             PbxAttribute.Create("Skill_nb", 22),
        ///             PbxAttribute.Create("Skill_Level", 1),
        ///             PbxAttribute.Create("Skill_Activate", true)
        ///         }
        ///     });
        /// </code>
        /// </exemple>
        public static PbxAttribute Create(string attrName, List<PbxAttributeMap> sequences)
        {
            return new()
            {
                Name = attrName,
                _attributeMaps = sequences
            };
        }



        /// <summary>
        /// Create a new <c>PbxAttribute</c> of the <see langword="int"/> type.
        /// </summary>
        /// <param name="attrName">The attribute name.</param>
        /// <param name="value">The integer value.</param>
        /// <returns>The created <see cref="PbxAttribute"/> object.</returns>
        /// <example>
        /// <code>
        ///     PbxAttribute simple = PbxAttribute.Create("AttrName", 10);
        /// </code>
        /// </example>
        public static PbxAttribute Create(string attrName, int value)
        {
            return new()
            {
                Name = attrName,
                _values = ValueConverter.FromNative<int>(value, ValueConverter.FromIntConverter)
            };
        }

        /// <summary>
        /// Return the attributes set at the specified index.
        /// </summary>
        /// <param name="index">The index of the attributes set.</param>
        /// <value>
        /// A <see cref="PbxAttributeMap"/> that represents the attribute sets
        /// </value>
        /// <exception cref="FormatException">If the attribute is not a Set attribute.</exception>
        /// <remarks>
        /// This method is used when the attribute is a set of sequences. 
        /// </remarks>
        /// <exemple>
        /// for exemple the SkillSet attribute in object ACD2_Operator_data:
        /// <pre>
        ///   SkillSet := Set
        ///     [
        ///       Sequence 
        ///         {
        ///           Skill_Level := Integer
        ///           Skill_Nb := Integer
        ///           Skill_Activate := Boolean
        ///         }
        ///     ]
        /// </pre>
        /// <code>
        ///     PbxAttribute attr = pbxObject.Attribute["SkillSet"];
        ///     int skillLevel = attr[0]["Skill_Level"].AsInt();
        /// </code>
        /// </exemple>
        ///         
        public PbxAttributeMap this[int index]
        {
            get
            {
                if (_attributeMaps == null)
                {
                    throw new FormatException("This attribute is not a Set");
                }
                return _attributeMaps[index];
            }
        }

        /// <summary>
        /// Return the value of this attribute as a sequence of attributes.
        /// </summary>
        /// <returns>
        /// The <see cref="PbxAttributeMap"/> object that reprepresents this attribute sequence value.
        /// </returns>
        public PbxAttributeMap AsAttributeMap()
        {
            return _sequenceMap;
        }

        /// <summary>
        /// Return the value of this attribute as a list of <see cref="PbxAttributeMap"/>.
        /// 
        /// </summary>
        /// <returns>A list of <see cref="PbxAttributeMap"/> objects that represents a set of sequences.</returns>
        public List<PbxAttributeMap> AsListOfMaps()
        {
            return _attributeMaps;
        }


        /// <summary>
        /// Return the value of this attribute as a list of parameters.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list</typeparam>
        /// <returns>
        /// A list of parameters of type T.
        /// </returns>
        /// <remarks>
        /// This method accept conversion to the base types, <see langword="string"/>, <see langword="int"/>, <see langword="bool"/>.
        /// </remarks>
        /// <exception cref="InvalidCastException">In case of an impossible format conversion</exception>
        public List<T> AsList<T>()
        {
            return ValueConverter.ToNative<List<T>>(_values, ValueConverter.ToListConverter<T>);
        }



        /// <summary>
        /// Return the value of this attribute as a boolean.
        /// </summary>
        /// <returns>
        /// The <see langword="bool"/> value that corresponds to this attribute value.
        /// </returns>
        /// <exception cref="FormatException">If the attribute can't be converted to a boolean value.</exception>
        public bool AsBool()
        {
            return ValueConverter.ToNative<bool>(_values, ValueConverter.ToBoolConverter);
        }

        /// <summary>
        /// Set this attribute an <see langword="bool"/> value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <remarks>
        /// Caution, there is no control on the attribute type.
        /// </remarks>
        public void Set(bool value)
        {
            _values = ValueConverter.FromNative<bool>(value, ValueConverter.FromBooleanConverter);
            _sequenceMap = null;
            _attributeMaps = null;
        }


        /// <summary>
        /// Return the value of this attribute as an integer.
        /// </summary>
        /// <returns>
        /// The <see langword="int"/> value that corresponds to this attribute value.
        /// </returns>
        /// <exception cref="FormatException">If the attribute can't be converted to an int value.</exception>
        public int AsInt()
        {
            return ValueConverter.ToNative<int>(_values, ValueConverter.ToIntConverter);
        }

        /// <summary>
        /// Set this attribute an <see langword="int"/> value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <remarks>
        /// Caution, there is no control on the attribute type.
        /// </remarks>
        public void Set(int value)
        {
            _values = ValueConverter.FromNative<int>(value, ValueConverter.FromIntConverter);
            _sequenceMap = null;
            _attributeMaps = null;
        }

        /// <summary>
        /// Return the value of this attribute as a string.
        /// </summary>
        /// <returns>
        /// The <see langword="string"/> value that corresponds to this attribute value.
        /// </returns>
        /// <exception cref="FormatException">If the attribute can't be converted to a string value.</exception>
        public string AsString()
        {
            return ValueConverter.ToNative<string>(_values, ValueConverter.ToStringConverter);
        }

        /// <summary>
        /// Set this attribute a <see langword="string"/> value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <remarks>
        /// Caution, there is no control on the attribute type.
        /// </remarks>
        public void Set(string value)
        {
            _values = ValueConverter.FromNative<string>(value, ValueConverter.FromStringConverter);
            _sequenceMap = null;
            _attributeMaps = null;
        }


        /// <summary>
        /// Return the value of this attribute as a string that represent the enumerated value.
        /// </summary>
        /// <returns>
        /// The <see langword="string"/> value that corresponds to this enumerated value.
        /// </returns>
        /// <exception cref="FormatException">If the attribute can't be converted to a string value.</exception>
        public string AsEnum()
        {
            return ValueConverter.ToNative<string>(_values, ValueConverter.ToStringConverter);
        }


        internal static PbxAttribute Build(O2GPbxAttribute a)
        {
            List<PbxAttributeMap> attrSet = null;
            if (a.ComplexValue != null)
            {
                attrSet = a.ComplexValue.Select((cv) => PbxAttributeMap.Build(cv)).ToList();
            }

            return new()
            {
                Name = a.Name,
                _attributeMaps = attrSet,
                _values = a.Value,
            };
        }

        internal static List<O2GPbxAttribute> From(PbxAttribute attr)
        {
            List<O2GPbxAttribute> listAttr = new();
            if (attr._sequenceMap != null)
            {
                listAttr.AddRange(attr._sequenceMap.Names.Select((name) => new O2GPbxAttribute()
                {
                    Name = string.Format("{0}.{1}", attr.Name, name),
                    Value = attr._sequenceMap[name]._values
                }).ToList());
            }
            else if (attr._attributeMaps == null)
            {
                listAttr.Add(new()
                {
                    Name = attr.Name,
                    Value = attr._values
                });
            }
            else
            {
                listAttr.Add(new()
                {
                    Name = attr.Name,
                    Value = attr._values,
                    ComplexValue = attr._attributeMaps.Select((attrMap) => PbxAttributeMap.From(attrMap, attr.Name)).ToList()
                });
            }

            return listAttr;
        }


        internal static void AddSequenceAttribute(PbxAttribute pbxAttribute, string name, O2GPbxAttribute attr)
        {
            if (pbxAttribute._sequenceMap == null)
            {
                pbxAttribute._sequenceMap = PbxAttributeMap.Create();
            }

            pbxAttribute._sequenceMap.Add(new()
            {
                Name = name,
                _values = attr.Value
            });
        }
    }
}
