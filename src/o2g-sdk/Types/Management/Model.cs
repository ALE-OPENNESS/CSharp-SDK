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

using o2g.Utility;
using System.Collections.Generic;
using System.Linq;

namespace o2g.Types.ManagementNS
{
    /// <summary>
    /// <c>Model</c> represents an object model.
    /// <para>
    /// It provides the detail of object attributes: 
    /// whether the attribute is mandatory/optional in the object creation, what range of value is authorized, what are the possible 
    /// enumeration value.
    /// </para>
    /// </summary>
    public class Model
    {

        /// <summary>
        /// Return the attributes of this object model
        /// </summary>
        /// <value>
        /// A <see cref="StringMap{T}"/> object that represents the attributes of this object model.
        /// </value>
        public StringMap<ModelAttribute> Attributes { get; init; }

        /// <summary>
        /// Return the child object models of this object model
        /// </summary>
        /// <value>
        /// A <see cref="StringMap{T}"/> object that represents the child object models of this object model.
        /// </value>
        public StringMap<Model> Child { get; init; }


        /// <summary>
        /// Return the object name this <c>ObjectModel</c> describes.
        /// </summary>
        /// <value>
        /// The <see langword="string"/> value of the object name.
        /// </value>
        public string Name { get; init; }

        /// <summary>
        /// Return wheter this object is hidden.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the object is hidden; <see langword="false"/> otherwise.
        /// </value>
        public bool Hidden { get; init; }

        /// <summary>
        /// Return wheter this object can be created.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the object can be created; <see langword="false"/> otherwise.
        /// </value>
        public bool Create { get; init; }

        /// <summary>
        /// Return wheter this object can be deleted.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the object can be deleted; <see langword="false"/> otherwise.
        /// </value>
        public bool Delete { get; init; }

        /// <summary>
        /// Return wheter this object can be updated.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the object can be updated; <see langword="false"/> otherwise.
        /// </value>
        public bool Set { get; init; }

        /// <summary>
        /// Return wheter this object can be read.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the object can be read; <see langword="false"/> otherwise.
        /// </value>
        public bool Get { get; init; }

        /// <summary>
        /// Return the list of other possible actions on this object.
        /// </summary>
        /// <value>
        /// A list of <see langword="string"/> that represents the other possible actions.
        /// </value>
        public List<string> OtherActions { get; init; }

        /// <summary>
        /// Find the model for the specified object.
        /// </summary>
        /// <param name="objectDefinition">The requested object definition</param>
        /// <returns>
        /// A <see cref="Model"/> object that represents the object model, or <see langword="null"/> if there is no such object in the model.
        /// </returns>
        public Model Find(List<string> objectDefinition)
        {
            Model obj = this;
            foreach (string objectName in objectDefinition)
            {
                obj = obj.Child[objectName];
                if (obj == null)
                {
                    break;
                }
            }

            return obj;
        }

        /// <summary>
        /// Return the list of mandatory attributes.
        /// </summary>
        /// <returns>
        /// A list of <see cref="ModelAttribute"/> that represents the mandatory attributes for this object model.
        /// </returns>
        public List<ModelAttribute> GetMandatoryAttributes()
        {
            return Attributes.Where((a) => a.Mandatory).ToList();
        }
    }
}
