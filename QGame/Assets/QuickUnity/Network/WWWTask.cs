using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System;

namespace QuickUnity
{
    public class WWWTask : CoroutineTask
    {
        public WWWTask(string url)
        {
            _url = url;
        }

        public WWWTask(string url, WWWForm form)
        {
            _url = url;
            _form = form;
        }

        public WWWTask(string url, byte[] postData)
        {
            _url = url;
            _postData = postData;
        }

        public WWWTask(string url, byte[] postData, Dictionary<string, string> headers)
        {
            _url = url;
            _postData = postData;
            _headers = headers;
        }

        protected override IEnumerator OnProcess()
        {
            if(string.IsNullOrEmpty(url))
            {
                SetFail("Invalid url");
                yield break;
            }

            WWW www = null;
            if(_postData != null)
            {
                if(_headers != null)
                {
                    www = new WWW(url, _postData, _headers);
                }
                else
                {
                    www = new WWW(url, _postData);
                }
            }
            else if(_form != null)
            {
                www = new WWW(url, _form);
            }
            else
            {
                www = new WWW(url);
            }

            while (!www.isDone)
            {
                SetProgress(www.uploadProgress + www.progress);
                if (hasTimeout)
                {
                    www.Dispose();
                    www = null;
                    yield break; 
                }
                else
                {
                    yield return null;
                }
            }

            if (!string.IsNullOrEmpty(www.error))
            {
                SetFail(www.error);
                www.Dispose();
                www = null;
                yield break;
            }

            _responseHeaders = www.responseHeaders;

            yield return OnProcessWWW(www);
            if (processor != null) processor(www);

            www.Dispose();
            www = null;
        }

        protected virtual IEnumerator OnProcessWWW(WWW www) { yield break; }

        public string url { get { return _url; } }
        protected string _url;

        public WWWForm form { get { return _form; } }
        protected WWWForm _form;

        public byte[] postData { get { return _postData; } }
        protected byte[] _postData;

        protected Dictionary<string, string> headers { get { return _headers; } }
        protected Dictionary<string, string> _headers;

        public WWWProcessor processor { get { return _processor; } }
        protected WWWProcessor _processor;

        public Dictionary<string, string> responseHeaders;
        protected Dictionary<string, string> _responseHeaders;

        public delegate System.Object WWWProcessor(WWW www);
    }

    public class WWWRequest : WWWTask
    {
        public WWWRequest(string url) : base(url) { }

        public WWWRequest(string url, WWWForm form) : base(url, form) { }

        public WWWRequest(string url, byte[] postData) : base(url, postData) { }

        public WWWRequest(string url, byte[] postData, Dictionary<string, string> headers) : base(url, postData, headers) { }

        protected override IEnumerator OnProcessWWW(WWW www)
        {
            _bytes = www.bytes;
            if (_bytes == null || _bytes.Length <= 0) { SetFail(string.Format("WWW can not read any bytes from {0}", _url)); }
            yield break;
        }

        public byte[] bytes { get { return _bytes; } }
        protected byte[] _bytes;
    }

    public class WWWReadTextTask : WWWTask
    {
        public WWWReadTextTask(string url)
            : base(url)
        {
            _encoding = Encoding.UTF8;
        }

        public WWWReadTextTask(string url, Encoding encoding)
            : base(url)
        {
            _encoding = encoding;
        }

        protected override IEnumerator OnProcessWWW(WWW www)
        {
            MemoryStream stream = null;
            StreamReader sr = null;
            try
            {
                stream = new MemoryStream(www.bytes);
                sr = new StreamReader(stream, Encoding.UTF8);
                _text = sr.ReadToEnd();
                if (string.IsNullOrEmpty(_text)) { SetFail(string.Format("WWW can not read any text from {0}", _url)); }
            }
            catch (System.Exception e)
            {
                SetFail(e);
            }
            finally
            {
                if (sr != null) { sr.Dispose(); sr = null; }
                if (stream != null) { stream.Dispose(); stream = null; }
            }
            yield break;           
        }

        public string text { get { return _text; } }
        protected string _text = string.Empty;

        public Encoding encoding { get { return _encoding; } }
        protected Encoding _encoding;
    }


    public class WWWReadBytesTask : WWWRequest
    {
        public WWWReadBytesTask(string url)
            : base(url)
        {
        }
    }

    public class WWWReadBundleTask : WWWTask
    {
        public WWWReadBundleTask(string url, bool isCheckMD5)
            : base(url)
        {
            _isCheckMD5 = isCheckMD5;
        }

        protected override IEnumerator OnProcessWWW(WWW www)
        {
            byte[] bytes = www.bytes;
            if(bytes != null)
            {
                AssetBundleCreateRequest request = AssetBundle.LoadFromMemoryAsync(bytes);
                yield return request;
                _assetBundle = request.assetBundle;
                if (isCheckMD5) _md5 = Utility.MD5.Compute(bytes);
            }
            if (_assetBundle == null) { SetFail(string.Format("WWW can not read AssetBundle from {0}", _url)); }
            yield break;
        }

        public AssetBundle assetBundle { get { return _assetBundle; } }
        protected AssetBundle _assetBundle;

        public string md5 { get { return _md5; } }
        protected string _md5;

        public bool isCheckMD5 { get { return _isCheckMD5; } }
        protected bool _isCheckMD5;
    }


    public class WWWDownloadTask : WWWTask
    {
        public WWWDownloadTask(
            string url, 
            string fileSavePath, 
            string expectMD5,
            int expectFileSize)
            : base(url)
        {
            _fileSavePath = fileSavePath;
            _expectMD5 = expectMD5;
            _expectFileSize = expectFileSize;
        }

        protected override IEnumerator OnProcessWWW(WWW www)
        {
            byte[] bytes = www.bytes;
            bool success = false;
            QFileStream fs = null;
            try
            {
                string tmpFile = _fileSavePath + QConfig.Network.tempDownloadFileSuffix;
                if (!FileManager.CreateDirectory(fileSavePath))
                {
                    SetFail(string.Format("Can Not Create Directory For {0}", fileSavePath));
                    yield break;
                }

                fs = new QFileStream(tmpFile, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
                int length = bytes.Length;

                fs.Write(bytes, 0, length);
                fs.Flush(true);

                _fileSize = length;

                success = true;

                /// check MD5
                /// Bugfix: Close File And Open Again. Maybe a little Probability Failed. It's Dangerous.
                fs.Seek(0, SeekOrigin.Begin);
                _md5 = Utility.MD5.Compute(fs);
                if (!string.IsNullOrEmpty(expectMD5) && _md5 != expectMD5)
                {
                    SetFail(string.Format("Check Md5 {0} Failed from {1}", tmpFile, _url));
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
            catch(Exception e)
            {
                SetFail(e);
                success = false;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Dispose();
                    fs = null;
                }
            }
            yield break;
        }

        public string fileSavePath { get { return _fileSavePath; } }
        protected string _fileSavePath;

        public string expectMD5 { get { return _expectMD5; } }
        protected string _expectMD5;

        public int expectFileSize { get { return _expectFileSize; } }
        protected int _expectFileSize = 0;

        public string md5 { get { return _md5; } }
        protected string _md5;

        public int fileSize { get { return _fileSize; } }
        protected int _fileSize = 0;

    }

}
