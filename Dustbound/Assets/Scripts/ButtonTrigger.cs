using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem; // Added for the New Input System

public class ButtonTrigger : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent onButtonPressed;

    private bool isPlayerHovering = false;

    void Update()
    {
        // Make sure a keyboard is connected before checking keys
        if (Keyboard.current == null) return;

        // Check for main Enter key OR Numpad Enter key
        bool enterPressed = Keyboard.current.enterKey.wasPressedThisFrame || 
                            Keyboard.current.numpadEnterKey.wasPressedThisFrame;

        if (isPlayerHovering && enterPressed)
        {
            onButtonPressed.Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerHovering = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerHovering = false;
        }
    }
}
