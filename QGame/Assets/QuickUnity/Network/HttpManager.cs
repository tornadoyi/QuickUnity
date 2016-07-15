using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace QuickUnity
{
    public class HttpManager : BaseManager<HttpManager>
    {
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
            int retryCount,
            float timeout)
        {
            var task = new HttpDownloadTask(
                url, fileSavePath, automaticDecompression, internetProxy, expectMD5, expectFileSize, retryCount);
            task.timeout = timeout;
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
            int expectFileSize)
        {
            if (QConfig.Network.donwloadMode == DownloadMode.WWW ||
                url.StartsWith("file://"))
            {
                var task = new WWWDownloadTask(url, fileSavePath, expectMD5, expectFileSize);
                task.timeout = QConfig.Network.downloadTimeout;
                AddTask(new RequestTask(task));
                return task;
            }
            else
            {
                var task = new HttpDownloadTask(
                    url, fileSavePath, false, string.Empty, expectMD5, expectFileSize, QConfig.Network.maxDownloadRetryCount);
                task.timeout = QConfig.Network.downloadTimeout;
                AddTask(new RequestTask(task));
                return task;
            }
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
            request.task.doneHandler += (result) => { runingList.Remove(request); TryRunTask(); };
            request.task.Start();
        }

        protected HashSet<RequestTask> runingList = new HashSet<RequestTask>();
        protected Queue<RequestTask> waitingList = new Queue<RequestTask>();

        protected class RequestTask
        {
            public RequestTask(WWWTask task)
            {
                type = RequestType.WWW;
                this.task = task;
            }

            public RequestTask(HttpTask task)
            {
                type = RequestType.HTTP;
                this.task = task;
            }

            public RequestType type { get; private set; }
            public Task task { get; private set; }

            public enum RequestType { WWW = 0, HTTP}
        }
    }
}
