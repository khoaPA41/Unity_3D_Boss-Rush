public class PlayerAttackState : PlayerBaseState
{
    AttackData attackData;
    float previousTime;
    bool alreadyApplyForce;

    public PlayerAttackState(PlayerStateMachine playerStateMachine, int attackDataIndex) : base(playerStateMachine)
    {
        attackData = playerStateMachine.AttackData[attackDataIndex];
    }

    public override void Enter()
    {
        playerStateMachine.Animator.CrossFadeInFixedTime(attackData.AnimationName, attackData.AnimationTransition, 0);
        playerStateMachine.WeaponDealDamage.SetDamage(attackData.AttackDamage);
    }

    public override void Tick(float deltaTime)
    {
        float normalizeTime = GetNormalizeTime(playerStateMachine.Animator, attackData.AnimationTag, 0);
        if (normalizeTime >= previousTime && normalizeTime <= 1f)
        {
            if (normalizeTime >= attackData.ForceTime)
            {
                TryApplyForce();
            }

            if (playerStateMachine.InputReader.IsAttack)
            {
                TryCombo(normalizeTime);
            }

        }
        else
        {
            playerStateMachine.ReturnLocomotion();
        }

        previousTime = normalizeTime;
        FaceTarget(deltaTime);
        Move(deltaTime);

    }

    public override void PhysicTick(float fixedDeltaTime)
    {

    }

    public override void Exit()
    {
        //playerStateMachine.Animator.CrossFadeInFixedTime("Sword_Regular_A_Rec", playerStateMachine.AnimationCrossFade);
    }

    void TryCombo(float normalizeTime)
    {
        if (attackData.NextAttackDataIndex == -1) { return; }
        if (normalizeTime < attackData.AttackAnimationTime) { return; }

        playerStateMachine.SwitchState(new PlayerAttackState(
            playerStateMachine,
            attackData.NextAttackDataIndex
            ));
    }

    void TryApplyForce()
    {
        if (alreadyApplyForce) { return; }
        playerStateMachine.ForceReceiver.AddForce(playerStateMachine.transform.forward * attackData.Force);
        alreadyApplyForce = true;
    }
}
