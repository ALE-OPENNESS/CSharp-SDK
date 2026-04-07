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

using o2g.Types.ManagementNS;
using o2g.Utility;
using System.Collections.Generic;

namespace o2g.Internal.Types.Management
{
    internal class O2GObjectModel
    {
        public string ObjectName { get; init; }
        public List<O2GAttributeModel> Attributes { get; init; }
        public List<O2GObjectModel> Objects { get; init; }
        public bool Hidden { get; init; }
        public bool Create { get; init; }
        public bool Delete { get; init; }
        public bool Set { get; init; }
        public bool Get { get; init; }
        public List<string> OtherActions { get; init; }


        static public Model Build(O2GObjectModel o2gObjModel)
        {
            // Create the Map Object
            Dictionary<string, Model> mapObjects = new();
            if (o2gObjModel.Objects != null)
            {
                foreach (O2GObjectModel o2gModel in o2gObjModel.Objects)
                {
                    Model objModel = Build(o2gModel);
                    mapObjects.Add(objModel.Name, objModel);
                }
            }

            Dictionary<string, ModelAttribute> mapAttributes = new();
            if (o2gObjModel.Attributes != null)
            {
                o2gObjModel.Attributes.ForEach(a => mapAttributes.Add(a.Name, a.ToAttributeModel(o2gObjModel.ObjectName)));
            }

            return new()
            {
                Name = o2gObjModel.ObjectName,
                Hidden = o2gObjModel.Hidden,
                Create = o2gObjModel.Create,
                Delete = o2gObjModel.Delete,
                Set = o2gObjModel.Set,
                OtherActions = o2gObjModel.OtherActions,
                Attributes = new StringMap<ModelAttribute>()
                {
                    Map = mapAttributes
                },
                Child = new StringMap<Model>()
                {
                    Map = mapObjects
                }
            };
        }
    }
}
