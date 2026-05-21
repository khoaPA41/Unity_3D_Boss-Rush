using System;
using Script.Design_Pattern.Object_Pooling;
using UnityEngine;
using static UnityEngine.ParticleSystem;

[RequireComponent(typeof(PooledObject))]

public class SkillPooledObject : MonoBehaviour
{
    private ParticleSystem particle;

    private PooledObject pooled;

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
        pooled = GetComponent<PooledObject>();
    }
    
    private void OnEnable()
    {
        particle?.Clear();
        particle?.Play();
    }
    
    private void OnParticleSystemStopped()
    {
        particle?.Stop();
        
        pooled.Release(this.gameObject.name);
    }
}
