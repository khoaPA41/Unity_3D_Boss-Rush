using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GraphicsUI : MonoBehaviour
{
    [Header("Left Column")] public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown displayModeDropdown;
    public Toggle vsyncToggle;
    public TMP_InputField fpsLimitInput;
    public TMP_Dropdown qualityPresetDropdown;
    public Toggle shadowsToggle;

    [Header("Right Column")] public TMP_Dropdown antiAliasingDropdown;
    public TMP_Dropdown textureQualityDropdown;
    public Toggle bloomToggle;
    public Toggle motionBlurToggle;
    public Toggle ambientOcclusionToggle;

    private bool isInitializing;

    private void Start()
    {
        isInitializing = true;
        ResolutionDropdown();
        DisplayModeDropdown();
        AntiAliasingDropdown();
        QualityPresetDropdown();
        TextureQualityDropdown();
        
        
        LoadCurrentValuesToUI();
        BindListener();
        isInitializing = false;
    }
    
    private void ResolutionDropdown()
    {
        resolutionDropdown.ClearOptions();
        var options = new List<string>();
        foreach (var resolution in GraphicManager.Instance.AvailableResolutions)
        {
            options.Add($"{resolution.width}x{resolution.height}");
        }

        resolutionDropdown.AddOptions(options);
    }

    private void DisplayModeDropdown()
    {
        displayModeDropdown.ClearOptions();
        displayModeDropdown.AddOptions(new List<string> { "Full Screen", "Borderless", "Windowed" });
    }

    private void AntiAliasingDropdown()
    {
        antiAliasingDropdown.ClearOptions();
        antiAliasingDropdown.AddOptions(new List<string> { "None", "2x MSAA", "4x MSAA", "8x MSAA" });
    }

    private void QualityPresetDropdown()
    {
        qualityPresetDropdown.ClearOptions();
        qualityPresetDropdown.AddOptions(new List<string>(QualitySettings.names));
    }

    private void TextureQualityDropdown()
    {
        textureQualityDropdown.ClearOptions();
        textureQualityDropdown.AddOptions(new List<string> { "Ultra", "High", "Medium", "Low" });
    }

    private void LoadCurrentValuesToUI()
    {
        resolutionDropdown.SetValueWithoutNotify(SaveManagers.Instance.CurrentSaveData.resolutionIndex);
        displayModeDropdown.SetValueWithoutNotify(SaveManagers.Instance.CurrentSaveData.displayModeIndex);
        vsyncToggle.SetIsOnWithoutNotify(SaveManagers.Instance.CurrentSaveData.vsync);
        fpsLimitInput.SetTextWithoutNotify(SaveManagers.Instance.CurrentSaveData.fps.ToString());
        qualityPresetDropdown.SetValueWithoutNotify(SaveManagers.Instance.CurrentSaveData.qualityPresentIndex);
        shadowsToggle.SetIsOnWithoutNotify(SaveManagers.Instance.CurrentSaveData.shadow);
        antiAliasingDropdown.SetValueWithoutNotify(SaveManagers.Instance.CurrentSaveData.antiAliasingIndex);
        textureQualityDropdown.SetValueWithoutNotify(SaveManagers.Instance.CurrentSaveData.textureQualityIndex);
        bloomToggle.SetIsOnWithoutNotify(SaveManagers.Instance.CurrentSaveData.bloom);
        motionBlurToggle.SetIsOnWithoutNotify(SaveManagers.Instance.CurrentSaveData.motionBlur);
        ambientOcclusionToggle.SetIsOnWithoutNotify(SaveManagers.Instance.CurrentSaveData.ambientOcclusion);
    }

    private void BindListener()
    {
        resolutionDropdown.onValueChanged.AddListener(i =>
        {
            if(!isInitializing) GraphicManager.Instance.SetResolution(i);
        });
        
        displayModeDropdown.onValueChanged.AddListener(i =>
        {
            if(!isInitializing) GraphicManager.Instance.SetDisplayMode(i);
        });
        
        vsyncToggle.onValueChanged.AddListener(i =>
        {
            if(!isInitializing) GraphicManager.Instance.SetVsync(i);
        });
        
        shadowsToggle.onValueChanged.AddListener(i =>
        {
            if (!isInitializing) GraphicManager.Instance.SetShadow(i);
        });
        
        fpsLimitInput.onValueChanged.AddListener(i =>
        {
            if(!isInitializing) GraphicManager.Instance.SetFPSLimit(int.Parse(i));
        });
        
        qualityPresetDropdown.onValueChanged.AddListener(i =>
        {
            if(!isInitializing) GraphicManager.Instance.SetQualityPresent(i);
        });
        
        antiAliasingDropdown.onValueChanged.AddListener(i =>
        {
            if (!isInitializing) GraphicManager.Instance.SetAntiAliasing(i);
        });
        
        textureQualityDropdown.onValueChanged.AddListener(i =>
        {
            if (!isInitializing) GraphicManager.Instance.SetTextureQuality(i);
        });
 
        bloomToggle.onValueChanged.AddListener(i =>
        {
            if (!isInitializing) GraphicManager.Instance.SetBloom(i);
        });
 
        motionBlurToggle.onValueChanged.AddListener(i =>
        {
            Debug.Log(i);
            if (!isInitializing) GraphicManager.Instance.SetMotionBlur(i);
        });
 
        ambientOcclusionToggle.onValueChanged.AddListener(i =>
        {
            if (!isInitializing) GraphicManager.Instance.SetAmbientOcclusion(i);
        });
    }
}