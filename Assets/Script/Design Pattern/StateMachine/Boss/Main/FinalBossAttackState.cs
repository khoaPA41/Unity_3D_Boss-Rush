using Script.Design_Pattern.StateMachine.Boss.Base;
using UnityEngine;
public class FinalBossAttackState : FinalBossBaseState
{
    AttackData attackData;
    float previousTime;
    bool alreadyApplyForce;
    Vector3 dir;
    public FinalBossAttackState(FinalBossStateMachine finalBossStateMachine, int index) : base(finalBossStateMachine)
    {
        attackData = finalBossStateMachine.AttackDatas[index];
    }

    public override void Enter()
    {
        finalBossStateMachine.Health.HitAction += finalBossStateMachine.EnterHitState;

        finalBossStateMachine.Animator.CrossFadeInFixedTime(attackData.AnimationName, attackData.AnimationTransition);
    }

    public override void Tick(float deltaTime)
    {
        dir = GetDirToPlayer();
        float normalizeTime = GetNormalizeTime(finalBossStateMachine.Animator, "Attack", 0);
        if (normalizeTime >= previousTime && normalizeTime < +1f)
        {
            if (normalizeTime >= attackData.ForceTime)
            {
                TryApplyForce();
            }

            TryCombo();
        }
        else
        {
            finalBossStateMachine.ReturnLocomotion();
        }

        previousTime = normalizeTime;
        Move(deltaTime);
        FaceTarget(dir);
    }

    public override void PhysicTick(float fixedDeltaTime)
    {

    }

    public override void Exit()
    {
        finalBossStateMachine.Health.HitAction -= finalBossStateMachine.EnterHitState;

    }

    void TryCombo()
    {
        if (attackData.NextAttackDataIndex == -1) { return; }
        if (previousTime < attackData.AttackAnimationTime) { return; }
        finalBossStateMachine.SwitchState(new FinalBossAttackState(
            finalBossStateMachine,
            attackData.NextAttackDataIndex
            ));
    }

    void TryApplyForce()
    {
        if (alreadyApplyForce) { return; }
        finalBossStateMachine.ForceReceiver.AddForce(finalBossStateMachine.transform.forward * attackData.Force);
        alreadyApplyForce = true;
    }
}
