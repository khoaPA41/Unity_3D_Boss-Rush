using Script.Design_Pattern.StateMachine.Boss.Base;
using Script.Design_Pattern.Tree_Behavior.Base;

namespace Script.Design_Pattern.Tree_Behavior.LeafNode
{
    public class TaskUltimateNode : BehaviorNode
    {
        FinalBossStateMachine bossSystem;

        public TaskUltimateNode(FinalBossStateMachine bossSystem)
        {
            this.bossSystem = bossSystem;
        }

        public override NodeState Evaluate()
        {
            bossSystem.IsActiveUltimate = true;
            bossSystem.IsChangePhase = true;
            return NodeState.Success;
        }
    }
}