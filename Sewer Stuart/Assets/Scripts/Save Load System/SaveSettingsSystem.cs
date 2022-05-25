using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSettingsSystem
{
    public static void SaveSettings(Settings settings)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/settings.data";
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveSettingsData data = new SaveSettingsData(settings);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SaveSettingsData LoadSettings()
    {
        string path = Application.persistentDataPath + "/settings.data";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveSettingsData data = formatter.Deserialize(stream) as SaveSettingsData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Saved settings not found in " + path);
            return null;
        }
    }
}
