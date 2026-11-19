using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
    [Tooltip("Exact name of the scene to load (e.g., LevelSelect)")]
    public string nextSceneName = "LevelSelect";

    void Update()
    {
        // Check for Left Mouse Click anywhere on the screen
        bool mouseClicked = Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;

        // Check for Enter or Numpad Enter key
        bool enterPressed = Keyboard.current != null && 
            (Keyboard.current.enterKey.wasPressedThisFrame || Keyboard.current.numpadEnterKey.wasPressedThisFrame);

        if (mouseClicked || enterPressed)
        {
            LoadNextScene();
        }
    }

    void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("Next Scene Name is empty on TitleScreenManager!");
        }
    }
}
