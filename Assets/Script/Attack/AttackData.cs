using System;
using Attack.Skill_Factory;
using UnityEngine;
[Serializable]
public class AttackData
{
    [field: SerializeField] public string AnimationName { get; private set; }
    [field: SerializeField] public string VfxName { get; private set; }
    [field: SerializeField] public string AnimationTag { get; private set; } = "Attack";
    [field: SerializeField] public int NextAttackDataIndex { get; private set; } = -1;
    [field: SerializeField] public float AttackAnimationTime { get; private set; }
    [field: SerializeField] public float AnimationTransition { get; private set; }
    [field: SerializeField] public float ForceTime { get; private set; }
    [field: SerializeField] public float Force { get; private set; }
    [field: SerializeField] public float AttackDamage { get; set; }
    [field: SerializeField] public SkillType SkillType { get; private set; }
}
