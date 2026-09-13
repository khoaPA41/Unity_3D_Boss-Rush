using System;
using System.Collections;
using Script.Attack;
using Script.Attack.Skill_Factory;
using Script.Design_Pattern.EventBus;
using Script.Design_Pattern.StateMachine.Base;
using Script.Design_Pattern.StateMachine.Player.Base;
using Script.Design_Pattern.Tree_Behavior;
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
        [field: SerializeField] public BossBehaviorBrain BossBehaviorBrain { get; private set; }
        [field: SerializeField] public PlayerSFX PlayerSFX { get; private set; }
        [field: SerializeField] public UltimateCombo[] UltimateCombo { get; private set; }
        [field: SerializeField] public NormalCombo[] NormalCombo { get; private set; }
        [field: SerializeField] public WeaponTrail[] DealsDamage { get; private set; }
        [field: SerializeField] public Health Health { get; private set; }

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


        [field: Header("Event")] public Health Player { get; private set; }
        public PlayerStateMachine PlayerStateMachine { get; private set; }
        public bool IsWalking { get; set; }
        public event Action<int> SkillAction;
        public bool IsCanMove { get; set; } = false;
        public Transform Target { get; set; }


        public Vector2 InputMovement { get; set; }
        public bool IsChasing { get; set; }
        public bool IsSprint { get; set; }
        public bool IsCounterAttack { get; set; }
        public bool IsAttack { get; set; }
        public int SkillNumber { get; set; }


        //State
        private State LocomotionState;
        private State CounterAttackState;
        private State HitState;
        private State DeadState;


        private void Awake()
        {
            Player = GameObject.FindWithTag("Player").GetComponent<Health>();
            Target = Player.gameObject.transform;
            PlayerStateMachine = Player.GetComponent<PlayerStateMachine>();
            LocomotionState = new BossLocomotionState(this);
            HitState = new BossHitState(this);
            DeadState = new BossDeathState(this);
            CounterAttackState = new BossCounterAttackState(this);
        }

        private void Start()
        {

            GameEventManagers.Instance.OnSkillCasted += HandleSkillEvent;
        }

        private void OnEnable()
        {
            Health.HitAction += EnterHitState;
            Health.DeathAction += EnterDeathState;
        }

        private void OnDisable()
        {
            Health.HitAction -= EnterHitState;
            Health.DeathAction -= EnterDeathState;
            GameEventManagers.Instance.OnSkillCasted -= HandleSkillEvent;
        }

        public void ReturnLocomotion()
        {
            SwitchState(LocomotionState);
        }

        public void EnterCounterAttackState()
        {
            SwitchState(CounterAttackState);
        }

        private void EnterHitState()
        {
            SwitchState(HitState);
        }

        private void EnterDeathState()
        {
            SwitchState(DeadState);
        }

        public void ComsumeMana(int amount)
        {
            // Nothing with boss
        }

        public Transform GetTransform()
        {
            return transform;
        }

        public GameObject TargetCaster()
        {
            return Target.gameObject;
        }

        private void HandleSkillEvent(ICaster caster, SkillEffect skillEffect)
        {
            if (caster.GetTransform().gameObject.TryGetComponent(out BossStateMachine _)) return;
            ActiveEnventBySkill(skillEffect)?.Invoke();
        }
        private Action ActiveEnventBySkill(SkillEffect skillEffect)
        {
            return skillEffect switch
            {
                SkillEffect.NonEffect => () => { Debug.Log("NonEffect"); }
                ,
                SkillEffect.Inescapable => () => Coroutine(3f, () =>
                    {
                        ForceReceiver.SetCoefficientOfMovement(0f);
                        ReturnLocomotion();
                    },
                    () => ForceReceiver.SetCoefficientOfMovement(1f)
                ),
                SkillEffect.Invisible => ReturnLocomotion,
                _ => null
            };
        }

        private void Coroutine(float time, Action action1, Action action2)
        {
            StartCoroutine(WaitToContinue(time, action1, action2));
        }

        private IEnumerator WaitToContinue(float time, Action action1, Action action2)
        {
            action1?.Invoke();
            yield return new WaitForSecondsRealtime(time);
            action2?.Invoke();
        }
    }
}

