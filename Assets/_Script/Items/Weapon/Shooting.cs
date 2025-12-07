using UnityEngine;
using System.Collections;
using System;
using UnityEngine.InputSystem;

public class Shooting : MonoBehaviour
{
  [Header("Reference")]
  public InputActionReference shootActionRef;
  public InputActionReference changeWeaponRef;
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
    LoadWeapons();
    OnUpdateAllInfo?.Invoke(currentWeapon.weaponIconSprite, currentWeapon.currentAmmo, currentWeapon.maxAmmo, currentWeapon.storageAmmo);
  }

  private void LoadWeapons()
  {
    foreach(Weapon weapon in player.listToSaveandLoad)
    {
      weapon.LoadWeaponData();
    } 
  }
  private void OnEnable()
  {
    shootActionRef.action.performed += TryToShoot;
    shootActionRef.action.canceled += StopShooting;
    changeWeaponRef.action.performed += TryToChangeWeapon;
  }

  private void OnDisable()
  {
    shootActionRef.action.performed -= TryToShoot;
    shootActionRef.action.canceled -= StopShooting;
    changeWeaponRef.action.performed -= TryToChangeWeapon;
  }

  private void TryToChangeWeapon(InputAction.CallbackContext value)
  {
    if (currentWeapon == null
    || player.stateMachine.currentState == PlayerState.State.Ladder
    || player.stateMachine.currentState == PlayerState.State.Ladder
    || player.stateMachine.currentState == PlayerState.State.WallSlide
    || player.stateMachine.currentState == PlayerState.State.Knockback) return;

    if (currentWeapon.isReloading) return;

    if (currentWeapon.ItemType == ItemType.PrimaryWeapon)
    {
      if (player.secondaryWeaponPrefab == null)
        return;
      
      player.primaryWeaponPrefab.SetActive(false);
      player.secondaryWeaponPrefab.SetActive(true);
      player.currentWeaponPrefab = player.secondaryWeaponPrefab;
      currentWeaponType = ItemType.SecondaryWeapon;
      player.currentWeaponType = currentWeaponType;
      currentWeapon = player.currentWeaponPrefab.GetComponent<Weapon>();
      player.anim.SetLayerWeight(1, 1);
      player.SetWeaponPosition();
    }
    else
    {
      if (player.primaryWeaponPrefab == null)
        return;

      player.primaryWeaponPrefab.SetActive(true);
      player.secondaryWeaponPrefab.SetActive(false);
      player.currentWeaponPrefab = player.primaryWeaponPrefab;
      currentWeaponType = ItemType.PrimaryWeapon;
      player.currentWeaponType = currentWeaponType;
      currentWeapon = player.currentWeaponPrefab.GetComponent<Weapon>();
      player.anim.SetLayerWeight(1, 0);
      player.SetWeaponPosition();
    }
    OnUpdateAllInfo?.Invoke(currentWeapon.weaponIconSprite, currentWeapon.currentAmmo, currentWeapon.maxAmmo, currentWeapon.storageAmmo);
  }

  private void StopChangeWeapon(InputAction.CallbackContext value)
  {
    
  }

  private void TryToShoot(InputAction.CallbackContext value)
  {

    if (currentWeapon == null
    || player.stateMachine.currentState == PlayerState.State.Ladder
    || player.stateMachine.currentState == PlayerState.State.Ladder
    || player.stateMachine.currentState == PlayerState.State.WallSlide
    || player.stateMachine.currentState == PlayerState.State.Knockback) return;

    if (shootButtonHeld || !shootCooldownOver) return;

    if (currentWeapon.isAutomatic)
    {
      shootButtonHeld = true;
      return;
    }
    shootButtonHeld = true;
    Shoot();
  }

  private void StopShooting(InputAction.CallbackContext value)
  {
    shootButtonHeld=false;
  }

  private void Shoot()
  {
    if (currentWeapon.currentAmmo <= 0 || currentWeapon.isReloading) return;

    currentWeapon.source.Play();
    Instantiate(currentWeapon.shellPrefab, currentWeapon.shellSpawnPoint.position, currentWeapon.transform.rotation);
    currentWeapon.effectPrefab.transform.position = currentWeapon.shootingPoint.position;
    currentWeapon.effectPrefab.SetActive(true);

    if (player.stateMachine.currentState != PlayerState.State.ShootUp)
      currentWeapon.transform.localPosition = player.defaultWeaponVectorPos - Vector3.right * currentWeapon.recoilStrength;
    else
      currentWeapon.transform.localPosition = player.defaultWeaponVectorPos - Vector3.up * currentWeapon.recoilStrength;

    lineRenderer.positionCount = 2;
    lineRenderer.widthMultiplier = currentWeapon.widthMultiplier;
    Vector3 direction = currentWeapon.shootingPoint.right;
    RaycastHit2D hitInfo = Physics2D.Raycast(currentWeapon.shootingPoint.position, direction, shootRange, whatToHit);
    
    if(hitInfo)
    {
      startPoint = currentWeapon.shootingPoint.position;
      endPoint = hitInfo.point;
      lineRenderer.SetPosition(0, startPoint);
      lineRenderer.SetPosition(1, endPoint);

      Vector2 normal = hitInfo.normal;
      float angle = Mathf.Atan2(normal.y, normal.x) * Mathf.Rad2Deg;
      Quaternion rotation = Quaternion.Euler(0, 0, angle);
      Instantiate(currentWeapon.hitEffectPrefab, hitInfo.point, rotation);

      EnemyStats enemyStats = hitInfo.collider.GetComponent<EnemyStats>();
      if(enemyStats != null)
      {
        enemyStats.TakeDamage(currentWeapon.damage);
      }
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

  public void AddStorageAmmo(string ID, int ammoToAdd)
  {
    foreach(Weapon weapon in player.listToSaveandLoad)
    {
      if(weapon.ID == ID)
      {
        weapon.storageAmmo += ammoToAdd;
        OnUpdateAllInfo?.Invoke(currentWeapon.weaponIconSprite, currentWeapon.currentAmmo, currentWeapon.maxAmmo, currentWeapon.storageAmmo);
        break;
      }
    }
  }

  private IEnumerator ShootDelay()
  {
    shootCooldownOver = false;
    yield return new WaitForSeconds(currentWeapon.recoilTime);
    currentWeapon.transform.localPosition = player.defaultWeaponVectorPos;
    yield return new WaitForSeconds(currentWeapon.shootCooldown - currentWeapon.recoilTime);
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
