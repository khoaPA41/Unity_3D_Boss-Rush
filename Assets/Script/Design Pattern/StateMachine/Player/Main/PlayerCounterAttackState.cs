using Script.Design_Pattern.StateMachine.Player.Base;
using UnityEngine;

public class PlayerCounterAttackState : PlayerBaseState
{
    private readonly int _counterattackAnimation = Animator.StringToHash("Attack_3");
    private readonly string attackAnimationTag = "Attack";
    public PlayerCounterAttackState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        playerStateMachine.Animator.speed = 2f;
        playerStateMachine.Animator.CrossFadeInFixedTime(_counterattackAnimation, playerStateMachine.AnimationCrossFade);
        playerStateMachine.DealDamage.SetDamage(playerStateMachine.AttackData[0].AttackDamage * 3);
    }

    public override void Tick(float deltaTime)
    {
        var normalizedTime = GetNormalizeTime(playerStateMachine.Animator, attackAnimationTag, 0);
        if (normalizedTime is > .8f and <= 1f)
        {
            playerStateMachine.ReturnLocomotion();
        }
        FaceTarget(deltaTime);
        Move(deltaTime);
    }

    public override void PhysicTick(float fixedDeltaTime)
    {
    }

    public override void Exit()
    {
        playerStateMachine.Animator.speed = 1f;
        playerStateMachine.IsCounterAttack = false;
    }
}
