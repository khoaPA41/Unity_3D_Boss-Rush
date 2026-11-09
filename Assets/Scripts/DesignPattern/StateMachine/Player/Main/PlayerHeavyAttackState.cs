using Attack;
using DesignPattern.StateMachine.Player.Base;
using DesignPattern.ObjectPooling;
using UnityEngine;

namespace DesignPattern.StateMachine.Player.Main
{
    public class PlayerHeavyAttackState : PlayerBaseState
    {
        private readonly string _slashVfxName = "Basic_Sword_Slash_I";
        private readonly int _heavyAttackAnimationHash = Animator.StringToHash("HeavyAttack");
        private const string HeavyAttackAnimationTag = "HeavyAttack";

        private const float HoldTimeLimit = 1f;
        private float _holdTime;
        private float _holdDamage;

        private float _previousTime;
        private Vector3 _swordEndPos;
        public PlayerHeavyAttackState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
        }

        public override void Enter()
        {
            _holdTime = 0f;
            if (playerStateMachine.CheckLowStamina())
            {
                playerStateMachine.ReturnLocomotion();
                return;
            }

            playerStateMachine.Animator.CrossFadeInFixedTime(_heavyAttackAnimationHash, playerStateMachine.AnimationCrossFade, 0);
            playerStateMachine.ActiveSlashVfxAction += ActiveVfx;
            playerStateMachine.WeaponTrailVisual.SetActive(true);
        }

        public override void Tick(float deltaTime)
        {
            CountHoldTime(deltaTime); // Calculate hold time

            var normalizedTime = GetNormalizeTime(playerStateMachine.Animator, HeavyAttackAnimationTag, 0);
            if (normalizedTime >= _previousTime && normalizedTime > .8f)
            {
                playerStateMachine.ReturnLocomotion();
                return;
            }

            _previousTime = normalizedTime;
        }

        public override void Exit()
        {
            playerStateMachine.InputReader.IsHeavyAttack = false;
            playerStateMachine.Animator.speed = 1f;
            playerStateMachine.ActiveSlashVfxAction -= ActiveVfx;
            playerStateMachine.WeaponTrailVisual.SetActive(false);
        }

        private void CountHoldTime(float deltaTime)
        {
            if (playerStateMachine.InputReader.IsCharging)
            {
                playerStateMachine.Stamina.ChangeStamina(playerStateMachine.Stamina.heavyAttackReduce);
                playerStateMachine.Animator.speed = .3f;
                _holdTime += deltaTime;
                _holdTime = Mathf.Clamp(_holdTime, 0f, HoldTimeLimit);
            }

            if (_holdTime >= HoldTimeLimit || !playerStateMachine.InputReader.IsCharging)
            {
                playerStateMachine.InputReader.IsCharging = false;
                playerStateMachine.Animator.speed = 1f;
                CalculateDamage();
            }
        }

        private void CalculateDamage()
        {
            _holdDamage = playerStateMachine.AttackData[0].AttackDamage * _holdTime * 3f;
            playerStateMachine.WeaponTrail.SetDamage(_holdDamage);
        }

        private void ActiveVfx()
        {
            _swordEndPos = playerStateMachine.WeaponTip.position;
            var slashDir = _swordEndPos - playerStateMachine.StartSwordPos;

            var vfx = ObjectPoolManager.Instance.GetPooledObject(_slashVfxName, playerStateMachine.WeaponTransform.position);
            if (slashDir.sqrMagnitude > 0.001f)
            {
                var rot = Quaternion.FromToRotation(Vector3.right, slashDir.normalized);
                vfx.transform.rotation = rot;
            }
        }
    }
}
