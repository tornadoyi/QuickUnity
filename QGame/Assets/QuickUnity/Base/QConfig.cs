﻿using UnityEngine;
using System.Collections;

namespace QuickUnity
{
    public class QConfig
    {

        public class Network
        {
            // Max network buffer size
            public const int maxMessageSize = 1024 * 1024 * 1024;

            // Max received buffer for socket
            public const int socketBufferLength = 1024 * 1024 * 1024;

            // Poll timeout use received check, and effect on disconnect time
            public const float socketPollTimeout = 0.016f;

            // TCP connect timeout
            public const float tcpConnectTimeout = 10.0f;


            // Fetch file time from server base on WWW
            public static float downloadTimeout = 15.0f;

            // fetch file time from local base on WWW
            public static float wwwReadFileTimeout = 1.0f;

            // HttpWebRequest Timeout property
            public static float httpResponseTimeout = 15.0f;

            // HttpWebRequest ReadWriteTimeout property
            public static float httpReadWriteTimeout = 180.0f;

            // Temp file suffix when downloading
            public const string tempDownloadFileSuffix = ".tmp";

            // Read from http stream buffer size
            public static int httpBufferSize = 4096;

            // Retry count for Download
            public static int maxDownloadRetryCount = 3;

            // Max running count of WWW and HttpWebRequest 
            public static int maxHttpRequestCount = 10;

            // Use www or http to download
            public static DownloadMode donwloadMode = DownloadMode.HTTP;
        }
            

        public class Asset
        {
            public enum AssetPathType
            {
                StreamingAssets = 0,
                Resources = 1,
                AssetServer = 2,
            }

            public static bool loadAssetFromAssetBundle = true;

            public static bool useVersionAsFileName = true;

            public static string assetBundleSuffix = "unity3d";
        }

        public class Lua
        {
            public const string instanceFunctionName = "CreateByLuaComponent";
        }


        public class Symbol
        {
            public const string textLibrary = "Text";

            public const string assetLibrary = "Image";
        }
    }

    public enum DownloadMode { WWW = 0, HTTP = 1,}


    #region Events and handlers
    
	public delegate void QEventHandler();
    public delegate void QEventHandler1(System.Object parm1);
    public delegate void QEventHandler2(System.Object parm1, System.Object parm2);
    public delegate void QEventHandler3(System.Object parm1, System.Object parm2, System.Object parm3);
    public delegate void QEventHandler4(System.Object parm1, System.Object parm2, System.Object parm3, System.Object parm4);

    #endregion
}

