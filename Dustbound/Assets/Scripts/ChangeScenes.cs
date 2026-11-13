using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScenes : MonoBehaviour
{
    public void GotoSceneLevelSelect()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void GotoSceneLevel1()
    {
        SceneManager.LoadScene("Level1");
    }

    public void GotoSceneLevel2()
    {
        SceneManager.LoadScene("Level2");
    }
    
    public void GotoSceneLevel3()
    {
        SceneManager.LoadScene("Level3");
    }
    
    public void GotoSceneMinibossLevel()
    {
        SceneManager.LoadScene("MinibossLevel");
    }
    
    public void GotoSceneBossLevel()
    {
        SceneManager.LoadScene("BossLevel");
    }
    
    public void GotoSceneTitleScreen()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}
