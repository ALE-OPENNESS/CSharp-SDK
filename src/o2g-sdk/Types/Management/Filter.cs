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

using o2g.Internal.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace o2g.Types.ManagementNS
{
    /// <summary>
    /// <c>AttributeFilter</c> represents the possible operation to apply to an attribute to build a filter.
    /// </summary>
    /// <seealso cref="Filter"/>
    public enum AttributeFilter
    {
        /// <summary>
        /// The attribute is equal to the value.
        /// </summary>
        Equals,

        /// <summary>
        /// The attribute starts with the value.
        /// </summary>
        StartsWith,

        /// <summary>
        /// The attributes ends with the value.
        /// </summary>
        EndsWith,

        /// <summary>
        /// The attribute is not equals to the value.
        /// </summary>
        NotEquals,

        /// <summary>
        /// The attribute is greather than or equals to the value.
        /// </summary>
        GreatherThanOrEquals,

        /// <summary>
        /// The attribute is Less than or equals to the value.
        /// </summary>
        LessThanOrEquals
    }


    /// <summary>
    /// <c>Filter</c> class represents a filter that can be used to query OmniPCX Enterprise Object instances.
    /// </summary>
    /// <example>
    /// The <c>Filter</c> class provides static method to build logical combination of simple filters.
    /// <code>
    ///     Filter complex = Filter.And(
    ///                             Filter.Or(
    ///                                 Filter.Create("Station_Type", AttributeFilter.Equals, "ANALOG"),
    ///                                 Filter.Create("Station_Type", AttributeFilter.Equals, "ALE-300")
    ///                             ),
    ///                             Filter.Create("Directory_Name", OperationFilter.StartsWith, "f")
    ///                        );
    /// 
    /// </code>
    /// </example>
    public class Filter
    {
        /// <summary>
        /// Return the value this filter apply
        /// </summary>
        /// <value>
        /// A <see langword="string"/> containing the value of this filter.
        /// </value>
        public string Value { get; init; }

        /// <summary>
        /// Create a new filter with the specified attribute, operation and value.
        /// </summary>
        /// <param name="attribute">The attribute</param>
        /// <param name="operation">The operation.</param>
        /// <param name="value">The value to test.</param>
        /// <returns>
        /// The created <see cref="Filter"/> object.
        /// </returns>
        /// <seealso cref="Create(string, AttributeFilter, string)"/>
        public static Filter Create(PbxAttribute attribute, AttributeFilter operation, string value)
        {
            return Create(attribute.Name, operation, value);
        }

        /// <summary>
        /// Create a new filter with the specified attribute name, operation and value.
        /// </summary>
        /// <param name="attrName">The attribute name.</param>
        /// <param name="operation">The operation.</param>
        /// <param name="value">The value to test.</param>
        /// <returns>
        /// The created <see cref="Filter"/> object.
        /// </returns>
        /// <example>
        /// <code>
        ///     Filter aFilter = Filter.Create("Station_Type", AttributeFilter.Equals, "ANALOG");
        /// </code>
        /// </example>
        public static Filter Create(string attrName, AttributeFilter operation, string value)
        {
            if (operation == AttributeFilter.Equals) return new Filter() { Value = string.Format("{0}=={1}", attrName, value) };
            else if (operation == AttributeFilter.NotEquals) return new Filter() { Value = string.Format("{0}!={1}", attrName, value) };
            else if (operation == AttributeFilter.StartsWith) return new Filter() { Value = string.Format("{0}=={1}*", attrName, value) };
            else if (operation == AttributeFilter.EndsWith) return new Filter() { Value = string.Format("{0}==*{1}", attrName, value) };
            else if (operation == AttributeFilter.GreatherThanOrEquals) return new Filter() { Value = string.Format("{0}=ge={1}", attrName, value) };
            else if (operation == AttributeFilter.LessThanOrEquals) return new Filter() { Value = string.Format("{0}=le={1}", attrName, value) };
            else
            {
                throw new ArgumentException(string.Format("Unknown operation: {0}", operation));
            }
        }

        private static Filter CombineOperator(string ope, Filter filter1, Filter filter2, List<Filter> otherFilters)
        {
            string result = string.Format("{0} {1} {2}",
                AssertUtil.NotNull<Filter>(filter1, "filter1").Value,
                ope,
                AssertUtil.NotNull<Filter>(filter2, "filter2").Value);

            StringBuilder sb = new(result);

            if (otherFilters != null)
            {
                otherFilters.ForEach(f => sb.Append(string.Format(" {0} {1}", ope, f.Value)));
            }

            return new() { Value = sb.ToString() };
        }

        /// <summary>
        /// Combine a set of filter with a logical And. 
        /// </summary>
        /// <param name="filter1">The first filter.</param>
        /// <param name="filter2">The second filter.</param>
        /// <param name="otherFilters">Other optional filters</param>
        /// <returns>
        /// A new <see cref="Filter"/> object that represents the logical And combination of the givern filters.
        /// </returns>
        /// <example>
        /// <code>
        ///     Filter aFilter = Filter.And(
        ///                             Filter.Create("Station_Type", AttributeFilter.Equals, "ANALOG"),
        ///                             Filter.Create("Directory_Name", AttributeFilter.StartsWith, "B"));
        /// </code>
        /// </example>
        public static Filter And(Filter filter1, Filter filter2, params Filter[] otherFilters)
        {
            return CombineOperator("and", filter1, filter2, (otherFilters == null) ? null : new List<Filter>(otherFilters));
        }

        /// <summary>
        /// Combine a set of filter with a logical Or. 
        /// </summary>
        /// <param name="filter1">The first filter.</param>
        /// <param name="filter2">The second filter.</param>
        /// <param name="otherFilters">Other optional filters</param>
        /// <returns>
        /// A new <see cref="Filter"/> object that represents the logical Or combination of the givern filters.
        /// </returns>
        /// <example>
        /// <code>
        ///     Filter aFilter = Filter.Or(
        ///                             Filter.Create("Station_Type", AttributeFilter.Equals, "ANALOG"),
        ///                             Filter.Create("Directory_Name", AttributeFilter.StartsWith, "B"));
        /// </code>
        /// </example>
        public static Filter Or(Filter filter1, Filter filter2, params Filter[] otherFilters)
        {
            return CombineOperator("or", filter1, filter2, (otherFilters == null) ? null : new List<Filter>(otherFilters));
        }
    }
}
