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
        [field: SerializeField] public InputBuffering InputBuffering { get; private set; }

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
        [field: SerializeField] public SkillActive SkillActive { get; private set; }
        [field: SerializeField] public WeaponTrail DealDamage { get; private set; }
        [field: SerializeField] public Health Health { get; private set; }
        [field: SerializeField] public Mana Mana { get; private set; }
        [field: SerializeField] public Stamina Stamina { get; private set; }
        [field: SerializeField] public int TimeToGetKnockBackHit { get; private set; } = 3;
        [field: SerializeField] public DodgeAward DodgeAward { get; private set; }
        
        [Header("Potion")]
        [field: SerializeField]
        public HealthPotion HealthPotion { get; private set; }
        [field: SerializeField] public ManaPotion ManaPotion { get; private set; }
        [field: SerializeField] public SubPotion SubPotion { get; private set; }

        
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

        [Header("Effect")]
        [field: SerializeField] public GameObject PotionLight { get; private set; }
        [field: SerializeField] public ParticleSystem HealthParticle { get; private set; }
        [field: SerializeField] public ParticleSystem ManaParticle { get; private set; }
        
        [Header("Coins")]
        [field: SerializeField] public int PlayerSpiritualPower { get; private set; }
        public event Action<int> UpdateSpiritualPower;
        public Transform MainCameraTransform { get; private set; }
        public bool Invincible;
        public bool Invisible;
        public int SkillNumber;
        public bool isAttackState;
        public bool IsActiveEffect { get; set; } = false;
        public bool IsAttractiveForce { get; set; }
        
        /*Potion*/
        public bool IsHealthPotion { get; set; } = true;
        public bool IsIncreaseDamePotion { get; set; }

        public bool isCanNotSubSpiritual;

        public bool isCanNotAddSpiritual;
        /*Dodge Award*/
        public bool IsCounterAttack { get; set; }
        /****************************************************************/
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
        public GameObject Boss { get; private set; }
        
        private void Start()
        {
            Boss = GameObject.FindWithTag("Boss");
            AddSpiritualPower();
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
            InputReader.ChangeHealthPotionAction += ChangeHealthPotion;
            InputReader.ChangeManaPotionAction += ChangeManaPotion;
        }
        
        private void OnDisable()
        {
            GameEventManagers.Instance.OnSkillCasted += HandleEffectedState;
            Health.HitAction -= HandleHitState;
            Health.DeathAction -= HandleDeathState;
            InputReader.ChangeHealthPotionAction -= ChangeHealthPotion;
            InputReader.ChangeManaPotionAction -= ChangeManaPotion;
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
        }
        
        public void ReturnLocomotion()
        {
            Debug.Log("Return Locomotion");
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
        
        public void HandleUsePotionState()
        {
            if (IsHealthPotion)
            {
                if (HealthPotion.CurrentPotion <= 0) return;
            }
            else
            {
                if (ManaPotion.CurrentPotion <= 0) return;
            }
            
            SwitchState(new PlayerUsePotionState(this));
        }
        
        public void HandleUseSubPotionState()
        {
            if (SubPotion.currentPotion.quantity <= 0)
            {
                Debug.Log("Sold out");
                return;
            }
            SwitchState(new PlayerUseSubPotionState(this));
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
            
            SkillNumber =  skillNumber;
            SwitchState(skillState);
        }

        public bool CheckLowStamina()
        {
            return Stamina.currentStamina <= 0f;
        }
        
        public void HandleAttackState()
        {
            if (!InputReader.IsAttack) return;
            if (!isAttackState)
            {
                EnterChangeAction(true);
                return;
            }
            SwitchState(attackState1);
        }
        
        public void HandleHeavyAttackState()
        {
            if (!InputReader.IsHeavyAttack) return;
            if (!isAttackState)
            {
                EnterChangeAction(true);
                return;
            }
            SwitchState(heavyAttack);
        }

        private void HandleEffectedState(ICaster caster, SkillEffect effect)
        {
            if (caster.GetTransform().gameObject.TryGetComponent(out PlayerStateMachine _))return;
            SwitchState(new PlayerAffectedState(this, caster, effect));
        }
        
        private void ChangeHealthPotion()
        {
            IsHealthPotion = true;
        }
        
        private void ChangeManaPotion()
        {
            IsHealthPotion = false;
        }

        public void ComsumeMana(int amount)
        {
            Mana.ChangeMana(amount);
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
        
        public void SubSpiritualPower()
        {
            if (PlayerSpiritualPower <= 0)
            {
                isCanNotSubSpiritual = true;
                return;
            }
            PlayerSpiritualPower = Mathf.Max(PlayerSpiritualPower - 1, 0);
            UpdateSpiritualPower?.Invoke(PlayerSpiritualPower);
        }

        public void AddSpiritualPower()
        {
            if (isCanNotAddSpiritual)
            {
                isCanNotAddSpiritual = false;
                return;
            }
            PlayerSpiritualPower = Mathf.Min(PlayerSpiritualPower + 1, 1000000);
            UpdateSpiritualPower?.Invoke(PlayerSpiritualPower);
        }

        private void ResetStateSpiritualPower()
        {
            isCanNotAddSpiritual = false;
            isCanNotSubSpiritual = false;
        }
        
        public void AddDamage()
        {
            if (isCanNotSubSpiritual || PlayerSpiritualPower <= 0)
            { 
                isCanNotSubSpiritual = false;
                return;
            }
            foreach (var damage in AttackData)
            {
                damage.AttackDamage += 1f;
            }
        }
        
        public void SubtractDamage()
        {
            if (AttackData[0].AttackDamage == 10)
            {
                isCanNotAddSpiritual = true;
                return;
            }
            foreach (var damage in AttackData)
            {
                damage.AttackDamage = Mathf.Max(damage.AttackDamage - 1, 10);
            }
        }
    }
}