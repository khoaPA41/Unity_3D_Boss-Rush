using Script.Attack.Skill_Factory;
using Script.Design_Pattern.Object_Pooling;
using Script.Design_Pattern.StateMachine.Boss.Base;
using UnityEngine;

[RequireComponent(typeof(PooledObject))]

public class SkillPooledObject : MonoBehaviour
{
    private ParticleSystem _particle;
    private PooledObject _pooled;
    private FinalBossStateMachine _boss;

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
    
    private void OnDisable()
    {
        _particle?.Clear();
        _particle?.Stop();
    }
    
    private void OnParticleSystemStopped()
    {
        Release();
    }
    
    private void Release()
    {
        _particle?.Stop();
        _pooled.Release(gameObject.name);
    }
    
}
