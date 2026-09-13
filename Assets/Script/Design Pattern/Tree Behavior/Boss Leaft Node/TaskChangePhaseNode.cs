using Script.Design_Pattern.StateMachine;
using Script.Design_Pattern.Tree_Behavior.Base;
using UnityEngine;


namespace Script.Design_Pattern.Tree_Behavior
{
    public class TaskChangePhaseNode : BehaviorNode
    {
        private BossStateMachine bossStateMachine;
        private BossBehaviorBrain bossBehaviorBrain;


        public TaskChangePhaseNode(BossStateMachine bossStateMachine, BossBehaviorBrain bossBehaviorBrain)
        {
            this.bossStateMachine = bossStateMachine;
            this.bossBehaviorBrain = bossBehaviorBrain;
        }

        public override NodeState Evaluate()
        {
            bossBehaviorBrain.CurrentPhase++;
            bossBehaviorBrain.NextPhase = bossBehaviorBrain.CurrentPhase + 1;

            return NodeState.Success;
        }
    }
}