using System.IO; // Required to delete the file directly
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    public void RestartLevel()
    {
        // 1. Find and delete the exact save file directly
        string saveFilePath = Path.Combine(Application.persistentDataPath, "savedata.json");
        
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
            Debug.Log("Save file wiped for full restart!");
        }
        else
        {
            Debug.Log("No save file found to delete.");
        }

        // 2. Reset time scale and reload the entire scene to original positions
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
