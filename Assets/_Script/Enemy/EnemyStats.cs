using UnityEngine;
using System.Collections;

public class EnemyStats : MonoBehaviour
{
    //[SerializeField] public EnemySimpleStateMachine enemyStateMachine;
    [SerializeField] protected float health;
    [SerializeField] protected float maxHealth;


    [Header("Flash")]
    [SerializeField] private float flashDuration;
    [SerializeField,Range(0,1)] private float flashStrength;
    [SerializeField] private Color flashCol;
    [SerializeField] private Material flashMaterial;
    private Material defaualtMaterial;
    [SerializeField]private SpriteRenderer sprite;

    protected Coroutine damageCoroutine;
    private Material flashMatInstance;

    private void Start()
    {
        defaualtMaterial = sprite.material;
        flashMatInstance = new Material(flashMaterial);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        DamageProcess();
        if(damageCoroutine != null)
            StopCoroutine(damageCoroutine);
        damageCoroutine = StartCoroutine(Flash());
        
        
        if (health <= 0)
        {
            DeathProcess();
        }
    }
    
    protected virtual void DamageProcess()
    {
        
    }
    protected virtual void DeathProcess()
    {
        //enemyStateMachine.ChangeState(EnemySimpleStateMachine.EnemyState.Death);
        // Override in derived classes for specific death behavior
    }

    private IEnumerator Flash()
    {
        sprite.material = flashMatInstance;
        flashMatInstance.SetColor("_FlashColor", flashCol);
        flashMatInstance.SetFloat("_FlashAmount", flashStrength);
        yield return new WaitForSeconds(flashDuration);
        sprite.material = defaualtMaterial;
        damageCoroutine = null;
       
    }



}
