using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveManager : MonoBehaviour
{
    const string extraDirectory = "/";

    public static void Save<T>(T objectToSave, string key)
    {
        string path = Application.persistentDataPath + extraDirectory;
        Directory.CreateDirectory(path);
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream fileStream = new FileStream(path + key + ".txt", FileMode.Create))
        {
            formatter.Serialize(fileStream, objectToSave);
        }
        Debug.Log("saved!");
    }

    public static T Load<T>(string key)
    {
        T returnValue = default(T);
        if (!SaveExists(key))
            return returnValue;

        string path = Application.persistentDataPath + extraDirectory;
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream fileStream = new FileStream(path + key + ".txt", FileMode.Open))
        {
            returnValue = (T)formatter.Deserialize(fileStream);
        }

        Debug.Log("Return value exists");
        return returnValue;
    }

    public static bool SaveExists(string key)
    {
        string path = Application.persistentDataPath + extraDirectory + key + ".txt";
        return File.Exists(path);
    }

    public static void SeriouslyDeleteAllSaveFiles()
    {
        string path = Application.persistentDataPath + extraDirectory;
        DirectoryInfo directory = new DirectoryInfo(path);
        directory.Delete(true);
        Directory.CreateDirectory(path);
    }
}
