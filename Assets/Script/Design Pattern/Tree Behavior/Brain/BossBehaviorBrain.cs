using System.Collections.Generic;
using Script.Design_Pattern.StateMachine;
using Script.Design_Pattern.Tree_Behavior.Base;
using UnityEngine;

namespace Script.Design_Pattern.Tree_Behavior
{
    public class BossBehaviorBrain : MonoBehaviour
    {
        [Header("Action Information")]
        [field: SerializeField] public float IdleDuration { get; private set; }
        [field: SerializeField] public float ChaseDuration { get; private set; }
        [field: SerializeField] public float AttackRange { get; private set; }
        [field: SerializeField] public float TimeOutCombo { get; private set; } = 2f;


        private BossStateMachine bossStateMachine;
        private BehaviorNode topNode { get; set; }



        public float NextToggleTime { get; set; }
        public bool IsChasingState { get; set; }

        private void Awake()
        {
            bossStateMachine = GetComponent<BossStateMachine>();
            bossStateMachine.ReturnLocomotion();
            ConstructBehaviorTree();
        }

        private void OnEnable()
        {
            bossStateMachine.ReturnLocomotion();
            // ConstructBehaviorTree();
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
                new CheckAttackRangeNode(bossStateMachine, this),
                new TaskAttackingNode(bossStateMachine, this)
            });

            var changePhaseSequence = new BehaviorSequence(new List<BehaviorNode>
            {
                new CheckChangePhaseNode(bossStateMachine, this),
                new TaskChangePhaseNode(bossStateMachine, this)
            });

            // /*Duoi theo*/
            var tacticalChasing = new TaskChasingNode(bossStateMachine, this);

            // /*Kiem tra mau player*/
            // var finishedCombat = new CheckPlayerHealth(bossSystem);

            /*Nhanh 3: uu tien tan cong truoc*/
            topNode = new BehaviorSelector(new List<BehaviorNode>
            {
                // finishedCombat,
                changePhaseSequence,
                atkSequence,
                tacticalChasing
            });
        }
    }
}