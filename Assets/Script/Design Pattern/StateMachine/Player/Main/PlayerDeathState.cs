using Script.Design_Pattern.StateMachine.Player.Base;
using UnityEngine;

public class PlayerDeathState : PlayerBaseState
{
    readonly int DeathAnimationOneHash = Animator.StringToHash("Death_1");
    readonly int DeathAnimationTwoHash = Animator.StringToHash("Death_2");
    readonly string DeathAnimationTag = "Death";

    int randomAnimation;
    public PlayerDeathState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {

    }

    public override void Enter()
    {
        randomAnimation = Random.Range(0, 2);
        playerStateMachine.Animator.CrossFadeInFixedTime(
            randomAnimation == 0 ? DeathAnimationOneHash : DeathAnimationTwoHash,
            playerStateMachine.AnimationCrossFade);
    }

    public override void Tick(float deltaTime)
    {
        var normalizeTime = GetNormalizeTime(playerStateMachine.Animator, DeathAnimationTag, 0);
        if (normalizeTime is > .9f and <= 1f)
        {
            GameManagers.Instance.ReturnCheckpoint();
        }
    }

    public override void PhysicTick(float fixedDeltaTime)
    {

    }

    public override void Exit()
    {
    }
}
