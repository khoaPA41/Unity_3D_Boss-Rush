using UnityEngine;
public abstract class PlayerBaseState : State
{
    protected PlayerStateMachine playerStateMachine;

    public PlayerBaseState(PlayerStateMachine playerStateMachine)
    {
        this.playerStateMachine = playerStateMachine;
    }


    protected void FaceDir(Vector3 movement, float deltaTime)
    {
        playerStateMachine.transform.rotation = Quaternion.Lerp(playerStateMachine.transform.rotation, Quaternion.LookRotation(movement), playerStateMachine.RotationDamping * deltaTime);
    }

    protected Vector3 CalculateMoment()
    {
        Vector3 forward = playerStateMachine.MainCameraTransform.forward;
        Vector3 right = playerStateMachine.MainCameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        return forward * playerStateMachine.InputReader.InputMovement.y + right * playerStateMachine.InputReader.InputMovement.x;

    }
}
