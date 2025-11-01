using UnityEngine;

public class AmmoPickUp : MonoBehaviour
{
    [SerializeField] private string ID;
    [SerializeField] private int ammo;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Shooting shooting))
        {
            shooting.AddStorageAmmo(ID, ammo);
            Destroy(gameObject);
        }
    }
}
