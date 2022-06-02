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
        Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

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
            _audioSources[(int)Define.Sound.Bgm].loop = true;
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
                AudioSource audioSource = _audioSources[(int)Define.Sound.Effect];
                audioSource.pitch = pitch;
                audioSource.PlayOneShot(audioClip);
            }
        }
        public void StopBGM()
        {
            _audioSources[(int)Define.Sound.Bgm].Stop();
            _audioSources[(int)Define.Sound.Bgm].clip = null;
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
    }
}

