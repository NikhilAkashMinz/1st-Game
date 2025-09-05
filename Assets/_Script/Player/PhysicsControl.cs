using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PhysicsControl : MonoBehaviour
{
    public Rigidbody2D rb;

    [Header("Ground")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private Transform leftGroundPoint;
    [SerializeField] private Transform rightGroundPoint;
    [SerializeField] private LayerMask whatToDetect;
    public bool grounded;
    private RaycastHit2D hitInfoLeft;
    private RaycastHit2D hitInfoRight;  

    [Header("Wall")]
     [SerializeField] private float wallRayDistance;
    [SerializeField] private Transform wallCheckPointUpper;
    [SerializeField] private Transform wallCheckPointLower;
    public bool wallDetected;
    private RaycastHit2D hitInfoWallUpper;
    private RaycastHit2D hitInfoWallLower;

    [Header("Collider")]
    [SerializeField] private BoxCollider2D standColl;
    [SerializeField] private BoxCollider2D crouchColl;

    [Header("Celling")]
    [SerializeField] private float cellingRayDistance;
    [SerializeField] private Transform cellingCheckPointL;
    [SerializeField] private Transform cellingCheckPointR;
    public bool cellingDetected;
    private RaycastHit2D hitInfoCellingL;
    private RaycastHit2D hitInfoCellingR;



    [Header("Coyote Time")]
    [SerializeField] private float coyoteSetTime;
    public float coyoteTimer;

    private float gravityValue;


    public float GetGravity()
    {
        return gravityValue;
    }
    
    private void OnDrawGizmos()
    {
        Debug.DrawRay(cellingCheckPointL.position, new Vector3(0, cellingRayDistance, 0));
        Debug.DrawRay(cellingCheckPointR.position, new Vector3(0, cellingRayDistance, 0));
    }
    private bool CheckGround()
    {
        hitInfoLeft = Physics2D.Raycast(leftGroundPoint.position, Vector2.down, groundCheckDistance, whatToDetect);
        hitInfoRight = Physics2D.Raycast(rightGroundPoint.position, Vector2.down, groundCheckDistance, whatToDetect);

        Debug.DrawRay(leftGroundPoint.position, new Vector3(0, -groundCheckDistance, 0), Color.red);
        Debug.DrawRay(rightGroundPoint.position, new Vector3(0, -groundCheckDistance, 0), Color.red);

        if (hitInfoLeft || hitInfoRight) return true;

        return false;
    }

    private bool CheckWall()
    {
        hitInfoWallUpper = Physics2D.Raycast(wallCheckPointUpper.position, transform.right, wallRayDistance, whatToDetect);
        hitInfoWallLower = Physics2D.Raycast(wallCheckPointLower.position, transform.right, wallRayDistance, whatToDetect);

        Debug.DrawRay(wallCheckPointUpper.position, new Vector3(wallRayDistance, 0, 0), Color.blue);
        Debug.DrawRay(wallCheckPointLower.position, new Vector3(wallRayDistance, 0, 0), Color.blue);

        if (hitInfoWallUpper && hitInfoWallLower) return true;
        
        return false;
    }

    private bool CheckCelling()
    {
        hitInfoCellingL = Physics2D.Raycast(cellingCheckPointL.position, Vector2.up, cellingRayDistance, whatToDetect);
        hitInfoCellingR = Physics2D.Raycast(cellingCheckPointR.position, Vector2.up, cellingRayDistance, whatToDetect);


        if (hitInfoCellingL || hitInfoCellingR) return true;

        return false;
    }

    public void DisableGravity()
    {
        rb.gravityScale = 0;
    }
    public void EnableGravity()
    {
        rb.gravityScale = gravityValue;
    }

    public void ResetVelocity()
    {
        rb.linearVelocity = Vector2.zero;
    }


    void Start()
    {
        gravityValue = rb.gravityScale;
        coyoteTimer = coyoteSetTime;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!grounded)
        {
            coyoteTimer -= Time.deltaTime;
        }
        else
        {
            coyoteTimer = coyoteSetTime;
        }
    }

    public void StandCollider()
    {
        standColl.enabled = true;
        crouchColl.enabled = false;
    }
    public void CrouchCollider()
    {
        crouchColl.enabled = true;
        standColl.enabled = false;
    }
    private void FixedUpdate()
    {
        grounded = CheckGround();
        wallDetected = CheckWall();
        cellingDetected = CheckCelling();
    }
}
