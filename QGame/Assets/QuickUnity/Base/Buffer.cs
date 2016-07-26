using UnityEngine;
using System.Collections;
using System;

namespace QuickUnity
{
    public abstract class IBuffer 
    {
        public int capacity { get{ return bytes == null ? 0 : bytes.Length;} }

        protected byte[] bytes { get; set;}

        public IBuffer()
        {
            bytes = new byte[0];
        }

        public abstract void Clear();
    }

    public class AlignBuffer : IBuffer
    {
        new public byte[] bytes { get { return base.bytes;} private set { base.bytes = value;} }

        public int length { get; private set; }

        public int availableCapacity { get { return maxCapacity - length; } }

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

        public override void Clear()
        {
            this.length = 0;
        }

        public int Read(byte[] bytes) { return Read(bytes, 0, bytes.Length); }

        public int Read(byte[] bytes, int length) { return Read(bytes, 0, length); }

        public int Read(byte[] bytes, int offset, int length)
        {
            length = Seek(bytes, offset, length);
            this.length -= length;
            return length;
        }

        public int Read(int length)
        {
            length = Math.Min(length, this.length);
            this.length -= length;
            return length;
        }

        public int Seek(byte[] bytes) { return Seek(bytes, 0, bytes.Length); }

        public int Seek(byte[] bytes, int length) { return Seek(bytes, 0, length); }

        public int Seek(byte[] bytes, int offset, int length)
        {
            length = Math.Min(length, this.length);
            if (length == 0) return 0;
            Buffer.BlockCopy(this.bytes, 0, bytes, offset, length);
            return length;
        }

        public void Write(byte[] bytes, int offset, int length)
        {
            int sumLength = length + this.length;
            if(sumLength > maxCapacity)
            {
                throw new ArgumentException(string.Format("Out of max capacity({0})", maxCapacity));
            }
            if (sumLength > capacity)
            {
                Realloc(Math.Min((int)(sumLength * 1.5), maxCapacity));
            }

            Buffer.BlockCopy(bytes, offset, this.bytes, this.length, length);
            this.length += length;
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

        public int availableCapacity { get { return maxCapacity - length; } }

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

        public override void Clear()
        {
            this.length = 0;
            start = 0;
        }

        public int Read(byte[] bytes) { return Read(bytes, 0, bytes.Length); }

        public int Read(byte[] bytes, int length) { return Read(bytes, 0, length); }

        public int Read(byte[] bytes, int offset, int length)
        {
            length = Seek(bytes, offset, length);
            start = (start + length) % capacity;
            this.length -= length;
            return length;
        }

        public int Read(int length)
        {
            length = Math.Min(length, this.length);
            start = (start + length) % capacity;
            this.length -= length;
            return length;
        }

        public int Seek(byte[] bytes) { return Seek(bytes, 0, bytes.Length); }

        public int Seek(byte[] bytes, int length) { return Seek(bytes, 0, length); }

        public int Seek(byte[] bytes, int offset, int length)
        {
            length = Math.Min(length, this.length);
            if (length == 0) return 0;

            int firstCopyLength = start + length > capacity ? capacity - start : length;
            int secondCopyLength = length - firstCopyLength;

            Buffer.BlockCopy(this.bytes, start, bytes, offset, firstCopyLength);
            if (secondCopyLength > 0) Buffer.BlockCopy(this.bytes, 0, bytes, offset+firstCopyLength, secondCopyLength);

            return length;
        }

        public void Write(byte[] bytes) { Write(bytes, bytes.Length); }

        public void Write(byte[] bytes, int length) { Write(bytes, 0, length); }

        public void Write(byte[] bytes, int offset, int length)
        {
            int sumLength = length + this.length;
            if (sumLength > maxCapacity)
            {
                throw new ArgumentException(string.Format("Out of max capacity({0})", maxCapacity));
            }
            if (sumLength > capacity)
            {
                Realloc(Math.Min((int)(sumLength * 1.5), maxCapacity));
            }

            int copyStart = (start + this.length) % capacity;
            int firstCopyLength = copyStart + length > capacity ? capacity - copyStart : length;
            int secondCopyLength = length - firstCopyLength;

            Buffer.BlockCopy(bytes, offset, this.bytes, copyStart, firstCopyLength);
            if(secondCopyLength > 0) Buffer.BlockCopy(bytes, offset+firstCopyLength, this.bytes, 0, secondCopyLength);

            this.length += length;

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



