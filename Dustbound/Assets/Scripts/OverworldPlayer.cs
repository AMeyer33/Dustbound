using UnityEngine;
using UnityEngine.SceneManagement;

public class OverworldPlayer : MonoBehaviour
{
    public MapNode startingNode;
    public float moveSpeed = 8f;

    private MapNode currentNode;
    private MapNode targetNode;
    private bool isMoving = false;
    private int unlockedLevel;

    void Start()
    {
        unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        currentNode = startingNode;
        transform.position = currentNode.transform.position;
    }

    void Update()
    {
        if (isMoving)
        {
            MoveTowardTarget();
        }
        else
        {
            HandleInput();
        }
    }

    void HandleInput()
    {
        // Check directional input
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            TryMoveTo(currentNode.upNode);
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            TryMoveTo(currentNode.downNode);
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            TryMoveTo(currentNode.leftNode);
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            TryMoveTo(currentNode.rightNode);

        // Enter the stage when pressing Space, Enter, or Left Click
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            if (!string.IsNullOrEmpty(currentNode.sceneToLoad))
            {
                SceneManager.LoadScene(currentNode.sceneToLoad);
            }
        }
    }

    void TryMoveTo(MapNode nextNode)
    {
        // Only allow moving if a node exists in that direction AND it is unlocked
        if (nextNode != null && nextNode.levelIndex <= unlockedLevel)
        {
            targetNode = nextNode;
            isMoving = true;
        }
    }

    void MoveTowardTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetNode.transform.position, moveSpeed * Time.deltaTime);

        // Snap into place once close enough
        if (Vector3.Distance(transform.position, targetNode.transform.position) < 0.01f)
        {
            transform.position = targetNode.transform.position;
            currentNode = targetNode;
            isMoving = false;
        }
    }
}
