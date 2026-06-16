using UnityEngine;

public class TriggerChamber : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float yPosStop;
    private Rigidbody rb;
    private bool isMoving = false;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
 
    private void Update()
    {
        if (transform.position.y >= yPosStop)
        {
            return;
        }
        
        if (isMoving)
        {
            rb.MovePosition(rb.position + Vector3.up * (speed * Time.deltaTime));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isMoving) return;

        if (other.tag == "Player")
        {
            Debug.Log("Player is moving");
            isMoving = true;
        }
    }
}
