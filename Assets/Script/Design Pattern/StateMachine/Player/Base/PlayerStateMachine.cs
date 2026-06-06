using System;
using System.Collections;
using Script.Attack;
using Script.Attack.Skill_Factory;
using Script.Design_Pattern.EventBus;
using Script.Design_Pattern.StateMachine.Base;
using Script.Design_Pattern.StateMachine.Player.Main;
using Script.Physics;
using Script.Target;
using UnityEngine;

namespace Script.Design_Pattern.StateMachine.Player.Base
{
    public class PlayerStateMachine : StateMachine.Base.StateMachine, ICaster
    {
        [Header("Input")]
        [field: SerializeField] public InputReader InputReader { get; private set; }
        
        [Header("Physics")]
        [field: SerializeField]
        public CharacterController CharacterController { get; private set; }
        [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
        [field: SerializeField] public float FreeLookMovementSpeed { get; set; } = 5f;
        [field: SerializeField] public float FreeLookMovementSprintSpeed { get; set; } = 5f;
        [field: SerializeField] public float MovementSpeedStunnedCoefficient { get; private set; } = .2f;
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
        [field: SerializeField] public AttackData[] SkillData { get; private set; }
        [field: SerializeField] public WeaponTrail DealDamage { get; private set; }
        [field: SerializeField] public Health Health { get; private set; }
        [field: SerializeField] public Mana Mana { get; private set; }
        [field: SerializeField] public int TimeToGetKnockBackHit { get; private set; } = 3;
        
        [Header("Animation")]
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public float AnimationCrossFade { get; private set; } = .1f;
        [field: SerializeField] public AnimationClip SwordIdleAnimationClip { get; private set; }
        [field: SerializeField] public AnimationClip IdleLoopAnimationClip { get; private set; }
        [field: SerializeField] public float TimeToBackIdleLoop { get; private set; }
        [field: SerializeField] public ManageAnimationSkillEvent ManageAnimationSkillEvent { get; private set; }


        [Header("Skill")]
        [field: SerializeField] public SkinnedMeshRenderer SkinnedMeshRenderer { get; private set; }
        [field: SerializeField] public float SkillTime { get; private set; }
        [field: SerializeField] public Material PhantomMaterial1 { get; private set; }
        [field: SerializeField] public Material PhantomMaterial2 { get; private set; }
        [field: SerializeField] public Material IronMaterial1 { get; private set; }
        [field: SerializeField] public Material IronMaterial2 { get; private set; }
        [field: SerializeField] public Material MainMaterial1 { get; private set; }
        [field: SerializeField] public Material MainMaterial2 { get; private set; }

        public Transform MainCameraTransform { get; private set; }
        
        public bool Invincible;
        public bool Invisible;
        public int SkillNumber;
        public bool isAttackState;
        public bool IsActiveEffect { get; set; } = false;
        public bool IsAttractiveForce { get; set; }
        
        public State freeLookState { get; private set; }
        public State hitState { get; private set; }
        public State deathState { get; private set; }
        public State jumpState { get; private set; }
        public State fallState { get; private set; }
        public State landingState { get; private set; }
        public State targetState { get; private set; }
        public State skillState { get; private set; }
        public State dodgeState { get; private set; }
        public State attackState1 { get; private set; }
        public State attackState2 { get; private set; }
        public State attackState3 { get; private set; }
        public State attackState4 { get; private set; }
        public State attackState5 { get; private set; }
        public State heavyAttack { get; private set; }
        
        public State changeAction { get; private set; }


        public GameObject Boss;
        private void Start()
        {
            Boss = GameObject.FindWithTag("Boss");
            SetupState();
            InputReader.ApplicationCursor();
            if (Camera.main is not null) MainCameraTransform = Camera.main.transform;
            ReturnLocomotion();
        }

        private void SetupState()
        {
            freeLookState = new FreeLookState(this);
            hitState = new PlayerHitState(this, false);
            deathState = new PlayerDeathState(this);
            jumpState = new PlayerStartJumpState(this);
            fallState = new PlayerFallState(this);
            landingState = new PlayerLandingState(this);
            targetState = new PlayerTargetState(this);
            skillState = new PlayerUseSkillState(this);
            dodgeState = new PlayerDodgeState(this);
            attackState1 = new PlayerAttackState(this, 0);
            attackState2 = new PlayerAttackState(this, 1);
            attackState3 = new PlayerAttackState(this, 2);
            attackState4 = new PlayerAttackState(this, 3);
            heavyAttack = new PlayerHeavyAttackState(this);
        }
        
        private void OnEnable()
        {
            GameEventManagers.Instance.OnSkillCasted += HandleEffectedState;
            Health.HitAction += HandleHitState;
            Health.DeathAction += HandleDeathState;
        }
        
        private void OnDisable()
        {
            GameEventManagers.Instance.OnSkillCasted += HandleEffectedState;
            Health.HitAction -= HandleHitState;
            Health.DeathAction -= HandleDeathState;
        }

        public void SendSituationEvent()
        {
            ManageAnimationSkillEvent.SendSituationEvent();
        }
        public void SendNextActionEvent()
        {
            ManageAnimationSkillEvent.SendNextActionEvent();

        }
        public void SendReleaseObjectEvent()
        {
            ManageAnimationSkillEvent.SendReleasePoolObjectEvent();
        }
        
        
        public void EnterChangeAction(bool isAttack)
        {
            SwitchState(new PlayerChangeAction(this, isAttack));
            return;
        }
        
        public void ReturnLocomotion()
        {
            SwitchState(Targeter.currentTarget is null ? freeLookState : targetState);
        }

        public void HandleJumpState()
        {
            SwitchState(jumpState);
        }

        public void HandleDodgeState()
        {
            SwitchState(dodgeState);
        }
        
        public void HandleTargetState()
        {
            if (!Targeter.SelectedTarget()) return;
            SwitchState(targetState);
        }

        private void HandleHitState()
        {
            SwitchState(hitState);
        }

        private void HandleDeathState()
        {
            SwitchState(deathState);
        }
        
        public void HandleSkillEvent(int skillNumber)
        {
            if (Invincible)
            {
                return;
            }

            if (Mana.currentMana <= 0)
            {
                return;
            }

            SkillNumber = skillNumber;
            SwitchState(skillState);
            
        }

        public void HandleAttackState()
        {
            if (!InputReader.IsAttack) return;
            SwitchState(attackState1);
        }
        
        public void HandleHeavyAttackState()
        {
            if (!InputReader.IsHeavyAttack) return;
            SwitchState(heavyAttack);
        }

        private void HandleEffectedState(ICaster caster, SkillEffect effect)
        {
            if (caster.GetTransform().gameObject.TryGetComponent(out PlayerStateMachine _))return;
            SwitchState(new PlayerAffectedState(this, caster, effect));
        }

        public void ComsumeMana(int amount)
        {
            Mana.currentMana = Mathf.Max(Mana.currentMana - amount, 0);
        }

        public Transform GetTransform()
        {
            return transform;
        }

        public GameObject TargetCaster()
        {
            return Targeter?.currentTarget.gameObject;
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
        
        public void Coroutine(float time, Action action1, Action action2)
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