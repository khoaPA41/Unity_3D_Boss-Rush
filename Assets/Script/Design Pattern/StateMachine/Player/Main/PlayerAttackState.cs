using Script.Design_Pattern.Object_Pooling;
using Script.Design_Pattern.StateMachine.Player.Base;
using UnityEngine;
namespace Script.Design_Pattern.StateMachine.Player.Main
{
    public class PlayerAttackState : PlayerBaseState
    {
        private readonly AttackData _attackData;
        private float _previousTime;
        private bool _alreadyApplyForce;
        private bool _alreadyActiveVfx;

        public PlayerAttackState(PlayerStateMachine playerStateMachine, int attackDataIndex) : base(playerStateMachine)
        {
            _attackData = playerStateMachine.AttackData[attackDataIndex];
        }

        public override void Enter()
        {
            if (playerStateMachine.CheckLowStamina())
            {
                playerStateMachine.ReturnLocomotion();
                return;
            }
            playerStateMachine.Stamina.ChangeStamina(playerStateMachine.Stamina.lightAttackReduce);
            playerStateMachine.Animator.CrossFadeInFixedTime(_attackData.AnimationName, _attackData.AnimationTransition,
                0);
            playerStateMachine.DealDamage.SetDamage(playerStateMachine.IsIncreaseDamePotion ? _attackData.AttackDamage * 1.5f : _attackData.AttackDamage);
            playerStateMachine.ActiveSlashVfxAction += ActiveVfx;
        }

        public override void Tick(float deltaTime)
        {

            var normalizeTime = GetNormalizeTime(playerStateMachine.Animator, _attackData.AnimationTag, 0);
            if (normalizeTime >= _previousTime && normalizeTime <= 1f)
            {
                // if (!_alreadyActiveVfx)
                // {
                //     if (normalizeTime > 0.3f)
                //     {
                //         _alreadyActiveVfx = true;

                //     }
                // }

                if (normalizeTime >= _attackData.AttackAnimationTime)
                {

                    if (playerStateMachine.InputBuffering.TryConsume(ActionType.Dodge))
                    {
                        playerStateMachine.SwitchState(new PlayerDodgeState(playerStateMachine));
                        return;
                    }

                    if (playerStateMachine.InputBuffering.TryConsume(ActionType.Attack))
                    {
                        TryCombo();
                    }

                    if (playerStateMachine.InputBuffering.TryConsume(ActionType.Jump))
                    {
                        playerStateMachine.SwitchState(new PlayerStartJumpState(playerStateMachine));
                        return;
                    }
                }

                if (normalizeTime >= _attackData.ForceTime)
                {
                    TryApplyForce();
                }
            }
            else
            {
                playerStateMachine.ReturnLocomotion();
            }

            _previousTime = normalizeTime;
            FaceTarget(deltaTime);
            Move(deltaTime);
        }

        public override void PhysicTick(float fixedDeltaTime)
        {
        }

        public override void Exit()
        {
            _alreadyActiveVfx = false;
            playerStateMachine.ActiveSlashVfxAction -= ActiveVfx;

        }

        private void TryCombo()
        {
            if (_attackData.NextAttackDataIndex == -1)
            {
                return;
            }

            playerStateMachine.SwitchState(new PlayerAttackState(
                playerStateMachine,
                _attackData.NextAttackDataIndex
            ));
        }

        private void TryApplyForce()
        {
            if (_alreadyApplyForce)
            {
                return;
            }

            playerStateMachine.ForceReceiver.AddForce(playerStateMachine.transform.forward * _attackData.Force);
            _alreadyApplyForce = true;
        }

        private void ActiveVfx()
        {
            var vfx = ObjectPooling.Instance.GetPooledObject(_attackData.VfxName, playerStateMachine.WeaponTranform.position);
            vfx.transform.SetParent(playerStateMachine.WeaponTranform);
            // var dir = CalculateMovementInFreeLook();
            // var rotation = new Vector3(playerStateMachine.DealDamage.transform.rotation.x, playerStateMachine.DealDamage.transform.rotation.y, playerStateMachine.DealDamage.transform.rotation.z);
            // vfx.transform.rotation = playerStateMachine.DealDamage.transform.rotation * Quaternion.Euler(0f, 180f, 0f); ;
            // vfx.transform.Rotate(rotation);
        }
    }
}