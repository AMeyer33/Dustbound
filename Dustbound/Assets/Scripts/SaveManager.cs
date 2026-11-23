using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [Header("Drag your Player here")]
    public Transform playerTransform;

    private string saveFilePath;

    private void Awake()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "savedata.json");
    }

    // Automatically loads when the game starts
    private void Start()
    {
        LoadPlayerPosition();
    }

    // Automatically saves right before the game closes (or when stopping Play Mode)
    private void OnApplicationQuit()
    {
        SavePlayerPosition();
    }

    public void SavePlayerPosition()
    {
        if (playerTransform == null)
        {
            Debug.LogError("Player Transform is not assigned in SaveManager!");
            return;
        }

        SaveData data = new SaveData
        {
            playerPosition = playerTransform.position
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Auto-saved position to: " + saveFilePath);
    }

    public void LoadPlayerPosition()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            SaveData loadedData = JsonUtility.FromJson<SaveData>(json);

            playerTransform.position = loadedData.playerPosition;
            Debug.Log("Auto-loaded position!");
        }
        else
        {
            Debug.Log("No save file found—starting at default position.");
        }
    }

    public void DeleteSave()
    {
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
            Debug.Log("Save file deleted!");
        }
    }

}
