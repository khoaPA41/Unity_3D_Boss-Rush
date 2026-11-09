using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Managers
{
    public class AudioSetting : MonoBehaviour
    {
        [Header("System")][SerializeField] private GameObject systemUI;

        [Header("Settings")][SerializeField] private GameObject settingsUI;

        [Header("Audio Mixer")]
        [SerializeField]
        private AudioMixer mixer;

        [Header("Sound Settings Slider")]
        [SerializeField]
        private Slider masterVolumeSlider;

        [SerializeField] private Slider BGMVolumeSlider;
        [SerializeField] private Slider SFXVolumeSlider;
        [SerializeField] private Slider UIVolumeSlider;

        [SerializeField] private Slider masterVolumeSliderNonCheckpoint;
        [SerializeField] private Slider BGMVolumeSliderNonCheckpoint;
        [SerializeField] private Slider SFXVolumeSliderNonCheckpoint;
        [SerializeField] private Slider UIVolumeSliderNonCheckpoint;

        public float MasterVolume { get; set; } = 1f;
        public float BgmVolume { get; set; } = 1f;
        public float SfxVolume { get; set; } = 1f;
        public float UiVolume { get; set; } = 1f;

        private void Start()
        {
            UpdateSoundSettingsUI();

            SetMasterVolume(MasterVolume);
            SetBGMVolume(BgmVolume);
            SetSFXVolume(SfxVolume);
            SetUIVolume(UiVolume);
        }

        private void UpdateSoundSettingsUI()
        {
            masterVolumeSlider.value = MasterVolume;
            BGMVolumeSlider.value = BgmVolume;
            SFXVolumeSlider.value = SfxVolume;
            UIVolumeSlider.value = UiVolume;

            masterVolumeSliderNonCheckpoint.value = MasterVolume;
            BGMVolumeSliderNonCheckpoint.value = BgmVolume;
            SFXVolumeSliderNonCheckpoint.value = SfxVolume;
            UIVolumeSliderNonCheckpoint.value = UiVolume;
        }

        public void SetMasterVolume(float volume)
        {
            mixer.SetFloat("Master", Mathf.Log10(volume) * 20);
            if (systemUI.activeInHierarchy)
            {
                MasterVolume = masterVolumeSlider.value;
                masterVolumeSliderNonCheckpoint.value = masterVolumeSlider.value;
                return;
            }

            if (settingsUI.activeInHierarchy)
            {
                MasterVolume = masterVolumeSliderNonCheckpoint.value;
                masterVolumeSlider.value = masterVolumeSliderNonCheckpoint.value;
            }
        }

        public void SetBGMVolume(float volume)
        {
            mixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
            if (systemUI.activeInHierarchy)
            {
                BgmVolume = BGMVolumeSlider.value;
                BGMVolumeSliderNonCheckpoint.value = BGMVolumeSlider.value;
                return;
            }

            if (settingsUI.activeInHierarchy)
            {
                BgmVolume = BGMVolumeSliderNonCheckpoint.value;
                BGMVolumeSlider.value = BGMVolumeSliderNonCheckpoint.value;
            }
        }

        public void SetSFXVolume(float volume)
        {
            mixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
            if (systemUI.activeInHierarchy)
            {
                SfxVolume = SFXVolumeSlider.value;
                SFXVolumeSliderNonCheckpoint.value = SFXVolumeSlider.value;
                return;
            }

            if (settingsUI.activeInHierarchy)
            {
                SfxVolume = SFXVolumeSliderNonCheckpoint.value;
                SFXVolumeSlider.value = SFXVolumeSliderNonCheckpoint.value;
            }
        }

        public void SetUIVolume(float volume)
        {
            mixer.SetFloat("UI", Mathf.Log10(volume) * 20);
            if (systemUI.activeInHierarchy)
            {
                UiVolume = UIVolumeSlider.value;
                UIVolumeSliderNonCheckpoint.value = UIVolumeSlider.value;
                return;
            }

            if (settingsUI.activeInHierarchy)
            {
                UiVolume = UIVolumeSliderNonCheckpoint.value;
                UIVolumeSlider.value = UIVolumeSliderNonCheckpoint.value;
            }
        }
    }
}
