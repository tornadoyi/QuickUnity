using UnityEngine;
using System.Collections;
using System.IO;


namespace QuickUnity
{
    public class QFileStream : System.IO.FileStream
    {
        // Note: when use SafeFileHandle, after this SafeFileHandle Dispose. The O/S handle will be close. It's Dangerous
        Microsoft.Win32.SafeHandles.SafeFileHandle m_InnerHandle;

        public QFileStream (string name, FileMode mode):base(name,mode){ }
        public QFileStream(string name, FileMode mode, FileAccess access, FileShare share)
            : base(name, mode, access, share) { }

        public void Flush(bool flushToDisk)
        {
            base.Flush();

            if (flushToDisk)
            {
                /// BUGFIX: After call to FileSream.SafeFileHandle, a new SafeFileHandle will create. After it Dispose. The O/S handle will be close. It's Dangerous
                if (m_InnerHandle == null)
                    m_InnerHandle = SafeFileHandle;

                //HeIO.Flush(m_InnerHandle.DangerousGetHandle());
                //HeWarpper.HeForceFlush(m_InnerHandle.DangerousGetHandle());
            }
        }

        public override void Close()
        {
            if (m_InnerHandle != null)
                m_InnerHandle.SetHandleAsInvalid();

            base.Close();
        }
    }
}

