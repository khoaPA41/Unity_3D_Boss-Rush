using Design_Pattern.StateMachine.Boss;
using Design_Pattern.Tree_Behavior.Base;
using Design_Pattern.Tree_Behavior.Boss;

namespace Design_Pattern.Tree_Behavior
{
    public class TaskUltimateAttackNode : BehaviorNode
    {
        private BossStateMachine bossStateMachine;
        private BossBehaviorBrain bossBehaviorBrain;


        public TaskUltimateAttackNode(BossStateMachine bossStateMachine, BossBehaviorBrain bossBehaviorBrain)
        {
            this.bossStateMachine = bossStateMachine;
            this.bossBehaviorBrain = bossBehaviorBrain;
        }
        public override NodeState Evaluate()
        {
            if (!bossStateMachine.IsUltimateAttack)
            {
                bossStateMachine.IsUltimateAttack = true;
            }

            if (bossStateMachine.IsFinishedAttack)
            {
                bossBehaviorBrain.IsUsedUltimate = true;
                bossStateMachine.IsUltimateAttack = false;
                return NodeState.Failure;
            }
            return NodeState.Running;
        }
    }
}