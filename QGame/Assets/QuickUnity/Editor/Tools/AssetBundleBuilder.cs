using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Text;
using SimpleJson;
using System.IO;
using Excel;
using System.Data;

namespace QuickUnity
{
    public class AssetBundleBuilderWindow : EditorWindow
    {
        [MenuItem("QuickUnity/CI/AssetBundle Builder")]
        public static void ShowWindow()
        {
            //Show existing window instance. If one doesn't exist, make one.
            EditorWindow.GetWindow<AssetBundleBuilderWindow>();
        }


        void OnGUI()
        {
            builder.OnGUI();
        }

        protected BundleBuilder builder = new BundleBuilder();
    }

    public class BundleBuilder
    {
        public virtual void OnGUI()
        {
            // Workspace and Output path
            ShowPath();

            // Asset table name
            target = (BuildTarget)EditorGUILayout.EnumPopup("Bundle Target", target);

            // Build option
            buildOption = (BuildAssetBundleOptions)EditorGUILayout.EnumPopup("Bundle Option", buildOption);

            // Bundle Suffixes
            bundleSuffixes = EditorGUILayout.TextField("Bundle Suffixes", bundleSuffixes);

            // Ignore
            ShowIgnoreUI();

            // Extension transform
            ShowExtensionTransformUI();

            // Load config
            if (GUILayout.Button("Load config"))
            {
                configFilePath = EditorUtility.OpenFilePanel("Select config file", Application.dataPath, "xlsx");
                if (!string.IsNullOrEmpty(configFilePath))
                {
                    LoadConfig(configFilePath);
                }
            }


            // Build
            bool build = false;
            if (GUILayout.Button("Build"))
            {
                build = true;
            }
            if (build)
            {
                Build(new List<BuildTarget>(){ target });
            }

            ShowJobUI();
            
            GUI.enabled = true;
        }

        public void ShowPath()
        {
            using (QuickEditor.BeginHorizontal())
            {
                EditorGUILayout.TextField("Workspace", defaultWorkspace);
                if (GUILayout.Button("Open", GUILayout.MaxWidth(100)))
                {
                    defaultWorkspace = EditorUtility.OpenFolderPanel("Select workspace", Application.dataPath, "");
                }
            }

            using (QuickEditor.BeginHorizontal())
            {
                EditorGUILayout.TextField("Built in Output", defaultBuiltinOutputPath);
                if (GUILayout.Button("Open", GUILayout.MaxWidth(100)))
                {
                    defaultBuiltinOutputPath = EditorUtility.OpenFolderPanel("Select output path", Application.dataPath, "");
                }
            }

            using (QuickEditor.BeginHorizontal())
            {
                EditorGUILayout.TextField("External Output", defaultExternalOutputPath);
                if (GUILayout.Button("Open", GUILayout.MaxWidth(100)))
                {
                    defaultExternalOutputPath = EditorUtility.OpenFolderPanel("Select output path", Application.dataPath, "");
                }
            }
        }

        public void ShowIgnoreUI()
        {
            EditorGUILayout.LabelField("Ignore Extension");
            using (QuickEditor.BeginContents())
            {
                
                for(int i=0; i<ignoreExtList.Count; ++i)
                {
                    EditorGUILayout.TextField("Element"+i, ignoreExtList[i]);
                }
            }
        }

        protected void ShowExtensionTransformUI()
        {
            EditorGUILayout.LabelField("File Extension Transform");
            using (QuickEditor.BeginContents())
            {

                for (int i = 0; i < extReplaceList.Count; ++i)
                {
                    using(QuickEditor.BeginHorizontal())
                    {
                        EditorGUILayout.TextField(extReplaceList[i].Key);
                        EditorGUILayout.LabelField("===>", GUILayout.Width(40));
                        EditorGUILayout.TextField(extReplaceList[i].Value);
                    }
                    
                }
            }
        }

        protected void ShowJobUI()
        {
            System.Action<BundleData> show_bundle = (bundle) =>
            {
                using (QuickEditor.BeginHorizontal())
                {
                    if (bundle.isDependBundle)
                    {
                        EditorGUILayout.LabelField("[D]", GUILayout.Width(24));
                    }
                    else
                    {
                        using (QuickEditor.BeginVertical(GUILayout.Width(24)))
                        {
                            bundle.buildTag = EditorGUILayout.Toggle(bundle.buildTag);
                        }
                    }

                    using (QuickEditor.BeginVertical(GUILayout.Width(150)))
                    {
                        bundle.pathFoldout = EditorGUILayout.Foldout(bundle.pathFoldout, bundle.name);
                    }

                    EditorGUILayout.LabelField("location:" + bundle.location.ToString(), GUILayout.Width(100));

                    string variant = "variant:";
                    variant += string.IsNullOrEmpty(bundle.variant) ? "None" : bundle.variant;
                    EditorGUILayout.LabelField(variant, GUILayout.Width(100));
                }

                if (!bundle.pathFoldout) return;
                for (int k = 0; k < bundle.pathOrFile.Count; ++k)
                {
                    using (QuickEditor.BeginContents(20))
                    {
                        EditorGUILayout.LabelField(bundle.pathOrFile[k]);
                    }
                }
            };

            System.Action<BuildJob> show_job = (job) =>
            {
                using (QuickEditor.BeginHorizontal())
                {
                    using (QuickEditor.BeginVertical(GUILayout.Width(24)))
                    {
                        var selectAll = job.isSelectAll;
                        job.isSelectAll = EditorGUILayout.Toggle(job.isSelectAll);
                        if(selectAll != job.isSelectAll)
                        {
                            foreach (var bundle in job.bundleList)
                            {
                                bundle.buildTag = job.isSelectAll;
                            }
                        }
                    }
                    using (QuickEditor.BeginVertical(GUILayout.Width(24)))
                    {
                        job.isFoldout = EditorGUILayout.Foldout(job.isFoldout, job.name);
                    }
                } 
                if (!job.isFoldout) return;
                using (QuickEditor.BeginContents())
                {
                    foreach (var bundle in job.dependList)
                    {
                        show_bundle(bundle);
                    }
                    foreach (var bundle in job.bundleList)
                    {
                        show_bundle(bundle);
                    }
                }
            };

            using (QuickEditor.BeginScrollView(ref scrollPos))
            {
                for (int i = 0; i < jobList.Count; ++i)
                {
                    using (QuickEditor.BeginVertical("TextArea"))
                    {
                        BuildJob job = jobList[i];
                        show_job(job);
                    }
                }
            }
        }

        
        
        protected virtual bool LoadConfig(string outputPath)
        {
            // Read Excel
            FileStream stream = null;
            IExcelDataReader excelReader = null;
            try
            {
                stream = File.Open(outputPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);//File.OpenRead(outputPath);
                excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            }
            catch(Exception e)
            {
                Debug.LogError(e);
                if (stream != null) stream.Dispose();
                return false;
            }


            // Create read line functions
            DataSet result = excelReader.AsDataSet();
            DataRowCollection rows = result.Tables[0].Rows;
            int columns = result.Tables[0].Columns.Count;
            int rowCount = rows.Count;
            int curLine = 0;

            System.Action<string[], string, System.Action<string, DataRow>> _lineFromKeys = (keys, parent, callback) =>
            {
                string[] ks = new string[keys.Length];
                for(int i=0; i<keys.Length; ++i) { ks[i] = keys[i].ToLower(); }
                string p = parent != null ? parent.ToLower() : null;
                for (int i = curLine; i < rowCount; ++i)
                {
                    DataRow row = rows[i];
                    string originKey = row[0].ToString();
                    string curKey = originKey.ToLower();
                    if (curKey == p) break;
                    bool hasFind = false;
                    for(int j=0; j<ks.Length; ++j)
                    {
                        if (curKey != ks[j]) continue;
                        hasFind = true;
                        break;
                    }
                    if (!hasFind) continue;
                    curLine = i+1;
                    callback(originKey, row);
                    return;
                }
            };

            System.Action<string, string, System.Action<DataRow>> _line = (key, parent, callback) =>
            {
                _lineFromKeys(new string[] { key }, parent, (findKey, row)=>{callback(row);});
            };

            // Base config read
            _line("IgnoreFileSuffixes", null, (row) =>
            {
                ignoreExtList.Clear();
                for(int i=1; i<columns; ++i)
                {
                    if (string.IsNullOrEmpty(row[i].ToString())) break;
                    ignoreExtList.Add(row[i].ToString());
                }
            });
            _line("FileSuffixesReplace", null, (row) =>
            {
                extReplaceList.Clear();
                for (int i = 2; i < columns; i=i+2)
                {
                    string src = row[i-1].ToString();
                    string dst = row[i].ToString();
                    if (string.IsNullOrEmpty(src) || string.IsNullOrEmpty(dst)) break;
                    extReplaceList.Add(new KeyValuePair<string, string>(src, dst));
                }
            });
            _line("BundleSuffixes", null, (row) => { bundleSuffixes = row[1].ToString(); });
            

            // Job read
            jobList.Clear();
            do
            {
                bool next_job = false;
                _line("Job", null, (row_job) =>
                {
                    next_job = true;
                    BuildJob job = new BuildJob();
                    job.name = row_job[1].ToString();
                    jobList.Add(job);

                    do
                    {
                        bool next_bundle = false;
                        _lineFromKeys(new string[] { "Bundle", "Depend" }, "Job", (key, row_bundle) =>
                        {
                            int c = 1;
                            next_bundle = true;
                            BundleData bundle = new BundleData();
                            bundle.isDependBundle = key.ToLower() == "depend";
                            bundle.name = row_bundle[c++].ToString();
                            bundle.outputRelativePath = row_bundle[c++].ToString();
                            bundle.variant = row_bundle[c++].ToString();

                            var loc = row_bundle[c++].ToString();
                            bundle.location = loc == "builtin" ? QConfig.Asset.AssetPathType.StreamingAssets : bundle.location;

                            for (int i = c; i < columns; ++i )
                            {
                                string path = row_bundle[i].ToString();
                                if (string.IsNullOrEmpty(path)) break;
                                bundle.pathOrFile.Add(path);
                            }
                            if(bundle.isDependBundle)
                            {
                                job.dependList.Add(bundle);
                            }
                            else
                            {
                                job.bundleList.Add(bundle);
                            }
                        });
                        if (!next_bundle) break;
                    } while (true);
                    
                });
                if (!next_job) break;
            } while (true);

            stream.Dispose();
            return true;
        }


        protected virtual bool Build(List<BuildTarget> buildTargetList)
        {
            var task = new AssetBundleBuildTask(
                jobList,
                defaultBuiltinOutputPath,
                defaultExternalOutputPath,
                defaultWorkspace,
                buildTargetList,
                buildOption,
                ignoreExtList,
                extReplaceList,
                bundleSuffixes);

            bool result = task.Build();
            ClearBuildTag();
            return result;
        }


        protected void ClearBuildTag()
        {
            for(int i=0; i<jobList.Count; ++i)
            {
                var job = jobList[i];
                job.isSelectAll = false;

                for (int j=0; j<job.bundleList.Count; ++j)
                {
                    var bundle = job.bundleList[j];
                    bundle.buildTag = false;
                }
            }
        }

        protected BuildTarget target = BuildTarget.WebPlayer;

        protected BuildAssetBundleOptions buildOption = BuildAssetBundleOptions.None;

        protected List<string> ignoreExtList = new List<string>();

        protected List<KeyValuePair<string, string>> extReplaceList = new List<KeyValuePair<string, string>>();

        protected string bundleSuffixes = ".unity3d";

        protected List<BuildJob> jobList = new List<BuildJob>();

        protected Vector2 scrollPos;

        protected virtual string configFilePath { get; set; }

        protected virtual string defaultWorkspace { get; set; }

        protected virtual string defaultBuiltinOutputPath { get; set; }

        protected virtual string defaultExternalOutputPath { get; set; }


        protected class BuildJob
        {
            public string name = string.Empty;
            public List<BundleData> bundleList = new List<BundleData>();
            public List<BundleData> dependList = new List<BundleData>();
            public bool isFoldout = true;
            public bool isSelectAll = false;
        }

        protected class BundleData
        {
            public string name = "Default";
            public string outputRelativePath = string.Empty;
            public string variant = string.Empty;
            public QConfig.Asset.AssetPathType location = QConfig.Asset.AssetPathType.Server;

            public string md5 = string.Empty;
            public List<string> pathOrFile = new List<string>();
            public bool isDependBundle = false;

            public bool pathFoldout = false;
            public bool buildTag = false;
        }


        protected class AssetBundleBuildTask
        {
            public AssetBundleBuildTask(
                List<BuildJob> jobList,
                string builtinOutputPath,
                string externalOutputPath,
                string workspace,
                List<BuildTarget> buildTargetList,
                BuildAssetBundleOptions buildOption,
                List<string> ignoreExtList,
                List<KeyValuePair<string, string>> extReplaceList,
                string bundleSuffixes)
            {
                this.builtinOutputPath = builtinOutputPath;
                this.externalOutputPath = externalOutputPath;
                this.workspace = workspace;
                this.buildTargetList = buildTargetList;
                this.buildOption = buildOption;
                this.ignoreExtList = ignoreExtList;
                this.extReplaceList = extReplaceList;
                this.bundleSuffixes = bundleSuffixes;

                // Pick job need to build
                foreach(var job in jobList)
                {
                    bool needBuild = false;
                    foreach (BundleData data in job.bundleList)
                    {
                        if (!data.buildTag) continue;
                        needBuild = true;
                        break;
                    }
                    if (!needBuild) continue;
                    this.jobList.Add(job);
                }
            }

            public bool Build()
            {
                try
                {
                    // Check
                    if (!Check()) return false;

                    // Collect JobBuildCommand
                    var jobBuildList = new List<JobBuildCommand>();
                    foreach (var job in jobList)
                    {
                        var command = GenerateJobBuildCommand(job);
                        if (command == null) return false;
                        jobBuildList.Add(command);
                    }

                    // Build for all platform
                    foreach(var platform in buildTargetList)
                    {
                        // Clear output path
                        RecreateWorkspace();

                        // Build 
                        foreach (var command in jobBuildList)
                        {
                            if (!BuildOneJob(command, platform)) return false;
                        }
                        
                    }
                }
                catch(System.Exception e)
                {
                    Debug.LogError(e.ToString());
                    return false;
                }
                finally
                {
                    DeleteAllTempFiles();
                }
                return true;
            }

            protected bool Check()
            {
                // Check external output path
                if (!Directory.Exists(externalOutputPath))
                {
                    Debug.LogError(string.Format("Output path {0} is not exist", externalOutputPath));
                    return false;
                }

                // Check built in output path
                if (!Directory.Exists(builtinOutputPath))
                {
                    Debug.LogError(string.Format("Output path {0} is not exist", builtinOutputPath));
                    return false;
                }

                // Check files or path exist
                System.Func<string, bool> check_file_exist = (path) =>
                {
                    string fullPath = FileManager.PathCombine(FileManager.projectPath, path);
                    if (File.Exists(fullPath))
                    {
                        return true;
                    }
                    else if (Directory.Exists(fullPath))
                    {
                        return true;
                    }
                    else
                    {
                        Debug.LogError(string.Format("A path or file {0} can not be find", path));
                        return false;
                    }
                };
                foreach (BuildJob job in jobList)
                {
                    foreach (BundleData data in job.bundleList)
                    {
                        if (!data.buildTag && !data.isDependBundle) continue;
                        foreach(var path in data.pathOrFile)
                        {
                            if(!check_file_exist(path))
                            {
                                return false;
                            }
                        }
                    }
                }
                return true;
            }

            protected JobBuildCommand GenerateJobBuildCommand(BuildJob job)
            {
                var bundleBuildList = new List<KeyValuePair<BundleData, AssetBundleBuild>>();

                var buildList = new List<BundleData>();
                foreach (var data in job.dependList) buildList.Add(data);
                foreach (var data in job.bundleList) { if (data.buildTag) { buildList.Add(data); } }

                foreach (BundleData data in buildList)
                {
                    if (!data.buildTag && !data.isDependBundle) continue;

                    // Process path or files
                    var assetList = new List<string>();
                    foreach (string path in data.pathOrFile)
                    {
                        string fullPath = FileManager.PathCombine(FileManager.projectPath, path);
                        if (File.Exists(fullPath))
                        {
                            if (CheckFileIgnore(fullPath)) continue;
                            string processPath = ProcessFileExtension(fullPath);
                            string relativePath = FileManager.GetRelativePath(processPath, FileManager.projectPath);
                            assetList.Add(relativePath);
                        }
                        else if (Directory.Exists(fullPath))
                        {
                            // Get all files in directory
                            string[] files = FileManager.GetFilesFromDirectory(fullPath);
                            foreach (string file in files)
                            {
                                if (CheckFileIgnore(file)) continue;
                                string processPath = ProcessFileExtension(file);
                                string relativePath = FileManager.GetRelativePath(processPath, FileManager.projectPath);
                                assetList.Add(relativePath);
                            }
                        }
                        else
                        {
                            Debug.LogError(string.Format("A path or file {0} can not find", path));
                            return null;
                        }
                    }

                    UnityEditor.AssetBundleBuild build = new UnityEditor.AssetBundleBuild();
                    build.assetBundleName = FileManager.PathCombine(data.outputRelativePath, data.name) + bundleSuffixes;
                    if (!string.IsNullOrEmpty(data.variant)) build.assetBundleVariant = data.variant;
                    build.assetNames = assetList.ToArray();

                    bundleBuildList.Add(new KeyValuePair<BundleData, AssetBundleBuild>(data, build));
                };
                return new JobBuildCommand(job, bundleBuildList);
            }

            protected bool BuildOneJob(JobBuildCommand command, BuildTarget platform)
            {
                // Generate build map
                var buildList = new List<AssetBundleBuild>();
                foreach(var pair in command.bundleBuildList) { buildList.Add(pair.Value); }
                AssetBundleBuild[] buildMap = buildList.ToArray();

                // Build !!!
                StringBuilder buildInfo = new StringBuilder();
                buildInfo.Append("Build => ");
                foreach (var build in buildMap)
                {
                    buildInfo.Append(build.assetBundleName + " ");
                }
                Debug.Log(buildInfo.ToString());
                AssetBundleManifest manifest = UnityEditor.BuildPipeline.BuildAssetBundles(workspace, buildMap, buildOption, platform);
                if (manifest == null) return false;

                // Export
                var externalOutputRootPath = FileManager.PathCombine(externalOutputPath, platform.ToString());
                var builtinOutputRootPath = FileManager.PathCombine(builtinOutputPath, platform.ToString());

                foreach (var pair in command.bundleBuildList)
                {
                    var data = pair.Key;
                    var build = pair.Value;

                    // Get output path for this asset bundle
                    var outputRootPath = data.location == QConfig.Asset.AssetPathType.StreamingAssets ? builtinOutputRootPath : externalOutputRootPath;

                    // Copy bundle to output path
                    string bundleFileGeneratePath = FileManager.PathCombine(workspace, build.assetBundleName);
                    string bundleFileOutputPath = FileManager.PathCombine(outputRootPath, build.assetBundleName);
                    if (!FileManager.CreateDirectory(Path.GetDirectoryName(bundleFileOutputPath))) return false;
                    File.Copy(bundleFileGeneratePath, bundleFileOutputPath, true);

                    // Export to json from manifest
                    var json = ExportBundleConfig(bundleFileOutputPath, data, build, manifest, command.job);
                    if (json == null) return false;
                    var content = SimpleJson.SimpleJson.SerializeObject(json);
                    string jsonOutputPath = Path.ChangeExtension(bundleFileOutputPath, ".json");
                    FileManager.SaveFile(content, jsonOutputPath);
                }
                return true;
            }

            protected virtual SimpleJson.JsonObject ExportBundleConfig(string bundleFilePath, BundleData bundleData, AssetBundleBuild build, AssetBundleManifest manifest, BuildJob job)
            {
                var json = new SimpleJson.JsonObject();

                // Bundle Name
                json.Add("name", bundleData.name);

                // Relative path
                json.Add("relative_path", bundleData.outputRelativePath);

                // MD5
                byte[] bytes = FileManager.LoadBinaryFile(bundleFilePath);
                string md5 = Utility.MD5.Compute(bytes);
                json.Add("md5", md5);

                // Size
                json.Add("size", bytes.Length);

                // Assets
                var jAssets = new SimpleJson.JsonArray();
                json.Add("assets", jAssets);
                var assetBundle = AssetBundle.LoadFromMemory(bytes);
                var assetNames = build.assetNames;
                foreach (var name in assetNames)
                {
                    if (!assetBundle.Contains(name))
                    {
                        Debug.LogError(string.Format("Asset {0} can not contain in asset bundle {1}", name, build.assetBundleName));
                        continue;
                    }
                    jAssets.Add(name);
                }
                assetBundle.Unload(true);

                // Dependencies
                var depends = manifest.GetDirectDependencies(build.assetBundleName);
                if(depends.Length > 0)
                {
                    var jDepends = new SimpleJson.JsonArray();
                    json.Add("depends", jDepends);
                    foreach (var name in depends)
                    {
                        var nameNoSuffix = Path.GetFileNameWithoutExtension(name);
                        // get relative path from depend bundle data
                        BundleData dependData = null;
                        foreach (var data in job.dependList)
                        {
                            if (data.name.ToLower() != nameNoSuffix) continue;
                            dependData = data;
                            break;
                        }
                        if (dependData == null)
                        {
                            Debug.LogError(string.Format("depend bundle {0} not in depend list", name));
                            return null;
                        }
                        jDepends.Add(FileManager.PathCombine(dependData.outputRelativePath, dependData.name));
                    }
                }
                return json;
            }

            protected bool CheckFileIgnore(string fullPath)
            {
                // Check file ignore
                string fext = Path.GetExtension(fullPath);
                foreach (var ignoreExt in ignoreExtList)
                {
                    if (ignoreExt == fext)
                    {
                        return true;
                    }
                }
                return false;
            }

            protected string ProcessFileExtension(string fullPath)
            {
                string file = fullPath;
                string ext = Path.GetExtension(file);
                string newExt = null;
                for (int i = 0; i < extReplaceList.Count; ++i)
                {
                    if (extReplaceList[i].Key != ext) continue;
                    newExt = extReplaceList[i].Value;
                    break;
                }
                if (newExt == null) return file;
                string newFile = Path.ChangeExtension(file, newExt);
                tempFileList.Add(newFile);
                File.Copy(file, newFile);
                return newFile;
            }

            protected void DeleteAllTempFiles()
            {
                for (int i = 0; i < tempFileList.Count; ++i)
                {
                    string file = tempFileList[i];
                    File.Delete(file);
                }
                tempFileList.Clear();
            }

            protected bool RecreateWorkspace()
            {
                if (Directory.Exists(workspace)) Directory.Delete(workspace, true);
                var info = Directory.CreateDirectory(workspace);
                if (!info.Exists)
                {
                    Debug.LogError(string.Format("Create workspace {0} failed", workspace));
                    return false;
                }
                return true;
            }

            protected string externalOutputPath = string.Empty;

            protected string builtinOutputPath = string.Empty;

            protected string workspace = string.Empty;

            protected List<string> ignoreExtList = new List<string>();

            protected List<KeyValuePair<string, string>> extReplaceList = new List<KeyValuePair<string, string>>();

            protected List<BuildJob> jobList = new List<BuildJob>();

            protected List<BuildTarget> buildTargetList = new List<BuildTarget>();

            protected List<string> tempFileList = new List<string>();

            protected string bundleSuffixes = ".unity3d";

            protected BuildAssetBundleOptions buildOption = BuildAssetBundleOptions.None;


            protected class JobBuildCommand
            {
                public JobBuildCommand(BuildJob job, List<KeyValuePair<BundleData, AssetBundleBuild>> bundleBuildList)
                {
                    this.job = job;
                    this.bundleBuildList = bundleBuildList;
                }
                public BuildJob job;
                public List<KeyValuePair<BundleData, AssetBundleBuild>> bundleBuildList = new List<KeyValuePair<BundleData, AssetBundleBuild>>();
            }
        }
    }
}


