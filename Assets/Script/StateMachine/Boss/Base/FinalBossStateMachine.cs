using UnityEngine;

public class FinalBossStateMachine : StateMachine
{

    [Header("Physics")]
    [field: SerializeField] public CharacterController CharacterController { get; private set; }
    [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
    [field: SerializeField] public float MovementSpeed { get; private set; } = 5f;
    [field: SerializeField] public float SprintSpeed { get; private set; } = 5f;


    [Header("Attack")]
    [field: SerializeField] public AttackData[] AttackDatas { get; private set; }
    [field: SerializeField] public WeaponDealDamage WeaponDealDamage { get; private set; }
    [field: SerializeField] public Health Health { get; private set; }
    [field: SerializeField] public float AttackRange { get; private set; } = 5f;


    [Header("Animation")]
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public float AnimationCrossFade { get; private set; } = .1f;

    [Header("State")]
    [field: SerializeField] public float TimeToEnterChasing { get; private set; } = 1f;
    //[field: SerializeField] public float TimeToEnterIdle { get; private set; } = 1f;

    public Health Player { get; private set; }

    void Start()
    {
        Player = GameObject.FindWithTag("Player").GetComponent<Health>();
        ReturnLocomotion();
    }

    void OnEnable()
    {
        Health.DeathAction += EnterDeathState;
    }

    private void OnDisable()
    {
        Health.DeathAction -= EnterDeathState;
    }

    public void ReturnLocomotion()
    {
        SwitchState(new FinalBossLocomotionState(this));
        return;
    }

    public void EnterChasingState()
    {
        SwitchState(new FinalBossChasingState(this));
        return;
    }

    public void EnterAttackState()
    {
        SwitchState(new FinalBossAttackState(this, 0));
        return;
    }

    void EnterDeathState()
    {
        SwitchState(new FinalBossDeathState(this));
        return;
    }
}
