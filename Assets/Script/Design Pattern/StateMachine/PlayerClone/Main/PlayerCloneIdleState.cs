using Script.Design_Pattern.StateMachine.PlayerClone.Base;
using Script.Design_Pattern.StateMachine.PlayerClone.Main;
using UnityEngine;

public class PlayerCloneIdleState : PlayerCloneBaseState
{
    private readonly int _idleSwordAnimationHash = Animator.StringToHash("Sword_Idle");
    public PlayerCloneIdleState(PlayerCloneStateMachine cloneStateMachine) : base(cloneStateMachine)
    {
    }

    public override void Enter()
    {
        // IsFinished = false;
        cloneStateMachine.Animator.CrossFadeInFixedTime(_idleSwordAnimationHash, cloneStateMachine.AnimationCrossFade);
    }

    public override void Tick(float deltaTime)
    {
        cloneStateMachine.CountTime += deltaTime;

        if (!(cloneStateMachine.CountTime >= cloneStateMachine.ChangeChasingState)) return;
        
        if (IsAttackRange())
        {
            cloneStateMachine.SwitchState(new PlayerCloneAttackState(cloneStateMachine, 0));
        }
        cloneStateMachine.SwitchState(new PlayerCloneChasingState(cloneStateMachine));
    }

    public override void PhysicTick(float fixedDeltaTime)
    {
        
    }

    public override void Exit()
    {
        cloneStateMachine.CountTime = 0f;
    }
}
