using UnityEngine;

public class BaseAbility : MonoBehaviour
{
    protected Player player;
    protected GatherInput linkedInput;
    protected StateMachine linkedStateMachine;
    protected Animator linkedAnimator;
    protected PhysicsControl linkedPhysics;
    [SerializeField] protected AudioSource source;
    [SerializeField] protected AudioClip audioClip;

    public PlayerState.State thisAbilityState;
    public bool isPermitted = true;

    protected virtual void Start()
    {
        Initialization();
    }
    public virtual void EnterAbility()
    {

    }
    public virtual void ExitAbility()
    {

    }
    public virtual void ProcessAbility()
    {

    }
    public virtual void ProcessFixedAbility()
    {

    }
    public virtual void UpdateAnimator()
    {

    }

    protected virtual void Initialization()
    {
        player = GetComponent<Player>();
        if (player != null)
        {
            linkedInput = player.gatherInput;
            linkedStateMachine = player.stateMachine;
            linkedAnimator = player.anim;
            linkedPhysics = player.physicsControl;
        }

    }
}
