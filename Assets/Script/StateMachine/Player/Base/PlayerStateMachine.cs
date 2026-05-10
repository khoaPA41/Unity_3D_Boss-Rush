using UnityEngine;

public class PlayerStateMachine : StateMachine, ICaster
{
    [Header("Input")]
    [field: SerializeField] public InputReader InputReader { get; private set; }

    [Header("Physics")]
    [field: SerializeField] public CharacterController CharacterController { get; private set; }
    [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
    [field: SerializeField] public float FreeLookMovementSpeed { get; private set; } = 5f;
    [field: SerializeField] public float FreeLookMovementSprintSpeed { get; private set; } = 5f;
    [field: SerializeField] public float RotationDamping { get; private set; } = .5f;
    [field: SerializeField] public Targeter Targeter { get; private set; }
    [field: SerializeField] public float JumpForce { get; private set; }
    [field: SerializeField] public float DodgeLength { get; private set; }
    [field: SerializeField] public float DodgeDuration { get; private set; }
    [field: SerializeField] public float HitForceTime { get; private set; } = .3f;
    [field: SerializeField] public float HitForce { get; private set; } = 3f;
    [field: SerializeField] public float HitKnockback { get; private set; } = 8f;


    [Header("Attack")]
    [field: SerializeField] public AttackData[] AttackData { get; private set; }
    [field: SerializeField] public WeaponDealDamage WeaponDealDamage { get; private set; }
    [field: SerializeField] public Health Health { get; private set; }
    [field: SerializeField] public Mana Mana { get; private set; }
    [field: SerializeField] public int TimeToGetKnockBackHit { get; private set; } = 3;



    [Header("Animation")]
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public float AnimationCrossFade { get; private set; } = .1f;
    [field: SerializeField] public AnimationClip SwordIdleAnimationClip { get; private set; }
    [field: SerializeField] public AnimationClip IdleLoopAnimationClip { get; private set; }
    [field: SerializeField] public float TimeToBackIdleLoop { get; private set; }



    public Transform MainCameraTransform { get; private set; }
    public int hitTimes { get; set; }

    public bool isAttackState;

    void Start()
    {
        MainCameraTransform = Camera.main.transform;
        SwitchState(new FreeLookState(this));
    }

    void OnEnable()
    {
        Health.DeathAction += EnterDeathState;
    }
    void OnDisable()
    {
        Health.DeathAction -= EnterDeathState;
    }

    public void ReturnLocomotion()
    {
        if (Targeter.currentTarget == null)
        {
            SwitchState(new FreeLookState(this));
        }
        else
        {
            SwitchState(new PlayerTargetState(this));
        }
        return;
    }

    public void EnterAttackState(int attackDataIndex)
    {
        SwitchState(new PlayerAttackState(this, attackDataIndex));
        return;
    }

    public void EnterDeathState()
    {
        SwitchState(new PlayerDeathState(this));
        return;
    }

    public void EnterChangeAction(bool isAttack)
    {
        SwitchState(new PlayerChangeAction(this, isAttack));
        return;
    }

    public void EnterHitState()
    {
        hitTimes++;
        bool isKnockBack = false;

        if (hitTimes == TimeToGetKnockBackHit)
        {
            isKnockBack = true;
            hitTimes = 0;
        }
        SwitchState(new PlayerHitState(this, isKnockBack));
    }


    public void EnterSkillState()
    {
        if (Mana.currentMana <= 0)
        {
            return;
        }
        SwitchState(new PlayerUseSkillState(this));
    }

    public void ComsumeMana(int amount)
    {
        Mana.currentMana = Mathf.Max(Mana.currentMana - amount, 0);
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
