using Design_Pattern.StateMachine.Boss;
using Design_Pattern.Tree_Behavior.Base;
using Design_Pattern.Tree_Behavior.Boss;

namespace Design_Pattern.Tree_Behavior
{
    public class CheckUltimateNode : BehaviorNode
    {
        private BossStateMachine bossStateMachine;
        private BossBehaviorBrain bossBehaviorBrain;


        public CheckUltimateNode(BossStateMachine bossStateMachine, BossBehaviorBrain bossBehaviorBrain)
        {
            this.bossStateMachine = bossStateMachine;
            this.bossBehaviorBrain = bossBehaviorBrain;
        }
        public override NodeState Evaluate()
        {
            if (bossBehaviorBrain.IsUsedUltimate) return NodeState.Failure;
            if (bossStateMachine.UltimateCombo.AttackData.Length == 0 || bossBehaviorBrain.IsUsedUltimate) return NodeState.Failure;

            bossStateMachine.IsUltimateAttack = true;
            return NodeState.Success;
        }
    }
}