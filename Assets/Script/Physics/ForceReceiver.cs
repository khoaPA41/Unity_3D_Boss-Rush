using UnityEngine;

public class ForceReceiver : MonoBehaviour
{

    [Header("Physics")]
    [SerializeField] float verticalVelocity;
    CharacterController characterController;

    public Vector3 Movement => Vector3.up * verticalVelocity;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (verticalVelocity < 0f && characterController.isGrounded)
        {
            verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }
    }
}
