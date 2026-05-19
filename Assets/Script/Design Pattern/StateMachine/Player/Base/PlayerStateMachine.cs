using System;
using System.Collections;
using Script.Attack;
using Script.Attack.Skill_Factory;
using Script.Design_Pattern.EventBus;
using Script.Design_Pattern.StateMachine.Player.Main;
using Script.Physics;
using UnityEngine;

namespace Script.Design_Pattern.StateMachine.Player.Base
{
    public class PlayerStateMachine : StateMachine.Base.StateMachine, ICaster
    {
        [Header("Input")]
        [field: SerializeField]
        public InputReader InputReader { get; private set; }

        [Header("Physics")]
        [field: SerializeField]
        public CharacterController CharacterController { get; private set; }
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

        [Header("Skill")]
        [field: SerializeField]
        public SkinnedMeshRenderer SkinnedMeshRenderer { get; private set; }

        [field: SerializeField] public float SkillTime { get; private set; }
        [field: SerializeField] public Material PhantomMaterial1 { get; private set; }
        [field: SerializeField] public Material PhantomMaterial2 { get; private set; }
        [field: SerializeField] public Material IronMaterial1 { get; private set; }
        [field: SerializeField] public Material IronMaterial2 { get; private set; }
        [field: SerializeField] public Material MainMaterial1 { get; private set; }
        [field: SerializeField] public Material MainMaterial2 { get; private set; }

        public Transform MainCameraTransform { get; private set; }
        
        public bool Invisible;
        
        public bool Invincible;
        private int HitTimes { get; set; }
        public float CountSkillTime { get; set; }
        public bool isAttackState;
        public bool IsActiveEffect { get; set; } = false;
        private void Start()
        {
            InputReader.ApplicationCursor();
            if (Camera.main is not null) MainCameraTransform = Camera.main.transform;
            SwitchState(new FreeLookState(this));
        }

        private void OnEnable()
        {
            GameEventManagers.OnSkillCasted += HandleSkillEvent;
            Health.HitAction += EnterHitState;
            Health.DeathAction += EnterDeathState;
        }

        private void OnDisable()
        {
            GameEventManagers.OnSkillCasted += HandleSkillEvent;
            Health.HitAction -= EnterHitState;
            Health.DeathAction -= EnterDeathState;
        }

        public void ReturnLocomotion()
        {
            if (Targeter.currentTarget is null)
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

        private void EnterDeathState()
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
            HitTimes++;
            var isKnockBack = false;

            if (HitTimes == TimeToGetKnockBackHit)
            {
                isKnockBack = true;
                HitTimes = 0;
            }

            SwitchState(new PlayerHitState(this, isKnockBack));
        }

        public void EnterSkillState(int skillNumber)
        {
            if (Invincible) {return;}
            
            if (Mana.currentMana <= 0)
            {
                return;
            }
            
            SwitchState(new PlayerUseSkillState(this, skillNumber));
        }

        public void ComsumeMana(int amount)
        {
            Mana.currentMana = Mathf.Max(Mana.currentMana - amount, 0);
        }

        public Transform GetTransform()
        {
            return this.transform;
        }

        public GameObject TargetCaster()
        {
            return this.gameObject;
        }

        private void HandleSkillEvent(ICaster caster, SkillEffect skillEffect)
        {
            ActiveSkillEvent(skillEffect)?.Invoke();
        }
        
        private Action ActiveSkillEvent(SkillEffect skillEffect)
        {
            return skillEffect switch
            {
                SkillEffect.NonEffect => () => {Debug.Log("NonEffect"); },
                SkillEffect.Inescapable => () => {Debug.Log("Inescapable");},
                SkillEffect.Stunned => () => {Debug.Log("Stunned"); },
                SkillEffect.ThrowUp => () => {Debug.Log("ThrowUp"); },
                SkillEffect.NoDamage => () =>
                {
                    Debug.Log("NoDamage");
                },
                SkillEffect.Invisible => () => { },
                _ => null
            };
        }

        public void InvincibleState()
        {
            FreeLookMovementSpeed *= 1.5f;
            FreeLookMovementSprintSpeed *= 0.8f;

            foreach (var dame in AttackData)
            {
                dame.AttackDamage *= 2;
            }
        }
    }
}