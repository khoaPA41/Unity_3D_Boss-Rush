using System;
using System.Collections.Generic;
using Script.Design_Pattern.StateMachine.Base;
using UnityEngine;

namespace Script.Design_Pattern.Tree_Behavious
{
    public abstract class BaseBrain: MonoBehaviour
    {
        protected Dictionary<State, List<Transitions>> transitions = new Dictionary<State, List<Transitions>>();

        protected List<Transitions> anyStateTransitions = new List<Transitions>();
        
        protected abstract void SetupTransitions();
        
        protected void AddTransitions(State from, State to, Func<bool> condition)
        {
            if (!transitions.TryGetValue(from, out var currentTransitions))
            {
                currentTransitions = new List<Transitions>();
                transitions.Add(from, currentTransitions);
            }
            currentTransitions.Add(new Transitions(to, condition));
        }

        protected void AddAnyTransitions(State to, Func<bool> condition)
        {
            anyStateTransitions.Add(new Transitions(to, condition));
        }
    }
}
