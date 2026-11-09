using DesignPattern.StateMachine.Player.Main;
using Audio;
using System;
using System.Collections;
using Physics;
using Target;
using UnityEngine;
using UnityEngine.Audio;
using Attack;
using Attack.Status;
using Attack.Status.Potion;
using Attack.Status.Award;
using Attack.SkillFactory;
using DesignPattern.EventBus;
using DesignPattern.StateMachine.Base;
using Interact;
using Input;

namespace DesignPattern.StateMachine.Player.Base
{
    public class PlayerStateMachine : StateMachine.Base.StateMachine, ICaster
    {
        [field: Header("Input")]
        [field: SerializeField] public InputReader InputReader { get; private set; }
        [field: SerializeField] public InputBuffering InputBuffering { get; private set; }

        [field: Header("Physics")]
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

        [field: Header("Attack")]
        [field: SerializeField] public AttackData[] AttackData { get; private set; }
        [field: SerializeField] public WeaponTrail WeaponTrail { get; private set; }
        [field: SerializeField] public Transform WeaponTip { get; private set; }
        [field: SerializeField] public Transform WeaponTransform { get; private set; }
        [field: SerializeField] public GameObject WeaponTrailVisual { get; private set; }
        [field: SerializeField] public SkillActive SkillActive { get; private set; }

        public Vector3 StartSwordPos { get; set; }
        public Vector3 EndSwordPos { get; set; }

        [field: SerializeField] public Health Health { get; private set; }
        [field: SerializeField] public Mana Mana { get; private set; }
        [field: SerializeField] public Stamina Stamina { get; private set; }
        [field: SerializeField] public int TimeToGetKnockBackHit { get; private set; } = 3;
        [field: SerializeField] public DodgeAward DodgeAward { get; private set; }
        [field: SerializeField] public PlayerSFX PlayerSFX { get; private set; }

        [field: Header("Potion")]
        [field: SerializeField]
        public HealthPotion HealthPotion { get; private set; }
        [field: SerializeField] public ManaPotion ManaPotion { get; private set; }
        [field: SerializeField] public SubPotion SubPotion { get; private set; }


        [field: Header("Animation")]
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public float AnimationCrossFade { get; private set; } = .1f;
        [field: SerializeField] public AnimationClip SwordIdleAnimationClip { get; private set; }
        [field: SerializeField] public AnimationClip IdleLoopAnimationClip { get; private set; }
        [field: SerializeField] public float TimeToBackIdleLoop { get; private set; }
        [field: SerializeField] public SkillAnimationEvents ManageAnimationSkillEvent { get; private set; }

        [field: Header("Skill")]
        [field: Header("MeshRenderer Object Normal")]
        [field: SerializeField] public GameObject ArmNormal { get; private set; }
        [field: SerializeField] public GameObject HeadNormal { get; private set; }
        [field: SerializeField] public GameObject TorsoNormal { get; private set; }
        [field: SerializeField] public GameObject ClothNormal { get; private set; }
        [field: SerializeField] public GameObject HairNormal { get; private set; }
        [field: SerializeField] public GameObject HarnessNormal { get; private set; }
        [field: SerializeField] public GameObject LegNormal { get; private set; }

        [field: Header("MeshRenderer Object Indestructible Skill")]
        [field: SerializeField] public GameObject ArmIndestructible { get; private set; }
        [field: SerializeField] public GameObject HeadIndestructible { get; private set; }
        [field: SerializeField] public GameObject TorsoIndestructible { get; private set; }

        [field: Header("MeshRenderer Object Invisible Skill")]
        [field: SerializeField] public GameObject ArmInvisible { get; private set; }
        [field: SerializeField] public GameObject HeadInvisible { get; private set; }
        [field: SerializeField] public GameObject TorsoInvisible { get; private set; }
        [field: SerializeField] public GameObject ClothInvisible { get; private set; }
        [field: SerializeField] public GameObject HairInvisible { get; private set; }
        [field: SerializeField] public GameObject HarnessInvisible { get; private set; }
        [field: SerializeField] public GameObject LegInvisible { get; private set; }


        [field: Header("Effect")]
        [field: SerializeField] public GameObject PotionLight { get; private set; }
        [field: SerializeField] public ParticleSystem HealthParticle { get; private set; }
        [field: SerializeField] public ParticleSystem ManaParticle { get; private set; }

        [field: Header("Effect")]
        [field: SerializeField]
        public CheckPoint CheckPoint { get; private set; }
        [field: SerializeField] public GameObject TargetPoint { get; set; }

        [field: Header("Coins")]
        [field: SerializeField] public int PlayerSpiritualPower { get; private set; }


        [field: Header("Audio")]
        [field: SerializeField] public AudioResource SwordSwingAudioResource { get; private set; }
        public event Action<int> UpdateSpiritualPower;
        public Transform MainCameraTransform { get; private set; }
        public bool Invincible;
        public bool Invisible;
        public int SkillNumber;
        public bool IsAttackState;
        public bool IsActiveEffect { get; set; } = false;
        public bool IsAttractiveForce { get; set; }

        /*Potion*/
        public bool IsHealthPotion { get; set; } = true;
        public bool IsIncreaseDamagePotion { get; set; }

        public bool IsCanNotSubSpiritual;

        public bool IsCanNotAddSpiritual;
        /*Dodge Award*/
        public bool IsCounterAttack { get; set; }
        /****************************************************************/
        public State FreeLookState { get; private set; }
        public State HitState { get; private set; }
        public State DeathState { get; private set; }
        public State JumpState { get; private set; }
        public State FallState { get; private set; }
        public State LandingState { get; private set; }
        public State TargetState { get; private set; }
        public State SkillState { get; private set; }
        public State DodgeState { get; private set; }
        public State AttackState1 { get; private set; }
        public State HeavyAttack { get; private set; }
        public State UseMainPotionAction { get; private set; }
        public GameObject Boss { get; private set; }

        public event Action ActiveSlashVfxAction = delegate { };

        private void Start()
        {
            Boss = GameObject.FindWithTag("Boss");
            UpdateSpiritualPower?.Invoke(PlayerSpiritualPower);
            SetupState();
            InputReader.ApplicationCursor();
            if (Camera.main != null) MainCameraTransform = Camera.main.transform;
            ReturnLocomotion();
        }

        private void SetupState()
        {
            FreeLookState = new FreeLookState(this);
            HitState = new PlayerHitState(this, false);
            DeathState = new PlayerDeathState(this);
            JumpState = new PlayerStartJumpState(this);
            FallState = new PlayerFallState(this);
            LandingState = new PlayerLandingState(this);
            TargetState = new PlayerTargetState(this);
            SkillState = new PlayerUseSkillState(this);
            DodgeState = new PlayerDodgeState(this);
            AttackState1 = new PlayerAttackState(this, 0);
            HeavyAttack = new PlayerHeavyAttackState(this);
            UseMainPotionAction = new PlayerUsePotionState(this);

            GameEventManager.Instance.OnSkillCasted += HandleEffectedState;
        }

        private void OnEnable()
        {
            Health.HitAction += HandleHitState;
            Health.DeathAction += HandleDeathState;
            InputReader.ChangeHealthPotionAction += ChangeHealthPotion;
            InputReader.ChangeManaPotionAction += ChangeManaPotion;
        }

        private void OnDisable()
        {
            GameEventManager.Instance.OnSkillCasted -= HandleEffectedState;
            Health.HitAction -= HandleHitState;
            Health.DeathAction -= HandleDeathState;
            InputReader.ChangeHealthPotionAction -= ChangeHealthPotion;
            InputReader.ChangeManaPotionAction -= ChangeManaPotion;
        }

        public void CallSlashVfx()
        {
            ActiveSlashVfxAction?.Invoke();
        }

        public void RecordSwordStartPos()
        {
            StartSwordPos = WeaponTip.transform.position;
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
            SwitchState(new PlayerChangeActionState(this, isAttack));
        }

        public void ReturnLocomotion()
        {
            SwitchState(Targeter.CurrentTarget is null ? FreeLookState : TargetState);
        }

        public void HandleJumpState()
        {
            if (Stamina.CurrentStamina < Stamina.jumpReduce) return;
            SwitchState(JumpState);
        }

        public void HandleDodgeState()
        {
            if (Stamina.CurrentStamina < Stamina.DodgeReduce) return;
            SwitchState(DodgeState);
        }

        public void HandleTargetState()
        {
            if (!Targeter.SelectTarget()) return;
            SwitchState(TargetState);
        }

        private void HandleHitState()
        {
            SwitchState(HitState);
        }

        private void HandleDeathState()
        {
            SwitchState(DeathState);
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

            SwitchState(UseMainPotionAction);
        }

        public void HandleUseSubPotionState()
        {
            if (SubPotion.currentPotion.quantity <= 0)
            {
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

            if (Mana.CurrentMana <= 0)
            {
                return;
            }

            SkillNumber = skillNumber;
            SwitchState(SkillState);
        }

        public bool CheckLowStamina()
        {
            return Stamina.CurrentStamina <= 0f;
        }

        public bool HandleAttackState()
        {
            if (Stamina.CurrentStamina < Stamina.lightAttackReduce) return false;

            if (!InputReader.IsAttack) return false;
            if (!IsAttackState)
            {
                EnterChangeAction(true);
                return true;
            }
            SwitchState(AttackState1);
            return true;
        }

        public bool HandleHeavyAttackState()
        {
            if (Stamina.CurrentStamina < Stamina.heavyAttackReduce) return false;

            if (!InputReader.IsHeavyAttack) return false;
            if (!IsAttackState)
            {
                EnterChangeAction(true);
                return true;
            }
            SwitchState(HeavyAttack);
            return true;
        }

        private void HandleEffectedState(ICaster caster, SkillEffect effect)
        {
            if (caster.GetTransform().gameObject.TryGetComponent(out PlayerStateMachine _)) return;
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

        public void ConsumeMana(int amount)
        {
            Mana.ChangeMana(amount);
        }

        public Transform GetTransform()
        {
            return transform;
        }

        public GameObject TargetCaster()
        {
            return Targeter != null ? Targeter.CurrentTarget.gameObject : null;
        }

        public void ApplyInvincibleBuff()
        {
            FreeLookMovementSpeed *= 1.5f;
            FreeLookMovementSprintSpeed *= 0.8f;

            foreach (var damage in AttackData)
            {
                damage.AttackDamage *= 2;
            }
        }

        public void StartTimedAction(float time, Action action1, Action action2)
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
                IsCanNotSubSpiritual = true;
                return;
            }
            PlayerSpiritualPower = Mathf.Max(PlayerSpiritualPower - 1, 0);
            UpdateSpiritualPower?.Invoke(PlayerSpiritualPower);
        }

        public void AddSpiritualPower()
        {
            if (IsCanNotAddSpiritual)
            {
                IsCanNotAddSpiritual = false;
                return;
            }
            PlayerSpiritualPower = Mathf.Min(PlayerSpiritualPower + 1, 1000000);
            UpdateSpiritualPower?.Invoke(PlayerSpiritualPower);
        }

        public void AddBossSpiritualPower(int value)
        {
            PlayerSpiritualPower = Mathf.Min(PlayerSpiritualPower + value, 1000000);
            UpdateSpiritualPower?.Invoke(PlayerSpiritualPower);
        }

        public void AddDamage()
        {
            if (IsCanNotSubSpiritual)
            {
                IsCanNotSubSpiritual = false;
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
                IsCanNotAddSpiritual = true;
                return;
            }
            foreach (var damage in AttackData)
            {
                damage.AttackDamage = Mathf.Max(damage.AttackDamage - 1, 10);
            }
        }
    }
}