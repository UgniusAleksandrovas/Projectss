using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveSettingsData
{
    public float brightness;

    public float masterVolume;
    public float sfxVolume;
    public float dialogueVolume;
    public float musicVolume;
    public float menuMusicVolume;

    public SaveSettingsData(Settings settings)
    {
        brightness = settings.brightness.value;

        masterVolume = settings.masterVolume.value;
        sfxVolume = settings.sfxVolume.value;
        dialogueVolume = settings.dialogueVolume.value;
        musicVolume = settings.musicVolume.value;
        menuMusicVolume = settings.menuMusicVolume.value;
    }
}
