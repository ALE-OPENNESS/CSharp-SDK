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
    /// The <c>IPbxManagement</c> service allows an administrator to manage an OmniPcx Enterprise, that is to create/modify/delete 
    /// any object or sub-object in the OmniPcx Enterprise object model.
    /// Using this service requires having a <b>MANAGEMENT</b> license.
    /// <para>
    /// <b>WARNING:</b> Using this service requires to have a good knowledge of the OmniPCX Enterprise object model.
    /// </para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// The service uses two kinds of resource: the object model resource and the object instance resource.
    /// <h3>The object model resource</h3>
    /// The object model can be retrieved for the whole Pbx or for a particular object. It provides the detail of object attributes: 
    /// whether the attribute is mandatory/optional in the object creation, what range of value is authorized, what are the possible 
    /// enumeration value.
    /// <h3>The object instance resources</h3>
    /// It is used to create, modify, retrieve or remove any instances of any object, giving the reference of this object. For 
    /// the creation or the modification of an object, the body must be compliant with the object model.
    /// </para>
    /// <para>
    /// The list of sub-objects which are returned by a get instance of an object corresponds to the relative path of the first 
    /// instanciable objects in the hierarchy in order to be able by recursion to build the path to access to any object and sub-object.
    /// </para>
    /// <para>
    /// When access to an object which is a sub-object, the full path must be given : <c>{object1Name}/{object1Id}/{object2Name}/{object2Id}/..../{objectxName}/{objectxId}</c>. 
    /// </para><para>
    ///     ex: pbxs/1/instances/System_Parameters/1/System_Parameters_2/1/Network_Parameters
    /// </para>
    /// </remarks>
    public interface IPbxManagement : IService
    {
        /// <summary>
        /// Occurs when a PBX object instance is created. 
        /// </summary>
        /// <remarks>
        /// Only Object Subscriber is concerned by this event.
        /// </remarks>
        public event EventHandler<O2GEventArgs<OnPbxObjectInstanceCreatedEvent>> PbxObjectInstanceCreated;

        /// <summary>
        /// Occurs when a PBX object instance is deleted. 
        /// </summary>
        /// <remarks>
        /// Only Object Subscriber is concerned by this event.
        /// </remarks>
        public event EventHandler<O2GEventArgs<OnPbxObjectInstanceDeletedEvent>> PbxObjectInstanceDeleted;

        /// <summary>
        /// Occurs when a PBX object instance is modified. 
        /// </summary>
        /// <remarks>
        /// Only Object Subscriber is concerned by this event.
        /// </remarks>
        public event EventHandler<O2GEventArgs<OnPbxObjectInstanceModifiedEvent>> PbxObjectInstanceModified;

        /// <summary>
        /// Get the list of Pbx connected on this O2G server.
        /// </summary>
        /// <returns>
        /// A list of <see langword="int"/> That represents the Pbx node ID; Or <see langword="null"/> in case of error.
        /// </returns>
        Task<List<int>> GetPbxsAsync();

        /// <summary>
        /// Get the OmniPCX Enterprise specified by its node id.
        /// </summary>
        /// <param name="nodeId">The PCX Enterprise node id.</param>
        /// <returns>
        /// A <see cref="Pbx"/> object that represents the OmniPCX Enterprise node; Or <see langword="null"/> in case of error.
        /// </returns>
        Task<Pbx> GetPbxAsync(int nodeId);

        /// <summary>
        /// Get the description of the data model for the specified object on the specified OmniPCX Enterprise node.
        /// </summary>
        /// <param name="nodeId">The OmniPCX Enterprise node id.</param>
        /// <param name="objectName">The object name. This parameter is case sensitive</param>
        /// <returns>
        /// An <see cref="Model"/> object that describes the requested object model; Or <see langword="null"/> in case of error. 
        /// </returns>
        /// <remarks>
        /// If <c>objectName</c> is <see langword="null"/>, the global object model of the OmniPCX Enterprise node is returned.
        /// </remarks>
        Task<Model> GetObjectModelAsync(int nodeId, string objectName = null);


        /// <summary>
        /// Get the node(root) object.
        /// </summary>
        /// <param name="nodeId">The OmniPCX Enterprise node id.</param>
        /// <returns>
        /// A <see cref="PbxObject"/> that represents the node object.
        /// </returns>
        /// <example>
        /// <code>
        ///     PbxObject obj = await pbxManagementService.GetObjectAsync(5);
        /// </code>
        /// </example>
        /// <seealso cref="GetObjectAsync(int, string, string, string)"/>
        Task<PbxObject> GetNodeObjectAsync(int nodeId);


        /// <summary>
        /// Get the object specified by its instance definition and its instance id.
        /// </summary>
        /// <param name="nodeId">The OmniPCX Enterprise node id.</param>
        /// <param name="objectInstanceDefinition">The object instance definition.</param>
        /// <param name="objectId">The object instance id.</param>
        /// <param name="attributes">The object attributes to request.</param>
        /// <returns>
        /// A <see cref="PbxObject"/> that represents the requested object.
        /// </returns>
        /// <remarks>
        /// If <c>attributes</c> is not <see langword="null"/>, this method return the list of the specified attributes and the list of 
        /// sub-object paths of the current object. 
        /// <para>
        /// The <c>attributes</c> value is a comma separated object attribute name list: <c>"Station_Type,Directory_Number,..."</c>
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        ///     PbxObject obj = await pbxManagementService.GetObjectAsync(5, "Subscriber", "36530", "Station_Type,Directory_Number");
        /// </code>
        /// </example>
        /// <seealso cref="GetObjectAsync(int, string, string, List{PbxAttribute})"/>
        Task<PbxObject> GetObjectAsync(int nodeId, string objectInstanceDefinition, string objectId, string attributes = null);


        /// <summary>
        /// Get the object specified by its instance definition and its instance id.
        /// </summary>
        /// <param name="nodeId">The OmniPCX Enterprise node id.</param>
        /// <param name="objectInstanceDefinition">The object instance definition.</param>
        /// <param name="objectId">The object instance id.</param>
        /// <param name="attributes">The list of attributes that will be returned.</param>
        /// <returns>
        /// A <see cref="PbxObject"/> that represents the requested object.
        /// </returns>
        /// <remarks>
        /// This method return the list of the specified attributes and the list of sub-object paths of the current object. 
        /// </remarks>
        /// <seealso cref="GetObjectAsync(int, string, string, string)"/>
        Task<PbxObject> GetObjectAsync(int nodeId, string objectInstanceDefinition, string objectId, List<PbxAttribute> attributes);


        /// <summary>
        /// Get the object specified by its instance definition and its instance id.
        /// </summary>
        /// <param name="nodeId">The OmniPCX Enterprise node id.</param>
        /// <param name="objectInstanceDefinition">The object instance definition.</param>
        /// <param name="objectId">The object instance id.</param>
        /// <param name="attributes">The list of attributes that will be returned.</param>
        /// <returns>
        /// A <see cref="PbxObject"/> that represents the requested object.
        /// </returns>
        /// <remarks>
        /// This method return the list of the specified attributes and the list of sub-object paths of the current object. 
        /// </remarks>
        /// <seealso cref="GetObjectAsync(int, string, string, string)"/>
        Task<PbxObject> GetObjectAsync(int nodeId, string objectInstanceDefinition, string objectId, string[] attributes);


        /// <summary>
        /// Query the list of object instances that match the specified filter.
        /// </summary>
        /// <param name="nodeId">The OmniPCX Enterprise node id.</param>
        /// <param name="objectInstanceDefinition">The object instance definition.</param>
        /// <param name="filter">The <see cref="Filter"/> object that represents a filter on the object attribute.</param>
        /// <returns>
        /// A list of <see langword="string"/> that represents the instance of the found objects, or <see langword="null"/> in case of error or 
        /// if no instance match the specified filter.
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
        /// Query the list of object instances that match the specified filter.
        /// </summary>
        /// <param name="nodeId">The OmniPCX Enterprise node id.</param>
        /// <param name="objectInstanceDefinition">The object instance definition.</param>
        /// <param name="filter">The filter on the object attribute or null to return all the instances of the specified object.</param>
        /// <returns>
        /// A list of <see langword="string"/> that represents the instance of the found object, or <see langword="null"/> in case of error or 
        /// if no instance match the specified filter.
        /// </returns>
        /// <seealso cref="GetObjectInstancesAsync(int, string, Filter)"/>
        Task<List<string>> GetObjectInstancesAsync(int nodeId, string objectInstanceDefinition, string filter = null);

        /// <summary>
        /// Change one or several attribute values of the specified object.
        /// </summary>
        /// <param name="nodeId">The OmniPCX Enterprise node id.</param>
        /// <param name="objectInstanceDefinition">The object instance definition.</param>
        /// <param name="objectId">The object instance id.</param>
        /// <param name="attributes">The list of <see cref="PbxAttribute"/> to change.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise</returns>
        /// <remarks>
        /// If an update on the same object has been performed by other administrator since last operation a conflict error
        /// occurs and a GET operation must be done to allow the update, it avoids change done by other to be cancelled.
        /// </remarks>
        /// <example>
        /// <code>
        ///     List&lt;PbxAttribute> attrs = new();
        ///     attrs.Add(PbxAttribute.Create("Station_Type", "ANALOG"));
        ///     
        ///     if (! await pbxManagementService.SetObjectAsync(5, "Subscriber", "23100", attrs))
        ///     {
        ///         Console.WriteLine("Error");
        ///     }
        ///     
        /// </code>
        /// </example>
        Task<bool> SetObjectAsync(int nodeId, string objectInstanceDefinition, string objectId, List<PbxAttribute> attributes);

        /// <summary>
        /// Delete the specified instance of object.
        /// </summary>
        /// <param name="nodeId">The OmniPCX Enterprise node id.</param>
        /// <param name="objectInstanceDefinition">The object instance definition.</param>
        /// <param name="objectId">The object instance id.</param>
        /// <param name="forceDelete">Use the "<c>FORCED_DELETE</c>" action to delete the object.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// The "<c>FORCED_DELETE</c>" action is not available for all object. Check the availability in the <see cref="Model"/> corresponding to this object.
        /// This option can be used for exemple to delete a <c>Subscriber</c> having voice mails in his mail box.
        /// </remarks>
        Task<bool> DeleteObjectAsync(int nodeId, string objectInstanceDefinition, string objectId, bool forceDelete = false);


        /// <summary>
        /// Create a new object with the specified list of attributes
        /// </summary>
        /// <param name="nodeId">The OmniPCX Enterprise node id.</param>
        /// <param name="objectInstanceDefinition">The object instance definition.</param>
        /// <param name="attributes">The attributes.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        Task<bool> CreateObjectAsync(int nodeId, string objectInstanceDefinition, List<PbxAttribute> attributes);
    }
}
