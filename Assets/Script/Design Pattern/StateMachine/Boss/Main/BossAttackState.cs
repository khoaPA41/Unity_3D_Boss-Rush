using UnityEngine;

namespace Script.Design_Pattern.StateMachine
{
    public class BossAttackState : BossBaseState
    {
        private readonly AttackData _attackData;
        private float _previousTime;
        private bool _alreadyApplyForce;
        private int normalComboIndex;
        private int indexCombo;
        float enterStateTime;

        public BossAttackState(BossStateMachine bossStateMachine, int normalComboIndex, int indexCombo, int index) : base(bossStateMachine)
        {
            _attackData = bossStateMachine.NormalCombo[normalComboIndex].Combo[indexCombo].AttackData[index];
            this.normalComboIndex = normalComboIndex;
            this.indexCombo = indexCombo;
        }

        public override void Enter()
        {
            enterStateTime = Time.time;

            bossStateMachine.IsFinishedAttack = false;
            // _previousTime = 0f;
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
            // if (bossStateMachine.IsCanMove)
            // {
            //     var dir = GetDirToPlayer(bossStateMachine.Target);
            //     Move(dir * bossStateMachine.DashSpeed, deltaTime);
            // }
            // else
            // {
            //     Move(deltaTime);
            // }


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

        public override void PhysicTick(float fixedDeltaTime)
        {
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

            if (Time.time - enterStateTime < 0.2f)
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
                normalComboIndex,
                indexCombo,
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

