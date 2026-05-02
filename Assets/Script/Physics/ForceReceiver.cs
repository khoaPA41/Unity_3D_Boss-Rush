using UnityEngine;

public class ForceReceiver : MonoBehaviour
{
    [Header("Physics")]
    [SerializeField] float verticalVelocity;
    [SerializeField] float drag = .3f;

    CharacterController characterController;
    Vector3 dampingVelocity;
    Vector3 impact;
    public Vector3 Movement => impact + Vector3.up * verticalVelocity;

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


        impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, drag);
    }

    public void AddForce(Vector3 force)
    {
        impact += force;
    }
}
