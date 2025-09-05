using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] private float spikeDamage;
    [SerializeField] private float knockBackDuration;
    [SerializeField] private Vector2 knockBackForce;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        KnockbackAblity knockBackAblity = collision.GetComponentInParent<KnockbackAblity>();
        knockBackAblity.StartKnockBack(knockBackDuration, knockBackForce, transform);
        //StartCoroutine(knockBackAblity.KnockBack(knockBackDuration,knockBackForce,transform));

        PlayerStats playerStats = collision.GetComponent<PlayerStats>();
        playerStats.DamagePlayer(spikeDamage);
    }
}
