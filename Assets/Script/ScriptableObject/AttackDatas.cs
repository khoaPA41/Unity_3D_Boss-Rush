using UnityEngine;

[CreateAssetMenu(fileName = "AttackData", menuName = "Scriptable Objects/AttackData")]
public class AttackDatas : ScriptableObject
{
    public string AnimationName { get; private set; }
    public string AnimationTag { get; private set; } = "Attack";
    public float AnimationSpeed { get; set; }
    public int NextAttackDataIndex { get; private set; } = -1;
    public float AttackAnimationTime { get; private set; }
    public float AnimationTransition { get; private set; }
    public float ForceTime { get; private set; }
    public float Force { get; private set; }
    public float AttackDamage { get; set; }
}
