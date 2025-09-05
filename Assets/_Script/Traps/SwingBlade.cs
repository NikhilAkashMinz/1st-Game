using UnityEngine;

public class SwingBlade : MonoBehaviour
{
    [Header("Blade Settings")]
    [SerializeField] private float maxAngle;
    [SerializeField] private float speed = 2f;
    private float timer;

    [Header("Damage Settings")]
    [SerializeField] private float bladeDamage;
    [SerializeField] private float knockBackDuration;
    [SerializeField] private Vector2 knockBackForce;

    private int pushDirection = 1;
    private float previousAngle = 0f;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        KnockbackAblity knockBackAblity = collision.GetComponentInParent<KnockbackAblity>();
        knockBackAblity.StartSwingKnockBack(knockBackDuration, knockBackForce, pushDirection);
        //StartCoroutine(knockBackAblity.KnockBack(knockBackDuration,knockBackForce,transform));

        PlayerStats playerStats = collision.GetComponent<PlayerStats>();
        playerStats.DamagePlayer(bladeDamage);
    }


    // Update is called once per frame
    void Update()
    {
        timer += speed * Time.deltaTime;
        float angle = maxAngle * Mathf.Sin(timer);
        transform.localRotation = Quaternion.Euler(0, 0, angle);

        if (angle > previousAngle)
        {
            pushDirection = 1;
        }
        else if (angle < previousAngle)
        {
            pushDirection = -1;
        }
        
        previousAngle = angle;
    }
}
