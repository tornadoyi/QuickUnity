using UnityEngine;
using System.Collections;
using System.Net;
using System.IO;

namespace QuickUnity
{
    public class HttpTask : AsyncTask
    {
        public HttpTask(string url)
        {
            _url = url;
        }

        public HttpTask(string url, int retryCount)
        {
            _url = url;
            _maxRetryCount = retryCount;
        }

        protected override void OnAsyncProcess()
        {
            for (int i = 0; i < _maxRetryCount; ++i)
            {
                // Check timeout
                if (HasTimeout()) break;

                // Clear previous result
                SetResultSucess();

                // Request
                DoRequest();
                _retryCount = i + 1;
                if (asyncResult) break;
            }
        }

        private void DoRequest()
        {
            HttpWebResponse response = null;
            try
            {
                // Check
                OnCheck();
                if (!asyncResult) return;

                // Process request 
                HttpWebRequest request = WebRequest.Create(_url) as HttpWebRequest;
                ProcessRequest(request);

                // Process response
                response = (HttpWebResponse)request.GetResponse();
                ProcessResponse(response);

            }
            catch (System.Exception e)
            {
                SetResultFailed(e);
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

        public string url { get { return _url; } }
        protected string _url;

        public int maxRetryCount { get { return _maxRetryCount; } }
        protected int _maxRetryCount = 1;

        public int retryCount { get { return _retryCount; } }
        protected int _retryCount = 0;
    }

    public class HttpDownloadTask : HttpTask
    {
        public HttpDownloadTask(
            string url,
            string fileSavePath,
            bool automaticDecompression,
            string internetProxy,
            string expectMD5,
            int expectFileSize,
            int retryCount) : base(url, retryCount)
        {
            _fileSavePath = fileSavePath;
            _internetProxy = internetProxy;
            _automaticDecompression = automaticDecompression;
            _expectMD5 = expectMD5;
            _expectFileSize = expectFileSize;
        }

        protected override void OnCheck()
        {
            if (!FileManager.CreateDirectory(fileSavePath))
            {
                SetResultFailed(string.Format("Can Not Create Directory For {0}", fileSavePath));
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
            if (!string.IsNullOrEmpty(_internetProxy))
                request.Proxy = new WebProxy(_internetProxy, true);
        }

        protected override void ProcessResponse(HttpWebResponse response)
        {
            bool success = false;
            QFileStream fs = null;
            Stream stream = null;
            try
            {
                stream = response.GetResponseStream();

                string tmpFile = _fileSavePath + QConfig.Network.tempDownloadFileSuffix;

                fs = new QFileStream(tmpFile, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);

                byte[] bytes = new byte[QConfig.Network.httpBufferSize];
                int length = stream.Read(bytes, 0, QConfig.Network.httpBufferSize);

                while (length > 0)
                {
                    fs.Write(bytes, 0, length);
                    _downloadSize += length;
                    length = stream.Read(bytes, 0, QConfig.Network.httpBufferSize);
                    fs.Flush(true);
                    Progress(_fileSize == 0 ? 0.0f : _downloadSize / _fileSize);
                }
                fs.Flush(true);
                _fileSize = length;

                success = true;

                /// check MD5
                /// Bugfix: Close File And Open Again. Maybe a little Probability Failed. It's Dangerous.
                fs.Seek(0, SeekOrigin.Begin);
                _md5 = Utility.MD5.Compute(fs);
                if (!string.IsNullOrEmpty(expectMD5) && _md5 != expectMD5)
                {
                    SetResultFailed(string.Format("Check Md5 {0} Failed from {1}", tmpFile, _url));
                    success = false;
                }

                fs.Dispose();
                fs = null;

                // change Name
                if (success)
                {
                    if (File.Exists(_fileSavePath))
                    {
                        File.Delete(_fileSavePath);
                    }

                    File.Move(tmpFile, _fileSavePath);
                }
            }
            catch (System.Exception e)
            {
                SetResultFailed(e);
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

        public string fileSavePath { get { return _fileSavePath; } }
        protected string _fileSavePath;

        public bool automaticDecompression { get { return _automaticDecompression; } }
        protected bool _automaticDecompression;

        public string internetProxy { get { return _internetProxy; } }
        protected string _internetProxy;

        public string expectMD5 { get { return _expectMD5; } }
        protected string _expectMD5;

        public int expectFileSize { get { return _expectFileSize; } }
        protected int _expectFileSize = 0;

        public string md5 { get { return _md5; } }
        protected string _md5;

        public int fileSize { get { return _fileSize; } }
        protected int _fileSize = 0;

        public int downloadSize { get { return _downloadSize; } }
        protected volatile int _downloadSize = 0;

        public int processBytes { get { return _downloadSize; } }


    }

}
