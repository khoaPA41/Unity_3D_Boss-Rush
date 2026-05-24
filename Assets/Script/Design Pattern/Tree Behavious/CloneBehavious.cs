using System;
using System.Linq;
using Script.Design_Pattern.Object_Pooling;
using Script.Design_Pattern.StateMachine.PlayerClone.Base;
using Script.Design_Pattern.StateMachine.PlayerClone.Main;
using Script.Design_Pattern.Tree_Behavious.Dependency_Injection;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Rendering;
using State = Script.Design_Pattern.StateMachine.Base.State;

namespace Script.Design_Pattern.Tree_Behavious
{
    public class CloneBehavious : BaseBrain
    {
        [SerializeField] private PooledObject pooledObject;
        [SerializeField] private float timeToRelease = 10f;
        private float countTime;
        private PlayerCloneStateMachine _sm;

        ICombatInput inputHandler;

        private State idleState;
        private State chasingState;
        private State hitState;    
        private State deathState; 
        private State attackState;
        // private State attackState2;
        // private State attackState3;
        // private State attackState4;
        // private State attackState5;
        private void Awake()
        {
            inputHandler = GetComponent<ICombatInput>();
            _sm = GetComponentInParent<PlayerCloneStateMachine>();
            pooledObject = GetComponent<PooledObject>();
            idleState = new PlayerCloneIdleState(_sm);
            chasingState = new PlayerCloneChasingState(_sm);
            attackState = new PlayerCloneAttackState(_sm, 0);
            hitState = new PlayerCloneHitState(_sm);
            // attackState2 = new PlayerCloneAttackState(_sm, 1);
            // attackState3 = new PlayerCloneAttackState(_sm, 2);
            // attackState4 = new PlayerCloneAttackState(_sm, 3);
            // attackState5 = new PlayerCloneAttackState(_sm, 4);

        
            // _sm.SwitchState(idleState);
            SetupTransitions();
        }

        private void Update()
        {
            // if (_sm.currentState == new PlayerCloneAttackState(_sm, 4))
            // {
            //     pooledObject.Release(this.gameObject.name);
            // }
            countTime -= Time.deltaTime;
            if (countTime <= 0)
            {
                pooledObject.Release(this.gameObject.name);
            }
            
            foreach (var transition in anyStateTransitions.Where(transition => transition.Condition()))
            {
                _sm.SwitchState(transition.ToState);
            }
            
            if (_sm.currentState == null) return;
            if (!transitions.TryGetValue(_sm.currentState, out var currentListTransition)) return;
            foreach (var transition in currentListTransition.Where(transition => transition.Condition()))
            {
                _sm.SwitchState(transition.ToState);
            }
        }

        private void OnEnable()
        {
            countTime = timeToRelease;
            _sm.SwitchState(idleState);
            _sm.Health.HitAction += HandleHitEvent;
            _sm.Health.DeathAction += HandleDeathEvent;
        }

        private void OnDisable()
        {
            _sm.Health.HitAction -= HandleHitEvent;
            _sm.Health.DeathAction -= HandleDeathEvent;
        }

        protected override void SetupTransitions()
        {
            AddTransitions(idleState, chasingState, () => idleState.IsFinished);
            
            AddTransitions(chasingState, attackState, () => inputHandler.IsAttack && chasingState.IsFinished);
            
            AddTransitions(idleState, attackState, () => inputHandler.IsAttack && idleState.IsFinished);
            
            // AddAnyTransitions(idleState, () => _sm.currentState != idleState && _sm.currentState.IsFinished);
        }
    
        private void HandleHitEvent()
        {
            _sm.SwitchState(hitState);
        }

        private void HandleDeathEvent()
        {
            _sm.SwitchState(deathState);
        }
    
    }
}
