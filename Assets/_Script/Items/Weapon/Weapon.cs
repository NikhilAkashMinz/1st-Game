using UnityEngine;

public class Weapon : MonoBehaviour
{
   public string ID;
   public ItemType ItemType;
   public float damage;
   public  float shootCooldown;
   public bool isAutomatic;

   [Header("Ammo")]
   public int currentAmmo;
   public int maxAmmo;
   public int storageAmmo;

   [Header("Reload")]
   public float reloadTime;
   public bool isReloading;

   [Header("Reference")]
   public Transform shootingPoint;
   public Transform shellSpawnPoint;
   public GameObject shellPrefab;
   public Sprite weaponIconSprite;

   public float visibleLineTime;
 
 
   public bool ReloadCheck()
   {
         int neededAmmo = maxAmmo - currentAmmo;
         if(neededAmmo <=0 || storageAmmo <= 0) return false;

         return true;
   }

   public void Reload()
   {
      int neededAmmo = maxAmmo - currentAmmo;
      int ammoToReload = Mathf.Min(neededAmmo, storageAmmo);
      currentAmmo += ammoToReload;
      storageAmmo -= ammoToReload;
      isReloading = false;
   }
}
