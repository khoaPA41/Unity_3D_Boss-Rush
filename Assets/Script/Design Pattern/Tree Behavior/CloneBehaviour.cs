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
            Debug.Log(_countTime);
        }

        private void Update()
        {
            _countTime -= Time.deltaTime;
            Debug.Log(_countTime);
            if (_countTime <= 0)
            {
                pooledObject.Release(gameObject.name);
            }
        }


    }
}
