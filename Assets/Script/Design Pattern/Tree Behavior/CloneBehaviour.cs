using Script.Design_Pattern.Object_Pooling;
using UnityEngine;


namespace Script.Design_Pattern.Tree_Behavious
{
    public class CloneBehaviour : MonoBehaviour
    {
        [SerializeField] private PooledObject pooledObject;
        [SerializeField] private float timeToRelease;
        private float _countTime;
        
        private void Awake()
        {
            pooledObject = GetComponent<PooledObject>();
        }
        
        private void OnEnable()
        {
            _countTime = timeToRelease;
        }

        private void Update()
        {
            _countTime -= Time.deltaTime;
            if (_countTime <= 0)
            {
                pooledObject.Release(gameObject.name);
            }
        }
    }
}
