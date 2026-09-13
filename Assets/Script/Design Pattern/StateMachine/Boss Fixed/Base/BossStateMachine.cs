using System;
using Script.Attack;
using Script.Attack.Skill_Factory;
using Script.Design_Pattern.StateMachine.Base;
using Script.Design_Pattern.StateMachine.Player.Base;
using Script.Design_Pattern.Tree_Behavious.Dependency_Injection;
using Script.Physics;
using UnityEngine;

namespace Script.Design_Pattern.StateMachine
{
    [Serializable]
    public struct Combo
    {
        public AttackData[] AttackData;
    }

    [Serializable]
    public struct NormalCombo
    {
        public float HealthThreshold;
        public Combo[] Combo;
    }

    [Serializable]
    public struct UltimateCombo
    {
        public float HealthThreshold;
        public AttackData[] AttackData;
    }

    public class BossStateMachine : Base.StateMachine, ICaster, ICombatInput
    {
        [field: Header("Physics")]
        [field: SerializeField]
        public CharacterController CharacterController { get; private set; }
        [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
        [field: SerializeField] public float MovementSpeed { get; private set; } = 5f;
        [field: SerializeField] public float SprintSpeed { get; private set; } = 5f;
        [field: SerializeField] public float DashSpeed { get; private set; } = 30f;

        [field: Header("Attack")]
        [field: SerializeField] public PlayerSFX PlayerSFX { get; private set; }
        public bool isRightWeaponVFX { get; set; }
        public bool isBothWeaponVFX { get; set; }
        [field: SerializeField] public UltimateCombo[] UltimateCombo { get; private set; }
        [field: SerializeField] public NormalCombo[] NormalCombo { get; private set; }
        [field: SerializeField] public WeaponTrail[] DealsDamage { get; private set; }
        [field: SerializeField] public Health Health { get; private set; }
        [field: SerializeField] public float AttackRange { get; private set; } = 5f;
        [field: SerializeField] public float AttackFurtherRange { get; private set; } = 30f;
        [field: SerializeField] public float WalkRange { get; private set; } = 8f;
        [field: SerializeField] public int TimesToHit { get; private set; } = 3;
        private int TimesHit { get; set; } = 0;
        public int NextAttackIndex { get; set; }
        public int CurrentComboIndex { get; set; }

        [field: Header("Animation")]
        [field: SerializeField]
        public Animator Animator { get; private set; }
        [field: SerializeField] public ManageAnimationSkillEvent ManageAnimationSkillEvent { get; private set; }
        [field: SerializeField] public float AnimationCrossFade { get; private set; } = .1f;

        [field: Header("State")]
        [field: SerializeField]
        public float ChaseDuration { get; private set; } = 4f;
        [field: SerializeField] public float AttackDuration { get; private set; } = 8f;
        [field: SerializeField] public float IdleDuration { get; private set; } = 2f;
        public float NextPhaseToggleTime { get; set; }
        public bool IsChasingState { get; set; }
        public bool IsChangePhase { get; set; }

        public float LastAttackTime { get; set; } // Last time cal from last atk anim

        public bool IsFinishedAttack { get; set; } // Check attack combo done

        public int CurrentPhase { get; set; } = 0;
        public int NextPhase { get; set; } = 0;

        public bool IsActiveUltimate { get; set; }

        [field: Header("Event")] public Health Player { get; private set; }
        public PlayerStateMachine PlayerStateMachine { get; private set; }
        public bool IsWalking { get; set; }
        private State LocomotionState;

        public event Action<int> SkillAction;

        public bool IsCanMove { get; set; } = false;
        public Transform Target { get; set; }
        public Vector2 InputMovement { get; set; }
        public Vector2 Look { get; set; }
        public bool IsChasing { get; set; }
        public bool IsSprint { get; set; }
        public bool IsAttack { get; set; }
        public int SkillNumber { get; set; }

        private void Awake()
        {
            Player = GameObject.FindWithTag("Player").GetComponent<Health>();
            Target = Player.gameObject.transform;
            PlayerStateMachine = Player.GetComponent<PlayerStateMachine>();

            LocomotionState = new BossLocomotionState(this);
        }

        public void ReturnLocomotion()
        {
            SwitchState(LocomotionState);
        }


        public void ComsumeMana(int amount)
        {
            throw new NotImplementedException();
        }

        public Transform GetTransform()
        {
            throw new NotImplementedException();
        }

        public GameObject TargetCaster()
        {
            throw new NotImplementedException();
        }

        // public bool IsAttackRange()
        // {
        //     return ((Player.transform.position -
        //              transform.position)
        //                .sqrMagnitude <= AttackRange *
        //                AttackRange) &&
        //            !PlayerStateMachine.Invisible;
        // }

        // public bool IsWalkRange()
        // {
        //     return ((Player.transform.position -
        //              transform.position)
        //                .sqrMagnitude <= AttackRange *
        //                WalkRange) &&
        //            !PlayerStateMachine.Invisible;
        // }
    }
}

