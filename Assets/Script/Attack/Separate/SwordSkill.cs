using Script.Design_Pattern.Object_Pooling;
using UnityEngine;

public class SwordSkill : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float homingSensitivity;
    [SerializeField] private float hitDistance;
    [SerializeField] private float timeToReturn;


    public Vector3 TargetPosition { get; set; }

    private Vector3 _currentVelocity;
    private Vector3 _direction;
    private PooledObject _pooledObject;
    private float _countTime;
    private bool alreadySendEvent = false;

    private void Start()
    {
        GetComponent<Rigidbody>();
        _pooledObject = GetComponent<PooledObject>();
    }

    private void OnEnable()
    {
        SkillSituationEvent.Instance.ReleasePoolObjectEvent += Release;
        _countTime = timeToReturn;
        var direction = (TargetPosition - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(direction);
    }
    
    private void OnDisable()
    {
        SkillSituationEvent.Instance.ReleasePoolObjectEvent -= Release;
    }

    private void Release()
    {
        _pooledObject.Release(this.name);
    }

    private void Update()
    {
        // _countTime -= Time.deltaTime;
        // if (_countTime <= 0)
        // {
        //     _pooledObject.Release(this.name);
        // }

        var targetCenter = TargetPosition + new Vector3(0f, .8f, 0f);
        var distanceToTarget = targetCenter - transform.position;

        if (distanceToTarget.sqrMagnitude <= hitDistance * hitDistance)
        {
            if (alreadySendEvent) return;
            SkillSituationEvent.Instance.SendNextActionEvent();
            alreadySendEvent = true;
            return;
        }

        _direction = distanceToTarget.normalized;
        transform.rotation = Quaternion.LookRotation(_direction) * Quaternion.Euler(90f, 0f, 0f);

        Move();
    }

    private void Move()
    {
        var desiredVelocity = _direction * speed;
        _currentVelocity = Vector3.Lerp(_currentVelocity, desiredVelocity, homingSensitivity * Time.deltaTime);
        transform.position += _currentVelocity * Time.deltaTime;
    }
}