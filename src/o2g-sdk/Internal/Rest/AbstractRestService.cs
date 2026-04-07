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
using o2g.Internal.Services;
using o2g.Internal.Utility;
using o2g.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace o2g.Internal.Rest
{
    /// <summary>
    /// The abstract base class for REST services.
    /// </summary>
    public abstract class AbstractRESTService : IService
    {
        internal static JsonSerializerOptions serializeOptions = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = {
                  new JsonStringEnumMemberConverter()
            }
        };

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 
        [Injection]
        protected HttpClient httpClient;
        protected Uri uri;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        internal AbstractRESTService(Uri uri)
        {
            this.uri = uri;
        }

        private RestErrorInfo _lastError = null;

        /// <summary>
        /// Return a value indicating the lastest occured error.
        /// </summary>
        /// 
        public RestErrorInfo LastError { get => _lastError; }

        internal async Task<bool> IsSucceeded(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                SetLastError(null);
                return true;
            }
            else
            {
                try
                {
                    string jsonError = await response.Content.ReadAsStringAsync();
                    SetLastError(JsonSerializer.Deserialize<RestErrorInfo>(jsonError, serializeOptions));
                }
                catch (Exception)
                {
                    SetLastError(null);
                }
                return false;
            }
        }

        internal async Task<T> GetResult<T>(HttpResponseMessage response)
        {
            string jsonCode = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                SetLastError(null);

                if (jsonCode.Length == 0)
                {
                    // Case of no response found, ex: in Anlytics
                    return default(T);
                }
                else
                {
                    return JsonSerializer.Deserialize<T>(jsonCode, serializeOptions);
                }
            }
            else
            {
                try
                {
                    SetLastError(JsonSerializer.Deserialize<RestErrorInfo>(jsonCode, serializeOptions));
                }
                catch (Exception)
                {
                    SetLastError(null);
                }
                return default;
            }
        }


        private static string GetFilename(HttpResponseMessage response)
        {
            string contentDisposition = null;
            
            IEnumerable<string> values;
            if (response.Content.Headers.TryGetValues("Content-Disposition", out values))
            {
                contentDisposition = values.First();
            }

            if (contentDisposition != null)
            {
                ContentDisposition cd = new ContentDisposition(contentDisposition);
                if (cd.DispositionType == "attachment")
                {
                    return cd.FileName;
                }
            }

            return null;
        }

        internal async Task<string> DownloadFile(HttpResponseMessage response, string filePath, string extension = null)
        {
            if (response.IsSuccessStatusCode)
            {
                string downloadedFile = filePath;                
                if (downloadedFile == null)
                {
                    // Create a file on download directory.
                    // try to get file given in response if any
                    string fileName = GetFilename(response);
                    if (fileName != null)
                    {
                        downloadedFile = Path.Combine(FileUtil.GetSystemPath(SystemFolder.Downloads), fileName);
                    }
                    else
                    {
                        // Generate a file name
                        downloadedFile = Path.Combine(FileUtil.GetSystemPath(SystemFolder.Downloads), FileUtil.GetRandomFileName(10, extension));
                    }
                }

                FileStream fileStream = new(downloadedFile, FileMode.Create);
                await response.Content.CopyToAsync(fileStream);

                return downloadedFile;
            }
            else
            { 
                return null;
            }
        }


#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        protected internal void SetLastError(RestErrorInfo value)
        {
            _lastError = value;
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
