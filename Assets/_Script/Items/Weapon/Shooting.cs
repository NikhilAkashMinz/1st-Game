using UnityEngine;
using System.Collections;
using System;
using UnityEngine.InputSystem;

public class Shooting : MonoBehaviour
{
  [Header("Reference")]
  public InputActionReference shootActionRef;
  public Weapon currentWeapon;
  private Player player;

  private ItemType currentWeaponType;
  private bool shootButtonHeld;
  private bool shootCooldownOver = true;

  [Header("Raycast")]
  public LayerMask whatToHit;
  public float shootRange = 20f;
  [SerializeField]private LineRenderer lineRenderer;
  private bool isShootingLineActivate = false;
  private Vector3 startPoint;
  private Vector3 endPoint;

  public static Action<Sprite,int,int,int> OnUpdateAllInfo;
  public static Action<int,int,int> OnUpdateAmmo;

  private void Awake()
  {
    player = GetComponent<Player>();
  }

  void Start()
  {
    currentWeapon = player.currentWeaponPrefab.GetComponent<Weapon>();
    OnUpdateAllInfo?.Invoke(currentWeapon.weaponIconSprite, currentWeapon.currentAmmo, currentWeapon.maxAmmo, currentWeapon.storageAmmo);
  }
  
  private void OnEnable()
  {
    shootActionRef.action.performed += TryToShoot;
    shootActionRef.action.canceled += StopShooting;
  }

  private void OnDisable()
  {
    shootActionRef.action.performed -= TryToShoot;
    shootActionRef.action.canceled -= StopShooting;
  }

  private void TryToShoot(InputAction.CallbackContext value)
  {

    if(currentWeapon == null 
    || player.stateMachine.currentState == PlayerState.State.Ladder 
    || player.stateMachine.currentState == PlayerState.State.Ladder 
    || player.stateMachine.currentState == PlayerState.State.WallSlide
    ||player.stateMachine.currentState == PlayerState.State.Knockback)
    
    if(shootButtonHeld || !shootCooldownOver) return;

    if(currentWeapon.isAutomatic)
    {
      shootButtonHeld=true;
      return;  
    }
    shootButtonHeld=true;
    Shoot(); 
  }

  private void StopShooting(InputAction.CallbackContext value)
  {
    shootButtonHeld=false;
  }

  private void Shoot()
  {
    if(currentWeapon.currentAmmo <= 0  || currentWeapon.isReloading) return;
    
    lineRenderer.positionCount = 2;
    Vector3 direction = currentWeapon.shootingPoint.right;
    RaycastHit2D hitInfo = Physics2D.Raycast(currentWeapon.shootingPoint.position, direction, shootRange, whatToHit);
    
    if(hitInfo)
    {
      startPoint = currentWeapon.shootingPoint.position;
      endPoint = hitInfo.point;
      lineRenderer.SetPosition(0, startPoint);
      lineRenderer.SetPosition(1,endPoint);
        Debug.Log("We Hit something");
    } 
    else 
    {
      startPoint = currentWeapon.shootingPoint.position;
      endPoint = currentWeapon.shootingPoint.position + direction*10;
      lineRenderer.SetPosition(0, startPoint);
      lineRenderer.SetPosition(1,endPoint);
        Debug.Log("We Hit Nothing");
    }

    currentWeapon.currentAmmo -= 1;
    StartCoroutine(ShootDelay());
    StartCoroutine(ResetShootLine());
    OnUpdateAmmo.Invoke(currentWeapon.currentAmmo, currentWeapon.maxAmmo, currentWeapon.storageAmmo);
  }

  private IEnumerator ShootDelay()
  {
    shootCooldownOver = false;
    yield return new WaitForSeconds(currentWeapon.shootCooldown);
    shootCooldownOver = true;
  }

  private IEnumerator ResetShootLine()
  {
    //isShootingLineActivate = true;
    yield return new WaitForSeconds(currentWeapon.visibleLineTime);
    lineRenderer.positionCount = 0;
    //isShootingLineActivate = false;
  }

  void Update()
  {
    if(shootButtonHeld && currentWeapon.isAutomatic && shootCooldownOver)
    {
     
      Shoot();
    }
    if(isShootingLineActivate)
    {
      // lineRenderer.SetPosition(0,currentWeapon.shootingPoint.position);
      // lineRenderer.SetPosition(1,endPoint);
    }
  }
}
