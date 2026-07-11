using System;
using System.Collections;
using System.Collections.Generic;
using Script.Attack;
using Script.Attack.Skill_Factory;
using Script.Design_Pattern.EventBus;
using Script.Design_Pattern.StateMachine.Boss.Main;
using Script.Design_Pattern.StateMachine.Player.Base;
using Script.Design_Pattern.Tree_Behavious.Dependency_Injection;
using Script.Physics;
using UnityEngine;
using UnityEngine.Playables;
using State = Script.Design_Pattern.StateMachine.Base.State;

namespace Script.Design_Pattern.StateMachine.Boss.Base
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

    public class FinalBossStateMachine : StateMachine.Base.StateMachine, ICaster, ICombatInput
    {
        [field: Header("Physics")]
        [field: SerializeField]
        public CharacterController CharacterController { get; private set; }
        [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
        [field: SerializeField] public float MovementSpeed { get; private set; } = 5f;
        [field: SerializeField] public float SprintSpeed { get; private set; } = 5f;
        [field: SerializeField] public float DashSpeed { get; private set; } = 30f;

        [field: Header("Attack")]
        [field: SerializeField]
        public GameObject WeaponRight { get; private set; }
        [field: SerializeField] public GameObject WeaponLeft { get; private set; }
        [field: SerializeField] public GameObject Weapon { get; private set; }
        [field: SerializeField] public PlayerSFX PlayerSFX { get; private set; }

        public Material WeaponRightMaterial { get; set; }
        public Material WeaponLeftMaterial { get; set; }
        public Material WeaponMaterial { get; set; }
        public Color WeaponRightEmissionColor { get; set; }
        public Color WeaponLeftEmissionColor { get; set; }
        public Color WeaponEmissionColor { get; set; }
        public bool isRightWeaponVFX { get; set; }
        public bool isBothWeaponVFX { get; set; }

        [field: SerializeField] public AnimationCurve AnimationWeaponEmissionCurve { get; private set; }
        [field: SerializeField] public UltimateCombo[] UltimateCombo { get; private set; }
        [field: SerializeField] public NormalCombo[] NormalCombo { get; private set; }
        [field: SerializeField] public WeaponTrail[] DealsDamage { get; private set; }
        [field: SerializeField] public WeaponDealDamage[] AllDamageDealer { get; private set; }
        [field: SerializeField] public Health Health { get; private set; }
        [field: SerializeField] public float AttackRange { get; private set; } = 5f;
        [field: SerializeField] public float AttackFurtherRange { get; private set; } = 30f;
        [field: SerializeField] public float WalkRange { get; private set; } = 8f;
        [field: SerializeField] public float TimeOutCombo { get; private set; } = 2f;
        [field: SerializeField] public int TimesToHit { get; private set; } = 3;
        private int TimesHit { get; set; } = 0;
        public int NextAttackIndex { get; set; }
        public int CurrentComboIndex { get; set; }
        public float LastAttackTime { get; set; }

        [field: Header("Animation")]
        [field: SerializeField]
        public Animator Animator { get; private set; }
        [field: SerializeField] public ManageAnimationSkillEvent ManageAnimationSkillEvent { get; private set; }
        [field: SerializeField] public float AnimationCrossFade { get; private set; } = .1f;
        [field: SerializeField] public GameObject Neck { get; private set; }
        
        [field: Header("State")]
        [field: SerializeField]
        public float ChaseDuration { get; private set; } = 4f;
        [field: SerializeField] public float AttackDuration { get; private set; } = 8f;
        [field: SerializeField] public float IdleDuration { get; private set; } = 2f;
        public float NextPhaseToggleTime { get; set; }
        public bool IsChasingState { get; set; }
        public bool IsChangePhase { get; set; }
        public bool IsFinishedAttack { get; set; }
        public int CurrentPhase { get; set; } = 0;
        public int NextPhase { get; set; } = 0;

        public bool IsActiveUltimate { get; set; }

        [field: Header("Event")] public Health Player { get; private set; }
        public PlayerStateMachine PlayerStateMachine { get; private set; }
        public bool IsWalking { get; set; }
        private State _locomotionState;
        public bool IsStillUltimate { get; set; } = false;
        public bool IsCanMove { get; set; } = false;

        public Transform Target { get; set; }
        
        private void Awake()
        {
            _locomotionState = new FinalBossLocomotionState(this);
        }

        private void Start()
        {
            Player = GameObject.FindWithTag("Player").GetComponent<Health>();
            Target = Player.gameObject.transform;
            PlayerStateMachine = Player.GetComponent<PlayerStateMachine>();
            
            WeaponRightMaterial = WeaponRight.GetComponent<MeshRenderer>().material;
            WeaponRightEmissionColor = WeaponRightMaterial.GetColor("_EmissionColor");
            
            WeaponLeftMaterial = WeaponLeft.GetComponent<MeshRenderer>().material;
            WeaponLeftEmissionColor = WeaponLeftMaterial.GetColor("_EmissionColor");
            
            WeaponMaterial = Weapon.GetComponent<MeshRenderer>().material;
            WeaponEmissionColor = WeaponMaterial.GetColor("_EmissionColor");
        }

        private void OnEnable()
        {
            // Player.DeathAction += FinishedCombat;
            Health.HitAction += EnterHitState;
            Health.DeathAction += EnterDeathState;
            GameEventManagers.Instance.OnSkillCasted += HandleSkillEvent;
        }

        private void OnDisable()
        {
            // Player.DeathAction -= FinishedCombat;
            Health.HitAction -= EnterHitState;
            Health.DeathAction -= EnterDeathState;
            GameEventManagers.Instance.OnSkillCasted -= HandleSkillEvent;
        }

        // public void FinishedCombat()
        // {
        //      ReturnLocomotion();
        // }

        public void SendEvent()
        {
            ManageAnimationSkillEvent.SendSituationEvent();
        }

        public void SendActionEvent()
        {
            ManageAnimationSkillEvent.SendNextActionEvent();
        }

        public void SendReleasePoolObjectEvent()
        {
            ManageAnimationSkillEvent.SendReleasePoolObjectEvent();
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
            if (caster.GetTransform().gameObject.TryGetComponent(out FinalBossStateMachine _)) return;
            ActiveEnventBySkill(skillEffect)?.Invoke();
        }

        public void ComsumeMana(int amount)
        {
            throw new System.NotImplementedException();
        }

        public Transform GetTransform()
        {
            return transform;
        }

        public GameObject TargetCaster()
        {
            return Target.gameObject;
        }

        private Action ActiveEnventBySkill(SkillEffect skillEffect)
        {
            return skillEffect switch
            {
                SkillEffect.NonEffect => () => { Debug.Log("NonEffect"); },
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

        public Vector3 GetDirToPlayer(Transform target)
        {
            if (target.TryGetComponent(out Health _))
            {
                if (PlayerStateMachine.Invisible)
                {
                    return Vector3.zero;
                }
            }

            var dir = (target.position - transform.position)
                .normalized;
            dir.y = 0;
            return dir;
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

        public void Move(Vector3 motion, float deltaTime)
        {
            if (PlayerStateMachine.Invisible)
            {
                return;
            }

            CharacterController.Move(
                (motion + ForceReceiver.Movement) *
                (ForceReceiver.GetCoefficientOfMovement() * deltaTime));
        }

        public void FaceTarget(Vector3 dir, Transform target)
        {
            if (target.TryGetComponent(out Health _))
            {
                if (PlayerStateMachine.Invisible)
                {
                    return;
                }
            }

            transform.rotation = Quaternion.LookRotation(dir);
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