using System;
using System.Collections.Generic;
using Script.Design_Pattern.StateMachine.Player.Main;
using Script.Design_Pattern.StateMachine.PlayerClone.Base;
using Script.Design_Pattern.StateMachine.PlayerClone.Main;
using Script.Design_Pattern.Tree_Behavious;
using Unity.VisualScripting;
using UnityEngine;
using State = Script.Design_Pattern.StateMachine.Base.State;

public class CloneBehavious : MonoBehaviour
{
    private PlayerCloneStateMachine _sm;
    // private InputReader inputReader;

    ICombatInput inputHandler;

    private State chasingState;
    private State attackState;
    private State hitState;    
    private State deathState; 
    
    private Dictionary<State, List<Transitions>> transitions =  new Dictionary<State, List<Transitions>>();
    
    private List<Transitions> anyStateTransitions = new List<Transitions>();
    
    private void Awake()
    {
        inputHandler = GetComponent<ICombatInput>();
        _sm = GetComponentInParent<PlayerCloneStateMachine>();
        chasingState = new PlayerCloneChasingState(_sm);
        attackState = new PlayerCloneEnterAttack(_sm);
        
        
        SetupTransitions();
    }

    private void OnEnable()
    {
        _sm.SwitchState(chasingState);
        _sm.Health.HitAction += HandleHitEvent;
        _sm.Health.DeathAction += HandleDeathEvent;
    }

    private void OnDisable()
    {
        _sm.Health.HitAction -= HandleHitEvent;
        _sm.Health.DeathAction -= HandleDeathEvent;
    }


    private void SetupTransitions()
    {
        void AddTransition(State from, State to, Func<bool> transition)
        {
            if (!transitions.TryGetValue(from, out var currentTransitions))
            {
                currentTransitions = new List<Transitions>();
                transitions.Add(from, currentTransitions);
            }
            currentTransitions.Add(new Transitions(to, transition));
        }

        void AddAnyTransition(State to, Func<bool> transition)
        {
            anyStateTransitions.Add(new Transitions(to, transition));
        }


        AddTransition(chasingState, attackState, () => inputHandler.IsAttack);
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
