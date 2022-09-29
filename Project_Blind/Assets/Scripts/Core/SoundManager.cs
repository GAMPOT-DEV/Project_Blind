using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blind
{
    /// <summary>
    /// Sound를 관리하는 매니저
    /// BGM 재생 : SoundManager.Instance.Play(경로 or audioClip, Define.Sound.Bgm);
    /// EFFECT 재생 : SoundManager.Instance.Play(경로 or audioClip, Define.Sound.Effect);
    /// BGM 중지 : SoundManager.Instance.StopBGM();
    /// 한번 재생된 클립은 _audioClips 딕셔너리에 저장해서 다음번에 재생할때 빠르게 가져옴(캐싱)
    /// </summary>
    public class SoundManager : Manager<SoundManager>
    {
        AudioSource[] _audioSources = new AudioSource[(int)Define.Sound.MaxCount];
        float[] _volumes = new float[(int)Define.Sound.MaxCount];
        Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

        //private float _masterVolume = DataManager.Instance.GameData.mastetVolume;

        protected override void Awake()
        {
            base.Awake();
            Init();
        }

        public void Init()
        {
            string[] soundNames = System.Enum.GetNames(typeof(Define.Sound));
            for (int i = 0; i < soundNames.Length - 1; i++)
            {
                GameObject go = new GameObject { name = soundNames[i] };
                _audioSources[i] = go.AddComponent<AudioSource>();
                go.transform.parent = transform;
            }
            _volumes[(int)Define.Sound.Bgm] = DataManager.Instance.GameData.bgmVolume;
            _volumes[(int)Define.Sound.Effect] = DataManager.Instance.GameData.effectVolume;
            _audioSources[(int)Define.Sound.Bgm].loop = true;
            RefreshSound();
        }
        public void Clear()
        {
            foreach (AudioSource audioSource in _audioSources)
            {
                audioSource.clip = null;
                audioSource.Stop();
            }
            _audioClips.Clear();
        }
        public void Play(string path, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f)
        {
            AudioClip audioClip = GetOrAddAudioClip(path, type);
            Play(audioClip, type, pitch);
        }

        public void Play(AudioClip audioClip, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f)
        {
            if (audioClip == null) return;

            if (type == Define.Sound.Bgm)
            {
                AudioSource audioSource = _audioSources[(int)Define.Sound.Bgm];
                if (audioSource.isPlaying)
                    audioSource.Stop();
                audioSource.pitch = pitch;
                audioSource.clip = audioClip;
                audioSource.Play();
            }
            else
            {
                AudioSource audioSource = ResourceManager.Instance.Instantiate("SFX/SoundEffect")
                    .GetComponent<AudioSource>();
                audioSource.clip = audioClip;
                audioSource.volume = DataManager.Instance.GameData.effectVolume;
                StartCoroutine(PlayBgm(audioSource));
            }
        }
        private IEnumerator PlayBgm(AudioSource audioSource)
        {
            audioSource.Play();
            Debug.Log(audioSource.clip.length);
            yield return new WaitForSeconds(audioSource.clip.length);
            Destroy(audioSource.gameObject);
        }
        public void StopBGM()
        {
            _audioSources[(int)Define.Sound.Bgm].Stop();
            _audioSources[(int)Define.Sound.Bgm].clip = null;
        }

        public void StopEffect()
        {
            _audioSources[(int)Define.Sound.Effect].Stop();
            _audioSources[(int)Define.Sound.Effect].clip = null;
        }
        AudioClip GetOrAddAudioClip(string path, Define.Sound type = Define.Sound.Effect)
        {
            if (path.Contains("Sounds/") == false)
            {
                path = $"Sounds/{path}";
            }
            AudioClip audioClip = null;
            if (type == Define.Sound.Bgm)
            {
                audioClip = ResourceManager.Instance.Load<AudioClip>(path);
            }
            else
            {
                if (_audioClips.TryGetValue(path, out audioClip) == false)
                {
                    audioClip = ResourceManager.Instance.Load<AudioClip>(path);
                    _audioClips.Add(path, audioClip);
                }
            }

            if (audioClip == null)
            {
                Debug.Log($"AudioClip Missing! {path}");
            }

            return audioClip;
        }
        public void ChangeMasterVolume(float volume)
        {
            DataManager.Instance.GameData.mastetVolume = volume;
            //_masterVolume = volume;
            RefreshSound();
        }
        public void ChangeVolume(Define.Sound sound, float volume)
        {
            switch (sound)
            {
                case Define.Sound.Bgm:
                    DataManager.Instance.GameData.bgmVolume = volume;
                    break;
                case Define.Sound.Effect:
                    DataManager.Instance.GameData.effectVolume = volume;
                    break;
            }
            _volumes[(int)sound] = volume;
            _audioSources[(int)sound].volume = _volumes[(int)sound] * DataManager.Instance.GameData.mastetVolume;
        }
        private void RefreshSound()
        {
            for(int i = 0; i < (int)Define.Sound.MaxCount; i++)
            {
                _audioSources[i].volume = _volumes[i] * DataManager.Instance.GameData.mastetVolume;
            }
        }
    }
}

