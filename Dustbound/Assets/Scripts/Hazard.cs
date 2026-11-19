using UnityEngine;
using UnityEngine.SceneManagement;

public class Hazard : MonoBehaviour
{
    // Trigger detection (when "Is Trigger" is checked on the Collider2D)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            RestartLevel();
        }
    }

    // Physical collision detection (when "Is Trigger" is unchecked)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            RestartLevel();
        }
    }

    private void RestartLevel()
    {
        // Reloads the currently active scene from the beginning
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
