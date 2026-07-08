using Script.Design_Pattern.StateMachine.Boss.Base;
using Script.Design_Pattern.Tree_Behavior.Base;
using UnityEngine;

public class CheckPlayerHealth : BehaviorNode
{
    private FinalBossStateMachine bossSystem;

    public CheckPlayerHealth(FinalBossStateMachine bossSystem)
    {
        this.bossSystem = bossSystem;
    }
    
    public override NodeState Evaluate()
    {
        if (bossSystem.Player.currentHealth > 0) return NodeState.Failure;

        bossSystem.IsChasingState = false;
        bossSystem.InputMovement = Vector2.zero;
        return NodeState.Success;
    }
}
