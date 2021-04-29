using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Sound
{
    const int kSoundEffectChannel = 4;

    const string kAudioMixer = "AudioMixer";
    const string kAudioMixerGroupBgm = "BGM";
    const string kAudioMixerGroupSe = "SE";

    enum Type
    {
        Bgm,
        Se,
    }

    class Data
    {
        public string key;

        public string resourceName;

        public AudioClip clip;

        public Data(string _key, string _resourceName)
        {
            key = _key;
            resourceName = _resourceName;

            Addressables.LoadAssetAsync<AudioClip>(_resourceName)
                .Completed += op =>
                {
                    if (op.Status != AsyncOperationStatus.Succeeded)
                    {
                        throw new System.Exception("failed to load AudioClip");
                    }

                    clip = op.Result;

                    Debug.LogFormat("AudioClip loaded. address={0}", resourceName);
                };
        }
    }

    GameObject m_object = null;

    AudioMixer m_audioMixer = null;
    AudioMixerGroup[] m_audioMixerGroupBgm;
    AudioMixerGroup[] m_audioMixerGroupSe;

    AudioSource m_sourceBgm = null;
    AudioSource m_sourceSeDefault = null;
    AudioSource[] m_sourceSeArray;

    Dictionary<string, Data> m_poolBgm = new Dictionary<string, Data>();
    Dictionary<string, Data> m_poolSe = new Dictionary<string, Data>();

    static Sound s_instance = null;

    public Sound()
    {
        m_sourceSeArray = new AudioSource[kSoundEffectChannel];
    }

    public IEnumerator InitializeCoroutine()
    {
        if (m_audioMixer == null)
        {
            var asyncOperationHandle = Addressables.LoadAssetAsync<AudioMixer>(kAudioMixer);

            yield return asyncOperationHandle;

            if (asyncOperationHandle.Status != AsyncOperationStatus.Succeeded)
            {
                throw new System.Exception("failed to load AudioMixer");
            }

            m_audioMixer = asyncOperationHandle.Result;

            m_audioMixerGroupBgm = m_audioMixer.FindMatchingGroups(kAudioMixerGroupBgm);
            m_audioMixerGroupSe = m_audioMixer.FindMatchingGroups(kAudioMixerGroupSe);
        }
    }

    public void LoadBgm(string key, string resourceName)
    {
        if (m_poolBgm.ContainsKey(key))
        {
            m_poolBgm.Remove(key);
        }

        m_poolBgm.Add(key, new Data(key, resourceName));
    }

    public void LoadSe(string key, string resourceName)
    {
        if (m_poolSe.ContainsKey(key))
        {
            m_poolSe.Remove(key);
        }

        m_poolSe.Add(key, new Data(key, resourceName));
    }

    public bool PlayBgm(string key)
    {
        if (!m_poolBgm.ContainsKey(key))
        {
#if UNITY_EDITOR
            throw new System.Exception("invalid bgm key");
#else
            return false;
#endif
        }

        StopBgm();

        var data = m_poolBgm[key];

        var audioSource = GetAudioSource(Type.Bgm);
        audioSource.loop = true;
        audioSource.clip = data.clip;
        audioSource.Play();

        return true;
    }

    public bool PlaySe(string key, int channel = -1)
    {
        if (!m_poolSe.ContainsKey(key))
        {
#if UNITY_EDITOR
            throw new System.Exception("invalid se key");
#else
            return false;
#endif
        }

        var data = m_poolSe[key];

        if (0 <= channel && channel < kSoundEffectChannel)
        {
            var audioSource = GetAudioSource(Type.Se, channel);

            audioSource.clip = data.clip;
            audioSource.Play();
        }
        else
        {
            var audioSource = GetAudioSource(Type.Se);

            // 複数の音を同時に再生できるようにするため、PlayOneShot を使用する。
            audioSource.PlayOneShot(data.clip);
        }

        return true;
    }

    public void StopBgm()
    {
        var audioSource = GetAudioSource(Type.Bgm);

        audioSource.Stop();
    }

    AudioSource GetAudioSource(Type type, int channel = -1)
    {
        if (m_object == null)
        {
            m_object = new GameObject("Sound");

            GameObject.DontDestroyOnLoad(m_object);

            m_sourceBgm = m_object.AddComponent<AudioSource>();
            m_sourceBgm.loop = true;
            m_sourceBgm.outputAudioMixerGroup = m_audioMixerGroupBgm[0];

            m_sourceSeDefault = m_object.AddComponent<AudioSource>();
            m_sourceSeDefault.loop = false;
            m_sourceSeDefault.outputAudioMixerGroup = m_audioMixerGroupSe[0];

            for (int i = 0; i < kSoundEffectChannel; i++)
            {
                m_sourceSeArray[i] = m_object.AddComponent<AudioSource>();
                m_sourceSeArray[i].loop = false;
                m_sourceSeArray[i].outputAudioMixerGroup = m_audioMixerGroupSe[0];
            }
        }

        if (type == Type.Bgm)
        {
            return m_sourceBgm;
        }
        else
        {
            if (0 <= channel && channel < kSoundEffectChannel)
            {
                return m_sourceSeArray[channel];
            }
            else
            {
                return m_sourceSeDefault;
            }
        }
    }

    public static Sound GetInstance()
    {
        if (s_instance == null)
        {
            s_instance = new Sound();
        }

        return s_instance;
    }
}
