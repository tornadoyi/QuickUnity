using UnityEngine;
using System.Collections;
using System;

namespace QuickUnity
{
    public enum ErrorCode
    {
        // Base
        None = 0,
        Others = 0xffff,

        // Network [0x0100 - 0x01ff]
        SOCKET_INVALID_STATE            = 0x0100,

        SOCKET_URL_PASE_ERROR,           

        SOCKET_CONNECT_TIMEOUT,       
        
        SOCKET_DISCONNECT_DETECTED,
    }

    public struct Error 
    {
        public static Error none { get { return _none; } }
        private static Error _none = new Error(ErrorCode.None);

        public int code { get; private set; }
        public string description { get; private set; }
        public Exception exception { get; private set; }

        public Error(int code)
        {
            this.code = code;
            this.description = string.Empty;
            this.exception = null;
        }

        public Error(Exception exception)
        {
            this.code = (int)ErrorCode.Others;
            this.exception = exception;
            this.description = exception == null ? string.Empty : exception.ToString();
        }

        public Error(int code, string description, params object[] parms)
        {
            this.code = code;
            this.exception = null;
            this.description = string.IsNullOrEmpty(description) ? string.Empty : string.Format(description, parms);
        }

        public Error(int code, Exception exception)
        {
            this.code = code;
            this.exception = exception;
            this.description = exception == null ? string.Empty : exception.ToString();
        }

        public Error(int code, Exception exception, string description, params object[] parms)
        {
            this.code = code;
            this.description = string.IsNullOrEmpty(description) ? string.Empty : string.Format(description, parms);
            this.exception = exception;
        }

        public Error(ErrorCode code)
        {
            this.code = (int)code;
            this.description = string.Empty;
            this.exception = null;
        }

        public Error(ErrorCode code, string description, params object[] parms)
        {
            this.code = (int)code;
            this.exception = null;
            this.description = string.IsNullOrEmpty(description) ? string.Empty : string.Format(description, parms);
        }

        public Error(ErrorCode code, Exception exception)
        {
            this.code = (int)code;
            this.exception = exception;
            this.description = exception == null ? string.Empty : exception.ToString();
        }

        public Error(ErrorCode code, Exception exception, string description, params object[] parms)
        {
            this.code = (int)code;
            this.description = string.IsNullOrEmpty(description) ? string.Empty : string.Format(description, parms);
            this.exception = exception;
        }

        public static implicit operator Error(Exception e)
        {
            return new Error(e);
        }

    }



}


