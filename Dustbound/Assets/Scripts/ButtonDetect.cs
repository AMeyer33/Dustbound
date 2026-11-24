using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonDetect : MonoBehaviour
{
    public float radius = 0.5f;
    // Update is called once per frame
    void FixedUpdate()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, radius);
        if(hit != null && hit.CompareTag("buttonCollider"))
        {
            Debug.Log("Buttton_Press_Method");
        }
    }
    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
