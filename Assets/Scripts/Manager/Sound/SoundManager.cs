using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : ManagerBase
{
    private const string MASTER_NAME = "Master";
    private const string BGM_NAME = "BGM";
    private const string SFX_NAME = "SFX";
    private const string GUN_NAME = "GUN";

    private const string AUDIOMIXER_PATH = "AudioMixer/AudioMixer";
    private const string AUDIOSO_PATH = "AudioSO";

    private List<AudioSO> _audioSOList; 
    private Dictionary<string, AudioSO> _audioDic; 

    private AudioSource _bgmSource; 
    private List<AudioSource> _sfxSourceList; 

    private Dictionary<string, AudioSource> _loopSFXSourceDic; 

    private int maxOneAudioCount = 10; 
    private Dictionary<string, int> _audioCountDic; 

    [SerializeField]
    private int _sfxSourceCount;

    private AudioMixer _audioMixer;
    private AudioMixerGroup _bgmMixer; //bgmMixerGroup
    private AudioMixerGroup _sfxMixer; //sfxMixerGroup
    private AudioMixerGroup _gunMixer; //gunMixerGruop

    public static bool isSFXMute = false;
    public static bool isBGMMute = false;

    public override void Init()
    {
        _sfxSourceList = new List<AudioSource>(); 
        _audioDic = new Dictionary<string, AudioSO>();
        _loopSFXSourceDic = new Dictionary<string, AudioSource>();
        _audioCountDic = new Dictionary<string, int>();

        _audioMixer = Resources.Load<AudioMixer>(AUDIOMIXER_PATH); 
        AudioMixerGroup[] audioMixerGroups = _audioMixer.FindMatchingGroups(MASTER_NAME); 

        _bgmMixer = audioMixerGroups[1]; 
        _sfxMixer = audioMixerGroups[2];
        _gunMixer = audioMixerGroups[3];

        _audioSOList = Resources.LoadAll<AudioSO>(AUDIOSO_PATH).ToList(); 

        for (int i = 0; i < _audioSOList.Count; i++)
        {
            _audioDic.Add(_audioSOList[i].audioName, _audioSOList[i]); 
        }

        CreateAudioSource();

        Play("BGM", 0.5f);
    }


    private void CreateAudioSource()
    {
        CreateAudioSource(AudioType.BGM); 

        for (int i = 0; i < _sfxSourceCount; i++) 
        {
            CreateAudioSource(AudioType.SFX);
        }
    }


    public void Mute(AudioType type)
    {
        if (type.Equals(AudioType.BGM))
        {
            if (isBGMMute)
            {
                isBGMMute = false;
                Play("BGM", 0.5f);
            }
            else
            {
                
                    _bgmSource.Stop();
                    isBGMMute = true;

            }
    }
        else
        {
            if (!isSFXMute)
            {

                _sfxSourceList.ForEach(s => s.Stop());
                isSFXMute = true;
            }
            else
            {
                isSFXMute = false;
            }
        }
    }


    public void Play(string audioName, float volume = 1f)
    {
      

        if (_audioDic.TryGetValue(audioName, out AudioSO audioSO)) 
        {
            if (_audioCountDic.TryGetValue(audioName, out int cnt))
            {
                if (cnt >= maxOneAudioCount)
                {
                    return;
                }

                _audioCountDic[audioName]++;
            }
            else
            {
                _audioCountDic.Add(audioName, 1);
            }

            StartCoroutine(DelayEvent(audioName, audioSO.clip.length));

            if (audioSO.audioType == AudioType.GUN)
            {
                AudioSource sfxSource = FindEmptySFXSource();
                sfxSource.outputAudioMixerGroup = _gunMixer;

                sfxSource.loop = false;
                sfxSource.clip = audioSO.clip;
                sfxSource.Play();
            }
            else if (audioSO.audioType == AudioType.SFX)
            {
                if (isSFXMute)
                {
                    return;
                }

                AudioSource sfxSource = FindEmptySFXSource();

                sfxSource.loop = false;
                sfxSource.clip = audioSO.clip;
                sfxSource.volume = volume;
                sfxSource.Play();
            }
            else if (audioSO.audioType == AudioType.BGM)
            {
                if (isBGMMute)
                {
                    return;
                }
                _bgmSource.clip = audioSO.clip;
                _bgmSource.volume = volume;
                _bgmSource.Play();
            }
            else
            {
                Debug.LogError($"Check {audioName}'s AudioType");
            }
        }
        else
        {
            Debug.LogError($"{audioName} not Exist");
        }
    }

 
    public void PlayLoopSFX(string audioName, string key)
    {
        if (_audioDic.TryGetValue(audioName, out AudioSO audioSO)) 
        {
            if (_loopSFXSourceDic.ContainsKey(audioName))
            {
              
                return;
            }

            AudioSource sfxSource = FindEmptySFXSource();

            _loopSFXSourceDic.Add(key, sfxSource);

            sfxSource.loop = true;
            sfxSource.clip = audioSO.clip;
            sfxSource.Play();
        }
        else
        {
            Debug.LogError($"{audioName} not Exist");
        }
    }


    public void Play()
    {
        _bgmSource.Play();
        for (int i = 0; i < _sfxSourceList.Count; i++)
        {
            _sfxSourceList[i].Play();
        }
    }


    public void Pause()
    {
        _bgmSource.Pause();
        for (int i = 0; i < _sfxSourceList.Count; i++)
        {
            _sfxSourceList[i].Pause();
        }
    }


    public void Stop()
    {
        _bgmSource.Stop();
        for (int i = 0; i < _sfxSourceList.Count; i++)
        {
            _sfxSourceList[i].Stop();
        }
    }


    public void StopLoopSFX(string key)
    {
        if (_loopSFXSourceDic.TryGetValue(key, out AudioSource sfxSource))
        {
            sfxSource.Stop();
            _loopSFXSourceDic.Remove(key);
        }
    }


    public void VolumeControl(AudioType audioType, float value)
    {
        value = Mathf.Clamp(value, -40.0f, 0.0f);

        if (audioType == AudioType.MASTER)
        {
            _audioMixer.SetFloat(MASTER_NAME, value);
        }
        else if (audioType == AudioType.BGM)
        {
            _audioMixer.SetFloat(BGM_NAME, value);
        }
        else if (audioType == AudioType.GUN)
        {
            _audioMixer.SetFloat(GUN_NAME, value);
        }
        else if (audioType == AudioType.SFX)
        {
            _audioMixer.SetFloat(SFX_NAME, value);
        }
    }

    private AudioSource CreateAudioSource(AudioType audioType)
    {
        AudioSource audioSource = null;

        if (audioType == AudioType.BGM)
        {
            GameObject bgmObject = new GameObject(BGM_NAME); 
            bgmObject.transform.parent = this.transform; 

            _bgmSource = bgmObject.AddComponent<AudioSource>();
            _bgmSource.playOnAwake = false; 
            _bgmSource.loop = true; 

            _bgmSource.outputAudioMixerGroup = _bgmMixer; 
        }
        else if (audioType == AudioType.SFX)
        {
            GameObject sfxObject = new GameObject($"{SFX_NAME} ({_sfxSourceList.Count})"); 
            sfxObject.transform.parent = this.transform;

            audioSource = sfxObject.AddComponent<AudioSource>(); 
            audioSource.playOnAwake = false; 

            audioSource.outputAudioMixerGroup = _sfxMixer;

            _sfxSourceList.Add(audioSource); 
        }

        return audioSource; 
    }


    private AudioSource FindEmptySFXSource()
    {
        AudioSource sfxSource = null;

        for (int i = 0; i < _sfxSourceList.Count; i++) 
        {
            if (!_sfxSourceList[i].isPlaying)
            {
                sfxSource = _sfxSourceList[i];
                break;
            }
        }

        if (sfxSource == null) 
        {
            sfxSource = CreateAudioSource(AudioType.SFX);
        }

        return sfxSource;
    }

    private IEnumerator DelayEvent(string key, float audioDuration)
    {
        yield return new WaitForSeconds(audioDuration);

        if (_audioCountDic.ContainsKey(key)) _audioCountDic[key]--;
    }

    public override void Load()
    {
    }

    public override void UpdateState(eUpdateState state)
    {
        switch (state)
        {
            case eUpdateState.Init:
                Init();
                break;
        }
    }

    private void OnApplicationQuit()
    {
        int a = isBGMMute ? 1 : 0;
        PlayerPrefs.SetInt("isBGMMute", a);
        a = isSFXMute ? 1 : 0;
        PlayerPrefs.SetInt("isSFXMute", a);
    }
}
