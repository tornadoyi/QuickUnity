using UnityEngine;
using System.Collections;
using System.Net;
using System.IO;

namespace QuickUnity
{
    public class HttpTask : AsyncTask
    {
        public class HttpThreadTask : ThreadTask
        {
            public string url { get; set; }

            protected override void OnAsyncProcess()
            {
                // Check timeout
                if (hasTimeout) return;

                // Request
                DoRequest();
            }

            private void DoRequest()
            {
                HttpWebResponse response = null;
                try
                {
                    // Check
                    OnCheck();
                    if (fail) return;

                    // Process request 
                    HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                    ProcessRequest(request);

                    // Process response
                    response = (HttpWebResponse)request.GetResponse();
                    ProcessResponse(response);

                }
                catch (System.Exception e)
                {
                    SetFail(e);
                    return;
                }
                finally
                {
                    if (response != null)
                    {
                        response.Close();
                        response = null;
                    }
                }
            }

            protected virtual void OnCheck() { }
            protected virtual void ProcessRequest(HttpWebRequest request) { }
            protected virtual void ProcessResponse(HttpWebResponse response) { }
        }

        public HttpTask(string url)
        {
            this.url = url;
        }

        protected override ThreadTask CreateThreadTask() { return new HttpThreadTask(); }

        protected override void OnSyncParameters(ThreadTask threadTask)
        {
            var task = threadTask as HttpThreadTask;
            task.url = url;
        }

        public string url { get; private set; }
    }

    public class HttpDownloadTask : HttpTask
    {
        public class HttpDownloadThreadTask : HttpThreadTask
        {
            public string fileSavePath { get; set; }
            public bool automaticDecompression { get; set; }
            public string internetProxy { get; set; }
            public string expectMD5 { get; set; }
            public int expectFileSize { get; set; }

            public string md5 { get; private set; }
            public int fileSize { get; private set; }
            public int downloadSize { get; private set; }


            protected override void OnCheck()
            {
                if (!FileManager.CreateDirectory(fileSavePath))
                {
                    SetFail(string.Format("Can Not Create Directory For {0}", fileSavePath));
                    return;
                }
            }

            protected override void ProcessRequest(HttpWebRequest request)
            {
                request.Timeout = (int)QConfig.Network.httpResponseTimeout * 1000;
                request.ReadWriteTimeout = (int)QConfig.Network.httpReadWriteTimeout * 1000;
                if (automaticDecompression)
                {
                    request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                }
                else
                {
                    request.AutomaticDecompression = DecompressionMethods.None;
                }

                /// Internet proxy if need
                if (!string.IsNullOrEmpty(internetProxy))
                    request.Proxy = new WebProxy(internetProxy, true);
            }

            protected override void ProcessResponse(HttpWebResponse response)
            {
                bool success = false;
                QFileStream fs = null;
                Stream stream = null;
                try
                {
                    stream = response.GetResponseStream();

                    string tmpFile = fileSavePath + QConfig.Network.tempDownloadFileSuffix;

                    fs = new QFileStream(tmpFile, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);

                    byte[] bytes = new byte[QConfig.Network.httpBufferSize];
                    int length = stream.Read(bytes, 0, QConfig.Network.httpBufferSize);

                    while (length > 0)
                    {
                        fs.Write(bytes, 0, length);
                        downloadSize += length;
                        length = stream.Read(bytes, 0, QConfig.Network.httpBufferSize);
                        fs.Flush(true);
                        SetProgress(expectFileSize == 0 ? 1.0f : downloadSize / expectFileSize);
                    }
                    fs.Flush(true);
                    fileSize = length;

                    success = true;

                    /// check MD5
                    /// Bugfix: Close File And Open Again. Maybe a little Probability Failed. It's Dangerous.
                    fs.Seek(0, SeekOrigin.Begin);
                    md5 = Utility.MD5.Compute(fs);
                    if (!string.IsNullOrEmpty(expectMD5) && md5 != expectMD5)
                    {
                        SetFail(string.Format("Check Md5 {0} Failed from {1}", tmpFile, url));
                        success = false;
                    }

                    fs.Dispose();
                    fs = null;

                    // change Name
                    if (success)
                    {
                        if (File.Exists(fileSavePath))
                        {
                            File.Delete(fileSavePath);
                        }

                        File.Move(tmpFile, fileSavePath);
                    }
                }
                catch (System.Exception e)
                {
                    SetFail(e);
                    success = false;
                }
                finally
                {
                    if (stream != null)
                    {
                        stream.Dispose();
                        stream = null;
                    }
                    if (fs != null)
                    {
                        fs.Dispose();
                        fs = null;
                    }
                }
            }
        }


        public string fileSavePath { get; private set; }
        public bool automaticDecompression { get; private set; }
        public string internetProxy { get; private set; }
        public string expectMD5 { get; private set; }
        public int expectFileSize { get; private set; }

        public string md5 { get; private set; }
        public int fileSize { get; private set; }
        public int downloadSize { get; private set; }


        public HttpDownloadTask(
            string url,
            string fileSavePath,
            bool automaticDecompression,
            string internetProxy,
            string expectMD5,
            int expectFileSize) : base(url)
        {
            this.fileSavePath = fileSavePath;
            this.internetProxy = internetProxy;
            this.automaticDecompression = automaticDecompression;
            this.expectMD5 = expectMD5;
            this.expectFileSize = expectFileSize;
        }

        protected override ThreadTask CreateThreadTask() { return new HttpDownloadThreadTask(); }

        protected override void OnSyncParameters(ThreadTask threadTask)
        {
            var task = threadTask as HttpDownloadThreadTask;
            base.OnSyncParameters(task);

            task.fileSavePath = fileSavePath;
            task.internetProxy = internetProxy;
            task.automaticDecompression = automaticDecompression;
            task.expectMD5 = expectMD5;
            task.expectFileSize = expectFileSize;

            md5 = task.md5;
            fileSize = task.fileSize;
            downloadSize = task.downloadSize;
        }

    }

}
