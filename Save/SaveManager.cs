using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


public static class SaveManager
{
    private static string filePath = Application.persistentDataPath + "/RougueHHGameData.ac";
    
    public static void SaveGameData(SaveData saveData)
    {
        BinaryFormatter binaryformatter = new BinaryFormatter();
        FileStream file = File.Create(filePath);
        binaryformatter.Serialize(file, saveData); 
        file.Close();
    }

    public static SaveData LoadGameData()
    {
        if (File.Exists(filePath))
        {
            try
            {
                BinaryFormatter binaryformatter = new BinaryFormatter();
                FileStream file = File.Open(filePath, FileMode.Open);
                SaveData saveData = (SaveData)binaryformatter.Deserialize(file);
                file.Close();
                return saveData;
            }
            catch
            {
                Debug.Log("Save data could not be loaded");
                return null;
            }
        }
        return null;
    }

    public static void DeleteSaveData()
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }
}
