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
        public delegate System.Object WWWProcessor(WWW www);

        public string url { get; private set;}
        public WWWForm form { get; private set;}
        public byte[] postData { get; private set;}
        public WWWProcessor processor { get; private set;}
        public Dictionary<string, string> responseHeaders { get; private set;}

        protected Dictionary<string, string> headers { get; private set;}

        public WWWTask(string url)
        {
            this.url = url;
        }

        public WWWTask(string url, WWWForm form)
        {
            this.url = url;
            this.form = form;
        }

        public WWWTask(string url, byte[] postData)
        {
            this.url = url;
            this.postData = postData;
        }

        public WWWTask(string url, byte[] postData, Dictionary<string, string> headers)
        {
            this.url = url;
            this.postData = postData;
            this.headers = headers;
        }

        protected override IEnumerator OnProcess()
        {
            if(string.IsNullOrEmpty(url))
            {
                SetFail("Invalid url");
                yield break;
            }

            WWW www = null;
            if(postData != null)
            {
                if(headers != null)
                {
                    www = new WWW(url, postData, headers);
                }
                else
                {
                    www = new WWW(url, postData);
                }
            }
            else if(form != null)
            {
                www = new WWW(url, this.form);
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

            this.responseHeaders = www.responseHeaders;

            yield return OnProcessWWW(www);
            if (processor != null) processor(www);

            www.Dispose();
            www = null;
        }

        protected virtual IEnumerator OnProcessWWW(WWW www) { yield break; }


    }

    public class WWWRequest : WWWTask
    {
        public byte[] bytes { get; private set;}

        public WWWRequest(string url) : base(url) { }

        public WWWRequest(string url, WWWForm form) : base(url, form) { }

        public WWWRequest(string url, byte[] postData) : base(url, postData) { }

        public WWWRequest(string url, byte[] postData, Dictionary<string, string> headers) : base(url, postData, headers) { }

        protected override IEnumerator OnProcessWWW(WWW www)
        {
            bytes = www.bytes;
            if (bytes == null || bytes.Length <= 0) { SetFail(string.Format("WWW can not read any bytes from {0}", url)); }
            yield break;
        }
            
    }

    public class WWWReadTextTask : WWWTask
    {
        public string text { get; private set;}
        public Encoding encoding { get; private set;}

        public WWWReadTextTask(string url)
            : base(url)
        {
            encoding = Encoding.UTF8;
        }

        public WWWReadTextTask(string url, Encoding encoding)
            : base(url)
        {
            this.encoding = encoding;
        }

        protected override IEnumerator OnProcessWWW(WWW www)
        {
            MemoryStream stream = null;
            StreamReader sr = null;
            try
            {
                stream = new MemoryStream(www.bytes);
                sr = new StreamReader(stream, Encoding.UTF8);
                text = sr.ReadToEnd();
                if (string.IsNullOrEmpty(text)) { SetFail(string.Format("WWW can not read any text from {0}", url)); }
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
        public AssetBundle assetBundle { get; private set;}
        public string md5 { get; private set;}
        public bool isCheckMD5 { get; private set;}

        public WWWReadBundleTask(string url, bool isCheckMD5)
            : base(url)
        {
            this.isCheckMD5 = isCheckMD5;
        }

        protected override IEnumerator OnProcessWWW(WWW www)
        {
            byte[] bytes = www.bytes;
            if(bytes != null)
            {
                AssetBundleCreateRequest request = AssetBundle.LoadFromMemoryAsync(bytes);
                yield return request;
                assetBundle = request.assetBundle;
                if (isCheckMD5) md5 = Utility.MD5.Compute(bytes);
            }
            if (assetBundle == null) { SetFail(string.Format("WWW can not read AssetBundle from {0}", url)); }
            yield break;
        }
            
    }


    public class WWWDownloadTask : WWWTask
    {
        public string fileSavePath { get; private set;}
        public string expectMD5 { get; private set;}
        public long expectFileSize { get; private set;}
        public string md5 { get; private set;}
        public long fileSize { get; private set;}

        public WWWDownloadTask(
            string url, 
            string fileSavePath, 
            string expectMD5,
            long expectFileSize)
            : base(url)
        {
            this.fileSavePath = fileSavePath;
            this.expectMD5 = expectMD5;
            this.expectFileSize = expectFileSize;
        }

        protected override IEnumerator OnProcessWWW(WWW www)
        {
            byte[] bytes = www.bytes;
            bool success = false;
            QFileStream fs = null;
            try
            {
                string tmpFile = fileSavePath + QConfig.Network.tempDownloadFileSuffix;
                if (!FileManager.CreateDirectory(fileSavePath))
                {
                    SetFail(string.Format("Can Not Create Directory For {0}", fileSavePath));
                    yield break;
                }

                fs = new QFileStream(tmpFile, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
                int length = bytes.Length;

                fs.Write(bytes, 0, length);
                fs.Flush(true);

                fileSize = length;

                success = true;

                /// check MD5
                /// Bugfix: Close File And Open Again. Maybe a little Probability Failed. It's Dangerous.
                fs.Seek(0, SeekOrigin.Begin);
                md5 = Utility.MD5.Compute(fs);
                if (!string.IsNullOrEmpty(expectMD5) && md5 != expectMD5)
                {
                    SetFail(string.Format("Check {0} Md5  Failed from {1}", tmpFile, url));
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



    }

}
