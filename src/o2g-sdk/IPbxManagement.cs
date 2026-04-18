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
using o2g.Events;
using o2g.Events.Management;
using o2g.Internal.Services;
using o2g.Types.ManagementNS;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace o2g
{
    /// <summary>
    /// <c>IPbxManagement</c> allows an administrator to manage an OmniPCX Enterprise, that is to create, modify or delete
    /// any object or sub-object in the OmniPCX Enterprise object model.
    /// Using this service requires having a <b>MANAGEMENT</b> license.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <b>WARNING:</b> Using this service requires a good knowledge of the OmniPCX Enterprise object model.
    /// </para>
    /// <para>
    /// The service uses two kinds of resource: the object model resource and the object instance resource.
    /// <h3>The object model resource</h3>
    /// The object model can be retrieved for the whole PBX or for a particular object. It provides the detail of object attributes:
    /// whether the attribute is mandatory or optional in the object creation, what range of values is authorized, and what the
    /// possible enumeration values are.
    /// <h3>The object instance resource</h3>
    /// It is used to create, modify, retrieve or remove any instances of any object, given the reference of this object. For
    /// the creation or modification of an object, the body must be compliant with the object model.
    /// </para>
    /// <para>
    /// The list of sub-objects returned by a get instance of an object corresponds to the relative path of the first
    /// instantiable objects in the hierarchy, in order to be able by recursion to build the path to access any object and sub-object.
    /// </para>
    /// <para>
    /// When accessing an object which is a sub-object, the full path must be given:
    /// <c>{object1Name}/{object1Id}/{object2Name}/{object2Id}/.../{objectxName}/{objectxId}</c>.
    /// </para>
    /// </remarks>
    public interface IPbxManagement : IService
    {
        /// <summary>
        /// Raised when a PBX object instance is created.
        /// </summary>
        /// <remarks>
        /// Only the <c>Subscriber</c> object is concerned by this event.
        /// </remarks>
        public event EventHandler<O2GEventArgs<OnPbxObjectInstanceCreatedEvent>> PbxObjectInstanceCreated;

        /// <summary>
        /// Raised when a PBX object instance is deleted.
        /// </summary>
        /// <remarks>
        /// Only the <c>Subscriber</c> object is concerned by this event.
        /// </remarks>
        public event EventHandler<O2GEventArgs<OnPbxObjectInstanceDeletedEvent>> PbxObjectInstanceDeleted;

        /// <summary>
        /// Raised when a PBX object instance is modified.
        /// </summary>
        /// <remarks>
        /// Only the <c>Subscriber</c> object is concerned by this event.
        /// </remarks>
        public event EventHandler<O2GEventArgs<OnPbxObjectInstanceModifiedEvent>> PbxObjectInstanceModified;

        /// <summary>
        /// Gets the list of OmniPCX Enterprise nodes connected on this O2G server.
        /// </summary>
        /// <returns>
        /// A list of <see langword="int"/> representing the node ids, or <see langword="null"/> in case of error.
        /// </returns>
        Task<List<int>> GetPbxsAsync();

        /// <summary>
        /// Gets the OmniPCX Enterprise specified by its node id.
        /// </summary>
        /// <param name="nodeId">The OmniPCX Enterprise node id.</param>
        /// <returns>
        /// A <see cref="Pbx"/> object representing the OmniPCX Enterprise node, or <see langword="null"/> in case of error.
        /// </returns>
        Task<Pbx> GetPbxAsync(int nodeId);

        /// <summary>
        /// Gets the description of the data model for the specified object on the specified OmniPCX Enterprise node.
        /// </summary>
        /// <param name="nodeId">The OmniPCX Enterprise node id.</param>
        /// <param name="objectName">The object name (case sensitive), or <see langword="null"/> to retrieve the global model.</param>
        /// <returns>
        /// A <see cref="Model"/> object describing the requested object model, or <see langword="null"/> in case of error.
        /// </returns>
        Task<Model> GetObjectModelAsync(int nodeId, string objectName = null);


        /// <summary>
        /// Gets the node (root) object.
        /// </summary>
        /// <param name="nodeId">The OmniPCX Enterprise node id.</param>
        /// <returns>
        /// A <see cref="PbxObject"/> representing the root node object, or <see langword="null"/> in case of error.
        /// </returns>
        /// <seealso cref="GetObjectAsync(int, string, string, string)"/>
        Task<PbxObject> GetNodeObjectAsync(int nodeId);


        /// <summary>
        /// Gets the object specified by its instance definition and its instance id.
        /// </summary>
        /// <param name="nodeId">The OmniPCX Enterprise node id.</param>
        /// <param name="objectInstanceDefinition">The object instance definition.</param>
        /// <param name="objectId">The object instance id.</param>
        /// <param name="attributes">A comma-separated list of attribute names to retrieve, or <see langword="null"/> to retrieve all attributes.</param>
        /// <returns>
        /// A <see cref="PbxObject"/> representing the requested object, or <see langword="null"/> in case of error.
        /// </returns>
        /// <remarks>
        /// When <c>attributes</c> is specified, only those attributes and the list of sub-object paths are returned.
        /// The value is a comma-separated attribute name list: <c>"Station_Type,Directory_Number,..."</c>
        /// </remarks>
        /// <example>
        /// <code>
        ///     PbxObject obj = await pbxManagementService.GetObjectAsync(5, "Subscriber", "36530", "Station_Type,Directory_Number");
        /// </code>
        /// </example>
        /// <seealso cref="GetObjectAsync(int, string, string, List{PbxAttribute})"/>
        Task<PbxObject> GetObjectAsync(int nodeId, string objectInstanceDefinition, string objectId, string attributes = null);


        /// <summary>
        /// Gets the object specified by its instance definition and its instance id, returning only the specified attributes.
        /// </summary>
        /// <param name="nodeId">The OmniPCX Enterprise node id.</param>
        /// <param name="objectInstanceDefinition">The object instance definition.</param>
        /// <param name="objectId">The object instance id.</param>
        /// <param name="attributes">The list of attributes to retrieve.</param>
        /// <returns>
        /// A <see cref="PbxObject"/> representing the requested object, or <see langword="null"/> in case of error.
        /// </returns>
        /// <remarks>
        /// Only the specified attributes and the list of sub-object paths of the current object are returned.
        /// </remarks>
        /// <seealso cref="GetObjectAsync(int, string, string, string)"/>
        Task<PbxObject> GetObjectAsync(int nodeId, string objectInstanceDefinition, string objectId, List<PbxAttribute> attributes);


        /// <summary>
        /// Gets the object specified by its instance definition and its instance id, returning only the specified attributes.
        /// </summary>
        /// <param name="nodeId">The OmniPCX Enterprise node id.</param>
        /// <param name="objectInstanceDefinition">The object instance definition.</param>
        /// <param name="objectId">The object instance id.</param>
        /// <param name="attributes">The array of attribute names to retrieve.</param>
        /// <returns>
        /// A <see cref="PbxObject"/> representing the requested object, or <see langword="null"/> in case of error.
        /// </returns>
        /// <remarks>
        /// Only the specified attributes and the list of sub-object paths of the current object are returned.
        /// </remarks>
        /// <seealso cref="GetObjectAsync(int, string, string, string)"/>
        Task<PbxObject> GetObjectAsync(int nodeId, string objectInstanceDefinition, string objectId, string[] attributes);


        /// <summary>
        /// Queries the list of object instances that match the specified filter.
        /// </summary>
        /// <param name="nodeId">The OmniPCX Enterprise node id.</param>
        /// <param name="objectInstanceDefinition">The object instance definition.</param>
        /// <param name="filter">A <see cref="Filter"/> object representing a filter on object attributes.</param>
        /// <returns>
        /// A list of <see langword="string"/> representing the matching object instance ids, or <see langword="null"/> in case of error or if no instance matches the filter.
        /// </returns>
        /// <example>
        /// <code>
        ///     Filter filter = Filter.Create("StationType", AttributeFilter.Equals, "ANALOG");
        ///     List&lt;string> objectInstances = await pbxManagementService.GetObjectInstancesAsync(5, "Subscriber", filter);
        /// </code>
        /// </example>
        /// <seealso cref="GetObjectInstancesAsync(int, string, string)"/>
        Task<List<string>> GetObjectInstancesAsync(int nodeId, string objectInstanceDefinition, Filter filter);

        /// <summary>
        /// Queries the list of object instances that match the specified filter expression.
        /// </summary>
        /// <param name="nodeId">The OmniPCX Enterprise node id.</param>
        /// <param name="objectInstanceDefinition">The object instance definition.</param>
        /// <param name="filter">A filter expression string on object attributes, or <see langword="null"/> to return all instances of the specified object.</param>
        /// <returns>
        /// A list of <see langword="string"/> representing the matching object instance ids, or <see langword="null"/> in case of error or if no instance matches the filter.
        /// </returns>
        /// <seealso cref="GetObjectInstancesAsync(int, string, Filter)"/>
        Task<List<string>> GetObjectInstancesAsync(int nodeId, string objectInstanceDefinition, string filter = null);

        /// <summary>
        /// Changes one or several attribute values of the specified object.
        /// </summary>
        /// <param name="nodeId">The OmniPCX Enterprise node id.</param>
        /// <param name="objectInstanceDefinition">The object instance definition.</param>
        /// <param name="objectId">The object instance id.</param>
        /// <param name="attributes">The list of <see cref="PbxAttribute"/> to change.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// If an update on the same object has been performed by another administrator since the last operation, a conflict error
        /// occurs and a GET operation must be performed first to allow the update. This prevents changes made by others from being overwritten.
        /// </remarks>
        /// <example>
        /// <code>
        ///     List&lt;PbxAttribute> attrs = new();
        ///     attrs.Add(PbxAttribute.Create("Station_Type", "ANALOG"));
        ///
        ///     if (!await pbxManagementService.SetObjectAsync(5, "Subscriber", "23100", attrs))
        ///     {
        ///         Console.WriteLine("Error");
        ///     }
        /// </code>
        /// </example>
        Task<bool> SetObjectAsync(int nodeId, string objectInstanceDefinition, string objectId, List<PbxAttribute> attributes);

        /// <summary>
        /// Deletes the specified instance of an object.
        /// </summary>
        /// <param name="nodeId">The OmniPCX Enterprise node id.</param>
        /// <param name="objectInstanceDefinition">The object instance definition.</param>
        /// <param name="objectId">The object instance id.</param>
        /// <param name="forceDelete">If <see langword="true"/>, uses the <c>FORCED_DELETE</c> action to delete the object.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// The <c>FORCED_DELETE</c> action is not available for all objects. Check its availability in the <see cref="Model"/> corresponding to the object.
        /// It can be used, for example, to delete a <c>Subscriber</c> that has voice mails in their mailbox.
        /// </remarks>
        Task<bool> DeleteObjectAsync(int nodeId, string objectInstanceDefinition, string objectId, bool forceDelete = false);


        /// <summary>
        /// Creates a new object with the specified list of attributes.
        /// </summary>
        /// <param name="nodeId">The OmniPCX Enterprise node id.</param>
        /// <param name="objectInstanceDefinition">The object instance definition.</param>
        /// <param name="attributes">The list of attributes to set at object creation.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        Task<bool> CreateObjectAsync(int nodeId, string objectInstanceDefinition, List<PbxAttribute> attributes);
    }
}
