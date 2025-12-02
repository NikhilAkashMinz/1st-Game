using UnityEngine;

public class AttackBoss : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float knockBackDuration;
    [SerializeField] private Vector2 knockBackForce;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        KnockbackAblity knockBackAbility = collision.GetComponentInParent<KnockbackAblity>();
        knockBackAbility.StartKnockBack(knockBackDuration, knockBackForce, transform.parent);
        collision.GetComponent<PlayerStats>().DamagePlayer(damage);
    }
}
