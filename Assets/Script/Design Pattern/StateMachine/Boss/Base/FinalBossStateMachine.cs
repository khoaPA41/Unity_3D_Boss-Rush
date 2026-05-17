using System;
using Script.Attack.Skill_Factory;
using Script.Design_Pattern.EventBus;
using Script.Physics;
using Unity.VisualScripting;
using UnityEngine;

namespace Script.Design_Pattern.StateMachine.Boss.Base
{
    public class FinalBossStateMachine : StateMachine.Base.StateMachine, ICaster
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

        private void Start()
        {
            Player = GameObject.FindWithTag("Player").GetComponent<Health>();
            ReturnLocomotion();
        }

        private void OnEnable()
        {
            Health.DeathAction += EnterDeathState;
            GameEventManagers.OnSkillCasted += HandleSkillEvent;
        }

        private void OnDisable()
        {
            Health.DeathAction -= EnterDeathState;
            GameEventManagers.OnSkillCasted -= HandleSkillEvent;
        }

        public void ReturnLocomotion()
        {
            SwitchState(new FinalBossLocomotionState(this));
            return;
        }

        public void EnterChasingState(bool isWalk)
        {
            SwitchState(new FinalBossChasingState(this, isWalk));
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

        public void EnterHitState()
        {
            if (TimesHitted == TimesToHit)
            {
                return;
            }

            TimesHitted += 1;

            SwitchState(new FinalBossHitState(this));
            return;
        }
    

        public void ResetMovement()
        {
            MovementSpeed = 5f;
            SprintSpeed = 5f;
        }

        private void HandleSkillEvent(ICaster caster, SkillEffect skillEffect)
        {
            if (TargetCaster() == caster.TargetCaster())
            {
                Debug.Log("It's Me!");
            }
            else
            {
                ActiveEnventBySkill(skillEffect)?.Invoke();
            }
        }

        public void ComsumeMana(int amount)
        {
            throw new System.NotImplementedException();
        }

        public Transform GetTransform()
        {
            throw new System.NotImplementedException();
        }

        public GameObject TargetCaster()
        {
            return gameObject;
        }

        private Action ActiveEnventBySkill(SkillEffect skillEffect)
        {
            return skillEffect switch
            {
                SkillEffect.NonEffect => null,
                SkillEffect.Inescapable => () =>
                {
                    ForceReceiver.SetCoefficientOfMovement(0f);
                    ReturnLocomotion();
                    Debug.Log("Non");
                },
                SkillEffect.Stunned => null,
                SkillEffect.ThrowUp => null,
                _ => null
            };
        }
    }
}
