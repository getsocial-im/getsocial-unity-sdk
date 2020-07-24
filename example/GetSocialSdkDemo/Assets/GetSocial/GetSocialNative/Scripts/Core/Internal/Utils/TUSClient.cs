using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR
namespace GetSocialSdk.Core
{
    internal class TUSClient
    {
        // ***********************************************************************************************
        // Public
        //------------------------------------------------------------------------------------------------

        internal TUSClient()
        {

        }

        internal async Task<string> Create(string URL, Stream stream, Dictionary<string, string> metadata = null)
        {
            if (metadata == null)
            {
                metadata = new Dictionary<string, string>();
            }

            var requestUri = new Uri(URL);
            var client = new TUSHttpClient();

            var request = new TUSHttpRequest(URL);
            request.Method = "POST";
            request.AddHeader("Tus-Resumable", "1.0.0");
            request.AddHeader("Upload-Length", stream.Length.ToString());
            request.AddHeader("Content-Length", "0");

            var metadatastrings = new List<string>();
            foreach (var meta in metadata)
            {
                string k = meta.Key.Replace(" ", "").Replace(",", "");
                string v = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(meta.Value));
                metadatastrings.Add(string.Format("{0} {1}", k, v));
            }
            request.AddHeader("Upload-Metadata", string.Join(",", metadatastrings.ToArray()));

            var response = await client.PerformRequestAsync(request);

            if (response.StatusCode == HttpStatusCode.Created)
            {
                if (response.Headers.ContainsKey("Location"))
                {
                    Uri locationUri;
                    if (Uri.TryCreate(response.Headers["Location"], UriKind.RelativeOrAbsolute, out locationUri))
                    {
                        if (!locationUri.IsAbsoluteUri)
                        {
                            locationUri = new Uri(requestUri, locationUri);
                        }
                        return locationUri.ToString();
                    }
                    else
                    {
                        throw new Exception("Invalid Location Header");
                    }

                }
                else
                {
                    throw new Exception("Location Header Missing");
                }

            }
            else
            {
                throw new Exception("CreateFileInServer failed. " + response.ResponseString);
            }
        }
        //------------------------------------------------------------------------------------------------

        internal async Task<bool> Upload(string URL, Stream stream)
        {
            Debug.Log("uploading to " + URL);
            long Offset = 0;// this.getFileOffset(URL);
            var client = new TUSHttpClient();
            System.Security.Cryptography.SHA1 sha = new System.Security.Cryptography.SHA1Managed();
            int ChunkSize = (int)Math.Ceiling(3 * 1024.0 * 1024.0); //3 mb
            while (Offset < stream.Length)
            {
                Debug.Log("Uploading chunk, [" + Offset + " / " + stream.Length + "]");
                stream.Seek(Offset, SeekOrigin.Begin);
                byte[] buffer = new byte[ChunkSize];
                var BytesRead = stream.Read(buffer, 0, ChunkSize);

                Array.Resize(ref buffer, BytesRead);
                var sha1hash = sha.ComputeHash(buffer);

                var request = new TUSHttpRequest(URL);
                request.Method = "PATCH";
                request.AddHeader("Tus-Resumable", "1.0.0");
                request.AddHeader("Upload-Offset", string.Format("{0}", Offset));
                request.AddHeader("Upload-Checksum", "sha1 " + Convert.ToBase64String(sha1hash));
                request.AddHeader("Content-Type", "application/offset+octet-stream");
                request.BodyBytes = buffer;

                try
                {
                    var response = await client.PerformRequestAsync(request);
                    Debug.Log("uploading chunk result: " + response.StatusCode);
                    if (response.StatusCode == HttpStatusCode.NoContent)
                    {
                        Offset += BytesRead;
                        Debug.Log("Uploaded chunk = [" + Offset + "]");
                        if (Offset == stream.Length)
                        {
                            Debug.Log("File completely uploaded");
                            return true;
                        }
                    }
                    else
                    {
                        Debug.Log("Failed to write to server");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("Exception during upload: " + ex);
                }
            }
            return false;
        }

        //------------------------------------------------------------------------------------------------
        internal async Task<TUSHttpResponse> Download(string URL)
        {
            var client = new TUSHttpClient();

            var request = new TUSHttpRequest(URL);
            request.Method = "GET";

            TUSHttpResponse response = null;

            try
            {
                response = await client.PerformRequestAsync(request);
            } catch (WebException exception)
            {
                Debug.Log(exception);
            }

            return response;
        }
        //------------------------------------------------------------------------------------------------
        internal async Task<TUSHttpResponse> Head(string URL)
        {
            var client = new TUSHttpClient();
            var request = new TUSHttpRequest(URL);
            request.Method = "HEAD";
            request.AddHeader("Tus-Resumable", "1.0.0");

            try
            {
                var response = await client.PerformRequestAsync(request);
                return response;
            }
            catch (TUSException ex)
            {
                var response = new TUSHttpResponse();
                response.StatusCode = ex.statuscode;
                return response;
            }
        }
        //------------------------------------------------------------------------------------------------
        internal class TUSServerInfo
        {
            public string Version = "";
            public string SupportedVersions = "";
            public string Extensions = "";
            public long MaxSize = 0;

            public bool SupportsDelete
            {
                get { return this.Extensions.Contains("termination"); }
            }

        }

        internal async Task<TUSServerInfo> getServerInfo(string URL)
        {
            var client = new TUSHttpClient();
            var request = new TUSHttpRequest(URL);
            request.Method = "OPTIONS";

            var response = await client.PerformRequestAsync(request);

            if (response.StatusCode == HttpStatusCode.NoContent || response.StatusCode == HttpStatusCode.OK)
            {
                // Spec says NoContent but tusd gives OK because of browser bugs
                var info = new TUSServerInfo();
                response.Headers.TryGetValue("Tus-Resumable", out info.Version);
                response.Headers.TryGetValue("Tus-Version", out info.SupportedVersions);
                response.Headers.TryGetValue("Tus-Extension", out info.Extensions);

                string MaxSize;
                if (response.Headers.TryGetValue("Tus-Max-Size", out MaxSize))
                {
                    info.MaxSize = long.Parse(MaxSize);
                }
                else
                {
                    info.MaxSize = 0;
                }

                return info;
            }
            else
            {
                throw new Exception("getServerInfo failed. " + response.ResponseString);
            }
        }

        //------------------------------------------------------------------------------------------------
        // ***********************************************************************************************
        // Internal
        //------------------------------------------------------------------------------------------------
        private async Task<long> getFileOffset(string URL)
        {
            var client = new TUSHttpClient();
            var request = new TUSHttpRequest(URL);
            request.Method = "HEAD";
            request.AddHeader("Tus-Resumable", "1.0.0");

            var response = await client.PerformRequestAsync(request);
            Debug.Log("get offset response: " + response.ToString());
            if (response.StatusCode == HttpStatusCode.NoContent || response.StatusCode == HttpStatusCode.OK)
            {
                if (response.Headers.ContainsKey("Upload-Offset"))
                {
                    return long.Parse(response.Headers["Upload-Offset"]);
                }
                else
                {
                    throw new Exception("Offset Header Missing");
                }
            }
            else
            {
                throw new Exception("getFileOffset failed. " + response.ResponseString);
            }
        }
        // ***********************************************************************************************
    } // End of Class
} // End of Namespace
#endif