using System;
using UnityEngine;

public interface ICombatInput
{
    Vector2 InputMovement { get;}
    Vector2 Look { get; }

    bool IsSprint { get;}
    bool IsAttack { get;}
    int SkillNumber { get;}

    event Action JumpAction;

    event Action DodgeAction;

    event Action TargetAction;

    event Action<int> SkillAction;
}