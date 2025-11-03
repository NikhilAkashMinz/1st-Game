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
    [SerializeField] private PolygonCollider2D attackCol;
    [SerializeField] private PolygonCollider2D statsCol;
    public bool inAttackRange;

    [Header("Player Ahead")]
    [SerializeField] private Transform frontCheckPoint;
    [SerializeField] private float rayFrontCheckLength;
    public bool playerAhead;

    [Header("Player Behind")]
    [SerializeField] private Transform backCheckPoint;
    [SerializeField] private float rayBackCheckLength;
    public bool playerBehind;
    public bool canCheckBehind = true;
    [SerializeField] private LayerMask playerDetectMask;


    private void FixedUpdate()
    {
        groundDetected = Physics2D.OverlapCircle(groundCheckPoint.position, checkRadius, whatToDetect);
        wallDetected = Physics2D.OverlapCircle(wallCheckPoint.position, checkRadius, whatToDetect);
        playerAhead = Physics2D.Raycast(frontCheckPoint.position, transform.right, rayFrontCheckLength, playerDetectMask);
        if (canCheckBehind)
            playerBehind = Physics2D.Raycast(backCheckPoint.position, transform.right*(-1), rayBackCheckLength, playerDetectMask);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheckPoint.position, checkRadius);
        Gizmos.DrawWireSphere(wallCheckPoint.position, checkRadius);
        Gizmos.DrawLine(frontCheckPoint.position, frontCheckPoint.position + transform.right * rayFrontCheckLength);
        Gizmos.DrawLine(backCheckPoint.position, backCheckPoint.position + transform.right * (-1) * rayBackCheckLength); 
    }

    public void NegateForces()
    {
        rb.linearVelocity = Vector2.zero;
    }

    public void ActivateAttackCol()
    {
        attackCol.enabled = true;
    }

    public void DeactivateAttackCol()
    {
        attackCol.enabled = false;
    }

    public void DeatCollliderDeactivation()
    {
        DeactivateAttackCol();
        attackDetectionCol.enabled = false;
        statsCol.enabled = false;
    }
}
