using DesignPattern.StateMachine.Boss.Base;
using DesignPattern.TreeBehavior.Base;
using DesignPattern.TreeBehavior.Brain;

namespace DesignPattern.TreeBehavior.BossLeafNode
{
    public class TaskCounterAttackNode : BehaviorNode
    {
        private BossStateMachine bossStateMachine;
        private BossBehaviorBrain bossBehaviorBrain;


        public TaskCounterAttackNode(BossStateMachine bossStateMachine, BossBehaviorBrain bossBehaviorBrain)
        {
            this.bossStateMachine = bossStateMachine;
            this.bossBehaviorBrain = bossBehaviorBrain;
        }

        public override NodeState Evaluate()
        {
            if (!bossStateMachine.IsCounterAttack)
            {
                bossStateMachine.IsCounterAttack = true;
            }

            if (bossStateMachine.IsFinishedAttack)
            {
                bossStateMachine.IsCounterAttack = false;
                bossBehaviorBrain.HitReceived = 0;
                return NodeState.Failure;
            }

            return NodeState.Running;
        }
    }
}