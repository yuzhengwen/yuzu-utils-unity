using UnityEngine;

namespace YuzuValen.Utils.BasicAudio
{
    public class PersistentSettings : MonoBehaviourSingleton<PersistentSettings>
    {
        private float _masterVolume, _musicVolume, _sfxVolume;
        private bool _vibrate, _notificationPermission;

        // Player Pref Keys
        public const string
            MasterVolumeKey = "MasterVolume",
            MusicVolumeKey = "MusicVolume",
            SfxVolumeKey = "SfxVolume",
            VibrateKey = "Vibrate",
            NotificationPermissionKey = "NotificationPermission";

        public float MasterVolume
        {
            get => _masterVolume;
            set
            {
                _masterVolume = value;
                PlayerPrefs.SetFloat(MasterVolumeKey, value);
                PlayerPrefs.Save();
            }
        }

        public float MusicVolume
        {
            get => _musicVolume;
            set
            {
                _musicVolume = value;
                PlayerPrefs.SetFloat(MusicVolumeKey, value);
                PlayerPrefs.Save();
            }
        }

        public float SfxVolume
        {
            get => _sfxVolume;
            set
            {
                _sfxVolume = value;
                PlayerPrefs.SetFloat(SfxVolumeKey, value);
                PlayerPrefs.Save();
            }
        }

        public bool Vibrate
        {
            get => _vibrate;
            set
            {
                _vibrate = value;
                PlayerPrefs.SetInt(VibrateKey, value ? 1 : 0);
                PlayerPrefs.Save();
            }
        }

        public bool NotificationPermission
        {
            get => PlayerPrefs.GetInt(NotificationPermissionKey, 0) == 1;
            set
            {
                _notificationPermission = value;
                PlayerPrefs.SetInt(NotificationPermissionKey, value ? 1 : 0);
                PlayerPrefs.Save();
            }
        }

        protected override void Awake()
        {
            base.Awake();
            _masterVolume = PlayerPrefs.GetFloat(MasterVolumeKey, 1f);
            _musicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 1f);
            _sfxVolume = PlayerPrefs.GetFloat(SfxVolumeKey, 1f);
            _notificationPermission = PlayerPrefs.GetInt(NotificationPermissionKey, 0) == 1;
            _vibrate = PlayerPrefs.GetInt(VibrateKey, 1) == 1;
        }
    }
}