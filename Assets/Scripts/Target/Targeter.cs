using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

namespace Target
{
    public class Targeter : MonoBehaviour
    {
        [SerializeField] private CinemachineTargetGroup cinemachineTargetGroup;

        private List<Target> targetList = new List<Target>();

        private Camera mainCamera;

        public Target CurrentTarget { get; set; }

        private Vector3 targetPos;

        private void Start()
        {
            mainCamera = Camera.main;
        }

        private void Update()
        {
            if (CurrentTarget is not null)
            {
                targetPos = CurrentTarget.transform.position;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<Target>(out var target)) { return; }
            targetList.Add(target);
            target.CancelTargetEvent += RemoveTarget;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent<Target>(out var target)) { return; }
            targetList.Remove(target);
        }

        public bool SelectTarget()
        {
            if (targetList.Count == 0) { return false; }

            Target closestTarget = null;
            var closestTargetDistance = Mathf.Infinity;

            foreach (var target in targetList)
            {
                Vector3 viewPoint = mainCamera.WorldToViewportPoint(target.transform.position);
                Debug.Log(viewPoint);
                if (viewPoint.x < 0 || viewPoint.x > 1 ||
                viewPoint.y < 0 || viewPoint.y > 1 ||
                viewPoint.z <= 0)
                {
                    continue;
                }
                var toCenter = (Vector2)viewPoint - new Vector2(0.5f, 0.5f);

                if (!(toCenter.sqrMagnitude < closestTargetDistance)) continue;
                closestTarget = target;
                closestTargetDistance = toCenter.sqrMagnitude;
            }

            if (closestTarget == null) { return false; }

            CurrentTarget = closestTarget;
            targetPos = CurrentTarget.transform.position;
            cinemachineTargetGroup.AddMember(CurrentTarget.transform, 2f, 1f);
            return true;
        }

        public void CancelTarget()
        {
            if (targetList.Count == 0) { return; }
            cinemachineTargetGroup.RemoveMember(CurrentTarget?.transform);
            CurrentTarget = null;
        }

        private void RemoveTarget(Target target)
        {
            if (CurrentTarget == target)
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
}

