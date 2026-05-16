using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
public class Targeter : MonoBehaviour
{
    [SerializeField] CinemachineTargetGroup cinemachineTargetGroup;

    List<Target> targetList = new List<Target>();

    Camera mainCamera;

    public Target currentTarget { get; set; }

    Vector3 targetPos;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if(currentTarget != null)
        {
            targetPos = currentTarget.transform.position;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Target>(out Target target)) { return; }
        targetList.Add(target);
        target.CancelTargetEvent += RemoveTarget;
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent<Target>(out Target target)) { return; }
        targetList.Remove(target);
    }

    public bool SelectedTarget()
    {
        if (targetList.Count == 0) { return false; }

        Target closestTarget = null;
        float closestTargetDistance = Mathf.Infinity;

        foreach (var target in targetList)
        {
            Vector2 viewPoint = mainCamera.WorldToViewportPoint(target.transform.position);

            if (viewPoint.x < 0 || viewPoint.x > 1 || viewPoint.y < 0 || viewPoint.y > 1)
            {
                continue;
            }
            Vector2 toCenter = viewPoint - new Vector2(0.5f, 0.5f);

            if (toCenter.sqrMagnitude < closestTargetDistance)
            {
                closestTarget = target;
                closestTargetDistance = toCenter.sqrMagnitude;
            }
        }

        if (closestTarget == null) { return false; }

        currentTarget = closestTarget;
        targetPos = currentTarget.transform.position;
        cinemachineTargetGroup.AddMember(currentTarget.transform, 2f, 1f);
        return true;
    }

    public void CancelTarget()
    {
        if (targetList.Count == 0) { return; }
        cinemachineTargetGroup.RemoveMember(currentTarget != null ? currentTarget.transform : null);
        currentTarget = null;
    }

    void RemoveTarget(Target target)
    {
        if (currentTarget == target)
        {
            CancelTarget();
        }

        target.CancelTargetEvent -= RemoveTarget;
        targetList.Remove(target);
    }

    public Vector3 GetTargetPosition()
    {
        return targetPos;
    }
}
