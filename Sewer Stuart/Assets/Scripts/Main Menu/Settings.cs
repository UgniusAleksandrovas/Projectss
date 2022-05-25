using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Settings : MonoBehaviour
{
    #region Save and Load Settings

    [SerializeField] bool loadDataOnAwake = true;
    void Awake()
    {
        if (loadDataOnAwake)
        {
            LoadSettings();
        }
    }

    void Start()
    {
        UpdateGammaSlider();
        UpdateVolumeSliders();

        ChangeMasterVolume();
        ChangeSFXVolume();
        ChangeDialogueVolume();
        ChangeMusicVolume();
        ChangeMenuMusicVolume();
    }

    [ContextMenu("Save Settings")]
    public void SaveSettings()
    {
        SaveSettingsSystem.SaveSettings(this);
    }

    [ContextMenu("Load Settings")]
    public void LoadSettings()
    {
        SaveSettingsData data = SaveSettingsSystem.LoadSettings();

        brightness.value = data.brightness;

        masterVolume.value = data.masterVolume;
        sfxVolume.value = data.sfxVolume;
        dialogueVolume.value = data.dialogueVolume;
        musicVolume.value = data.musicVolume;
        menuMusicVolume.value = data.menuMusicVolume;
    }

    #endregion

    #region Display Settings

    FullScreenMode fullScreenMode = FullScreenMode.ExclusiveFullScreen;
    public void ChangeDisplayMode(Dropdown windowModeDropdown)
    {
        string windowMode = windowModeDropdown.options[windowModeDropdown.value].text;
        if (windowMode == "FullScreen")
        {
            fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
        else if (windowMode == "Windowed")
        {
            fullScreenMode = FullScreenMode.Windowed;
        }
        else if (windowMode == "Borderless Windowed")
        {
            fullScreenMode = FullScreenMode.FullScreenWindow;
        }
    }

    string resolution = "1920x1080";
    public void ChangeResolution(Dropdown resolutionDropdown)
    {
        resolution = resolutionDropdown.options[resolutionDropdown.value].text;
    }

    bool vSync = false;
    public void ChangeVSync(Dropdown vSyncDropdown)
    {
        string vSyncOption = vSyncDropdown.options[vSyncDropdown.value].text;
        if (vSyncOption == "On")
        {
            vSync = true;
        }
        else if (vSyncOption == "Off")
        {
            vSync = false;
        }
    }

    int targetRefreshRate = 60;
    public void ChangeRefreshRate(Dropdown frameRateDropdown)
    {
        string frameRate = frameRateDropdown.options[frameRateDropdown.value].text;
        if (frameRate == "Unlimited")
        {
            targetRefreshRate = 1000;
        }
        else
        {
            targetRefreshRate = int.Parse(frameRateDropdown.options[frameRateDropdown.value].text);
        }
    }

    [SerializeField] VolumeProfile[] volumeProfiles;
    LiftGammaGain gammaSetting;
    public SliderSettings brightness;
    public void ChangeGamma()
    {
        brightness.value = brightness.slider.value;
        for (int i = 0; i < volumeProfiles.Length; i++)
        {
            if (volumeProfiles[i].TryGet(out gammaSetting))
            {
                gammaSetting.gamma.Override(new Vector4(1f, 1f, 1f, brightness.value));
            }
        }
        SaveSettings();
    }

    void UpdateGammaSlider()
    {
        brightness.slider.value = brightness.value;
    }

    public void ApplyDisplayChanges()
    {
        string[] res = resolution.Split('x');
        int x = int.Parse(res[0]);
        int y = int.Parse(res[1]);
        Screen.SetResolution(x, y, fullScreenMode, targetRefreshRate);
        QualitySettings.vSyncCount = vSync ? 2 : 0;
        SaveSettings();
    }

    #endregion

    #region Audio Settings

    [Header("Audio Settings")]
    [SerializeField] AudioMixer masterMixer;
    public SliderSettings masterVolume;
    public SliderSettings sfxVolume;
    public SliderSettings dialogueVolume;
    public SliderSettings musicVolume;
    public SliderSettings menuMusicVolume;

    void UpdateVolumeSliders()
    {
        masterVolume.slider.value = masterVolume.value;
        sfxVolume.slider.value = sfxVolume.value;
        dialogueVolume.slider.value = dialogueVolume.value;
        musicVolume.slider.value = musicVolume.value;
        menuMusicVolume.slider.value = menuMusicVolume.value;
    }

    public void ChangeMasterVolume()
    {
        masterVolume.value = masterVolume.slider.value;
        masterMixer.SetFloat("MasterVol", masterVolume.value);
        SaveSettings();
    }

    public void ChangeSFXVolume()
    {
        sfxVolume.value = sfxVolume.slider.value;
        masterMixer.SetFloat("SfxVol", sfxVolume.value);
        SaveSettings();
    }

    public void ChangeDialogueVolume()
    {
        dialogueVolume.value = dialogueVolume.slider.value;
        masterMixer.SetFloat("DialogueVol", dialogueVolume.value);
        SaveSettings();
    }

    public void ChangeMusicVolume()
    {
        musicVolume.value = musicVolume.slider.value;
        masterMixer.SetFloat("MusicVol", musicVolume.value);
        SaveSettings();
    }

    public void ChangeMenuMusicVolume()
    {
        menuMusicVolume.value = menuMusicVolume.slider.value;
        masterMixer.SetFloat("MenuMusicVol", menuMusicVolume.value);
        SaveSettings();
    }

    #endregion
}

[System.Serializable]
public class SliderSettings
{
    public float value;
    public Slider slider;
}
