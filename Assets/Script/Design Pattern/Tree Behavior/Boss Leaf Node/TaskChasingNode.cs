using Script.Design_Pattern.StateMachine;
using Script.Design_Pattern.Tree_Behavior.Base;
using UnityEngine;

namespace Script.Design_Pattern.Tree_Behavior
{
    public class TaskChasingNode : BehaviorNode
    {
        private BossStateMachine bossStateMachine;
        private BossBehaviorBrain bossBehaviorBrain;


        public TaskChasingNode(BossStateMachine bossStateMachine, BossBehaviorBrain bossBehaviorBrain)
        {
            this.bossStateMachine = bossStateMachine;
            this.bossBehaviorBrain = bossBehaviorBrain;
            bossBehaviorBrain.NextToggleTime = Time.time + bossBehaviorBrain.IdleDuration;
        }

        public override NodeState Evaluate()
        {
            bossStateMachine.IsFinishedAttack = false;
            if (Time.time >= bossBehaviorBrain.NextToggleTime)
            {
                bossBehaviorBrain.IsChasingState = !bossBehaviorBrain.IsChasingState;
                var nextToggleTime = bossBehaviorBrain.IsChasingState ? bossBehaviorBrain.ChaseDuration : bossBehaviorBrain.IdleDuration;
                bossBehaviorBrain.NextToggleTime = Time.time + nextToggleTime;
            }

            if (bossBehaviorBrain.IsChasingState)
            {
                bossStateMachine.IsChasing = true;
            }
            else
            {
                bossStateMachine.IsChasing = false;
            }

            return NodeState.Running;
        }
    }
}



