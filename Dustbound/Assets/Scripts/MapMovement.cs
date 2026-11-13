
using UnityEngine;

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
        // Place player at first node
        player.position = levelNodes[currentLevel].position;
    }

    void Update()
    {
        // Handle movement
        if (isMoving)
        {
            MovePlayer();
            return;
        }

        // Move right
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveToLevel(currentLevel + 1);
        }

        // Move left
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveToLevel(currentLevel - 1);
        }

        // Select level
        if (Input.GetKeyDown(KeyCode.Return))
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

        // Check if player has arrived
        if (Vector3.Distance(player.position, targetPosition) < 0.01f)
        {
            player.position = targetPosition;
            isMoving = false;
        }
    }

    void SelectLevel()
    {
        Debug.Log("Selected Level " + (currentLevel + 1));

        // We'll add scene loading later
    }
}
