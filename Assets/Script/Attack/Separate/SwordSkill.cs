using System;
using Script.Attack.Skill_Factory;
using Script.Design_Pattern.EventBus;
using Script.Design_Pattern.Object_Pooling;
using Script.Design_Pattern.StateMachine.Boss.Base;
using Unity.VisualScripting;
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
    private bool _alreadySendEvent;

    private FinalBossStateMachine boss;
    
    private void Awake()
    {
        boss = GameObject.FindWithTag("Boss").GetComponent<FinalBossStateMachine>();
    }
    
    private void Start()
    {
        GetComponent<Rigidbody>();
        _pooledObject = GetComponent<PooledObject>();
    }

    private void OnEnable()
    {
        boss.ManageAnimationSkillEvent.ReleasePoolObjectEvent += Release;
        _countTime = timeToReturn;
        var direction = (TargetPosition - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(direction);
    }
    
    private void OnDisable()
    {
        boss.ManageAnimationSkillEvent.ReleasePoolObjectEvent -= Release;
    }

    private void Release()
    {
        _pooledObject.Release(name);
    }

    private void Update()
    {
        var targetCenter = TargetPosition + new Vector3(0f, .8f, 0f);
        var distanceToTarget = targetCenter - transform.position;

        if (distanceToTarget.sqrMagnitude <= hitDistance * hitDistance)
        {
            if (_alreadySendEvent) return;
            boss.ManageAnimationSkillEvent.SendNextActionEvent();
            _alreadySendEvent = true;
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


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {        
            var caster = GameObject.Find("Enemy");
            caster.TryGetComponent(out ICaster casterObj);
            GameEventManagers.Instance.TriggerSkillCasted(casterObj, SkillEffect.Stunned);
        }
        
        if (other.CompareTag("Boss"))
        {
            var boss = other.GetComponent<FinalBossStateMachine>();
            var weaponTouch = other.GetComponent<WeaponHandler>();
            weaponTouch.OnGetWeapon();
            boss.SendActionEvent();
            boss.SendReleasePoolObjectEvent();
        }
    }
}