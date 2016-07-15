using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace QuickUnity
{
    public class AudioEnginee : BaseManager<AudioEnginee>
    {
        public static AudioSource PlayBGM(AudioClip clip)
        {
            return PlayBGM(clip, null);
        }

        public static AudioSource PlayBGM(AudioClip clip, string sample)
        {
            if (clip == null)
            {
                Debug.LogError("Invalid audio clip");
                return null;
            }

            if(instance.bgm != null)
            {
                instance.bgm.Stop();
                Object.Destroy(instance.bgm);
            }
            instance.bgm = instance.gameObject.AddComponent<AudioSource>();
            if(!string.IsNullOrEmpty(sample))
            {
                ResetAudioSource(instance.bgm, sample);
            }
            instance.bgm.mute = muteBGM;
            return instance.bgm;
        }

        public static void PauseBGM()
        {
            if (instance.bgm == null) return;
            instance.bgm.Pause();
        }

        public static void StopBGM()
        {
            if (instance.bgm == null) return;
            instance.bgm.Stop();
            Object.Destroy(instance.bgm);
        }

        public static bool CreateAudioSourceSample(string name, AudioSourceSample sample)
        {
            if (string.IsNullOrEmpty(name) || sample == null)
            {
                Debug.LogError("Invalid arguments");
                return false;
            }

            if(instance.sampleDict.ContainsKey(name))
            {
                Debug.LogError(name + " sample has been set");
                return false;
            }

            instance.sampleDict.Add(name, sample);
            return true;
        }

        public static bool ResetAudioSource(AudioSource source)
        {
            if (source == null ) return false;
            source.mute = muteSE;
            return true;
        }

        public static bool ResetAudioSource(AudioSource source, string sampleName)
        {
            if (source == null) return false;
            if (string.IsNullOrEmpty(sampleName)) return ResetAudioSource(source);

            AudioSourceSample sample = null;
            if (!instance.sampleDict.TryGetValue(sampleName, out sample)) return false;

            source.mute = muteSE;
            source.bypassEffects = sample.bypassEffects;
            source.bypassListenerEffects = sample.bypassListenerEffects;
            source.bypassReverbZones = sample.bypassReverbZones;
            source.dopplerLevel = sample.dopplerLevel;
            source.ignoreListenerPause = sample.ignoreListenerPause;
            source.ignoreListenerVolume = sample.ignoreListenerVolume;
            source.loop = sample.loop;
            source.maxDistance = sample.maxDistance;
            source.minDistance = sample.minDistance;
            source.panStereo = sample.panStereo;
            source.pitch = sample.pitch;
            source.priority = sample.priority;
            source.reverbZoneMix = sample.reverbZoneMix;
            source.rolloffMode = sample.rolloffMode;
            source.spatialBlend = sample.spatialBlend;
            source.spatialize = sample.spatialize;
            source.spread = sample.spread;

            return true;
        }

        public static bool muteBGM
        {
            get { return instance._muteBGM; }
            set 
            {
                instance._muteBGM = value;
                instance.bgm.mute = value;
            }
        }
        protected bool _muteBGM = false;

        public static bool muteSE = false;


        protected AudioSource bgm;

        protected Dictionary<string, AudioSourceSample> sampleDict = new Dictionary<string, AudioSourceSample>();

    }


    public class AudioSourceSample
    {
        public bool bypassEffects = false;
        public bool bypassListenerEffects = false;
        public bool bypassReverbZones = false;
        public float dopplerLevel = 1f;
        public bool ignoreListenerPause = false;
        public bool ignoreListenerVolume = false;
        public bool loop = false;
        public float maxDistance = 500f;
        public float minDistance = 1f;
        public float panStereo = 0f;
        public float pitch = 1f;
        public int priority = 128;
        public float reverbZoneMix = 1f;
        public AudioRolloffMode rolloffMode = AudioRolloffMode.Logarithmic;
        public float spatialBlend = 0f;
        public bool spatialize = false;
        public float spread = 0f;
    }
}


