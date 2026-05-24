using System;
using Script.Attack;
using Script.Attack.Skill_Factory;
using Script.Design_Pattern.EventBus;
using Script.Design_Pattern.StateMachine.Boss.Main;
using Script.Design_Pattern.StateMachine.Player.Base;
using Script.Design_Pattern.Tree_Behavious.Dependency_Injection;
using Script.Physics;
using Unity.VisualScripting;
using UnityEngine;

namespace Script.Design_Pattern.StateMachine.Boss.Base
{
    public class FinalBossStateMachine : StateMachine.Base.StateMachine, ICaster, ICombatInput
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
        [field: SerializeField] public float WalkRange { get; private set; } = 8f;


        [Header("Animation")]
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public float AnimationCrossFade { get; private set; } = .1f;

        [Header("State")]
        [field: SerializeField] public float TimeToEnterChasing { get; private set; } = 1f;
        [field: SerializeField] public int TimesToHit { get; private set; } = 3;
        public int TimesHitted { get; set; } = 0;

        public Health Player { get; private set; }
        
        public PlayerStateMachine PlayerStateMachine { get; private set; }

        
        public bool isWalking {get; set;} = false;
        private void Start()
        {
            Player = GameObject.FindWithTag("Player").GetComponent<Health>();
            PlayerStateMachine = Player.GetComponent<PlayerStateMachine>();
            // ReturnLocomotion();
        }

        private void OnEnable()
        {
            // Health.DeathAction += EnterDeathState;
            GameEventManagers.OnSkillCasted += HandleSkillEvent;
        }

        private void OnDisable()
        {
            // Health.DeathAction -= EnterDeathState;
            GameEventManagers.OnSkillCasted -= HandleSkillEvent;
        }

        public void ReturnLocomotion()
        {
            SwitchState(new FinalBossLocomotionState(this));
        }

        public void EnterChasingState(bool isWalk)
        {
            SwitchState(new FinalBossChasingState(this, isWalk));
        }

        public void EnterAttackState()
        {
            SwitchState(new FinalBossAttackState(this, 0));
        }

        private void EnterDeathState()
        {
            SwitchState(new FinalBossDeathState(this));
        }

        public void EnterHitState()
        {
            if (TimesHitted == TimesToHit)
            {
                return;
            }

            TimesHitted += 1;

            Debug.Log("Boss hit");

            SwitchState(new FinalBossHitState(this));
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
