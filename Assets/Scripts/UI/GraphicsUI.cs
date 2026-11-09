using System.Collections.Generic;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
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

        [Header("Left Column Non Checkpoint")] public TMP_Dropdown resolutionDropdownNonCheckpoint;
        public TMP_Dropdown displayModeDropdownNonCheckpoint;
        public Toggle vsyncToggleNonCheckpoint;
        public TMP_InputField fpsLimitInputNonCheckpoint;
        public TMP_Dropdown qualityPresetDropdownNonCheckpoint;
        public Toggle shadowsToggleNonCheckpoint;

        [Header("Right Column Non Checkpoint")] public TMP_Dropdown antiAliasingDropdownNonCheckpoint;
        public TMP_Dropdown textureQualityDropdownNonCheckpoint;
        public Toggle bloomToggleNonCheckpoint;
        public Toggle motionBlurToggleNonCheckpoint;
        public Toggle ambientOcclusionToggleNonCheckpoint;

        private bool isInitializing;

        private void Start()
        {
            isInitializing = true;
            PopulateResolutionDropdown();
            PopulateDisplayModeDropdown();
            PopulateAntiAliasingDropdown();
            PopulateQualityPresetDropdown();
            PopulateTextureQualityDropdown();


            LoadCurrentValuesToUI();
            BindListener();
            isInitializing = false;
        }

        private void PopulateResolutionDropdown()
        {
            resolutionDropdown.ClearOptions();
            var options = new List<string>();
            foreach (var resolution in GraphicManager.Instance.AvailableResolutions)
            {
                options.Add($"{resolution.width}x{resolution.height}");
            }

            resolutionDropdown.AddOptions(options);
        }

        private void PopulateDisplayModeDropdown()
        {
            displayModeDropdown.ClearOptions();
            displayModeDropdown.AddOptions(new List<string> { "Full Screen", "Borderless", "Windowed" });
        }

        private void PopulateAntiAliasingDropdown()
        {
            antiAliasingDropdown.ClearOptions();
            antiAliasingDropdown.AddOptions(new List<string> { "None", "2x MSAA", "4x MSAA", "8x MSAA" });
        }

        private void PopulateQualityPresetDropdown()
        {
            qualityPresetDropdown.ClearOptions();
            qualityPresetDropdown.AddOptions(new List<string>(QualitySettings.names));
        }

        private void PopulateTextureQualityDropdown()
        {
            textureQualityDropdown.ClearOptions();
            textureQualityDropdown.AddOptions(new List<string> { "Ultra", "High", "Medium", "Low" });
        }

        private void LoadCurrentValuesToUI()
        {
            if (!SaveManager.Instance.HaveSaveData())
            {
                resolutionDropdown.SetValueWithoutNotify(17);
                displayModeDropdown.SetValueWithoutNotify(0);
                return;
            }
            resolutionDropdown.SetValueWithoutNotify(SaveManager.Instance.CurrentSaveData.resolutionIndex);
            displayModeDropdown.SetValueWithoutNotify(SaveManager.Instance.CurrentSaveData.displayModeIndex);
            vsyncToggle.SetIsOnWithoutNotify(SaveManager.Instance.CurrentSaveData.vsync);
            fpsLimitInput.SetTextWithoutNotify(SaveManager.Instance.CurrentSaveData.fps.ToString());
            qualityPresetDropdown.SetValueWithoutNotify(SaveManager.Instance.CurrentSaveData.qualityPresetIndex);
            shadowsToggle.SetIsOnWithoutNotify(SaveManager.Instance.CurrentSaveData.shadow);
            antiAliasingDropdown.SetValueWithoutNotify(SaveManager.Instance.CurrentSaveData.antiAliasingIndex);
            textureQualityDropdown.SetValueWithoutNotify(SaveManager.Instance.CurrentSaveData.textureQualityIndex);
            bloomToggle.SetIsOnWithoutNotify(SaveManager.Instance.CurrentSaveData.bloom);
            motionBlurToggle.SetIsOnWithoutNotify(SaveManager.Instance.CurrentSaveData.motionBlur);
            ambientOcclusionToggle.SetIsOnWithoutNotify(SaveManager.Instance.CurrentSaveData.ambientOcclusion);
        }

        private void BindListener()
        {
            resolutionDropdown.onValueChanged.AddListener(i =>
            {
                if (!isInitializing) GraphicManager.Instance.SetResolution(i);
                resolutionDropdownNonCheckpoint.SetValueWithoutNotify(i);
            });

            displayModeDropdown.onValueChanged.AddListener(i =>
            {
                if (!isInitializing) GraphicManager.Instance.SetDisplayMode(i);
                displayModeDropdownNonCheckpoint.SetValueWithoutNotify(i);
            });

            vsyncToggle.onValueChanged.AddListener(i =>
            {
                if (!isInitializing) GraphicManager.Instance.SetVsync(i);
                vsyncToggleNonCheckpoint.SetIsOnWithoutNotify(i);
            });

            shadowsToggle.onValueChanged.AddListener(i =>
            {
                if (!isInitializing) GraphicManager.Instance.SetShadow(i);
                shadowsToggleNonCheckpoint.SetIsOnWithoutNotify(i);
            });

            fpsLimitInput.onValueChanged.AddListener(i =>
            {
                if (!isInitializing) GraphicManager.Instance.SetFPSLimit(int.Parse(i));
                fpsLimitInputNonCheckpoint.SetTextWithoutNotify(i);
            });

            qualityPresetDropdown.onValueChanged.AddListener(i =>
            {
                if (!isInitializing) GraphicManager.Instance.SetQualityPreset(i);
                qualityPresetDropdownNonCheckpoint.SetValueWithoutNotify(i);
            });

            antiAliasingDropdown.onValueChanged.AddListener(i =>
            {
                if (!isInitializing) GraphicManager.Instance.SetAntiAliasing(i);
                antiAliasingDropdownNonCheckpoint.SetValueWithoutNotify(i);

            });

            textureQualityDropdown.onValueChanged.AddListener(i =>
            {
                if (!isInitializing) GraphicManager.Instance.SetTextureQuality(i);
                textureQualityDropdownNonCheckpoint.SetValueWithoutNotify(i);
            });

            bloomToggle.onValueChanged.AddListener(i =>
            {
                if (!isInitializing) GraphicManager.Instance.SetBloom(i);
                bloomToggleNonCheckpoint.SetIsOnWithoutNotify(i);
            });

            motionBlurToggle.onValueChanged.AddListener(i =>
            {
                if (!isInitializing) GraphicManager.Instance.SetMotionBlur(i);
                motionBlurToggleNonCheckpoint.SetIsOnWithoutNotify(i);
            });

            ambientOcclusionToggle.onValueChanged.AddListener(i =>
            {
                if (!isInitializing) GraphicManager.Instance.SetAmbientOcclusion(i);
                ambientOcclusionToggleNonCheckpoint.SetIsOnWithoutNotify(i);
            });
        }
    }
}