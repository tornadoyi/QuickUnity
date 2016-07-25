using UnityEngine;
using System.Collections;
using System;

namespace QuickUnity
{
    public class SocketBuffer
    {
        public int maxCapacity { get; private set; }
        public int capacity { get; private set; }
        public int length { get; private set; }

        private byte[] content { get; set; }
        private int start { get; set; }
        private int end { get; set; }

        public SocketBuffer(int maxCapacity)
        {
            this.maxCapacity = maxCapacity;
            start = 0;
            end = -1;
        }

        public void Write(byte[] bytes)
        {
            int needLength = bytes.Length + length;
            if(needLength > capacity)
            {
                Realloc((int)(needLength * 1.5));
            }

            int firstCopyLength = capacity >= end + 1 + bytes.Length ? bytes.Length : capacity - end - 1;
            int secondCopyLength = bytes.Length - firstCopyLength;

            Buffer.BlockCopy(bytes, 0, content, end + 1, firstCopyLength);
            if(secondCopyLength > 0) Buffer.BlockCopy(bytes, 0, content, 0, secondCopyLength);

        }


        private bool Realloc(int capacity)
        {
            if (capacity < length)
            {
                throw new ArgumentException(string.Format("Capacity({0}) is less than length({1})", capacity, length));
            }
            if (capacity > maxCapacity)
            {
                throw new ArgumentException(string.Format("Capacity({0}) is bigger than max capacity({1})", capacity, maxCapacity));
            }

            this.capacity = capacity;
            var newContent = new byte[capacity];
            Buffer.BlockCopy(content, start, newContent, 0, end-start+1);
            content = newContent;
            start = 0;
            end = length - 1;

            return true;
        }
    }
}


