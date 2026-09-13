using Script.Design_Pattern.StateMachine;
using Script.Design_Pattern.Tree_Behavior.Base;
using UnityEngine;


namespace Script.Design_Pattern.Tree_Behavior
{
    public class CheckPlayerHealthNode : BehaviorNode
    {
        private BossStateMachine bossStateMachine;
        private BossBehaviorBrain bossBehaviorBrain;


        public CheckPlayerHealthNode(BossStateMachine bossStateMachine, BossBehaviorBrain bossBehaviorBrain)
        {
            this.bossStateMachine = bossStateMachine;
            this.bossBehaviorBrain = bossBehaviorBrain;
        }

        public override NodeState Evaluate()
        {
            if (bossStateMachine.Player.currentHealth > 0) return NodeState.Failure;

            bossStateMachine.IsChasing = false;
            return NodeState.Success;
        }
    }
}