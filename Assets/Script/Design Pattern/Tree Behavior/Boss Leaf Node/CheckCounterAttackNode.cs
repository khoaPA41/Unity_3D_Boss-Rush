using Script.Design_Pattern.StateMachine;
using Script.Design_Pattern.Tree_Behavior.Base;
using UnityEngine;

namespace Script.Design_Pattern.Tree_Behavior
{
    public class CheckCounterAttackNode : BehaviorNode
    {
        private BossStateMachine bossStateMachine;
        private BossBehaviorBrain bossBehaviorBrain;


        public CheckCounterAttackNode(BossStateMachine bossStateMachine, BossBehaviorBrain bossBehaviorBrain)
        {
            this.bossStateMachine = bossStateMachine;
            this.bossBehaviorBrain = bossBehaviorBrain;
        }

        public override NodeState Evaluate()
        {
            int hitToNeed = Random.Range(bossBehaviorBrain.EnduredTimes / 2, bossBehaviorBrain.EnduredTimes + 1);
            if (hitToNeed - bossBehaviorBrain.HitReceived == 0)
            {
                return NodeState.Success;
            }

            return NodeState.Failure;
        }
    }
}