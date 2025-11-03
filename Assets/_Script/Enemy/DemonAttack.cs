using UnityEngine;

public class DemonAttack : MonoBehaviour
{
    [SerializeField] private float damage;

    private void OnTriggerEnter2D(Collider2D collision)
{
    PlayerStats player = collision.GetComponent<PlayerStats>();
    if (player != null)
    {
        player.DamagePlayer(damage);
    }
}
}
