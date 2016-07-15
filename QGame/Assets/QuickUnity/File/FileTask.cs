using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;

namespace QuickUnity
{
    public class FileReadTask : AsyncTask
    {
        public FileReadTask(string filePath)
        {
            _filePath = filePath;
        }

        protected override void OnAsyncProcess()
        {
            if (string.IsNullOrEmpty(_filePath))
            {
                if (_filePath == null) _filePath = "";
                SetResultFailed(string.Format("Invalid file path \'{0}\'", _filePath));
                return;
            }
            if (!File.Exists(_filePath))
            {
                SetResultFailed(string.Format("File {0} not exist", _filePath));
                return;
            }


            QFileStream stream = null;
            try
            {
                stream = new QFileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                OnProcessStream(stream);
            }
            catch (System.Exception e)
            {
                SetResultFailed(e);
            }
            finally
            {
                if (stream != null) { stream.Dispose(); stream = null; }
            }
        }

        protected virtual void OnProcessStream(System.IO.Stream stream) { }

        public string filePath { get { return _filePath; } }
        protected string _filePath;
    }

    public class FileReadTextTask : FileReadTask
    {
        public FileReadTextTask(string filePath) : base(filePath)
        {
            _encoding = Encoding.UTF8;
        }

        public FileReadTextTask(string filePath, Encoding encoding) : base(filePath)
        {
            _encoding = encoding;
        }

        protected override void OnProcessStream(System.IO.Stream stream)
        {
            StreamReader sr = new StreamReader(stream, _encoding);
            _text = sr.ReadToEnd();
            if(sr != null) { sr.Dispose(); sr = null; }
        }

        
        public string text { get { return _text; } }
        protected string _text = string.Empty;

        public Encoding encoding { get { return _encoding; } }
        protected Encoding _encoding;
    }

    public class FileReadBytesTask : FileReadTask
    {
        public FileReadBytesTask(string filePath)
            : base(filePath)
        {
        }

        protected override void OnProcessStream(System.IO.Stream stream)
        {
            BinaryReader br = new BinaryReader(stream);
            br.BaseStream.Seek(0, SeekOrigin.Begin);
            _bytes = br.ReadBytes((int)br.BaseStream.Length);
            br.Close(); br = null;
            if (_bytes == null || _bytes.Length <= 0) SetResultFailed(string.Format("Can not read any bytes from {0}", _filePath));
        }

        public byte[] bytes { get { return _bytes; } }
        protected byte[] _bytes;
    }

}
