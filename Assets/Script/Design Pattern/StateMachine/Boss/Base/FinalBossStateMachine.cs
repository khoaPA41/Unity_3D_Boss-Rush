using System;
using System.Collections;
using Script.Attack;
using Script.Attack.Skill_Factory;
using Script.Design_Pattern.EventBus;
using Script.Design_Pattern.StateMachine.Boss.Main;
using Script.Design_Pattern.StateMachine.Player.Base;
using Script.Design_Pattern.Tree_Behavious.Dependency_Injection;
using Script.Physics;
using UnityEngine;
using State = Script.Design_Pattern.StateMachine.Base.State;

namespace Script.Design_Pattern.StateMachine.Boss.Base
{
    [Serializable]
    public struct Combo
    {
        public AttackData[] AttackData;
    }
    [Serializable]
    public struct UltimateCombo
    {
        public float HealthThreshold;
        public AttackData[] AttackData;
    }
    
    public class FinalBossStateMachine : StateMachine.Base.StateMachine, ICaster, ICombatInput
    {
        [Header("Physics")]
        [field: SerializeField] public CharacterController CharacterController { get; private set; }
        [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
        [field: SerializeField] public float MovementSpeed { get; private set; } = 5f;
        [field: SerializeField] public float SprintSpeed { get; private set; } = 5f;
        [field: SerializeField] public float DashSpeed { get; private set; } = 30f;

        [Header("Attack")]
        // [field: SerializeField] public AttackData[] AttackData { get; private set; }
        [field: SerializeField] public UltimateCombo[] UltimateCombo { get; private set; }
        [field: SerializeField] public Combo[] NormalCombo { get; private set; }
        public AttackData[] currentAttackData { get; set; }
        [field: SerializeField] public WeaponDealDamage WeaponDealDamage { get; private set; }
        [field: SerializeField] public GameObject Weapon { get; private set; }

        [field: SerializeField] public Health Health { get; private set; }
        [field: SerializeField] public float AttackRange { get; private set; } = 5f;
        [field: SerializeField] public float WalkRange { get; private set; } = 8f;
        [field: SerializeField] public float TimeOutCombo { get; private set; } = 2f;
        public int CurrentComboIndex { get; set; } = 0;
        public float LastAttackTime { get; set; } = 0;
        [field: SerializeField] public int TimesToHit { get; private set; } = 3;
        private int TimesHit { get; set; } = 0;

        [Header("Animation")]
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public float AnimationCrossFade { get; private set; } = .1f;
        
        [Header("State")]
        [field: SerializeField] public float ChaseDuration { get; private set; } = 4f;
        [field: SerializeField] public float IdleDuration { get; private set; } = 2f;
        public float NextPhaseToggleTime { get; set; }
        public bool IsChasingState { get; set; }
        public bool IsChangePhase { get; set; }
        public bool IsAttackState { get; set; }
        public int NextPhase { get; set; } = 0;
        public bool IsActiveUltimate { get; set; }

        private Health Player { get; set; }
        
        public PlayerStateMachine PlayerStateMachine { get; private set; }
        
        public bool IsWalking {get; set;} = false;

        private State _locomotionState;

        public State CurrentState;
        
        public bool IsStillUltimate { get; set; } = false;
        public bool IsCanMove {get; set; } = false;

        
        private void Awake()
        {
            _locomotionState = new FinalBossLocomotionState(this);
        }
        
        private void Start()
        {
            Player = GameObject.FindWithTag("Player").GetComponent<Health>();
            PlayerStateMachine = Player.GetComponent<PlayerStateMachine>();
        }
        
        private void OnEnable()
        {
            Health.HitAction += EnterHitState;
            Health.DeathAction += EnterDeathState;
            GameEventManagers.OnSkillCasted += HandleSkillEvent;
        }
        
        private void OnDisable()
        {
            Health.HitAction -= EnterHitState;
            Health.DeathAction -= EnterDeathState;
            GameEventManagers.OnSkillCasted -= HandleSkillEvent;
        }
        

        public void ReturnLocomotion()
        {
            SwitchState(_locomotionState);
        }
        
        private void EnterHitState()
        {
            if (TimesHit == TimesToHit)
            {
                TimesHit = 0;
                SwitchState(new FinalBossHitState(this));
            }

            TimesHit += 1;
        }
        private void EnterDeathState()
        {
            SwitchState(new FinalBossDeathState(this));
        }
        
        public void ResetMovement()
        {
            MovementSpeed = 5f;
            SprintSpeed = 5f;
        }

        private void HandleSkillEvent(ICaster caster, SkillEffect skillEffect)
        {
                ActiveEnventBySkill(skillEffect)?.Invoke();
        }
        
        

        public void ComsumeMana(int amount)
        {
            throw new System.NotImplementedException();
        }

        public Transform GetTransform()
        {
            return this.transform;
        }

        public GameObject TargetCaster()
        {
            return gameObject;
        }

        private Action ActiveEnventBySkill(SkillEffect skillEffect)
        {
            return skillEffect switch
            {
                SkillEffect.NonEffect => () => {Debug.Log("NonEffect");},
                SkillEffect.Inescapable => () =>
                {
                    ForceReceiver.SetCoefficientOfMovement(0f);
                    ReturnLocomotion();
                },
                SkillEffect.Stunned => () => {Debug.Log("Stunned"); },
                SkillEffect.ThrowUp => () => {Debug.Log("ThrowUp"); },
                SkillEffect.NoDamage => () => {Debug.Log("NoDamage"); },
                SkillEffect.Invisible => ReturnLocomotion,
                _ => null
            };
        }

        public bool IsAttackRange()
        {
            return ((Player.transform.position -
                     transform.position)
                       .sqrMagnitude <= AttackRange *
                       AttackRange) &&
                   !PlayerStateMachine.Invisible;
        }


        public bool IsWalkRange()
        {
            return ((Player.transform.position -
                     transform.position)
                       .sqrMagnitude <= AttackRange *
                       WalkRange) &&
                   !PlayerStateMachine.Invisible;
        }
        
        public Vector3 GetDirToPlayer()
        {
            if (PlayerStateMachine.Invisible)
            {
                return Vector3.zero;
            }

            var dir = (Player.transform.position - transform.position)
                .normalized;
            dir.y = 0;
            return dir;
        }

        public void Coroutine(float time, Action action)
        {
            StartCoroutine(WaitToContinue(time, action));
        }


        private IEnumerator WaitToContinue(float time, Action action)
        {
            yield return new WaitForSecondsRealtime(time);
            action?.Invoke();
        }
        
        public Vector2 InputMovement { get; set; }
        public Vector2 Look { get; set; }
        public bool IsChasing { get; set; }
        public bool IsSprint { get; set; }
        public bool IsAttack { get; set; }
        public int SkillNumber { get; set; }
        public event Action JumpAction;
        public event Action DodgeAction;
        public event Action TargetAction;
        public event Action<int> SkillAction;
    }
}
