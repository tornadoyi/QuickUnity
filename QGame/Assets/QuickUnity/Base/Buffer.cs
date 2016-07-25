using UnityEngine;
using System.Collections;
using System;

namespace QuickUnity
{
    public abstract class IBuffer 
    {
        public int capacity { get{ return bytes == null ? 0 : bytes.Length;} }

        protected byte[] bytes { get; set;}

    }

    public class AlignBuffer : IBuffer
    {
        new public byte[] bytes { get { return base.bytes;} private set { base.bytes = value;} }

        public int length { get; private set; }

        public int maxCapacity
        {
            get { return _maxCapacity;}
            set
            { 
                if(value < length) throw new ArgumentException(string.Format("Capacity({0}) is less than length({1})", value, length));
                _maxCapacity = value;
            }
        }
        private int _maxCapacity = int.MaxValue;

        public void Remove(int length)
        {
            int expectLength = Math.Min(this.length, length);
            Buffer.BlockCopy(bytes, expectLength, this.bytes, 0, this.length-expectLength);
            this.length -= expectLength;
        }

        public void Write(byte[] bytes, int offset, int writeLength)
        {
            if (bytes == null) return;
            int expectLength = Math.Min(writeLength, bytes.Length - offset);
            int needLength = expectLength + length;

            if (needLength > capacity)
            {
                Realloc((int)(needLength * 1.5));
            }

            Buffer.BlockCopy(bytes, offset, this.bytes, length, expectLength);
            length += expectLength;
        }

        private bool Realloc(int newCapacity)
        {
            if (newCapacity < length)
            {
                throw new ArgumentException(string.Format("Capacity({0}) is less than length({1})", capacity, length));
            }
            if (newCapacity > maxCapacity)
            {
                throw new ArgumentException(string.Format("Capacity({0}) is bigger than max capacity({1})", capacity, maxCapacity));
            }
                
            var newContent = new byte[newCapacity];
            Buffer.BlockCopy(this.bytes, 0, newContent, 0, length);
            this.bytes = newContent;

            return true;
        }
    }

    public class RecycleBuffer : IBuffer
    {
        public int length { get; private set; }

        public int maxCapacity
        {
            get { return _maxCapacity;}
            set
            { 
                if(value < length) throw new ArgumentException(string.Format("Capacity({0}) is less than length({1})", value, length));
                _maxCapacity = value;
            }
        }
        private int _maxCapacity = int.MaxValue;

        private int start { get; set; }

        public RecycleBuffer()
        {
            start = 0;
        }

        public byte[] Read(int length)
        {
            var bytes = Seek(length);
            start = (start + bytes.Length) % capacity;
            length -= bytes.Length;
            return bytes;
        }

        public byte[] Seek(int length)
        {
            int readLength = Math.Min(this.length, length);
            var bytes = new byte[readLength];

            int firstCopyLength = start + readLength > capacity ? capacity - start : readLength;
            int secondCopyLength = readLength - firstCopyLength;

            Buffer.BlockCopy(bytes, start, bytes, 0, firstCopyLength);
            if (secondCopyLength > 0) Buffer.BlockCopy(this.bytes, 0, bytes, firstCopyLength, secondCopyLength);

            return bytes;
        }

        public void Write(byte[] bytes)
        {
            Write(bytes, bytes.Length);
        }

        public void Write(byte[] bytes, int writeLength)
        {
            Write(bytes, 0, writeLength);
        }

        public void Write(byte[] bytes, int offset, int writeLength)
        {
            int expectLength = Math.Min(writeLength, bytes.Length - offset);
            int needLength = expectLength + length;
            if(needLength > capacity)
            {
                Realloc((int)(needLength * 1.5));
            }

            int copyStart = (start + length) % capacity;
            int firstCopyLength = copyStart + expectLength > capacity ? capacity - copyStart : expectLength;
            int secondCopyLength = expectLength - firstCopyLength;

            Buffer.BlockCopy(bytes, offset, this.bytes, copyStart, firstCopyLength);
            if(secondCopyLength > 0) Buffer.BlockCopy(bytes, offset+firstCopyLength, this.bytes, 0, secondCopyLength);

            length += expectLength;

        }


        private bool Realloc(int newCapacity)
        {
            if (newCapacity < length)
            {
                throw new ArgumentException(string.Format("Capacity({0}) is less than length({1})", capacity, length));
            }
            if (newCapacity > maxCapacity)
            {
                throw new ArgumentException(string.Format("Capacity({0}) is bigger than max capacity({1})", capacity, maxCapacity));
            }


            var newContent = new byte[newCapacity];

            int firstCopyLength = start + length > capacity ? capacity - start : length;
            int secondCopyLength = length - firstCopyLength;

            Buffer.BlockCopy(this.bytes, start, newContent, 0, firstCopyLength);
            if(secondCopyLength > 0) Buffer.BlockCopy(this.bytes, 0, newContent, firstCopyLength, secondCopyLength);

            this.bytes = newContent;
            start = 0;

            return true;
        }
    }
}



