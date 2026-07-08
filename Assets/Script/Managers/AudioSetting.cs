using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSetting : MonoBehaviour
{
    [Header("Audio Mixer")] [SerializeField]
    private AudioMixer mixer;

    [Header("Sound Settings Slider")] [SerializeField]
    private Slider masterVolumeSlider;

    [SerializeField] private Slider BGMVolumeSlider;
    [SerializeField] private Slider SFXVolumeSlider;
    [SerializeField] private Slider UIVolumeSlider;


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
    }

    public void SetMasterVolume(float volume)
    {
        mixer.SetFloat("Master", Mathf.Log10(volume) * 20);
        _masterVolume = masterVolumeSlider.value;
    }

    public void SetBGMVolume(float volume)
    {
        mixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
        _bgmVolume = BGMVolumeSlider.value;
    }

    public void SetSFXVolume(float volume)
    {
        mixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        _sfxVolume = SFXVolumeSlider.value;
    }

    public void SetUIVolume(float volume)
    {
        mixer.SetFloat("UI", Mathf.Log10(volume) * 20);
        _uiVolume = UIVolumeSlider.value;
    }
}
