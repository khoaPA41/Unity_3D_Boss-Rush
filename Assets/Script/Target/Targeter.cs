using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

namespace Script.Target
{
    public class Targeter : MonoBehaviour
    {
        [SerializeField] private CinemachineTargetGroup cinemachineTargetGroup;

        private List<Target> targetList = new List<Target>();

        private Camera mainCamera;

        public Target currentTarget { get; set; }

        private Vector3 targetPos;

        private void Start()
        {
            mainCamera = Camera.main;
        }

        private void Update()
        {
            if(currentTarget is not null)
            {
                targetPos = currentTarget.transform.position;
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

        public bool SelectedTarget()
        {
            if (targetList.Count == 0) { return false; }

            Target closestTarget = null;
            var closestTargetDistance = Mathf.Infinity;

            foreach (var target in targetList)
            {
                Vector2 viewPoint = mainCamera.WorldToViewportPoint(target.transform.position);

                if (viewPoint.x < 0 || viewPoint.x > 1 || viewPoint.y < 0 || viewPoint.y > 1)
                {
                    continue;
                }
                var toCenter = viewPoint - new Vector2(0.5f, 0.5f);

                if (!(toCenter.sqrMagnitude < closestTargetDistance)) continue;
                closestTarget = target;
                closestTargetDistance = toCenter.sqrMagnitude;
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
            cinemachineTargetGroup.RemoveMember(currentTarget?.transform);
            currentTarget = null;
        }

        private void RemoveTarget(Target target)
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
}

