using System.Collections.Generic;
using Script.Design_Pattern.StateMachine.Boss.Base;
using Script.Design_Pattern.StateMachine.Boss.Main;
using Script.Design_Pattern.Tree_Behavior.Base;
using Script.Design_Pattern.Tree_Behavior.LeafNode;
using UnityEngine;

namespace Script.Design_Pattern.Tree_Behavior.Brain
{
    public class BossBrain : MonoBehaviour
    {
        private FinalBossStateMachine bossSystem;
        private BehaviorNode topNode { get; set; }

        private void Start()
        {
            bossSystem = GetComponent<FinalBossStateMachine>();
            // bossSystem.SwitchState(new FinalBossLocomotionState(bossSystem));
            bossSystem.ReturnLocomotion();
            ConstructBehaviorTree();
        }

        private void Update()
        {
            topNode?.Evaluate();
        }

        private void ConstructBehaviorTree()
        {
            /*Nhanh ca hai dieu dien dung*/
            var atkSequence = new BehaviorSequence(new List<BehaviorNode>
            {
                new CheckAttackRange(bossSystem),
                new TaskAttackNode(bossSystem)
            });

            var changePhaseSequence = new BehaviorSequence(new List<BehaviorNode>
            {
                new CheckChangePhase(bossSystem),
                new TaskUltimateNode(bossSystem)
            });

            /*Duoi theo*/
            var taticalChasing = new TaskChasePlayerNode(bossSystem);
            var phaseTwo = new CheckChangePhase(bossSystem);

            /*Nhanh 3: uu tien tan cong truoc*/
            topNode = new BehaviorSelector(new List<BehaviorNode>
            {
                changePhaseSequence,
                atkSequence,
                phaseTwo,
                taticalChasing
            });
        }
    }
}