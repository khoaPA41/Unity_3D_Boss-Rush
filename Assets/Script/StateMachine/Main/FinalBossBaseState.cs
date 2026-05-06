using UnityEngine;
public abstract class FinalBossBaseState : State
{
    protected FinalBossStateMachine finalBossStateMachine;

    public FinalBossBaseState(FinalBossStateMachine finalBossStateMachine)
    {
        this.finalBossStateMachine = finalBossStateMachine;
    }



    protected void Move(Vector3 motion, float deltaTime)
    {
        finalBossStateMachine.CharacterController.Move((motion + finalBossStateMachine.ForceReceiver.Movement) * deltaTime);
    }

    protected void Move(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
    }

    protected void FaceTarget(Vector3 dir)
    {
        finalBossStateMachine.transform.rotation = Quaternion.LookRotation(dir);
    }

    protected Vector3 GetDirToPlayer()
    {
        Vector3 dir = (finalBossStateMachine.Player.transform.position - finalBossStateMachine.transform.position).normalized;
        dir.y = 0;
        return dir;
    }
}
