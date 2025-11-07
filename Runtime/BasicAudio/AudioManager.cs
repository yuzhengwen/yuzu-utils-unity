using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace YuzuValen.Utils.BasicAudio
{
    public class AudioManager : MonoBehaviourSingleton<AudioManager>
    {
        public AudioSource musicSource;
        public AudioSource sfxSource;
        public MusicClip[] musicClips;
        public NamedAudioClip[] sfxClips;

        public AudioMixer audioMixer;

        private void Start()
        {
            var savedMusicVolume = PersistentSettings.Instance.MusicVolume;
            var savedSfxVolume = PersistentSettings.Instance.SfxVolume;
            SetMusicVolume(savedMusicVolume);
            SetSfxVolume(savedSfxVolume);
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if (arg0.name == "GameScene")
            {
                PlayMusic(MusicType.Game);
            }
            else if (arg0.name == "StartScene" || arg0.name == "LevelScene")
            {
                PlayMusic(MusicType.Menu);
            }
            else if (arg0.name == "GameOverScene")
            {
                PlayMusic(MusicType.GameOver);
            }
        }

        public AudioClip GetMusicClip(MusicType type)
        {
            return musicClips.FirstOrDefault(c => c.type == type)?.clip;
        }

        public void PlayMusic(MusicType type, bool forceRestart = false)
        {
            var clip = GetMusicClip(type);
            if (clip == null)
            {
                Debug.LogWarning($"Music clip for type '{type}' not found!");
                return;
            }

            if (musicSource.isPlaying && musicSource.clip == clip && !forceRestart)
            {
                return; // Already playing the correct music
            }

            var musicVolume = GetMusicVolume();
            StartCoroutine(StartFade(musicSource, 1, 0));
            musicSource.clip = clip;
            musicSource.Play();
            StartCoroutine(StartFade(musicSource, 1, musicVolume));
        }

        private static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
        {
            float currentTime = 0f;
            float startVolume = audioSource.volume;

            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                float newVolume = Mathf.Lerp(startVolume, targetVolume, currentTime / duration);
                audioSource.volume = newVolume;
                yield return null;
            }

            audioSource.volume = targetVolume;
        }

        public void PlaySfx(string clipName, Transform transform = null, bool randomizePitch = false, float volumeEnhancement = 0)
        {
            if (transform == null)
            {
                transform = this.transform;
            }

            var clip = Array.Find(sfxClips, s => s.name == clipName)?.clip;
            if (clip == null)
            {
                Debug.LogWarning($"SFX clip '{clipName}' not found!");
                return;
            }

            var sfx = Instantiate(sfxSource, transform.position, Quaternion.identity, transform);
            sfx.clip = clip;
            sfx.pitch = randomizePitch ? UnityEngine.Random.Range(0.5f, 1.5f) : 1f;
            sfx.volume += volumeEnhancement;
            sfx.Play();
            Destroy(sfx.gameObject, clip.length + 0.1f); // Add a small buffer to ensure the clip finishes playing
        }

        public void SetMasterVolume(float volume)
        {
            audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
            PersistentSettings.Instance.MasterVolume = volume; // Save to persistent settings
        }

        public void SetMusicVolume(float volume)
        {
            audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
            PersistentSettings.Instance.MusicVolume = volume; // Save to persistent settings
        }

        public void SetSfxVolume(float volume)
        {
            audioMixer.SetFloat("SfxVolume", Mathf.Log10(volume) * 20);
            PersistentSettings.Instance.SfxVolume = volume; // Save to persistent settings
        }

        public float GetMusicVolume()
        {
            audioMixer.GetFloat("MusicVolume", out float volume);
            return Mathf.Pow(10, volume / 20);
        }

        public float GetSfxVolume()
        {
            audioMixer.GetFloat("SfxVolume", out float volume);
            return Mathf.Pow(10, volume / 20);
        }
    }

    public enum MusicType
    {
        Menu,
        Game,
        GameOver
    }

    [Serializable]
    public class NamedAudioClip
    {
        public string name;
        public AudioClip clip;

        public NamedAudioClip(string name, AudioClip clip)
        {
            this.name = name;
            this.clip = clip;
        }
    }

    [Serializable]
    public class MusicClip
    {
        public MusicType type;
        public AudioClip clip;
    }
}