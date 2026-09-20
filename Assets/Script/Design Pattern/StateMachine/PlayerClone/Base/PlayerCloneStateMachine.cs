using System;
using Attack;
using Script.Design_Pattern.Tree_Behavious.Dependency_Injection;
using Script.Physics;
using Status;
using UnityEngine;

namespace Design_Pattern.StateMachine.PlayerClone
{
    public class PlayerCloneStateMachine : Base.StateMachine, ICombatInput
    {
        [Header("Animation")]
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public float AnimationCrossFade { get; private set; } = .1f;
        [field: SerializeField] public AnimationClip SwordIdleAnimationClip { get; private set; }
        [field: SerializeField] public float ChangeChasingState { get; private set; } = 2f;


        [Header("Physics")]
        [field: SerializeField] public CharacterController CharacterController { get; private set; }
        [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
        [field: SerializeField] public float MovementSpeed { get; private set; } = 5f;
        [field: SerializeField] public float HitForceTime { get; private set; } = .3f;
        [field: SerializeField] public float HitForce { get; private set; } = 3f;
        [field: SerializeField] public float HitKnockback { get; private set; } = 8f;

        [Header("Attack")]
        [field: SerializeField] public AttackData[] AttackData { get; private set; }
        [field: SerializeField] public WeaponTrail WeaponDealDamage { get; private set; }
        [field: SerializeField] public Health Health { get; private set; }
        [field: SerializeField] public int TimeToGetKnockBackHit { get; private set; } = 3;
        [field: SerializeField] public float AttackRange { get; private set; } = 2f;

        public GameObject Target { get; set; }
        public float CountTime { get; set; }

        private void Start()
        {


        }

        private void OnEnable()
        {
            Target = GameObject.FindGameObjectWithTag("Boss");
            SwitchState(new PlayerCloneIdleState(this));
        }

        public Vector2 InputMovement { get; set; }
        public Vector2 Look { get; set; }
        public bool IsChasing { get; set; }
        public bool IsSprint { get; set; }
        public bool IsAttack { get; set; }

        public int SkillNumber { get; set; }
        public bool IsCounterAttack { get; set; }
        public bool IsUltimateAttack { get; set; }

        public event Action JumpAction = delegate { };
        public event Action DodgeAction = delegate { };
        public event Action TargetAction = delegate { };
        public event Action<int> SkillAction = delegate { };
    }
}
