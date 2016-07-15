using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;

namespace QuickUnity
{
    public class DataManager
    {
        public static void RegisterInfoManager<T, T1>(string dbName)
            where T : DataInfoManager, new()
            where T1 : DataInfo, new()
        {
            string mgrName = typeof(T).Name;
            string infoName = typeof(T1).Name;
            if (infoMgrMap.ContainsKey(mgrName))
            {
                Debug.LogError(string.Format("Repeated register, info manager {0}", mgrName));
            }
            else
            {
                var t = new T();
                infoMgrMap.Add(mgrName, t);
                info2Mgr.Add(infoName, mgrName);
                t.ReadDatas<T1>(dbName);
            }
        }

        public static T GetInfoMgr<T>() where T : DataInfoManager
        {
            string mgrName = typeof(T).Name;
            if (infoMgrMap.ContainsKey(mgrName))
            {
                return infoMgrMap[mgrName] as T;
            }
            else
            {
                return null;
            }
        }

        public static T GetInfo<T>(object key) where T : DataInfo
        {
            string infoName = typeof(T).Name;
            if (!info2Mgr.ContainsKey(infoName)) return null;
            string mgrName = info2Mgr[infoName];

            if (!infoMgrMap.ContainsKey(mgrName)) return null;
            DataInfoManager mgr = infoMgrMap[mgrName] as DataInfoManager;
            return mgr.GetInfo<T>(key);
        }

        public static void Purge()
        {
            infoMgrMap = null;
            info2Mgr = null;
            infoMgrMap = new Dictionary<string, DataInfoManager>();
            info2Mgr = new Dictionary<string, string>();
        }

        protected static Dictionary<string, DataInfoManager> infoMgrMap = new Dictionary<string, DataInfoManager>();
        protected static Dictionary<string, string> info2Mgr = new Dictionary<string, string>();
    }


    public class DataInfoManager
    {

        protected string version = null;
        protected int sumOfRow = 0;
        protected int sumOfCol = 0;


        protected Dictionary<object, DataInfo> dataMap = new Dictionary<object, DataInfo>();
        public Dictionary<object, DataInfo> GetInfoMap() { return this.dataMap; }

        public void ReadDatas<T>(string tablePath) where T : DataInfo, new()
        {

            string path = tablePath;

            // open
            FileStream fs = File.Open(path, FileMode.Open);
            BinaryReader br = new BinaryReader(fs);

            RowReader rr = new RowReader(br.ReadBytes((int)fs.Length));

            fs.Dispose();

            // read head 
            this.version = rr.ReadString();
            this.sumOfRow = rr.ReadInt();
            this.sumOfCol = rr.ReadInt();

            // read body
            for (int i = 0; i < sumOfRow; i++)
            {
                try
                {
                    // load
                    T info = new T();
                    info.LoadData(rr);
                    info.UpdateData();

                    // save
                    this.InsertData<T>(info);
                }
                catch (System.Exception ex)
                {
                    Debug.LogError(ex.ToString());
                }
            }

            br.Close();
            fs.Close();
        }

        public virtual void InsertData<T>(T data) where T : DataInfo
        {
            return;
        }

        public T GetInfo<T>(object key) where T : DataInfo
        {
            if (this.dataMap.ContainsKey(key))
                return this.dataMap[key] as T;
            else
                return null;
        }
    }

    public abstract class DataInfo
    {
        public int key = 0;


        public abstract void LoadData(RowReader br);
        public abstract void UpdateData();
    }

    public class RowInfo : DataInfo
    {
        public override void LoadData(RowReader br) { }
        public override void UpdateData() { }
    }


    public class RowReader
    {
        public RowReader(byte[] bytes)
        {
            context = bytes;
        }

        public byte[] context
        {
            get { return _context; }
            set
            {
                position = 0;
                _context = value;
            }
        }

        public byte ReadByte()
        {
            byte res = context[position];
            this.Shift(1);
            return res;
        }

        public int ReadInt()
        {
            int res = BitConverter.ToInt32(context, position);
            this.Shift(4);
            return res;
        }

        public double ReadDouble()
        {
            double res = BitConverter.ToDouble(context, position);
            this.Shift(8);
            return res;
        }

        public string ReadString()
        {
            int length = this.ReadInt();
            string res = System.Text.Encoding.UTF8.GetString(context, position, length);
            this.Shift(length);
            return res;
        }

        public bool ReadBool()
        {
            bool res = BitConverter.ToBoolean(context, position);
            this.Shift(1);
            return res;
        }

        public float ReadFloat()
        {
            float res = BitConverter.ToSingle(context, position);
            this.Shift(4);
            return res;
        }

        public Vector3 ReadVector3()
        {
            float x = ReadFloat();
            float y = ReadFloat();
            float z = ReadFloat();
            return new Vector3(x, y, z);
        }

        protected void Shift(int delta)
        {
            this.position += delta;
        }



        public bool endOfRead { get { return position >= _context.Length; } }

        protected int position = 0;
        protected byte[] _context;
    }
}

