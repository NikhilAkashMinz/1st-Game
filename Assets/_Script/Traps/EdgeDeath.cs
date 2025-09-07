using UnityEngine;

public class EdgeDeath : MonoBehaviour
{
    public float damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerStats stats = collision.GetComponent<PlayerStats>();
        stats.DamagePlayer(damage);     
    }

}
