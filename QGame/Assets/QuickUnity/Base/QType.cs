using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace QuickUnity
{
	public class QList: List<System.Object>
	{
		public QList() { }

		public QList(int capacity) : base(capacity) { }

		public new int Capacity { get { return base.Capacity; } set { base.Capacity = value; } }

		public new int Count { get { return base.Count; } set { base.Capacity = Count; } }

		public new void Add(System.Object item) { base.Add(item); }

		public new int BinarySearch(System.Object item) { return base.BinarySearch(item); }

		public new void Clear() { base.Clear(); }

		public new bool Contains(System.Object item) { return base.Contains(item); }

		public new void CopyTo(System.Object[] array) { base.CopyTo(array); }

		public new void CopyTo(System.Object[] array, int arrayIndex) { base.CopyTo(array, arrayIndex); }

		public new void CopyTo(int index, System.Object[] array, int arrayIndex, int count) { base.CopyTo(index, array, arrayIndex, count); }

		public new List<System.Object> GetRange(int index, int count) { return base.GetRange(index, count); }

		public new int IndexOf(System.Object item) { return base.IndexOf(item); }

		public new int IndexOf(System.Object item, int index) { return base.IndexOf(item, index); }

		public new int IndexOf(System.Object item, int index, int count) { return base.IndexOf(item, index, count); }

		public new void Insert(int index, System.Object item) { base.Insert(index, item); }

		public new int LastIndexOf(System.Object item) { return base.LastIndexOf(item); }

		public new int LastIndexOf(System.Object item, int index) { return base.LastIndexOf(item, index); }

		public new int LastIndexOf(System.Object item, int index, int count) { return base.LastIndexOf(item, index, count); }

		public new bool Remove(System.Object item) { return base.Remove(item); }

		public new void RemoveAt(int index) { base.RemoveAt(index); }

		public new void RemoveRange(int index, int count) { base.RemoveRange(index, count); }

		public new void Reverse() { base.Reverse(); }

		public new void Reverse(int index, int count) { base.Reverse(index, count); }

		public new void Sort() { base.Sort(); }
	}


	public class QDictionary : Dictionary<System.Object, System.Object>
	{

		public QDictionary() { }

		public QDictionary(int capacity) : base(capacity){ }

		public new int Count { get { return base.Count; } }

		public new void Add(System.Object key, System.Object value) { base.Add(key, value); }

		public new void Clear() { base.Clear(); }

		public new bool ContainsKey(System.Object key) { return base.ContainsKey(key); }

		public new bool ContainsValue(System.Object value) { return base.ContainsValue(value); }

		public new bool Remove(System.Object key) { return base.Remove(key); }

		public new bool TryGetValue(System.Object key, out System.Object value) { return base.TryGetValue(key, out value); }
	}


	public class QBytes
	{
		public QBytes(string str)
		{
			data = string.IsNullOrEmpty(str) ? new byte[0] : System.Text.Encoding.UTF8.GetBytes(str);
		}

		public QBytes(byte[] bytes)
		{
			data = (bytes == null) ? new byte[0] : bytes;
		}

		public byte[] data {  get; private set;}
		public int length { get{ return data.Length;}}
	}
}