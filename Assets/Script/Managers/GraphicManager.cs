using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering.Universal;
using Bloom = UnityEngine.Rendering.Universal.Bloom;
using MotionBlur = UnityEngine.Rendering.Universal.MotionBlur;
using ShadowQuality = UnityEngine.ShadowQuality;

/// <summary>
/// Management graphic settings and save it through GameManager
/// 
/// </summary>

public class GraphicManager : MonoBehaviour
{
    public static GraphicManager Instance {get; private set;}

    [Header("Post Processing")] public Volume postProcessingVolume;
    [Header("Ambient Occlusion")] public ScriptableRendererFeature ambientOcclusionFeature;
    
    private Bloom bloom;
    private MotionBlur motionBlur;
    
    public List<Resolution> AvailableResolutions { get; private set; }
    
    public int resolutionIndex { get; set; }
    public int displayModeIndex{ get; set; }
    public bool vsync { get; set; }
    public int fps { get; set; }
    public int qualityPresentIndex { get; set; }
    public bool shadow { get; set; }
    public int antiAliasingIndex { get; set; }
    public int textureQualityIndex { get; set; }
    public bool bloomData { get; set; }
    public bool motionBlurData { get; set; }
    public bool ambientOcclusion { get; set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        BuildResolutionList();

        if (postProcessingVolume == null || postProcessingVolume.profile == null) return;
        postProcessingVolume.profile.TryGet<Bloom>(out bloom);
        postProcessingVolume.profile.TryGet<MotionBlur>(out motionBlur);
    }
    
    
    private void Start()
    {
        LoadApplyAll();
    }

    private void BuildResolutionList()
    {
        AvailableResolutions = new List<Resolution>();
        var seen =  new HashSet<string>();
        
        /*********************************************************************************/
        var currentResolution = Screen.currentResolution;
        var currentKey = $"{currentResolution.width}x{currentResolution.height}";
        seen.Add(currentKey);
        AvailableResolutions.Add(currentResolution);
        /*********************************************************************************/
        foreach (var resolution in Screen.resolutions)
        {
            var key = $"{resolution.width}x{resolution.height}";
            if(seen.Contains(key)) continue;
            seen.Add(key);
            AvailableResolutions.Add(resolution);
        }
        AvailableResolutions.Sort((a, b) => (a.width * a.height).CompareTo(b.width * b.height));
    }
    
    /******************************** Apply each setting ********************************/

    public void SetResolution(int index)
    {
        if (index < 0 || index >= AvailableResolutions.Count) return;
        var resolution = AvailableResolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode);
        
        // Save data
        resolutionIndex = index;
    }

    public void SetDisplayMode(int index)
    {
        FullScreenMode mode = index switch
        {
            0 => FullScreenMode.ExclusiveFullScreen,
            1 => FullScreenMode.FullScreenWindow,
            2 => FullScreenMode.Windowed,
            _ => FullScreenMode.FullScreenWindow
        };
        Screen.fullScreenMode = mode;
        
        //Save data
        displayModeIndex = index;
    }

    public void SetVsync(bool active)
    {
        QualitySettings.vSyncCount = active ? 1 : 0;
        
        //Save data
        vsync = active;
    }

    public void SetFPSLimit(int fps)
    {
        Application.targetFrameRate = fps > 0 ? fps : 60; 
        //Save data
        this.fps = fps;
    }

    public void SetQualityPresent(int index)
    {
        QualitySettings.SetQualityLevel(index, true);
        //Save data
        qualityPresentIndex = index;
    }
    
    public void SetShadow(bool active)
    {
        QualitySettings.shadows = active ? ShadowQuality.All : ShadowQuality.Disable;
        
        //Save data
        shadow = active;
    }
    
    public void SetAntiAliasing(int index)
    {
        int[] msaaValue = {0, 2, 4, 8};

        var msaa = msaaValue[Math.Clamp(index, 0, msaaValue.Length)];
        QualitySettings.antiAliasing = msaa;

        //Save data
        antiAliasingIndex = index;
    }
        
    public void SetTextureQuality(int index)
    {
        QualitySettings.globalTextureMipmapLimit = index;

        //Save data
        textureQualityIndex = index;
    }
    
    public void SetBloom(bool active)
    {
        if(bloom != null) bloom.active = active;
 
        //Save data
        bloomData = active;
    }

    public void SetMotionBlur(bool active)
    {
        if(motionBlur != null) motionBlur.active = active;
        //Save data
        motionBlurData = active;
    }
    
    public void SetAmbientOcclusion(bool active)
    {
        if(motionBlur != null) ambientOcclusionFeature.SetActive(active);;
        //Save data
        ambientOcclusion =  active;
    }
    
    /******************************** Load At Start ********************************/
    public void LoadApplyAll()
    {
        SetResolution(SaveManagers.Instance.CurrentSaveData.resolutionIndex);
        SetDisplayMode(SaveManagers.Instance.CurrentSaveData.displayModeIndex);
        SetVsync(SaveManagers.Instance.CurrentSaveData.vsync);
        SetFPSLimit(SaveManagers.Instance.CurrentSaveData.fps);
        SetQualityPresent(SaveManagers.Instance.CurrentSaveData.qualityPresentIndex);
        SetShadow(SaveManagers.Instance.CurrentSaveData.shadow);
        SetAntiAliasing(SaveManagers.Instance.CurrentSaveData.antiAliasingIndex);
        SetTextureQuality(SaveManagers.Instance.CurrentSaveData.textureQualityIndex);
        SetBloom(SaveManagers.Instance.CurrentSaveData.bloom);
        SetMotionBlur(SaveManagers.Instance.CurrentSaveData.motionBlur);
        SetAmbientOcclusion(SaveManagers.Instance.CurrentSaveData.ambientOcclusion);
    }
}
