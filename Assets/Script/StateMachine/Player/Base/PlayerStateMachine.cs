using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    [Header("Input")]
    [field: SerializeField] public InputReader InputReader { get; private set; }

    [Header("Physics")]
    [field: SerializeField] public CharacterController CharacterController { get; private set; }
    [field: SerializeField] public float FreeLookMovementSpeed { get; private set; } = 5f;
    [field: SerializeField] public float FreeLookMovementSprintSpeed { get; private set; } = 5f;
    [field: SerializeField] public float RotationDamping { get; private set; } = .5f;

    [field: SerializeField] public Targeter Targeter { get; private set; }


    [Header("Animation")]
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public float AnimationCrossFade { get; private set; } = .1f;

    public Transform MainCameraTransform { get; private set; }

    void Start()
    {
        MainCameraTransform = Camera.main.transform;
        SwitchState(new FreeLookState(this));
    }
}
