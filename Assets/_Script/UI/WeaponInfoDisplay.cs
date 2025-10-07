using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WeaponInfo : MonoBehaviour
{
   [SerializeField] private Image currentWeaponIcon;
   [SerializeField] private TextMeshProUGUI currentAmmoText;
   [SerializeField] private TextMeshProUGUI storageAmmoText;

   private void OnEnable()
   {
        Shooting.OnUpdateAllInfo += UpdateAllWeaponInfo;
        Shooting.OnUpdateAmmo +=UpdateAmmoInfo;
   }

   private void OnDisable()
   {
        Shooting.OnUpdateAllInfo -=UpdateAllWeaponInfo;
        Shooting.OnUpdateAmmo -=UpdateAmmoInfo;
   }

   private void UpdateAllWeaponInfo(Sprite weaponSpriteIcon,int currentAmmo,int maxAmmo, int storageAmmo)
   {
        currentWeaponIcon.sprite = weaponSpriteIcon;
        currentAmmoText.text = currentAmmo.ToString() + "/" + maxAmmo.ToString();
        storageAmmoText.text = storageAmmo.ToString();
   }

   private void UpdateAmmoInfo(int currentAmmo,int maxAmmo,int storageAmmo)
   {
        currentAmmoText.text = currentAmmo.ToString() + "/" + maxAmmo.ToString();
        storageAmmoText.text = storageAmmo.ToString();
   }

}
