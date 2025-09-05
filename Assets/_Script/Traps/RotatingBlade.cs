using UnityEngine;

public class RotatingBlade : MonoBehaviour
{
    [SerializeField] private float bladeDamage;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float knockBackDuration;
    [SerializeField] private Vector2 knockBackForce;

    private void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        KnockbackAblity knockBackAblity = collision.GetComponentInParent<KnockbackAblity>();
        knockBackAblity.StartKnockBack(knockBackDuration, knockBackForce, transform);
        //StartCoroutine(knockBackAblity.KnockBack(knockBackDuration,knockBackForce,transform));

        PlayerStats playerStats = collision.GetComponent<PlayerStats>();
        playerStats.DamagePlayer(bladeDamage);
    }
}
