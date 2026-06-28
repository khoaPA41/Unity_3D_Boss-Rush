using Script.Attack.Skill_Factory;
using Script.Design_Pattern.EventBus;
using Script.Design_Pattern.Object_Pooling;
using Script.Design_Pattern.StateMachine.Boss.Base;
using UnityEngine;

public class SwordSkill : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float homingSensitivity;
    [SerializeField] private float hitDistance;
    [SerializeField] private float timeToReturn;
    [SerializeField] private bool isRelease = false;
    [SerializeField] private bool isActiveAnotherSkill = false; // if this skill have effect after touch player
    private bool isPlayedAnotherSkill = false; // if added skill has played
    [SerializeField] private string skillNameContinue;
    public Vector3 TargetPosition { get; set; }

    private Vector3 _currentVelocity;
    private Vector3 _direction;
    private PooledObject _pooledObject;
    private float _countTime;
    private bool _alreadySendEvent;

    private FinalBossStateMachine boss;
    
    private void Awake()
    {
        var container =  GameObject.FindWithTag("Boss");
        boss = container.GetComponentInChildren<FinalBossStateMachine>(true);
        GetComponent<Rigidbody>();
        _pooledObject = GetComponent<PooledObject>();
    }
    
    private void OnEnable()
    {
        boss.ManageAnimationSkillEvent.ReleasePoolObjectEvent += Release;
        isPlayedAnotherSkill = false;
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
            if (isActiveAnotherSkill && !isPlayedAnotherSkill) 
            {
                var getSkill = boss.GetComponent<GetSkill>();
                getSkill.SpawnSkill(skillNameContinue, transform.position);
                isPlayedAnotherSkill = true;
            }
            
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
            // var caster = GameObject.Find("Enemy");
            boss.TryGetComponent(out ICaster casterObj);
            GameEventManagers.Instance.TriggerSkillCasted(casterObj, SkillEffect.Stunned);
        }
        
        if (isRelease) return;
        if (other.CompareTag("Boss"))
        {
            // var boss = other.GetComponent<FinalBossStateMachine>();
            var weaponTouch = other.GetComponent<WeaponHandler>();
            weaponTouch.OnGetWeapon();
            boss.SendActionEvent();
            boss.SendReleasePoolObjectEvent();
        }
    }
}