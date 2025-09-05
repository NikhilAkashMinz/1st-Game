using System.Collections;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private HealthBarControl healthBarControl;
    [SerializeField] private float maxHealth;
    private float currentHealth;

    [Header("Flash")]
    [SerializeField] private float flashDuration;
    [SerializeField,Range(0,1)] private float flashStrength;
    [SerializeField] private Color flashCol;
    [SerializeField] private Material flashMaterial;

    private Material defaualtMaterial;
    private SpriteRenderer sprite;
    private bool canTakeDamage = true;

    [Header("StatsCollider")]
    [SerializeField] private Collider2D standStatsCol;
    [SerializeField] private Collider2D crouchStatsCol;
    private Collider2D currentStatsCol;

    void Start()
    {
        currentHealth = maxHealth;
        healthBarControl.SetSliderValue(currentHealth, maxHealth);
        sprite = GetComponentInParent<SpriteRenderer>();
        defaualtMaterial = sprite.material;
        currentStatsCol = standStatsCol;
    }

    public void DamagePlayer(float damage)
    {
        if(canTakeDamage == false)
            return;
        currentHealth -= damage;
        healthBarControl.SetSliderValue(currentHealth, maxHealth);
        StartCoroutine(Flash());
        if (currentHealth <= 0)
        {
            if (player.stateMachine.currentState != PlayerState.State.Knockback)
            {
                player.stateMachine.ChangeState(PlayerState.State.Death);
                //currentStatsCol.enabled = false;
            }
        }
    }

    private IEnumerator Flash()
    {
        canTakeDamage = false;
        sprite.material = flashMaterial;
        flashMaterial.SetColor("_FlashColor", flashCol);
        flashMaterial.SetFloat("_FlashAmount", flashStrength);
        yield return new WaitForSeconds(flashDuration);
        sprite.material = defaualtMaterial;
        if(currentHealth > 0)
            canTakeDamage = true;
    }

    public void EnableStatsStandCol()
    {
        if(currentHealth<=0) return;


        crouchStatsCol.enabled = false;
        standStatsCol.enabled = true;
        currentStatsCol = standStatsCol;
    }

    public void EnableStatsCrouchCol()
    {
        if(currentHealth<=0) return;

        standStatsCol.enabled = false;
        crouchStatsCol.enabled = true;
        currentStatsCol = crouchStatsCol;
    }

    public void EnableStatscollider()
    {
        if(currentHealth<=0) return;

        currentStatsCol.enabled = true;
    }
    public void DisableStatscollider()
    {
        currentStatsCol.enabled = false;
    }
    public void DisableDamage()
    {
        canTakeDamage = false;
    }

    public void EnableDamage()
    {

        canTakeDamage = true;
    }

    public bool GetCanTakeDamage()
    {
        return canTakeDamage;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }
}
