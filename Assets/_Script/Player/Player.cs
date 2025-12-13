using UnityEngine;
using System.Collections.Generic;

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

    [Header("Secondary Weapon")]
    public GameObject secondaryWeaponPrefab;

    [Header("Weapon Position")]
    [SerializeField] private Transform currentShootingPos;
    [SerializeField] private Transform standingShootPos;
    [SerializeField] private Transform crouchShootPos;
    [SerializeField] private Transform upShootPos;
    [HideInInspector] public Vector3 defaultWeaponVectorPos;

    [Header("Sceondary Weapon Poaition")]
    [SerializeField] private Transform secondaryStandingShootPos;
    [SerializeField] private Transform secondaryCrouchShootPos;
    [SerializeField] private Transform secondaryUpShootPos;


    public List<Weapon> listToSaveandLoad = new List<Weapon>();

    private void Awake()
    {
        stateMachine = new StateMachine();
        playerAbilities = GetComponents<BaseAbility>();
        stateMachine.arrayOfAbilities = playerAbilities;
        currentShootingPos = standingShootPos;
        defaultWeaponVectorPos = standingShootPos.localPosition;
    }

    private void OnDisable()
    {
        foreach(Weapon weapon in listToSaveandLoad)
        {
            weapon.SaveWeaponData();
        }
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
        Flip();
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

    public void SetWeaponPosition()
    {
        if(stateMachine.currentState == PlayerState.State.Crouch)
        {
            SetCrouchShootPos();
        }
        else if(stateMachine.currentState == PlayerState.State.ShootUp)
        {
            SetUpShootPos();
        }
        else
        {
            SetStandShootPos();
        }
    }

    public void SetStandShootPos()
    {
        if (currentWeaponType == ItemType.PrimaryWeapon)
        {
            currentShootingPos = standingShootPos;
            currentWeaponPrefab.transform.position = standingShootPos.position;
        }
        else if (currentWeaponType == ItemType.SecondaryWeapon)
        {
            currentShootingPos = secondaryStandingShootPos;
            currentWeaponPrefab.transform.position = secondaryStandingShootPos.position;
        }
        defaultWeaponVectorPos = currentShootingPos.localPosition;
        SetWeaponRotation(0);
    }

    public void SetCrouchShootPos()
    {
        if (currentWeaponType == ItemType.PrimaryWeapon)
        {
            currentShootingPos = crouchShootPos;
            currentWeaponPrefab.transform.position = crouchShootPos.position;
        }
        else if (currentWeaponType == ItemType.SecondaryWeapon)
        {
            currentShootingPos = secondaryCrouchShootPos;
            currentWeaponPrefab.transform.position = secondaryCrouchShootPos.position;
        }
        defaultWeaponVectorPos = currentShootingPos.localPosition;
        SetWeaponRotation(0);
    }

    public void SetUpShootPos()
    {
        if (currentWeaponType == ItemType.PrimaryWeapon)
        {
            currentShootingPos = upShootPos;
            currentWeaponPrefab.transform.position = upShootPos.position;
        }
        else if (currentWeaponType == ItemType.SecondaryWeapon)
        {
            currentShootingPos = secondaryUpShootPos;
            currentWeaponPrefab.transform.position = secondaryUpShootPos.position;
        }
        defaultWeaponVectorPos = currentShootingPos.localPosition;
        SetWeaponRotation(90);
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
