using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Sirenix.Serialization;
using Sirenix.OdinInspector;

[ShowOdinSerializedPropertiesInInspector]
[System.Serializable]
public abstract class ClassSaver
{
    public static void SaveClass(System.Object classToSave)
    {
        string location = GetFileLocation(classToSave.GetType().ToString());
        FileStream file;

        if (File.Exists(location)) file = File.OpenWrite(location);
        else file = File.Create(location);

        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, classToSave);
        file.Close();
    }

    public static System.Object LoadClass(string className)
    {
        FileStream file;

        string location = GetFileLocation(className);

        if (File.Exists(location)) file = File.OpenRead(location);
        else
        {
            Debug.LogError("File not found");
            return null;
        }

        BinaryFormatter bf = new BinaryFormatter();
        System.Object data = bf.Deserialize(file);
        file.Close();

        return data;
    }

    private static string GetFileLocation(string className)
    {
        string location = Application.persistentDataPath + "//" + className + ".dat";

        return location;
    }

    public abstract void UpdateClass();

}
