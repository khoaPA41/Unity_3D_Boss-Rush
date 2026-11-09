using Attack;
using DesignPattern.StateMachine.Boss.Base;
using UnityEngine;

namespace DesignPattern.StateMachine.Boss.Main
{
    public class BossAttackState : BossBaseState
    {
        private readonly AttackData _attackData;
        private float _previousTime;
        private bool _alreadyApplyForce;
        private int _normalComboIndex;
        private int _indexCombo;
        float _enterStateTime;

        public BossAttackState(BossStateMachine bossStateMachine, int normalComboIndex, int indexCombo, int index) : base(bossStateMachine)
        {
            _attackData = bossStateMachine.NormalCombo[normalComboIndex].Combo[indexCombo].AttackData[index];
            _normalComboIndex = normalComboIndex;
            _indexCombo = indexCombo;
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
                TryCombo(normalizeTime);
            }

            _previousTime = normalizeTime;
            Move(deltaTime);
            FaceTarget(GetDirToPlayer(bossStateMachine.Target), bossStateMachine.Target);
        }

        public override void Exit()
        {
            bossStateMachine.IsAttack = false;
        }

        private void TryCombo(float normalizeTime)
        {
            if (normalizeTime < _attackData.AttackAnimationTime)
            {
                return;
            }

            if (Time.time - _enterStateTime < 0.2f)
            {
                return;
            }

            if (_attackData.NextAttackDataIndex == -1)
            {
                bossStateMachine.IsFinishedAttack = true;
                bossStateMachine.NextAttackIndex = -1;
                bossStateMachine.ReturnLocomotion();
                return;
            }

            bossStateMachine.LastAttackTime = Time.time;
            bossStateMachine.NextAttackIndex = _attackData.NextAttackDataIndex;

            bossStateMachine.SwitchState(new BossAttackState(
                bossStateMachine,
                _normalComboIndex,
                _indexCombo,
                _attackData.NextAttackDataIndex
            ));
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

