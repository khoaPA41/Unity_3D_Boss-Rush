using UnityEngine;

namespace Script.Design_Pattern.Object_Pooling
{
    public class GetSkill : MonoBehaviour
    {
        [SerializeField] private ObjectPooling objectPooling;

        public GameObject Skill { get; private set; }
        
        public void SpawnSkill(string name, Vector3 skillPosition)
        {
            Skill = objectPooling.GetPooledObject(name, skillPosition).gameObject;
        }
    }
}