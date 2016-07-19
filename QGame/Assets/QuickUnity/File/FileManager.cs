using UnityEngine;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using SLua;

namespace QuickUnity
{
    public class FileManager
    {

        [DoNotToLua]
        public static string projectPath
        {
#if UNITY_EDITOR
            get { return Directory.GetParent(Application.dataPath).FullName; }
#else
            get { Debug.LogError("Can not get project path in run time"); return string.Empty; }
#endif
        }



        /// Instead System.IO.Path.Combine, For we only use "/" as directory separator char 
        public static string PathCombine(string path1, string path2)
        {
            if (!string.IsNullOrEmpty(path1) && !string.IsNullOrEmpty(path2))
            {
                return string.Format("{0}/{1}", path1, path2);
            }
            else if (!string.IsNullOrEmpty(path1))
            {
                return path1;
            }
            else if (!string.IsNullOrEmpty(path2))
            {
                return path2;
            }
            else
            {
                return string.Empty;
            }    
        }

        public static string PathCombine(string path1, string path2, string path3)
        {
            return PathCombine(PathCombine(path1, path2), path3);
        }

        public static string GetAppUrl(string relativePath)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                return relativePath;
            }
            else
            {
                if (relativePath.Contains("file://"))
                {
                    return relativePath;
                }
                return PathCombine("file://", relativePath);
            }
        }

        public static bool CreateDirectory(string fileOrPathName)
        {
            try
            {
                string path;
                string ext = Path.GetExtension(fileOrPathName);
                if (string.IsNullOrEmpty(ext))
                {
                    path = fileOrPathName;
                }
                else
                {
                    path = Path.GetDirectoryName(fileOrPathName);
                }


                if (Directory.Exists(path))
                {
                    return true;
                }
                else
                {
                    var info = Directory.CreateDirectory(path);
                    if (info == null) return false;
                    return info.Exists;
                }
            }
            catch(System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public static bool DeleteDirectory(string fileOrPathName)
        {
            if (string.IsNullOrEmpty(fileOrPathName)) return false;
            if (!Directory.Exists(fileOrPathName)) return false;
            Directory.Delete(fileOrPathName, true);
            return true;
        }

        public static string[] GetFilesFromDirectory(string path, bool recursively = true)
        {
            string[] files = _GetFilesFromDirectory(path, recursively);
#if UNITY_EDITOR
            for (int i = 0; i < files.Length; ++i )
            {
                files[i] = files[i].Replace('\\', '/');
            }
#endif      
            return files;
        }

        public static string[] _GetFilesFromDirectory(string path, bool recursively = true)
        {
            if (!Directory.Exists(path))
            {
                Debug.LogError("Invalid path" + path);
                return null;
            }

            string[] files = Directory.GetFiles(path);
            if (!recursively) return files;

            string[] paths = Directory.GetDirectories(path);
            foreach (string innerPath in paths)
            {
                string[] innerFiles = _GetFilesFromDirectory(innerPath, recursively);
                if (innerFiles.Length == 0) continue;
                string[] merge = new string[files.Length + innerFiles.Length];
                files.CopyTo(merge, 0);
                innerFiles.CopyTo(merge, files.Length);
                files = merge;
            }

            return files;
        }

        public static string GetRelativePath(string fullPath, string relativeRootPath)
        {
            fullPath = fullPath.Replace("\\", "/");
            relativeRootPath = relativeRootPath.Replace("\\", "/");
            int st = fullPath.IndexOf(relativeRootPath);
            if (st == -1) return null;
            if (fullPath.Length == relativeRootPath.Length) return "";
            return fullPath.Substring(st + relativeRootPath.Length + 1);
        }

        public static string GetDependPath(string fullPath, string rootPath)
        {
            int st = fullPath.IndexOf(rootPath);
            if (st == -1) return null;
            int ed = st + rootPath.Length - 1;
            while (ed >= 0)
            {
                if (fullPath[ed] == '/' || fullPath[ed] == '\\')
                {
                    return fullPath.Substring(ed + 1);
                }
                --ed;
            }
            return rootPath;
        }

        public static int GetFileSize(string path)
        {
            if (!File.Exists(path))
                return 0;

            int length = 0;
            try
            {
                System.IO.FileInfo fi = new System.IO.FileInfo(path);
                length = System.Convert.ToInt32(fi.Length);
            }
            catch(System.Exception e)
            {
                Debug.LogException(e);
            }
            return length;
        }




        public static bool SaveFile(string data, string path)
        {
            if(string.IsNullOrEmpty(path))
            {
                Debug.LogError("Invalid path");
                return false;
            }

            if (!FileManager.CreateDirectory(path)) return false;

            bool ret = true;
            QFileStream fs = null;
            StreamWriter sw = null;
            try
            {
                fs = new QFileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
                sw = new StreamWriter(fs, Encoding.UTF8);
                sw.Write(data);
                fs.Flush(true);
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                ret = false;
            }
            finally
            {
                if (sw != null) { sw.Dispose(); sw = null; }
                if (fs != null) { fs.Dispose(); fs = null; }
            }

            return ret;
        }

        public static bool SaveFile(byte[] data, string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogError("Invalid path");
                return false;
            }

            if (!FileManager.CreateDirectory(path))
            {
                Debug.LogError("Create directory failed");
                return false;
            }

            bool ret = true;
            QFileStream fs = null;
            BinaryWriter bw = null;
            try
            {
                //SetNoBackupFlag(path);
                fs = new QFileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
                bw = new BinaryWriter(fs, Encoding.UTF8);
                bw.Write(data);
                fs.Flush(true);
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                ret = false;
            }
            finally
            {
                if (bw != null) { bw.Close(); bw = null; }
                if (fs != null) { fs.Dispose(); fs = null; }
            }
            return ret;
        }


        public static bool SaveFileAndCheck(byte[] data, string path, string expectMD5 = default(string))
        {
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogError("Invalid path");
                return false;
            }

            if (!FileManager.CreateDirectory(path))
            {
                Debug.LogError("Create directory failed");
                return false;
            }

            if(string.IsNullOrEmpty(expectMD5))
            {
                expectMD5 = Utility.MD5.Compute(data);
            }

            bool ret = true;
            QFileStream fs = null;
            try
            {
                //SetNoBackupFlag(path);
                // Write and flush
                fs = new QFileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
                fs.Write(data, 0, data.Length);
                fs.Flush(true);

                // Read and check
                fs.Seek(0, SeekOrigin.Begin);
                if (expectMD5 != Utility.MD5.Compute(fs))
                {
                    Debug.LogErrorFormat("File {0} write error, MD5 error", path);
                    ret = false;
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                ret = false;
            }
            finally
            {
                if (fs != null) { fs.Dispose(); fs = null; }
            }
            return ret;
        }


        public static string LoadTextFile(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogError("Invalid path");
                return null;
            }

            if (!File.Exists(path))
            {
                Debug.LogError("Can not find file " + path);
                return null;
            }

            string data = null;
            QFileStream fs = null;
            StreamReader sw = null;
            try
            {
                fs = new QFileStream(path, FileMode.Open, FileAccess.Read, FileShare.None);
                sw = new StreamReader(fs, Encoding.UTF8);
                data = sw.ReadToEnd();
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
            }
            finally
            {
                if (sw != null) { sw.Dispose(); sw = null; }
                if (fs != null) { fs.Dispose(); fs = null; }
            }

            return data;
        }


        public static byte[] LoadBinaryFile(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogError("Invalid path");
                return null;
            }

            if (!File.Exists(path))
            {
                Debug.LogError("Can not find file " + path);
                return null;
            }

            byte[] data = null;
            QFileStream fs = null;
            BinaryReader br = null;
            try
            {
                fs = new QFileStream(path, FileMode.Open, FileAccess.Read, FileShare.None);
                br = new BinaryReader(fs);
                br.BaseStream.Seek(0, SeekOrigin.Begin);
                data = br.ReadBytes((int)br.BaseStream.Length);
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
            }
            finally
            {
                if (br != null) { br.Close(); br = null; }
                if (fs != null) { fs.Dispose(); fs = null; }
            }

            return data;
        }


        public static string ComputeFileMD5(string path)
        {
            string md5 = string.Empty;
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogError("Invalid path");
                return md5;
            }

            QFileStream fs = null;
            try
            {
                fs = new QFileStream(path, FileMode.Open, FileAccess.Read, FileShare.None);
                fs.Seek(0, SeekOrigin.Begin);
                md5 = Utility.MD5.Compute(fs);
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
            }
            finally
            {
                if (fs != null) { fs.Dispose(); fs = null; }
            }
            return md5;
        }


        public static void SetNoBackupFlag(string _path)
        {
#if UNITY_IPHONE
            UnityEngine.iOS.Device.SetNoBackupFlag(_path);
#endif
        }

        /// <summary>
        /// Resets the no backup flag. [Only Work in iPhone/iPad]
        /// </summary>
        /// <param name="_path">_path.</param>
        public static void ResetNoBackupFlag(string _path)
        {
#if UNITY_IPHONE
            UnityEngine.iOS.Device.ResetNoBackupFlag(_path);
#endif
        }


        public static void CopyPath(string srcPath, string dstPath, string filters = null, System.Action<string, string> deal = null)
        {
            var files = Directory.GetFiles(srcPath, "*.*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                if (!string.IsNullOrEmpty(filters))
                {
                    var ext = Path.GetExtension(file);
                    if (!string.IsNullOrEmpty(ext) && filters.Contains(ext))
                    {
                        //Debug.LogFormat("filter {0} ", file);
                        continue;
                    }
                }
                var relativeFilePath = GetRelativePath(file, srcPath);
                var dstFilePath = Path.Combine(dstPath, relativeFilePath);
                var dir = Path.GetDirectoryName(dstFilePath);
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                File.Copy(file, dstFilePath, true);
                //Debug.LogFormat("{0} ==> {1}", file, dstFilePath);

                if (deal != null) deal.Invoke(dstFilePath, relativeFilePath);
            }
        }


        public static string FormatLinuxPathSeparator(string path)
        {
            if (string.IsNullOrEmpty(path)) return path;
            return path.Replace("\\", "/");
        }

        public static string GetFilePathWithoutExtension(string path)
        {
            if (string.IsNullOrEmpty(path)) return path;
            return PathCombine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path));
        }


    }
}

