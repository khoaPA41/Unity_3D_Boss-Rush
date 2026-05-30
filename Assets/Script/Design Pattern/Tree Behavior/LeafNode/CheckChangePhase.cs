using Script.Design_Pattern.StateMachine.Boss.Base;
using Script.Design_Pattern.Tree_Behavior.Base;
using UnityEngine;

public class CheckChangePhase : BehaviorNode
{
    private FinalBossStateMachine bossSystem;

    public CheckChangePhase(FinalBossStateMachine bossSystem)
    {
        this.bossSystem = bossSystem;
    }
    
     
    public override NodeState Evaluate()
    {
        if (!bossSystem.IsChangePhase)
        {
            if (bossSystem.NextPhase == bossSystem.UltimateCombo.Length)
            {
                return NodeState.Failure;
            }

            if ((float)bossSystem.Health.currentHealth / bossSystem.Health.maxHealth <= bossSystem.UltimateCombo[bossSystem.NextPhase].HealthThreshold)
            {
                Debug.Log("True");
                return NodeState.Success;
            }
        }
        Debug.Log("False");

        return NodeState.Failure;
    }
}
