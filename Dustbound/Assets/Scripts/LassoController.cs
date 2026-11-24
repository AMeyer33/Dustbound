using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LineRenderer))]
public class LassoController : MonoBehaviour
{
    [Header("Lasso Settings")]
    public float maxRange = 7f;
    public float pullSpeed = 8f;
    public float stopDistance = 1.2f;
    public LayerMask lassoableLayer;

    [Header("Visuals")]
    public Transform throwPoint;

    private LineRenderer lineRenderer;
    private Rigidbody2D grabbedObject;
    private Camera mainCam;
    private bool isLassoActive = false;
    private float missTimer = 0f;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        mainCam = Camera.main;

        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;
    }

    void Update()
    {
        if (Keyboard.current == null || Mouse.current == null) return;

        // Hide the rope after a brief flash if a throw missed
        if (missTimer > 0f)
        {
            missTimer -= Time.deltaTime;
            if (missTimer <= 0f && !isLassoActive)
            {
                lineRenderer.enabled = false;
            }
        }

        // Trigger on EITHER the E key OR Left Mouse Click
        bool triggered = Keyboard.current.eKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame;

        if (triggered)
        {
            if (!isLassoActive)
            {
                TryThrowLasso();
            }
            else
            {
                ReleaseLasso();
            }
        }

        // Keep the rope attached to the grabbed object
        if (isLassoActive && grabbedObject != null)
        {
            Vector3 startPos = throwPoint != null ? throwPoint.position : transform.position;
            startPos.z = 0f;
            Vector3 endPos = grabbedObject.position;
            endPos.z = 0f;

            lineRenderer.SetPosition(0, startPos);
            lineRenderer.SetPosition(1, endPos);
        }
    }

    void FixedUpdate()
    {
        if (isLassoActive && grabbedObject != null)
        {
            Vector2 playerPos = throwPoint != null ? throwPoint.position : transform.position;
            float distance = Vector2.Distance(grabbedObject.position, playerPos);

            if (distance > stopDistance)
            {
                Vector2 newPos = Vector2.MoveTowards(
                    grabbedObject.position,
                    playerPos,
                    pullSpeed * Time.fixedDeltaTime
                );
                grabbedObject.MovePosition(newPos);
            }
        }
    }

    void TryThrowLasso()
    {
        if (mainCam == null)
        {
            Debug.LogError("LassoController: No camera tagged 'MainCamera' found in the scene!");
            return;
        }

        Vector2 origin = throwPoint != null ? throwPoint.position : transform.position;

        // Convert mouse position to 2D world coordinates
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPos = mainCam.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, Mathf.Abs(mainCam.transform.position.z)));
        Vector2 direction = ((Vector2)mouseWorldPos - origin).normalized;

        // Cast ray toward the cursor
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, maxRange, lassoableLayer);

        if (hit.collider != null)
        {
            if (hit.rigidbody != null)
            {
                grabbedObject = hit.rigidbody;
                isLassoActive = true;
                missTimer = 0f;
                lineRenderer.enabled = true;
                Debug.Log("Lasso grabbed: " + hit.collider.name);
            }
            else
            {
                Debug.LogWarning("Hit " + hit.collider.name + ", but it needs a Rigidbody2D component to be pulled!");
            }
        }
        else
        {
            // Briefly draw the line toward the cursor so you can see where you aimed
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, new Vector3(origin.x, origin.y, 0f));
            Vector2 endPoint = origin + direction * maxRange;
            lineRenderer.SetPosition(1, new Vector3(endPoint.x, endPoint.y, 0f));
            missTimer = 0.15f;
        }
    }

    void ReleaseLasso()
    {
        isLassoActive = false;
        grabbedObject = null;
        lineRenderer.enabled = false;
    }
}