
using UnityEngine;
using UnityEngine.InputSystem;

public class MapMovement : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform[] levelNodes;

    [Header("Movement")]
    public float moveSpeed = 4f;

    private int currentLevel = 0;
    private bool isMoving = false;

    void Start()
    {
        player.position = levelNodes[currentLevel].position;
    }

    void Update()
    {
        // Move character if currently travelling
        if (isMoving)
        {
            MovePlayer();
            return;
        }

        // Move right
        if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            MoveToLevel(currentLevel + 1);
        }

        // Move left
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            MoveToLevel(currentLevel - 1);
        }

        // Select level
        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            SelectLevel();
        }
    }

    void MoveToLevel(int level)
    {
        if (level < 0 || level >= levelNodes.Length)
            return;

        currentLevel = level;
        isMoving = true;
    }

    void MovePlayer()
    {
        Vector3 targetPosition = levelNodes[currentLevel].position;

        player.position = Vector3.MoveTowards(
            player.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(player.position, targetPosition) < 0.01f)
        {
            player.position = targetPosition;
            isMoving = false;
        }
    }

    void SelectLevel()
    {
        Debug.Log("Selected Level " + (currentLevel + 1));
    }
}
