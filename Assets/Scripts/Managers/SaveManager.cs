using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    private string permanentDataFilePath;

    private GameData _gameData = new GameData();

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
        permanentDataFilePath = Application.persistentDataPath + "/GameData.save";

        InitializeSpellBookDictionaries();
    }

    private void InitializeSpellBookDictionaries()
    {
    }

    public void SavePermanentData()
    {
        try
        {
            string json = JsonUtility.ToJson(_gameData, true);
            File.WriteAllText(permanentDataFilePath, json);
        }
        catch (IOException ex)
        {
            Debug.LogError($"Failed to save data to {permanentDataFilePath}: {ex.Message}");
        }
    }

    public void LoadPermanentData()
    {
        if (File.Exists(permanentDataFilePath))
        {
            try
            {
                string json = File.ReadAllText(permanentDataFilePath);
                JsonUtility.FromJsonOverwrite(json, _gameData);
            }
            catch (IOException ex)
            {
                Debug.LogError($"Failed to load data from {permanentDataFilePath}: {ex.Message}");
            }
        }
    }

    public void DeleteAllSaveData()
    {
        if (File.Exists(permanentDataFilePath))
        {
            File.Delete(permanentDataFilePath);
        }
    }

    public bool HasSaveData()
    {
        return File.Exists(permanentDataFilePath);
    }
}


public class GameData
{
    public string playerName;
}
