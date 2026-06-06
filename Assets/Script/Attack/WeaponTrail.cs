using System.Collections.Generic;
using Script.Attack;
using UnityEngine;

public class WeaponTrail : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask= 255;
    [SerializeField] private GameObject _legalOwner;
    
    private List<GameObject> alreadyObjectHit = new();
    private BoxCollider _collider;
    private Vector3 previousPosition;

    private int damage;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
    }

    private void OnEnable()
    {
        alreadyObjectHit.Clear();
        if(_collider is not null) previousPosition = transform.TransformPoint(_collider.center);
    }

    public void ResetObjectHitList()
    {
        alreadyObjectHit.Clear();
    }

    private void LateUpdate()
    {
        var currentPosition = transform.TransformPoint(_collider.center);
        var sizeWorld = Vector3.Scale(_collider.size, transform.lossyScale);
        var halfExtend = sizeWorld * .5f;
        var direction = (currentPosition - previousPosition).normalized;
        var sweepDistance = Vector3.Distance(previousPosition, currentPosition);

        var overlaps = Physics.OverlapBox(currentPosition, halfExtend , transform.rotation, _layerMask);
        foreach (var overlap in overlaps)
        {
            DealDamage(overlap.gameObject);
        }

        if (sweepDistance > 0.01f)
        {
            var raycastHits = Physics.BoxCastAll(previousPosition, halfExtend, direction, transform.rotation, sweepDistance, _layerMask);
            foreach (var hit in raycastHits)
            {
                DealDamage(hit.collider.gameObject);
            }
        }
        
        previousPosition  = currentPosition;
    }

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }


    private void DealDamage(GameObject other)
    {
        if (other.gameObject == _legalOwner || other.gameObject == gameObject) return;
        if (alreadyObjectHit.Contains(other)) return;

        alreadyObjectHit.Add(other);
        
        if (!other.TryGetComponent(out Health health)) return;
        if (health.noDamage) return;
        
        health.DealDamage(damage);
        health.HitStop();
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
