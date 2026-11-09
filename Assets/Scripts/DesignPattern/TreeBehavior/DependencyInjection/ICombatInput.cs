using System;
using UnityEngine;

namespace DesignPattern.TreeBehavior.DependencyInjection
{
    public interface ICombatInput
    {
        Vector2 InputMovement { get; set; }
        bool IsChasing { get; set; }
        bool IsSprint { get; set; }
        bool IsAttack { get; set; }
        bool IsCounterAttack { get; set; }
        bool IsUltimateAttack { get; set; }


        event Action<int> SkillAction;
    }
}