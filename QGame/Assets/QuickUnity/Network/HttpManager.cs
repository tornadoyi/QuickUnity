using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace QuickUnity
{
    public class HttpManager : BaseManager<HttpManager>
    {
        protected class RequestTask
        {
            public RequestTask(Task task)
            {
                this.type = task is WWWTask ? RequestType.WWW : RequestType.HTTP;
                this.task = task;
            }

            public RequestType type { get; private set; }
            public Task task { get; private set; }

            public enum RequestType { WWW = 0, HTTP }
        }

        protected HashSet<RequestTask> runingList = new HashSet<RequestTask>();
        protected Queue<RequestTask> waitingList = new Queue<RequestTask>();

        protected override void OnDestroy()
        {
            runingList.Clear();
            waitingList.Clear();
            base.OnDestroy();
        }


        public static WWWReadBytesTask GetBytes(string url, float timeout)
        {
            var task = new WWWReadBytesTask(url);
            task.timeout = timeout;
            AddTask(new RequestTask(task));
            return task;
        }

        public static WWWReadTextTask GetText(string url, float timeout)
        {
            var task = new WWWReadTextTask(url);
            task.timeout = timeout;
            AddTask(new RequestTask(task));
            return task;
        }

        public static WWWReadTextTask GetText(string url, Encoding encoding, float timeout)
        {
            var task = new WWWReadTextTask(url, encoding);
            task.timeout = timeout;
            AddTask(new RequestTask(task));
            return task;
        }

        public static HttpDownloadTask Download(
            string url,
            string fileSavePath,
            bool automaticDecompression,
            string internetProxy,
            string expectMD5,
            int expectFileSize,
            float timeout)
        {
            var task = new HttpDownloadTask(
                url, fileSavePath, automaticDecompression, internetProxy, expectMD5, expectFileSize);
            task.timeout = timeout;
            task.retryCount = QConfig.Network.maxDownloadRetryCount;
            AddTask(new RequestTask(task));
            return task;
        }

        public static WWWDownloadTask Download(
           string url,
           string fileSavePath,
           string expectMD5,
           int expectFileSize,
           float timeout)
        {
            var task = new WWWDownloadTask(url, fileSavePath, expectMD5, expectFileSize);
            AddTask(new RequestTask(task));
            task.timeout = timeout;
            task.retryCount = QConfig.Network.maxDownloadRetryCount;
            return task;
        }

        public static Task Download(string url, string fileSavePath)
        {
            return Download(url, fileSavePath, string.Empty);
        }

        public static Task Download(string url, string fileSavePath, string expectMD5)
        {
            return Download(url, fileSavePath, expectMD5, 0);
        }

        public static Task Download(
            string url,
            string fileSavePath,
            string expectMD5,
            long expectFileSize)
        {
            Task task = null;
            if (QConfig.Network.donwloadMode == DownloadMode.WWW ||
                url.StartsWith("file://"))
            {
                task = new WWWDownloadTask(url, fileSavePath, expectMD5, expectFileSize);
            }
            else
            {
                task = new HttpDownloadTask(
                    url, fileSavePath, false, string.Empty, expectMD5, expectFileSize);
                
            }
            task.timeout = QConfig.Network.downloadTimeout;
            task.retryCount = QConfig.Network.maxDownloadRetryCount;
            AddTask(new RequestTask(task));
            return task;
        }

        public static WWWRequest Request(string url)
        {
            return Request(url, null);
        }

        public static WWWRequest Request(string url, byte[] postData)
        {
            return Request(url, postData, null);
        }

        public static WWWRequest Request(string url, byte[] postData, Dictionary<string, string> headers)
        {
            var task = new WWWRequest(url, postData, headers);
            AddTask(new RequestTask(task));
            return task;
        }



        protected static void AddTask(RequestTask request)
        {
            instance.waitingList.Enqueue(request);
            instance.TryRunTask();
        }

        protected void TryRunTask()
        {
            if (runingList.Count >= QConfig.Network.maxHttpRequestCount) return;
            if (waitingList.Count <= 0) return;
            var request = waitingList.Dequeue();
            runingList.Add(request);
            request.task.Finish((result) => { runingList.Remove(request); TryRunTask(); });
            request.task.Start();
        }

        
    }
}
