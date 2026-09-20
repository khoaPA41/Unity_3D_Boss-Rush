using UnityEngine;

namespace Script.Physics
{
    public class ForceReceiver : MonoBehaviour
    {
        [Header("Physics")] [SerializeField] private float drag = .3f;
        private float VerticalVelocity { get; set; }
        private float CoefficientOfMovement { get; set; } = 1f;

        private CharacterController characterController;
        private Vector3 dampingVelocity;
        private Vector3 impact;
        public Vector3 Movement=> impact + Vector3.up * VerticalVelocity;
        
        private void Start()
        {
            characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            if (VerticalVelocity < 0f && characterController.isGrounded)
            {
                VerticalVelocity = UnityEngine.Physics.gravity.y * Time.deltaTime;
            }
            else
            {
                VerticalVelocity += UnityEngine.Physics.gravity.y * Time.deltaTime;
            }

            impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, drag);
        }

        public void AddForce(Vector3 force)
        {
            impact += force;
        }

        public void Jump(float jumpForce)
        {
            VerticalVelocity += jumpForce;
        }

        public void SetCoefficientOfMovement(float coefficientOfMovement)
        {
            CoefficientOfMovement = coefficientOfMovement;
        }

        public float GetCoefficientOfMovement() => CoefficientOfMovement;
        
    }
}