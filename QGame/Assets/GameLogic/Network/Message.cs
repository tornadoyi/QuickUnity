using UnityEngine;
using System.Collections;
using System;
using System.Text;

public class Message
{

    public byte[] bytes { get; private set; }
    public string content { get; private set; }

    public Message(byte[] bytes)
    {
        this.bytes = bytes;
        this.content = System.Text.Encoding.UTF8.GetString(this.bytes);
    }

    public Message(string content)
    {
        this.content = content;
        this.bytes = Encoding.UTF8.GetBytes(this.content);
    }
}
