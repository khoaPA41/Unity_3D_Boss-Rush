using UnityEngine;

public class GetSkill : MonoBehaviour
{
    [SerializeField] ObjectPooling objectPooling;


    public void SpawnSkill(string name, Vector3 skillPosition)
    {
        objectPooling.GetPooledObject(name, skillPosition);
    }
}
