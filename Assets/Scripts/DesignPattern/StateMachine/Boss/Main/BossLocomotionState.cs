using DesignPattern.StateMachine.Boss.Base;
using UnityEngine;


namespace DesignPattern.StateMachine.Boss.Main
{
    public class BossLocomotionState : BossBaseState
    {
        private readonly int targetLookBlendTreeHash = Animator.StringToHash("TargetLookBlendTree");
        private readonly int movementParam = Animator.StringToHash("Movement");



        private float animationValue;
        private float speed;
        public BossLocomotionState(BossStateMachine bossStateMachine) : base(bossStateMachine)
        {
        }

        public override void Enter()
        {
            animationValue = 0f;
            speed = 0f;
            bossStateMachine.Animator.CrossFadeInFixedTime(targetLookBlendTreeHash, bossStateMachine.AnimationCrossFade);

        }

        public override void Tick(float deltaTime)
        {
            if (bossStateMachine.IsUltimateAttack)
            {
                bossStateMachine.SwitchState(new BossUltimateState(bossStateMachine, 0));
                return;
            }

            if (!bossStateMachine.IsChasing)
            {
                animationValue = 0;
                speed = 0;
            }
            else
            {
                animationValue = 2f;
                speed = bossStateMachine.SprintSpeed;
                var dir = GetDirToPlayer(bossStateMachine.Target);
                Move(dir * speed, deltaTime);
                FaceTarget(GetDirToPlayer(bossStateMachine.Target), bossStateMachine.Target);
            }

            // Change State
            if (EnterCounterAttackState()) return;
            if (EnterAttackState()) return;

            UpdateAnimation(animationValue, deltaTime);
        }

        public override void Exit()
        {
            bossStateMachine.IsAttack = false;
        }

        private void UpdateAnimation(float value, float deltaTime)
        {
            bossStateMachine.Animator.SetFloat(movementParam, value, bossStateMachine.AnimationCrossFade, deltaTime);
        }

        private bool EnterAttackState()
        {
            if (!bossStateMachine.IsAttack) return false;
            var randomCombo = Random.Range(0, bossStateMachine.NormalCombo[bossStateMachine.BossBehaviorBrain.CurrentPhase].Combo.Length);
            if (bossStateMachine.NextAttackIndex == -1)
            {
                bossStateMachine.CurrentComboIndex = randomCombo;
                bossStateMachine.NextAttackIndex = 0;
            }
            else
            {
                randomCombo = bossStateMachine.CurrentComboIndex;
                if (bossStateMachine.CurrentComboIndex >= bossStateMachine.NormalCombo[bossStateMachine.BossBehaviorBrain.CurrentPhase].Combo.Length)
                {
                    randomCombo = 0;
                }
            }

            bossStateMachine.SwitchState(new BossAttackState(bossStateMachine, bossStateMachine.BossBehaviorBrain.CurrentPhase, randomCombo, bossStateMachine.NextAttackIndex));
            return true;
        }

        private bool EnterCounterAttackState()
        {
            if (!bossStateMachine.IsCounterAttack) return false;

            bossStateMachine.EnterCounterAttackState();
            return true;
        }
    }
}