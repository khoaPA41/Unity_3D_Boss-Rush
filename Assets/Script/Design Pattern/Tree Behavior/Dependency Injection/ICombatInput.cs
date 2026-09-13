using System;
using UnityEngine;

namespace Script.Design_Pattern.Tree_Behavious.Dependency_Injection
{
    public interface ICombatInput
    {
        Vector2 InputMovement { get; set; }
        bool IsChasing { get; set; }
        bool IsSprint { get; set; }
        bool IsAttack { get; set; }
        bool IsCounterAttack { get; set; }

        event Action<int> SkillAction;
    }
}