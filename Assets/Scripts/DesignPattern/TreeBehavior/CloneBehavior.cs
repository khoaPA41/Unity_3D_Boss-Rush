using DesignPattern.ObjectPooling;
using UnityEngine;


namespace DesignPattern.TreeBehavior
{
    [RequireComponent(typeof(PooledObject))]
    public class CloneBehavior : MonoBehaviour
    {
        [SerializeField] private PooledObject _pooledObject;
        [SerializeField] private float _timeToRelease;
        private float _countTime;

        private void Awake()
        {
            _pooledObject = GetComponent<PooledObject>();
        }

        private void OnEnable()
        {
            _countTime = _timeToRelease;
        }

        private void Update()
        {
            _countTime -= Time.deltaTime;
            if (_countTime <= 0)
            {
                _pooledObject.Release(gameObject.name);
            }
        }
    }
}
