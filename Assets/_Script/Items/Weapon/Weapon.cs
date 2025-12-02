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

    [Header("Recoil")]
    public float recoilStrength;
    public float recoilTime; 

   [Header("Reference")]
   public Transform shootingPoint;
   public Transform shellSpawnPoint;
    public GameObject shellPrefab;
    public GameObject effectPrefab;
    public GameObject hitEffectPrefab;
    public Sprite weaponIconSprite;
   
    [Header("LineRender")]
    public  float widthMultiplier;
    public float visibleLineTime;

   [SerializeField]
   public WeaponData weaponData = new WeaponData();
 
 
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

   public void SaveWeaponData()
{
    weaponData.ID = ID;
    weaponData.currentAmmo = currentAmmo;
    weaponData.storageAmmo = storageAmmo;

    // ✅ Save the data object, not the method
    SaveLoadManager.Instance.Save<WeaponData>(
        weaponData,
        SaveLoadManager.Instance.folderName,
        ID + ".json"
    );
}

public void LoadWeaponData()
{
    SaveLoadManager.Instance.Load<WeaponData>(
        weaponData,
        SaveLoadManager.Instance.folderName,
        ID + ".json"
    );

    if (!string.IsNullOrEmpty(weaponData.ID))
    {
        currentAmmo = weaponData.currentAmmo;
        storageAmmo = weaponData.storageAmmo;
    }
}

}
