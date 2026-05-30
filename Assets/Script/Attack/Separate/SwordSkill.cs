using System;
using Script.Attack.Skill_Factory;
using Script.Design_Pattern.EventBus;
using Script.Design_Pattern.Object_Pooling;
using UnityEngine;

public class SwordSkill : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float homingSensitivity;
    [SerializeField] private float hitDistance;
    [SerializeField] private float timeToReturn;


    public Vector3 targetPosition { get; set; }


    private Rigidbody rb;
    private Vector3 currentVelocity;
    private Vector3 direction;
    private PooledObject pooledObject;
    private float countTime;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pooledObject = GetComponent<PooledObject>();
    }

    private void OnEnable()
    {
        countTime =  timeToReturn;
        var direction = (targetPosition - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    private void Update()
    { 
        countTime -= Time.deltaTime;
        if (countTime <= 0)
        {
            pooledObject.Release(this.name);
        }
        
        var targetCenter = targetPosition + new Vector3(0f, .8f, 0f);
        var distanceToTarget = targetCenter - transform.position;
        
        if (distanceToTarget.sqrMagnitude <= hitDistance * hitDistance)
        {
            return;
        }

        direction = distanceToTarget.normalized;
        transform.rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(90f, 0f, 0f);
        
        Move();
    }

    private void Move()
    {
        var desiredVelocity = direction * speed;
        currentVelocity = Vector3.Lerp(currentVelocity, desiredVelocity, homingSensitivity * Time.deltaTime);
        transform.position += currentVelocity * Time.deltaTime;
    }
}