using System.IO;
using UnityEngine;

/// <summary>
/// Management read/write save file on storage (JSON)
/// </summary>


public class SaveManagers : MonoBehaviour
{
    public static SaveManagers Instance { get; private set; }

    // Data in current playing (RAM)

    public SaveData CurrentSaveData { get; private set; }

    private string savePath => Path.Combine(Application.persistentDataPath, "saveGame.json");

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Check if player has save data
    public bool HaveSaveData()
    {
        return File.Exists(savePath);
    }

    // Write data to JSON
    public void SaveGame(SaveData saveData)
    {
        saveData.hasSaveData = true;
        saveData.saveDateTime = System.DateTime.Now.ToString();


        var jsonData = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(savePath, jsonData);

        CurrentSaveData = saveData;
        Debug.Log("[SaveManagers] Saved Game" + savePath);
    }

    // Read JSON file to convert save data
    public SaveData LoadGame()
    {
        if (!HaveSaveData())
        {
            Debug.LogWarning("[SaveManagers] Don't have save data]");
            return null;
        }

        var jsonData = File.ReadAllText(savePath);
        CurrentSaveData = JsonUtility.FromJson<SaveData>(jsonData);
        return CurrentSaveData;
    }

    // Delete save data
    public void DeteleSaveGame()
    {
        if (HaveSaveData())
        {
            File.Delete(savePath);
        }

        CurrentSaveData = null;
    }

    // Create new save data
    public void CreateNewSaveGame()
    {
        CurrentSaveData = new SaveData();
    }
}
