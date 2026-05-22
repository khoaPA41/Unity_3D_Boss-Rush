using System;
using UnityEngine;

public interface ICombatInput
{
    Vector2 InputMovement { get; set; }
    Vector2 Look { get; set;}

    bool IsSprint { get;set;}
    bool IsAttack { get; set;}
    int SkillNumber { get; set;}

    event Action JumpAction;

    event Action DodgeAction;

    event Action TargetAction;

    event Action<int> SkillAction;
}