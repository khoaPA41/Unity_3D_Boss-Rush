using UnityEngine;
using static UnityEngine.ParticleSystem;

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
        particle.Stop();
        pooled.Release(skillName);
    }
}
