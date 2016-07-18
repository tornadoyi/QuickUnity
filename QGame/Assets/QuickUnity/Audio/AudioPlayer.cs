using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace QuickUnity
{
    public class AudioPlayer : MonoBehaviour
    {
        public static AudioPlayer Get(GameObject go)
        {
            var audio = go.GetComponent<AudioPlayer>();
            if (audio == null) audio = go.AddComponent<AudioPlayer>();
            return audio;
        }

        public AudioSource CreateAudioSource() { return CreateAudioSource(null); }

        public AudioSource CreateAudioSource(string sample)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            AudioEnginee.ResetAudioSource(source, sample);
            return source;
        }

        public AudioSourceInfo PlayAsync(string name) { return PlayAsync(name, null, null); }

        public AudioSourceInfo PlayAsync(string name, QEventHandler callback) { return PlayAsync(name, null, callback); }

        public AudioSourceInfo PlayAsync(string name, string sampleName) { return PlayAsync(name, sampleName, null); }

        public AudioSourceInfo PlayAsync(string name, string sampleName, QEventHandler callback)
        {
            CleanStoppedAudioSource();
            AudioSource source = gameObject.AddComponent<AudioSource>();
            var info = new AudioSourceInfo(source);
            audioList.AddLast(info);
            StartCoroutine(Load(name, source, sampleName, () => {
                info.loading = false;
                source.Play(); 
                if (callback != null) callback.Invoke(); 
            }));
            return info;
        }

        public void Pause(AudioSourceInfo info)
        {
            CleanStoppedAudioSource();
            if (info == null)
            {
                Debug.LogError("Invalid audio info");
                return;
            }
            info.source.Pause();
            info.pause = true;
        }

        public void UnPause(AudioSourceInfo info)
        {
            CleanStoppedAudioSource();
            if (info == null)
            {
                Debug.LogError("Invalid audio info");
                return;
            }
            info.source.UnPause();
            info.pause = false;
        }

        public void Stop(AudioSourceInfo info)
        {
            if (info == null)
            {
                Debug.LogError("Invalid audio info");
                return;
            }
            info.source.Stop();
            CleanStoppedAudioSource();
        }

        protected IEnumerator Load(string name, AudioSource source, string sampleName, QEventHandler callback)
        {
            do
            {
                if (string.IsNullOrEmpty(name)) break;
                var task = AssetManager.LoadAudioClipAsync(name);
                yield return task.WaitForFinish();

                if (task.clip == null) break;
                AudioEnginee.ResetAudioSource(source, sampleName);
                source.clip = task.clip;
            } while (false);

            if (callback != null) callback.Invoke();
        }

        protected void CleanStoppedAudioSource()
        {
            LinkedListNode<AudioSourceInfo> node = audioList.First;
            while (node != null)
            {
                LinkedListNode<AudioSourceInfo> next = node.Next;
                AudioSourceInfo info = node.Value;
                if (!info.source.isPlaying && !info.loading && !info.pause)
                {
                    info.source.Stop();
                    Destroy(info.source);
                    audioList.Remove(node);
                }
                node = next;
            }
        }

        protected LinkedList<AudioSourceInfo> audioList = new LinkedList<AudioSourceInfo>();
    }

    public class AudioSourceInfo
    {
        public AudioSourceInfo(AudioSource source)
        {
            this.source = source;
        }
        public AudioSource source;
        public bool pause = false;
        public bool loading = true;
    }
}


