using UnityEngine;

namespace DesignPattern.ObjectPooling
{
    public class SkillSpawner : MonoBehaviour
    {
        [SerializeField] private ObjectPoolManager objectPooling;

        public GameObject Skill { get; private set; }
        
        public void SpawnSkill(string name, Vector3 skillPosition)
        {
            Skill = objectPooling.GetPooledObject(name, skillPosition).gameObject;
        }
    }
}