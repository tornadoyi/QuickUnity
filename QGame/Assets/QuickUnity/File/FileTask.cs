using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;

namespace QuickUnity
{
    public class FileReadTask : AsyncTask
    {
        public class FileReadThreadTask : ThreadTask
        {
            public string filePath { get; set; }

            protected override void OnAsyncProcess()
            {
                if (string.IsNullOrEmpty(filePath))
                {
                    if (filePath == null) filePath = "";
                    SetFail(string.Format("Invalid file path \'{0}\'", filePath));
                    return;
                }
                if (!File.Exists(filePath))
                {
                    SetFail(string.Format("File {0} not exist", filePath));
                    return;
                }


                QFileStream stream = null;
                try
                {
                    stream = new QFileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                    OnProcessStream(stream);
                }
                catch (System.Exception e)
                {
                    SetFail(e);
                }
                finally
                {
                    if (stream != null) { stream.Dispose(); stream = null; }
                }
            }

            protected virtual void OnProcessStream(System.IO.Stream stream) { }
        }


        public string filePath { get; private set; }

        public FileReadTask(string filePath)
        {
            this.filePath = filePath;
        }

        protected override ThreadTask CreateThreadTask() { return new FileReadThreadTask(); }

        protected override void OnSyncParameters(ThreadTask threadTask)
        {
            var task = threadTask as FileReadThreadTask;
            task.filePath = filePath;
        }


    }

    public class FileReadTextTask : FileReadTask
    {
        public class FileReadTextThreadTask : FileReadThreadTask
        {
            public Encoding encoding { get; set; }
            public string text { get; set; }

            protected override void OnProcessStream(System.IO.Stream stream)
            {
                StreamReader sr = new StreamReader(stream, encoding);
                text = sr.ReadToEnd();
                if (sr != null) { sr.Dispose(); sr = null; }
            }
        }


        public string text { get; private set; }
        public Encoding encoding { get; private set; }

        public FileReadTextTask(string filePath) : base(filePath)
        {
            this.encoding = Encoding.UTF8;
        }

        public FileReadTextTask(string filePath, Encoding encoding) : base(filePath)
        {
            this.encoding = encoding;
        }

        protected override ThreadTask CreateThreadTask() { return new FileReadTextThreadTask(); }

        protected override void OnSyncParameters(ThreadTask threadTask)
        {
            var task = threadTask as FileReadTextThreadTask;
            base.OnSyncParameters(task);
            task.encoding = encoding;
            text = task.text;
        }
    }

    public class FileReadBytesTask : FileReadTask
    {
        public class FileReadBytesThreadTask : FileReadThreadTask
        {
            public byte[] bytes { get; private set; }

            protected override void OnProcessStream(System.IO.Stream stream)
            {
                BinaryReader br = new BinaryReader(stream);
                br.BaseStream.Seek(0, SeekOrigin.Begin);
                bytes = br.ReadBytes((int)br.BaseStream.Length);
                br.Close(); br = null;
                if (bytes == null || bytes.Length <= 0)
                {
                    SetFail(string.Format("Can not read any bytes from {0}", filePath));
                }
            }
        }


        public byte[] bytes { get; private set; }

        public FileReadBytesTask(string filePath)
            : base(filePath)
        {
        }

        protected override ThreadTask CreateThreadTask() { return new FileReadBytesThreadTask(); }

        protected override void OnSyncParameters(ThreadTask threadTask)
        {
            var task = threadTask as FileReadBytesThreadTask;
            base.OnSyncParameters(task);
            bytes = task.bytes;
        }
    }

}
