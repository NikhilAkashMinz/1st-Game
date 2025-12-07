using UnityEngine;

public class AmmoPickUp : MonoBehaviour
{
    [SerializeField] private string ID;
    [SerializeField] private int ammo;

    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip audioClip;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Shooting shooting))
        {
            shooting.AddStorageAmmo(ID, ammo);
            source.PlayOneShot(audioClip);
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
