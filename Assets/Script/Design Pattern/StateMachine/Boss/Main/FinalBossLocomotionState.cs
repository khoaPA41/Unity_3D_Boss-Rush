using Script.Design_Pattern.StateMachine.Boss.Base;
using UnityEngine;

namespace Script.Design_Pattern.StateMachine.Boss.Main
{
    public class FinalBossLocomotionState : FinalBossBaseState
    {
        private readonly int targetLookBlendTreeHash = Animator.StringToHash("TargetLookBlendTree");
        private readonly int movementParam = Animator.StringToHash("Movement");
        
        private Vector3 dir;
        private bool isWalk;
        private float animationValue;
        private float speed;
        
        public FinalBossLocomotionState(FinalBossStateMachine finalBossStateMachine) : base(finalBossStateMachine)
        {
            
        }

        public override void Enter()
        {
            FinalBossStateMachine.Animator.CrossFadeInFixedTime(targetLookBlendTreeHash, FinalBossStateMachine.AnimationCrossFade);
        }

        public override void Tick(float deltaTime)
        {
            if (FinalBossStateMachine.IsActiveUltimate && !FinalBossStateMachine.IsStillUltimate)
            {
                var randomComboIndex = Random.Range(0, FinalBossStateMachine.NormalCombo.Length);
                FinalBossStateMachine.SwitchState(new FinalBossEnterPhaseState(FinalBossStateMachine, FinalBossStateMachine.NextPhase, 0));
            }
            
            var movementInput = FinalBossStateMachine.InputMovement;
            isWalk = FinalBossStateMachine.IsWalking;
            
            if (movementInput == Vector2.zero)
            {
                animationValue = 0;
                speed = 0;
            }
            else
            {
                animationValue = isWalk ? 1 : 2;
                speed = isWalk ? FinalBossStateMachine.SprintSpeed : FinalBossStateMachine.MovementSpeed;
                var dir = new Vector3(movementInput.x, 0, movementInput.y);
                Move(dir * speed, deltaTime);
            }


            EnterAttackState();
            
            UpdateAnimation(animationValue, deltaTime);
            FaceTarget(FinalBossStateMachine.GetDirToPlayer(FinalBossStateMachine.Target), FinalBossStateMachine.Target);
        }

        public override void PhysicTick(float fixedDeltaTime)
        {

        }

        public override void Exit()
        {
            FinalBossStateMachine.InputMovement = Vector2.zero;
        }

        private void UpdateAnimation(float value, float deltaTime)
        {
            FinalBossStateMachine.Animator.SetFloat(movementParam, value, FinalBossStateMachine.AnimationCrossFade, deltaTime);
        }

        private void EnterAttackState()
        {
            if (!FinalBossStateMachine.IsAttack) return;
            var randomCombo = Random.Range(0, FinalBossStateMachine.NormalCombo[FinalBossStateMachine.CurrentPhase].Combo.Length);
            if (FinalBossStateMachine.NextAttackIndex == -1)
            {
                FinalBossStateMachine.CurrentComboIndex = randomCombo;
                FinalBossStateMachine.NextAttackIndex = 0;
                Debug.Log("Random Combo: " + randomCombo);
            }
            else
            {
                randomCombo = FinalBossStateMachine.CurrentComboIndex;
                if (FinalBossStateMachine.CurrentComboIndex >= FinalBossStateMachine.NormalCombo[FinalBossStateMachine.CurrentPhase].Combo.Length)
                {
                    randomCombo = 0;
                }
                Debug.Log("Random Combo: " + randomCombo);
            }

            Debug.Log("Current Phase: " + FinalBossStateMachine.CurrentPhase);
            
            Debug.Log("Next Attack Index: " + FinalBossStateMachine.NextAttackIndex);

            FinalBossStateMachine.SwitchState(new FinalBossAttackState(FinalBossStateMachine, FinalBossStateMachine.CurrentPhase, randomCombo, FinalBossStateMachine.NextAttackIndex));
        }
    }
}
