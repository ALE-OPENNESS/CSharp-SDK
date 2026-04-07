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
using System.Collections.Generic;

namespace o2g.Types.DirectoryNS
{
    /// <summary>
    /// <c>Criteria</c> class is used to create a filter criteria for a directory search.
    /// </summary>
    /// <example>
    /// The <c>Criteria</c> class provides static method to build logical combination of simple criteria.
    /// <code>
    ///     Criteria complex = Criteria.Or(
    ///                             Criteria.And(
    ///                                 Criteria.Create(AttributeFilter.LastName, OperationFilter.BeginsWith, "Bond"),
    ///                                 Criteria.Create(AttributeFilter.FirstName, OperationFilter.EqualIgnoreCase, "james")
    ///                             ),
    ///                             Criteria.Create(AttributeFilter.PhoneNumber, OperationFilter.Contains, "007")
    ///                        );
    /// 
    /// </code>
    /// </example>
    public class Criteria
    {
        /// <summary>
        /// Return the operation associated to this criteria.
        /// </summary>
        /// <value>
        /// An <see cref="Operation"/> value that represent the criteria operation.
        /// </value>
        public string Operation { get; set; }

        /// <summary>
        /// Return the field associated to this criteria.
        /// </summary>
        /// <value>
        /// The <see langword="string"/> value that is the value associated to the criteria
        /// </value>
        public string Field { get; set; }

        /// <summary>
        /// Return the operand associated to this criteria. 
        /// </summary>
        public object Operand { get; set; }


        private static string MakeField(AttributeFilter attr)
        {
            if (attr == AttributeFilter.LastName) return "lastName";
            else if (attr == AttributeFilter.FirstName) return "firstName";
            else if (attr == AttributeFilter.PhoneNumber) return "id.phoneNumber";
            else return "id.loginName";
        }

        private static string MakeOperation(OperationFilter operation)
        {
            if (operation == OperationFilter.BeginsWith) return "BEGIN_WITH";
            else if (operation == OperationFilter.EqualIgnoreCase) return "EQUAL_IGNORE_CASE";
            else if (operation == OperationFilter.Contains) return "CONTAIN";
            else return "END_WITH";
        }

        /// <summary>
        /// Create a simple criteria with an attribute filter an operation and a value
        /// </summary>
        /// <param name="field">The attribute filter.</param>
        /// <param name="operation">The operation.</param>
        /// <param name="value">The value to test.</param>
        /// <returns>
        /// A <see cref="Criteria"/> that represents the search criteria.
        /// </returns>
        /// <example>
        /// To create a criteria search based on last name:
        /// <code>
        ///     Criteria criteria = Criteria.Create(AttributeFilter.LastName, OperationFilter.BeginsWith, "Bond");
        /// </code>
        /// 
        /// </example>
        public static Criteria Create(AttributeFilter field, OperationFilter operation, string value)
        {
            return new()
            {
                Field = Criteria.MakeField(field),
                Operation = Criteria.MakeOperation(operation),
                Operand = value
            };
        }

        /// <summary>
        /// Create a <c>Criteria</c> object thats is the logical <c>And</c> combination of the given criterias.
        /// </summary>
        /// <param name="criterias">A list of criterias to combine.</param>
        /// <returns>
        /// A <see cref="Criteria"/> that represents the logical <c>And</c> combination of the given criterias.
        /// </returns>
        /// <example>
        /// <code>
        ///     Criteria criteria = Criteria.And(
        ///                             Criteria.Create(AttributeFilter.LastName, OperationFilter.BeginsWith, "Bond"),
        ///                             Criteria.Create(AttributeFilter.FirstName, OperationFilter.EqualIgnoreCase, "james")
        ///                         );
        /// </code>
        /// 
        /// </example>
        public static Criteria And(params Criteria[] criterias)
        {
            return new()
            {
                Operation = "AND",
                Operand = new List<Criteria>(criterias)
            };
        }
        /// <summary>
        /// Create a <c>Criteria</c> object thats is the logical <c>Or</c> combination of the given criterias.
        /// </summary>
        /// <param name="criterias">A list of criterias to combine.</param>
        /// <returns>
        /// A <see cref="Criteria"/> that represents the logical <c>Or</c> combination of the given criterias.
        /// </returns>
        /// <example>
        /// <code>
        ///     Criteria criteria = Criteria.Or(
        ///                             Criteria.Create(AttributeFilter.LastName, OperationFilter.BeginsWith, "Bond"),
        ///                             Criteria.Create(AttributeFilter.FirstName, OperationFilter.EqualIgnoreCase, "james")
        ///                         );
        /// </code>
        /// 
        /// </example>
        public static Criteria Or(params Criteria[] criterias)
        {
            return new()
            {
                Operation = "AND",
                Operand = new List<Criteria>(criterias)
            };
        }
    }
}
