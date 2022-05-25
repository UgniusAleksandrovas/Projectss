using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour {

    public GameObject videoSettingsUI;
    public Dropdown resolution;
    public Dropdown fullscreenMode;
    public Toggle vsync;
    private bool fullscreen;

    public GameObject soundSettingsUI;
    public Slider masterVolumeSlider;

    public GameObject controlsSettingsUI;

    public void OpenVideoSettings() {
        videoSettingsUI.SetActive(true);
        soundSettingsUI.SetActive(false);
        controlsSettingsUI.SetActive(false);
    }

    public void ApplyVideoSettings() {
        string[] res = resolution.options[resolution.value].text.Split(' ');
        int x = int.Parse(res[0]);
        int y = int.Parse(res[2]);
        fullscreen = fullscreenMode.options[fullscreenMode.value].text == "Fullscreen";
        Screen.SetResolution(x, y, fullscreen);
        QualitySettings.vSyncCount = vsync.isOn ? 2 : 0;
    }

    public void OpenSoundSettings() {
        videoSettingsUI.SetActive(false);
        soundSettingsUI.SetActive(true);
        controlsSettingsUI.SetActive(false);
    }

    public void ApplySoundSettings() {
        AudioListener.volume = masterVolumeSlider.value;
    }

    public void OpenControlsSettings() {
        videoSettingsUI.SetActive(false);
        soundSettingsUI.SetActive(false);
        controlsSettingsUI.SetActive(true);
    }
}
