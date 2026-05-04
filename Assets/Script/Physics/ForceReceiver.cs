using UnityEngine;

public class ForceReceiver : MonoBehaviour
{
    [Header("Physics")]
    [SerializeField] float drag = .3f;
    public float verticalVelocity { get; set; }

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
        Debug.Log(verticalVelocity);

        impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, drag);
    }

    public void AddForce(Vector3 force)
    {
        impact += force;
    }

    public void Jump(float jumpForce)
    {
        verticalVelocity += jumpForce;
    }
}
