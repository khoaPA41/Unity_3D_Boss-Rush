using UnityEngine;

[RequireComponent(typeof(PooledObject))]

public class SkillPooledObject : MonoBehaviour
{
    [SerializeField] string skillName;

    ParticleSystem particle;

    PooledObject pooled;

    void Start()
    {
        particle = GetComponent<ParticleSystem>();
        pooled = GetComponent<PooledObject>();
    }

    private void OnParticleSystemStopped()
    {
        pooled.Release(skillName);
    }
}
