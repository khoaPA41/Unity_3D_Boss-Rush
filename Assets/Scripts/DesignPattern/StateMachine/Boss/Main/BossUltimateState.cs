using Attack;
using DesignPattern.StateMachine.Boss.Base;
using UnityEngine;

namespace DesignPattern.StateMachine.Boss.Main
{
    public class BossUltimateState : BossBaseState
    {
        private readonly AttackData _attackData;
        private float _previousTime;
        private bool _alreadyApplyForce;
        float _enterStateTime;

        public BossUltimateState(BossStateMachine bossStateMachine, int index) : base(bossStateMachine)
        {
            _attackData = bossStateMachine.UltimateCombo.AttackData[index];
        }

        public override void Enter()
        {
            _enterStateTime = Time.time;

            bossStateMachine.IsFinishedAttack = false;

            _alreadyApplyForce = false;
            foreach (var damage in bossStateMachine.DealsDamage)
            {
                damage.SetDamage(_attackData.AttackDamage);
            }


            bossStateMachine.Animator.CrossFadeInFixedTime(_attackData.AnimationName,
                _attackData.AnimationTransition);

            UseSkill(_attackData.SkillType);
        }

        public override void Tick(float deltaTime)
        {
            var normalizeTime = GetNormalizeTime(bossStateMachine.Animator, "Attack", 0);

            if (normalizeTime >= _previousTime && normalizeTime <= 1f)
            {
                if (normalizeTime >= _attackData.ForceTime)
                {
                    TryApplyForce();
                }
                if (TryCombo(normalizeTime)) return;
            }

            _previousTime = normalizeTime;
            Move(deltaTime);
            FaceTarget(GetDirToPlayer(bossStateMachine.Target), bossStateMachine.Target);
        }

        public override void Exit()
        {
            bossStateMachine.IsAttack = false;
        }

        private bool TryCombo(float normalizeTime)
        {
            if (normalizeTime < _attackData.AttackAnimationTime)
            {
                return false;
            }

            if (Time.time - _enterStateTime < 0.2f)
            {
                return false;
            }

            if (_attackData.NextAttackDataIndex == -1)
            {
                bossStateMachine.IsFinishedAttack = true;
                bossStateMachine.NextAttackIndex = -1;
                bossStateMachine.IsUltimateAttack = false;
                bossStateMachine.ReturnLocomotion();
                return true;
            }

            bossStateMachine.LastAttackTime = Time.time;
            bossStateMachine.NextAttackIndex = _attackData.NextAttackDataIndex;

            bossStateMachine.SwitchState(new BossUltimateState(
                bossStateMachine,
                _attackData.NextAttackDataIndex
            ));
            return true;
        }

        private void TryApplyForce()
        {
            if (_alreadyApplyForce)
            {
                return;
            }

            bossStateMachine.ForceReceiver.AddForce(bossStateMachine.transform.forward * _attackData.Force);
            _alreadyApplyForce = true;
        }
    }
}