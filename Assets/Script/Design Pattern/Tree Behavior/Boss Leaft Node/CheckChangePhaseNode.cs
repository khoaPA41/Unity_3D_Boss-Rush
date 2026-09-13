using Script.Design_Pattern.StateMachine;
using Script.Design_Pattern.Tree_Behavior.Base;
using UnityEngine;

namespace Script.Design_Pattern.Tree_Behavior
{
    public class CheckChangePhaseNode : BehaviorNode
    {
        private BossStateMachine bossStateMachine;
        private BossBehaviorBrain bossBehaviorBrain;


        public CheckChangePhaseNode(BossStateMachine bossStateMachine, BossBehaviorBrain bossBehaviorBrain)
        {
            this.bossStateMachine = bossStateMachine;
            this.bossBehaviorBrain = bossBehaviorBrain;
        }
        public override NodeState Evaluate()
        {
            if (!bossStateMachine.IsChangePhase)
            {
                if (bossStateMachine.NextPhase == bossStateMachine.UltimateCombo.Length)
                {
                    return NodeState.Failure;
                }

                if (bossStateMachine.UltimateCombo[bossStateMachine.NextPhase].HealthThreshold >= bossStateMachine.Health.currentHealth / bossStateMachine.Health.maxHealth)
                {
                    return NodeState.Success;
                }
            }

            return NodeState.Failure;
        }
    }
}