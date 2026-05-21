using Script.Design_Pattern.StateMachine.PlayerClone.Base;
using UnityEngine;

namespace Script.Design_Pattern.StateMachine.PlayerClone.Main
{
    public class PlayerCloneEnterAttack : PlayerCloneBaseState
    {
        readonly int SwordEnterAnimationHash = Animator.StringToHash("Sword_Enter");
        readonly string SwordChangeTag = "SwordChange";
        private float previousTime;
        readonly string SwordIdleAnimationName = "Sword_Idle";
        public PlayerCloneEnterAttack(PlayerCloneStateMachine cloneStateMachine) : base(cloneStateMachine)
        {
        }

        public override void Enter()
        {
            cloneStateMachine.Animator.CrossFadeInFixedTime(SwordEnterAnimationHash, cloneStateMachine.AnimationCrossFade, 1);
        }

        public override void Tick(float deltaTime)
        {
            float normalizedTime = GetNormalizeTime(cloneStateMachine.Animator, SwordChangeTag, 1);

            if (normalizedTime > previousTime && normalizedTime <= 1f)
            {
                if (normalizedTime > 0.8f)
                {
                    cloneStateMachine.SwitchState(new PlayerCloneChasingState(cloneStateMachine));
                }
            }

            previousTime = normalizedTime;
        }

        public override void PhysicTick(float fixedDeltaTime)
        {
            
        }

        public override void Exit()
        {
            ChangeSwordIdle(SwordIdleAnimationName, cloneStateMachine.SwordIdleAnimationClip);
            cloneStateMachine.SwitchState(new PlayerCloneChasingState(cloneStateMachine));
        }
    }
}
