using System.Collections.Generic;
using UnityEngine;

namespace Framework.Core
{
    /// <summary>
    /// BGM/SFX 재생을 담당하는 매니저.
    ///  - BGM: 전용 AudioSource 1개, 루프 재생.
    ///  - SFX: AudioSource 풀을 돌려쓰며 여러 효과음을 동시 재생.
    /// 사용: AudioManager.Instance.PlayBGM(clip);
    ///       AudioManager.Instance.PlaySFX(clip);
    /// </summary>
    public class AudioManager : MonoSingleton<AudioManager>
    {
        [Range(0f, 1f)] public float bgmVolume = 1f;
        [Range(0f, 1f)] public float sfxVolume = 1f;
        [SerializeField] private int sfxSourceCount = 8;

        private AudioSource _bgmSource;
        private readonly List<AudioSource> _sfxSources = new();
        private int _nextSfx; // 다음에 쓸 SFX 소스 인덱스 (라운드로빈)

        protected override void OnAwake()
        {
            // BGM 소스 1개 생성.
            _bgmSource = gameObject.AddComponent<AudioSource>();
            _bgmSource.loop = true;
            _bgmSource.playOnAwake = false;

            // SFX 소스 풀 생성.
            for (int i = 0; i < sfxSourceCount; i++)
            {
                var src = gameObject.AddComponent<AudioSource>();
                src.playOnAwake = false;
                _sfxSources.Add(src);
            }
        }

        public void PlayBGM(AudioClip clip)
        {
            if (clip == null) return;
            if (_bgmSource.clip == clip && _bgmSource.isPlaying) return; // 같은 곡이면 무시

            _bgmSource.clip = clip;
            _bgmSource.volume = bgmVolume;
            _bgmSource.Play();
        }

        public void StopBGM() => _bgmSource.Stop();

        public void PlaySFX(AudioClip clip)
        {
            if (clip == null) return;

            // 비어 있는 소스를 우선 찾고, 없으면 라운드로빈으로 덮어쓴다.
            var src = GetFreeSfxSource();
            src.volume = sfxVolume;
            src.PlayOneShot(clip);
        }

        private AudioSource GetFreeSfxSource()
        {
            foreach (var s in _sfxSources)
                if (!s.isPlaying) return s;

            // 전부 재생 중이면 가장 오래된 것(라운드로빈)을 재사용.
            var src = _sfxSources[_nextSfx];
            _nextSfx = (_nextSfx + 1) % _sfxSources.Count;
            return src;
        }
    }
}
