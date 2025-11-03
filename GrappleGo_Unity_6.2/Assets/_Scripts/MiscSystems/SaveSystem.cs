using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// Devin G Monaghan
/// 10/17/2025
/// Handles save system
/// </summary>

public class SaveSystem
{
    private static SaveData _saveData = new SaveData();

    // struct holding save datas
    // can be expanded to hold save data from other objects if needed, ie player or shop
    [System.Serializable]
    public struct SaveData
    {
        // GameManager save data
        public GameManagerSaveData gameManagerData;
    }

    // creates a save file name and path and puts it in a string using the persistent data path
    public static string SaveFileName()
    {
        string saveFile = Application.persistentDataPath + "/save" + ".json";
        return saveFile;
    }

    // save data and write it into a text file
    public static void Save()
    {
        // get save data
        HandleSaveData();
        // write save data to text file found at SaveFileName()'s path
        File.WriteAllText(SaveFileName(), JsonUtility.ToJson(_saveData, true));
    }

    // load data from text file
    public static void Load()
    {
        if (File.Exists(SaveFileName()))
        {
            // read the save text file and place it into a string
            string saveContent = File.ReadAllText(SaveFileName());
            // place the file contents from the string into our struct
            _saveData = JsonUtility.FromJson<SaveData>(saveContent);
            // load save data
            HandleLoadData();
        }
        else
            Debug.LogWarning("WARNING: save data does not exist");
    }

    // get save data from GameManager and put it into _saveData
    public static void HandleSaveData()
    {
        GameManager.Instance.Save(ref _saveData.gameManagerData);
    }

    // put save data into GameManager from _saveData
    public static void HandleLoadData()
    {
        GameManager.Instance.Load(ref _saveData.gameManagerData);
    }
}