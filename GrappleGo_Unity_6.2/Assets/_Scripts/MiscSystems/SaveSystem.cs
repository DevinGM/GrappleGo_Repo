using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// Devin G Monaghan
/// 12/21/2025
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
        // Shop save data
        public ShopSaveData shopData;
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

    // delete save data file & reset variables
    public static void DeleteSaveData()
    {
        if (File.Exists(SaveFileName()))
        {
            File.Delete(SaveFileName());
            Debug.Log("File deleted: " + SaveFileName());
        }
        else
            Debug.Log("File not found: " + SaveFileName());
        ResetData();
    }

    // get save data from scripts and put it into _saveData
    public static void HandleSaveData()
    {
        GameManager.Instance.Save(ref _saveData.gameManagerData);
        if (Shop.Instance != null)
            Shop.Instance.Save(ref _saveData.shopData);
    }

    // put save data into scripts from _saveData
    public static void HandleLoadData()
    {
        GameManager.Instance.Load(ref _saveData.gameManagerData);
        if (Shop.Instance != null)
            Shop.Instance.Load(ref _saveData.shopData);
    }

    // reset save data in scripts
    public static void ResetData()
    {
        GameManager.Instance.ResetData(ref _saveData.gameManagerData);
        if (Shop.Instance != null)
            Shop.Instance.ResetData(ref _saveData.shopData);
    }
}