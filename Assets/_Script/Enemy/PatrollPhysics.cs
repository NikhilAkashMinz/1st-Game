using UnityEngine;

public class PatrollPhysics : MonoBehaviour
{
    public Rigidbody2D rb;

    [Header("Detect Ground and Wall")]
    [SerializeField] private float checkRadius;
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private Transform wallCheckPoint;
    [SerializeField] private LayerMask whatToDetect;
    public bool groundDetected;
    public bool wallDetected;

    [Header("Collision Detection")]
    [SerializeField] private BoxCollider2D attackDetectionCol;
    public bool inAttackRange;


    private void FixedUpdate()
    {
        groundDetected = Physics2D.OverlapCircle(groundCheckPoint.position, checkRadius, whatToDetect);
        wallDetected = Physics2D.OverlapCircle(wallCheckPoint.position, checkRadius, whatToDetect);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheckPoint.position, checkRadius);
        Gizmos.DrawWireSphere(wallCheckPoint.position, checkRadius);
    }

    public void NegateForces()
    {
        rb.linearVelocity = Vector2.zero;
    }
}
