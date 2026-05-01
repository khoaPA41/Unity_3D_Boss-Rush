using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
public class Targeter : MonoBehaviour
{
    List<Target> targetList = new List<Target>();

    [SerializeField] CinemachineTargetGroup cinemachineTargetGroup;
    public Target currentTarget { get; set; }



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
        currentTarget = targetList[0];
        cinemachineTargetGroup.AddMember(currentTarget.transform, 2f, 1f);
        return true;
    }

    public void CancelTarget()
    {
        if (targetList.Count == 0) { return; }
        cinemachineTargetGroup.RemoveMember(currentTarget.transform);
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
}
