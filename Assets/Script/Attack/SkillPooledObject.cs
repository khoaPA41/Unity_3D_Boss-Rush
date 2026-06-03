using Script.Attack.Skill_Factory;
using Script.Design_Pattern.Object_Pooling;
using UnityEngine;

[RequireComponent(typeof(PooledObject))]

public class SkillPooledObject : MonoBehaviour
{
    private ParticleSystem _particle;
    private PooledObject _pooled;

    private void Awake()
    {
        _particle = GetComponent<ParticleSystem>();
        _pooled = GetComponent<PooledObject>();
    }
    
    private void OnEnable()
    {
        _particle?.Clear();
        _particle?.Play();
    }
    
    private void OnParticleSystemStopped()
    {
        _particle?.Stop();
        _pooled.Release(this.gameObject.name);
    }
}
