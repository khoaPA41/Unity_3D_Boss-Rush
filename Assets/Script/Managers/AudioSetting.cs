using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSetting : MonoBehaviour
{
    [Header("System")] [SerializeField] private GameObject systemUI;
    
    [Header("System")] [SerializeField] private GameObject settingsUI;

    [Header("Audio Mixer")] [SerializeField]
    private AudioMixer mixer;

    [Header("Sound Settings Slider")] [SerializeField]
    private Slider masterVolumeSlider;

    [SerializeField] private Slider BGMVolumeSlider;
    [SerializeField] private Slider SFXVolumeSlider;
    [SerializeField] private Slider UIVolumeSlider;

    [SerializeField] private Slider masterVolumeSliderNonCheckpoint;
    [SerializeField] private Slider BGMVolumeSliderNonCheckpoint;
    [SerializeField] private Slider SFXVolumeSliderNonCheckpoint;
    [SerializeField] private Slider UIVolumeSliderNonCheckpoint;

    public float _masterVolume { get; set; } = 1f;
    public float _bgmVolume { get; set; } = 1f;
    public float _sfxVolume { get; set; } = 1f;
    public float _uiVolume { get; set; } = 1f;

    private void Start()
    {
        // UpdateSaveSoundSettings();
        UpdateSoundSettingsUI();
        
        SetMasterVolume(_masterVolume);
        SetBGMVolume(_bgmVolume);
        SetSFXVolume(_sfxVolume);
        SetUIVolume(_uiVolume);
    }

    private void UpdateSaveSoundSettings()
    {
        _masterVolume = SaveManagers.Instance.CurrentSaveData.masterVolume;
        _bgmVolume = SaveManagers.Instance.CurrentSaveData.BGMVolume;
        _sfxVolume = SaveManagers.Instance.CurrentSaveData.SFXVolume;
        _uiVolume = SaveManagers.Instance.CurrentSaveData.UIVolume;
    }

    private void UpdateSoundSettingsUI()
    {
        masterVolumeSlider.value = _masterVolume;
        BGMVolumeSlider.value = _bgmVolume;
        SFXVolumeSlider.value = _sfxVolume;
        UIVolumeSlider.value = _uiVolume;
        
        masterVolumeSliderNonCheckpoint.value = _masterVolume;
        BGMVolumeSliderNonCheckpoint.value = _bgmVolume;
        SFXVolumeSliderNonCheckpoint.value = _sfxVolume;
        UIVolumeSliderNonCheckpoint.value = _uiVolume;
    }

    public void SetMasterVolume(float volume)
    {
        mixer.SetFloat("Master", Mathf.Log10(volume) * 20);
        if (systemUI.activeInHierarchy)
        {
            _masterVolume = masterVolumeSlider.value;
            masterVolumeSliderNonCheckpoint.value = masterVolumeSlider.value;
            return;
        }
        
        if (settingsUI.activeInHierarchy)
        {
            _masterVolume = masterVolumeSliderNonCheckpoint.value;
            masterVolumeSlider.value = masterVolumeSliderNonCheckpoint.value;
        }
    }

    public void SetBGMVolume(float volume)
    {
        mixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
        if (systemUI.activeInHierarchy)
        {
            _bgmVolume = BGMVolumeSlider.value;
            BGMVolumeSliderNonCheckpoint.value = BGMVolumeSlider.value;
            return;
        }

        if (settingsUI.activeInHierarchy)
        {
            _bgmVolume = BGMVolumeSliderNonCheckpoint.value;
            BGMVolumeSlider.value = BGMVolumeSliderNonCheckpoint.value;
        }
    }

    public void SetSFXVolume(float volume)
    {
        mixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        if (systemUI.activeInHierarchy)
        {
            _sfxVolume = SFXVolumeSlider.value;
            SFXVolumeSliderNonCheckpoint.value = SFXVolumeSlider.value;
            return;
        }

        if (settingsUI.activeInHierarchy)
        {
            _sfxVolume = SFXVolumeSliderNonCheckpoint.value;
            SFXVolumeSlider.value = SFXVolumeSliderNonCheckpoint.value;
        }
    }

    public void SetUIVolume(float volume)
    {
        mixer.SetFloat("UI", Mathf.Log10(volume) * 20);
        if (systemUI.activeInHierarchy)
        {
            _uiVolume = UIVolumeSlider.value;
            UIVolumeSliderNonCheckpoint.value = UIVolumeSlider.value;
            return;
        }

        if (settingsUI.activeInHierarchy)
        {
            _uiVolume = UIVolumeSliderNonCheckpoint.value;
            UIVolumeSlider.value = UIVolumeSliderNonCheckpoint.value;
        }
    }
}
