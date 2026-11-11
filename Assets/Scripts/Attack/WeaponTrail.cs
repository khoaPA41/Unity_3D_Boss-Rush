using Audio;
using System.Collections.Generic;
using UnityEngine;
using Attack.Status;


namespace Attack
{
    public class WeaponTrail : MonoBehaviour
    {
        [SerializeField] private LayerMask _layerMask = 255;
        [SerializeField] private GameObject _legalOwner;
        [SerializeField] private int _baseDamage;

        [SerializeField] private PlayerSFX _sfx;
        [SerializeField] private bool _isEffect;

        private List<GameObject> _alreadyObjectHit = new();
        private BoxCollider _collider;
        private Vector3 _previousPosition;

        private float _damage;

        private void Awake()
        {
            _collider = GetComponent<BoxCollider>();
            _damage = _baseDamage;
        }

        private void OnEnable()
        {
            _alreadyObjectHit.Clear();
            if (_collider != null) _previousPosition = transform.TransformPoint(_collider.center);
        }

        public void ResetObjectHitList()
        {
            _alreadyObjectHit.Clear();
        }

        private void LateUpdate()
        {
            var currentPosition = transform.TransformPoint(_collider.center);
            var sizeWorld = Vector3.Scale(_collider.size, transform.lossyScale);
            var halfExtents = sizeWorld * .5f;
            var direction = (currentPosition - _previousPosition).normalized;
            var sweepDistance = Vector3.Distance(_previousPosition, currentPosition);

            var overlaps = UnityEngine.Physics.OverlapBox(currentPosition, halfExtents, transform.rotation, _layerMask);
            foreach (var overlap in overlaps)
            {
                DealDamage(overlap.gameObject);
            }

            if (sweepDistance > 0.01f)
            {
                var raycastHits = UnityEngine.Physics.BoxCastAll(_previousPosition, halfExtents, direction, transform.rotation, sweepDistance, _layerMask);
                foreach (var hit in raycastHits)
                {
                    DealDamage(hit.collider.gameObject);
                }
            }

            _previousPosition = currentPosition;
        }

        public void SetDamage(float damage)
        {
            _damage = damage;
        }


        private void DealDamage(GameObject other)
        {
            if (other.gameObject == _legalOwner || other.gameObject == gameObject) return;
            if (_alreadyObjectHit.Contains(other)) return;

            _alreadyObjectHit.Add(other);

            if (!other.TryGetComponent(out Health health)) return;

            health.PerfectDodgeAward();

            if (health.NoDamage) return;

            health.DealDamage(_damage);
            health.HitStop();

            if (_isEffect) return;
            health.PlayerSFX.PlayHitSound(_sfx.HitResource);
        }


        private void OnDrawGizmos()
        {
            _collider ??= GetComponent<BoxCollider>();
            if (_collider is null) return;

            Gizmos.color = Color.red;

            var sizeWorld = Vector3.Scale(_collider.size, transform.lossyScale);
            var centerWorld = transform.TransformPoint(_collider.center);

            Gizmos.matrix = Matrix4x4.TRS(centerWorld, transform.rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, sizeWorld);
        }
    }
}