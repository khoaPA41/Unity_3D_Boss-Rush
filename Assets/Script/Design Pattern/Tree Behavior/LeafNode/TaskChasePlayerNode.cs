using Script.Design_Pattern.StateMachine.Boss.Base;
using Script.Design_Pattern.Tree_Behavior.Base;
using UnityEngine;

namespace Script.Design_Pattern.Tree_Behavior.LeafNode
{
    public class TaskChasePlayerNode : BehaviorNode
    {
        private FinalBossStateMachine bossSystem;

        public TaskChasePlayerNode( FinalBossStateMachine bossSystem)
        {
            this.bossSystem = bossSystem;
            bossSystem.NextPhaseToggleTime = Time.time + bossSystem.ChaseDuration; 
        }
        
        public override NodeState Evaluate()
        {
            if (Time.time >= bossSystem.NextPhaseToggleTime)
            {
                bossSystem.IsChasingState = !bossSystem.IsChasingState;
                var nextToggleTime = bossSystem.IsChasingState ? bossSystem.ChaseDuration : bossSystem.IdleDuration;
                bossSystem.NextPhaseToggleTime = Time.time + nextToggleTime;
            }

            if (bossSystem.IsChasingState)
            {
                var dir = bossSystem.GetDirToPlayer();
            
                bossSystem.InputMovement = new Vector2(dir.x, dir.z);
            
                bossSystem.IsWalking = bossSystem.IsWalkRange();
            }
            else
            {
                bossSystem.InputMovement = Vector2.zero;
            }
            
            return NodeState.Running;
        }
    }
}