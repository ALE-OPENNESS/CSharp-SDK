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
using o2g.Internal.Events;
using o2g.Internal.Types.Management;
using o2g.Internal.Utility;
using o2g.Types.ManagementNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace o2g.Internal.Rest
{
    class PbxList
    {
        public List<string> NodeIds { get; init; }

        public List<int> ToIntNodesList()
        {
            if (NodeIds == null)
            {
                return null;
            }
            else
            {
                return NodeIds.Select(int.Parse).ToList();
            }
        }
    }


    class O2GPbxAttributeList
    {
        public List<O2GPbxAttribute> Attributes { get; set; }

        public O2GPbxAttributeList(List<PbxAttribute> attr)
        {
            Attributes = new();
            attr.ForEach((a) => Attributes.AddRange(PbxAttribute.From(a)));
        }
    }


    internal class PbxManagementRest : AbstractRESTService, IPbxManagement
    {
#pragma warning disable CS0067, CS0649
        [Injection]
        private readonly EventHandlers _eventHandlers;
#pragma warning restore CS0067, CS0649

        public event EventHandler<O2GEventArgs<OnPbxObjectInstanceCreatedEvent>> PbxObjectInstanceCreated
        {
            add => _eventHandlers.PbxObjectInstanceCreated += value;
            remove => _eventHandlers.PbxObjectInstanceCreated -= value;
        }

        public event EventHandler<O2GEventArgs<OnPbxObjectInstanceDeletedEvent>> PbxObjectInstanceDeleted
        {
            add => _eventHandlers.PbxObjectInstanceDeleted += value;
            remove => _eventHandlers.PbxObjectInstanceDeleted -= value;
        }

        public event EventHandler<O2GEventArgs<OnPbxObjectInstanceModifiedEvent>> PbxObjectInstanceModified
        {
            add => _eventHandlers.PbxObjectInstanceModified += value;
            remove => _eventHandlers.PbxObjectInstanceModified -= value;
        }


        public PbxManagementRest(Uri uri) : base(uri)
        {

        }

        public async Task<PbxObject> GetNodeObjectAsync(int nodeId)
        {
            Uri uriGet = uri.Append(
                AssertUtil.AssertPositive(nodeId, "nodeId").ToString(),
                "instances");

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);
            O2GPbxObject o2GPbxObject = await GetResult<O2GPbxObject>(response);
            if (o2GPbxObject == null)
            {
                return null;
            }
            else
            {
                return PbxObject.Build(o2GPbxObject);
            }
        }

        public async Task<PbxObject> GetObjectAsync(int nodeId, string objectInstanceDefinition, string objectId, string attributes)
        {
            Uri uriGet = uri.Append(
                AssertUtil.AssertPositive(nodeId, "nodeId").ToString(),
                "instances",
                AssertUtil.NotNullOrEmpty(objectInstanceDefinition, "objectInstanceDefinition"),
                AssertUtil.NotNullOrEmpty(objectId, "objectId"));

            if ((attributes != null) && (attributes.Length > 0))
            {
                uriGet = uriGet.AppendQuery("attributes", attributes);
            }

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);
            O2GPbxObject o2GPbxObject = await GetResult<O2GPbxObject>(response);
            if (o2GPbxObject == null)
            {
                return null;
            }
            else
            {
                return PbxObject.Build(o2GPbxObject);
            }
        }

        public async Task<PbxObject> GetObjectAsync(int nodeId, string objectInstanceDefinition, string objectId, List<PbxAttribute> attributes)
        {
            List<string> lstAttributes = AssertUtil.NotNull<List<PbxAttribute>>(attributes, "attributes").Select(a => a.Name).ToList();

            return await this.GetObjectAsync(
                nodeId,
                objectInstanceDefinition,
                objectId,
                string.Join(",", lstAttributes)
                );
        }

        public async Task<PbxObject> GetObjectAsync(int nodeId, string objectInstanceDefinition, string objectId, string[] attributes)
        {
            return await this.GetObjectAsync(
                nodeId,
                objectInstanceDefinition,
                objectId,
                string.Join(",", AssertUtil.NotNull<string[]>(attributes, "attributes"))
                );
        }


        public async Task<List<string>> GetObjectInstancesAsync(int nodeId, string objectInstanceDefinition, string filter)
        {
            Uri uriGet = uri.Append(
                AssertUtil.AssertPositive(nodeId, "nodeId").ToString(),
                "instances",
                AssertUtil.NotNullOrEmpty(objectInstanceDefinition, "objectInstanceDefinition"));

            if (filter != null)
            {
                uriGet = uriGet.AppendQuery("filter", filter);
            }

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);
            O2GPbxObjectIds objectIds = await GetResult<O2GPbxObjectIds>(response);
            if (objectIds == null)
            {
                return null;
            }
            else
            {
                return objectIds.ObjectIds;
            }
        }

        public Task<List<string>> GetObjectInstancesAsync(int nodeId, string objectInstanceDefinition, Filter filter)
        {
            return this.GetObjectInstancesAsync(nodeId, objectInstanceDefinition, filter.Value);
        }

        public async Task<Model> GetObjectModelAsync(int nodeId, string objectName = null)
        {
            Uri uriGet = uri.Append(AssertUtil.AssertPositive(nodeId, "nodeId").ToString(), "model");
            if (objectName != null)
            {
                uriGet = uriGet.Append(objectName);
            }

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);
            O2GObjectModel o2gObjectModel = await GetResult<O2GObjectModel>(response);
            if (o2gObjectModel == null)
            {
                return null;
            }
            else
            {
                return O2GObjectModel.Build(o2gObjectModel);
            }
        }

        public async Task<Pbx> GetPbxAsync(int nodeId)
        {
            HttpResponseMessage response = await httpClient.GetAsync(uri.Append(AssertUtil.AssertPositive(nodeId, "nodeId").ToString()));
            return await GetResult<Pbx>(response);
        }

        public async Task<List<int>> GetPbxsAsync()
        {
            HttpResponseMessage response = await httpClient.GetAsync(uri);
            PbxList pbxs = await GetResult<PbxList>(response);
            if (pbxs == null)
            {
                return null;
            }
            else
            {
                return pbxs.ToIntNodesList();
            }
        }

        public async Task<bool> SetObjectAsync(int nodeId, string objectInstanceDefinition, string objectId, List<PbxAttribute> attributes)
        {
            Uri uriPut = uri.Append(
                AssertUtil.AssertPositive(nodeId, "nodeId").ToString(),
                "instances",
                AssertUtil.NotNullOrEmpty(objectInstanceDefinition, "objectInstanceDefinition"),
                AssertUtil.NotNullOrEmpty(objectId, "objectId"));

            O2GPbxAttributeList pbxAttributes = new(attributes);

            var json = JsonSerializer.Serialize(pbxAttributes, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PutAsync(uriPut, content);
            return await IsSucceeded(response);
        }

        public async Task<bool> CreateObjectAsync(int nodeId, string objectInstanceDefinition, List<PbxAttribute> attributes)
        {
            Uri uriPost = uri.Append(
                AssertUtil.AssertPositive(nodeId, "nodeId").ToString(),
                "instances",
                AssertUtil.NotNullOrEmpty(objectInstanceDefinition, "objectInstanceDefinition"));

            O2GPbxAttributeList pbxAttributes = new(attributes);

            var json = JsonSerializer.Serialize(pbxAttributes, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, content);
            return await IsSucceeded(response);
        }

        public async Task<bool> DeleteObjectAsync(int nodeId, string objectInstanceDefinition, string objectId, bool forceDelete)
        {
            Uri uriDelete = uri.Append(
                AssertUtil.AssertPositive(nodeId, "nodeId").ToString(),
                "instances",
                AssertUtil.NotNullOrEmpty(objectInstanceDefinition, "objectInstanceDefinition"),
                AssertUtil.NotNullOrEmpty(objectId, "objectId"));

            if (forceDelete)
            {
                uriDelete.AppendQuery("force");
            }


            HttpResponseMessage response = await httpClient.DeleteAsync(uriDelete);
            return await IsSucceeded(response);
        }


    }
}
