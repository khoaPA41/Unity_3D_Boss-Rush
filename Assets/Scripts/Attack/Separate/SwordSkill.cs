using Attack;
using Audio;
using Attack.SkillFactory;
using DesignPattern.EventBus;
using DesignPattern.StateMachine.Boss.Base;
using DesignPattern.ObjectPooling;
using UnityEngine;

namespace Attack.Separate
{
    public class SwordSkill : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _homingSensitivity;
        [SerializeField] private float _hitDistance;

        [SerializeField] private bool _isRelease;
        [SerializeField] private bool _isActiveAnotherSkill; // if this skill have effect after touch player
        private bool _isPlayedAnotherSkill; // if added skill has played
        [SerializeField] private string _skillNameContinue;
        public Vector3 TargetPosition { get; set; }

        private Vector3 _currentVelocity;
        private Vector3 _direction;
        private PooledObject _pooledObject;

        private BossStateMachine _boss;

        private void OnEnable()
        {
            var container = GameObject.FindWithTag("Boss");
            if (container == null) return;
            _boss = container.GetComponentInChildren<BossStateMachine>(true);

            _pooledObject = GetComponent<PooledObject>();
        }

        private void Release()
        {
            _pooledObject.Release(name);
        }

        private void Update()
        {
            var targetCenter = TargetPosition + new Vector3(0f, .8f, 0f);
            var distanceToTarget = targetCenter - transform.position;

            if (distanceToTarget.sqrMagnitude <= _hitDistance * _hitDistance)
            {
                if (_isActiveAnotherSkill && !_isPlayedAnotherSkill)
                {
                    var getSkill = _boss.GetComponent<SkillSpawner>();
                    getSkill.SpawnSkill(_skillNameContinue, transform.position);
                    AudioManager.Instance.PlaySound(transform, AudioManager.Instance.fireExplosionResource);
                    _isPlayedAnotherSkill = true;
                }

                Release();
                return;
            }

            _direction = distanceToTarget.normalized;
            transform.rotation = Quaternion.LookRotation(_direction) * Quaternion.Euler(90f, 0f, 0f);

            Move();
        }

        private void Move()
        {
            var desiredVelocity = _direction * _speed;
            _currentVelocity = Vector3.Lerp(_currentVelocity, desiredVelocity, _homingSensitivity * Time.deltaTime);
            transform.position += _currentVelocity * Time.deltaTime;
        }

        public void InitializeBullet()
        {
            _currentVelocity = Vector3.zero;
            _isPlayedAnotherSkill = false;
            var direction = (TargetPosition - transform.position).normalized;

            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _boss.TryGetComponent(out ICaster casterObj);
                GameEventManager.Instance.TriggerSkillCasted(casterObj, SkillEffect.Stunned);
                var weaponTouch = _boss.GetComponent<WeaponHandler>();
                weaponTouch.OnGetWeapon();
            }

            if (_isRelease) return;
            if (other.CompareTag("Boss"))
            {
                var weaponTouch = other.GetComponent<WeaponHandler>();
                weaponTouch.OnGetWeapon();
                Release();
            }
        }
    }
}