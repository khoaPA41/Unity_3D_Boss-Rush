using UnityEngine;

namespace Script.Design_Pattern.Object_Pooling
{
    public class GetSkill : MonoBehaviour
    {
        [SerializeField] private ObjectPooling objectPooling;

        public GameObject skill { get; private set; }
        
        public void SpawnSkill(string name, Vector3 skillPosition)
        {
            skill = objectPooling.GetPooledObject(name, skillPosition).gameObject;
        }
    }
}