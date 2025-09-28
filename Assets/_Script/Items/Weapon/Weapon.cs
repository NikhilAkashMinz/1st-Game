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

   [Header("Reference")]
   public Transform shootingPoint;
   public Transform shellSpawnPoint;
   public GameObject shellPrefab;

   public float visibleLineTime;
}
