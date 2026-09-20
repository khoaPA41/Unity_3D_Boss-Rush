using System;
using System.Collections.Generic;
using Design_Pattern.StateMachine.Boss;
using Design_Pattern.Tree_Behavior.Base;
using UnityEngine;


namespace Design_Pattern.Tree_Behavior.Boss
{
    public class BossBehaviorBrain : MonoBehaviour
    {
        [Header("Action Information")]
        [field: SerializeField] public float IdleDuration { get; private set; }
        [field: SerializeField] public float ChaseDuration { get; private set; }
        [field: SerializeField] public float AttackRange { get; private set; }
        [field: SerializeField] public int EnduredTimes { get; set; }
        [field: SerializeField] public float TimeOutCombo { get; private set; } = 2f;


        // Phase params
        public int Phase { get; private set; }
        public int CurrentPhase { get; set; }
        public int NextPhase { get; set; }
        public bool IsUsedUltimate { get; set; }


        public uint HitReceived { get; set; }
        public bool IsDead { get; set; }


        private BossStateMachine bossStateMachine;
        private BehaviorNode topNode { get; set; }


        public float NextToggleTime { get; set; }
        public bool IsChasingState { get; set; }


        public event Action DeadPhaseAction;
        private bool isCallDeadPhaseAction;

        private void Awake()
        {
            bossStateMachine = GetComponent<BossStateMachine>();
            ConstructBehaviorTree();
            Phase = bossStateMachine.NormalCombo.Length;
            CurrentPhase = 0;
            NextPhase = CurrentPhase + 1;
        }

        private void OnEnable()
        {
            bossStateMachine.ReturnLocomotion();
        }

        private void Update()
        {
            topNode?.Evaluate();
        }

        private void ConstructBehaviorTree()
        {
            // Branch both true
            var atkSequence = new BehaviorSequence(new List<BehaviorNode>
            {
                new CheckAttackRangeNode(bossStateMachine, this),
                new TaskAttackingNode(bossStateMachine, this)
            });

            var ultSequence = new BehaviorSequence(new List<BehaviorNode>
            {
                new CheckUltimateNode(bossStateMachine, this),
                new TaskUltimateAttackNode(bossStateMachine, this)
            });

            var changePhaseSequence = new BehaviorSequence(new List<BehaviorNode>
            {
                new CheckChangePhaseNode(bossStateMachine, this),
                new TaskChangePhaseNode(bossStateMachine, this)
            });

            var counterAtkSequence = new BehaviorSequence(new List<BehaviorNode>
            {
                new CheckCounterAttackNode(bossStateMachine, this),
                new TaskCounterAttackNode(bossStateMachine, this)
            });

            // Branch one condition
            var tacticalChasing = new TaskChasingNode(bossStateMachine, this);
            var finishedCombat = new CheckPlayerHealthNode(bossStateMachine, this);
            var bossDead = new CheckBossHealthNode(bossStateMachine, this);

            // Root node
            topNode = new BehaviorSelector(new List<BehaviorNode>
            {
                bossDead,
                finishedCombat,
                changePhaseSequence,
                ultSequence,
                counterAtkSequence,
                atkSequence,
                tacticalChasing
            });
        }

        public void CallDeadPhaseAction()
        {
            if (isCallDeadPhaseAction) return;
            DeadPhaseAction?.Invoke();
            isCallDeadPhaseAction = true;
        }
    }
}