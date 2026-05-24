using System;
using System.Linq;
using Script.Design_Pattern.StateMachine.Base;
using Script.Design_Pattern.StateMachine.Boss.Base;
using Script.Design_Pattern.StateMachine.Boss.Main;
using Script.Design_Pattern.Tree_Behavious;
using Script.Design_Pattern.Tree_Behavious.Dependency_Injection;
using UnityEngine;

public class BossBehavious : BaseBrain
{
    private FinalBossStateMachine _sm;

    ICombatInput inputHandler;

    private State idleState;
    private State chasingState;
    private State hitState;    
    private State deathState; 
    private State attackState;


    private void Awake()
    {
        _sm = GetComponent<FinalBossStateMachine>();
        inputHandler = GetComponent<ICombatInput>();
        idleState = new FinalBossLocomotionState(_sm);
        chasingState = new FinalBossChasingState(_sm, true);
        hitState = new FinalBossHitState(_sm);
        deathState = new FinalBossDeathState(_sm);
        attackState = new FinalBossAttackState(_sm, 0);

        
        _sm.SwitchState(idleState);
        SetupTransitions();
    }


    private void Update()
    {
        foreach (var transition in anyStateTransitions.Where(transition => transition.Condition()))
        {
            _sm.SwitchState(transition.ToState);
        }

        // if (_sm.currentState == null) return;
        if (!transitions.TryGetValue(_sm.currentState, out var currentListTransition)) return;

        foreach (var transition in currentListTransition.Where(transition => transition.Condition()))
        {
            _sm.SwitchState(transition.ToState);
        }
    }
    
    private void OnEnable()
    {
        _sm.Health.HitAction += HandleHitEvent;
        _sm.Health.DeathAction += HandleDeathEvent;
    }
    
    private void OnDisable()
    {
        _sm.Health.HitAction -= HandleHitEvent;
        _sm.Health.DeathAction -= HandleDeathEvent;
    }

    private void HandleDeathEvent()
    {
        _sm.SwitchState(deathState);
    }

    private void HandleHitEvent()
    {
        _sm.SwitchState(hitState);
    }
    
    protected override void SetupTransitions()
    {
        AddTransitions(idleState, chasingState, () =>  inputHandler.IsChasing);
        AddTransitions(chasingState, attackState, () => inputHandler.IsAttack);
        AddTransitions(idleState, attackState, () => inputHandler.IsAttack);

        AddAnyTransitions(idleState, () => _sm.currentState.IsFinished);
    }
}

