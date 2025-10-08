 using UnityEngine;

public class Player : MonoBehaviour
{
    public GatherInput gatherInput;
    public StateMachine stateMachine;
    public PhysicsControl physicsControl;
    public PlayerStats playerStats;
    public Animator anim;


    private BaseAbility[] playerAbilities;
    public bool isFacingRight = true;


    [Header("Current Weapon")]
    public GameObject currentWeaponPrefab;
    public ItemType currentWeaponType;

    [Header("Primay Weapon")]
    public GameObject primaryWeaponPrefab;

    [Header("Weapon Position")]
    [SerializeField] private Transform currentShootingPos;
    [SerializeField] private Transform standingShootPos;
    [SerializeField] private Transform crouchShootPos;
    [SerializeField] private Transform upShootPos;

    private void Awake()
    {
        stateMachine = new StateMachine();
        playerAbilities = GetComponents<BaseAbility>();
        stateMachine.arrayOfAbilities = playerAbilities;
        currentShootingPos = standingShootPos;
    }

    private void Update()
    {
        foreach (BaseAbility ability in playerAbilities)
        {
            if (ability.thisAbilityState == stateMachine.currentState)
            {
                ability.ProcessAbility();
            }
            ability.UpdateAnimator();
        }
       // Debug.Log($"Current State: {stateMachine.currentState}");
    }

    private void FixedUpdate()
    {
        foreach (BaseAbility ability in playerAbilities)
        {
            if (ability.thisAbilityState == stateMachine.currentState)
            {
                ability.ProcessFixedAbility();
            }
        }
    }

    public void Flip()
    {
        if (isFacingRight == true && gatherInput.horizontalInput < 0)
        {
            transform.Rotate(0, 180, 0);
            isFacingRight = !isFacingRight;
        }
        else if (isFacingRight == false && gatherInput.horizontalInput > 0)
        {
            transform.Rotate(0, 180, 0);
            isFacingRight = !isFacingRight;
        }
    }
    public void ForceFlip()
    {
        transform.Rotate(0, 180, 0);
        isFacingRight = !isFacingRight;

    }

    public void SetStandShootPos()
    {
        if(currentWeaponType == ItemType.PrimaryWeapon)
        {
            currentShootingPos = standingShootPos;
            currentWeaponPrefab.transform.position = standingShootPos.position;
            SetWeaponRotation(0);
        }
    }

    public void SetCrouchShootPos()
    {
        if(currentWeaponType == ItemType.PrimaryWeapon)
        {
            currentShootingPos = crouchShootPos;
            currentWeaponPrefab.transform.position = crouchShootPos.position;
            SetWeaponRotation(0);
        }
    }

    public void SetUpShootPos()
    {
        if(currentWeaponType == ItemType.PrimaryWeapon)
        {
            currentShootingPos = upShootPos;
            currentWeaponPrefab.transform.position = upShootPos.position;
            SetWeaponRotation(90);
        }
    }

    private void SetWeaponRotation(float zRoatation)
    {
        currentWeaponPrefab.transform.localEulerAngles = new Vector3(0, 0, zRoatation);
    }

    public void DeactivateCurrentWeapon()
    {
        currentWeaponPrefab.SetActive(false);
    }

    public void ActivateCurrentWeapon()
    {
        currentWeaponPrefab.SetActive(true);
    }

}
