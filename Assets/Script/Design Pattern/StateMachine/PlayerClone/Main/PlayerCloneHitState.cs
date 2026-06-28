using Script.Design_Pattern.StateMachine.PlayerClone.Base;
using UnityEngine;

public class PlayerCloneHitState : PlayerCloneBaseState
{
    private readonly int _hitAnimationHash =  Animator.StringToHash("Hit");
    private const string HitAnimationTag = "Hit";

    private float _previousTime;
    
    public PlayerCloneHitState(PlayerCloneStateMachine cloneStateMachine) : base(cloneStateMachine)
    {
    }

    public override void Enter()
    {
        // IsFinished = false;
        cloneStateMachine.Animator.CrossFadeInFixedTime(_hitAnimationHash, cloneStateMachine.AnimationCrossFade);
    }

    public override void Tick(float deltaTime)
    {
        var normalizedTime = GetNormalizeTime(cloneStateMachine.Animator, HitAnimationTag, 0);
        if (normalizedTime > _previousTime && normalizedTime > .8f)
        {
            // IsFinished = true;
        }
        
        _previousTime = normalizedTime;
    }

    public override void PhysicTick(float fixedDeltaTime)
    {
    }

    public override void Exit()
    {
    }
}
