using Script.Design_Pattern.StateMachine.PlayerClone.Base;
using UnityEngine;

public class PlayerCloneIdleState : PlayerCloneBaseState
{
    private readonly int idleSwordAnimationHash = Animator.StringToHash("Sword_Idle");
    private readonly string idleSwordAnimationTag = "Sword_Idle";
    public PlayerCloneIdleState(PlayerCloneStateMachine cloneStateMachine) : base(cloneStateMachine)
    {
    }

    public override void Enter()
    {
        IsFinished = false;
        cloneStateMachine.Animator.CrossFadeInFixedTime(idleSwordAnimationHash, cloneStateMachine.AnimationCrossFade);
    }

    public override void Tick(float deltaTime)
    {
        cloneStateMachine.CountTime += deltaTime;
        
        if (cloneStateMachine.CountTime >= cloneStateMachine.ChangeChasingState)
        {
            cloneStateMachine.IsChasing = true;
        }

        IsAttackRange();
    }

    public override void PhysicTick(float fixedDeltaTime)
    {
        
    }

    public override void Exit()
    {
        cloneStateMachine.CountTime = 0f;
    }
}
