using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] private Transform bossTargetPoint;
    
    [SerializeField] private float offsetDistance;

    private Camera mainCamera;
    
    private void Start()
    {
        mainCamera = Camera.main;
    }


    private void LateUpdate()
    {
        if (bossTargetPoint is null) return;
        var directionToCamera = (mainCamera.transform.position - bossTargetPoint.position).normalized;
            
        transform.position = bossTargetPoint.position + (directionToCamera * offsetDistance);
            
        transform.forward = mainCamera.transform.forward;
    }
}
