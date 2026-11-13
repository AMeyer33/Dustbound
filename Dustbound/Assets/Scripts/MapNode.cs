using UnityEngine;

public class MapNode : MonoBehaviour
{
    [Header("Level Data")]
    public string sceneToLoad;
    public string levelTitle;
    public int levelIndex = 1; // 1 for Level 1, 2 for Level 2, etc.

    [Header("Connected Neighbors")]
    public MapNode upNode;
    public MapNode downNode;
    public MapNode leftNode;
    public MapNode rightNode;

    // Optional: Draw lines in the Scene view to visualize your map paths
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (upNode != null) Gizmos.DrawLine(transform.position, upNode.transform.position);
        if (downNode != null) Gizmos.DrawLine(transform.position, downNode.transform.position);
        if (leftNode != null) Gizmos.DrawLine(transform.position, leftNode.transform.position);
        if (rightNode != null) Gizmos.DrawLine(transform.position, rightNode.transform.position);
    }
}

