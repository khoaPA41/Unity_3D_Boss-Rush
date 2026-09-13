using UnityEngine;


namespace Script.Design_Pattern.StateMachine
{
    public class BossLocomotionState : BossBaseState
    {
        private readonly int targetLookBlendTreeHash = Animator.StringToHash("TargetLookBlendTree");
        private readonly int movementParam = Animator.StringToHash("Movement");

        private Vector3 dir;
        private bool isWalk;
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
            if (!bossStateMachine.IsChasing)
            {
                animationValue = 0;
                speed = 0;
            }
            else
            {
                // animationValue = isWalk ? 1 : 2;
                // speed = isWalk ? bossStateMachine.SprintSpeed : bossStateMachine.MovementSpeed;
                // var dir = new Vector3(movementInput.x, 0, movementInput.y);

                animationValue = 2f;
                speed = bossStateMachine.SprintSpeed;
                var dir = GetDirToPlayer(bossStateMachine.Target);
                Move(dir * speed, deltaTime);
                FaceTarget(GetDirToPlayer(bossStateMachine.Target), bossStateMachine.Target);
            }

            // Change State
            EnterCounterAttackState();
            EnterAttackState();

            UpdateAnimation(animationValue, deltaTime);
        }

        public override void PhysicTick(float fixedDeltaTime)
        {
        }

        public override void Exit()
        {
            bossStateMachine.IsAttack = false;
        }

        private void UpdateAnimation(float value, float deltaTime)
        {
            bossStateMachine.Animator.SetFloat(movementParam, value, bossStateMachine.AnimationCrossFade, deltaTime);
        }

        private void EnterAttackState()
        {
            if (!bossStateMachine.IsAttack) return;
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
        }

        private void EnterCounterAttackState()
        {
            if (!bossStateMachine.IsCounterAttack) return;
            bossStateMachine.EnterCounterAttackState();
        }
    }
}