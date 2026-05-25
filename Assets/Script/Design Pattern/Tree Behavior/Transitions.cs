using System;
using Script.Design_Pattern.StateMachine.Base;

namespace Script.Design_Pattern.Tree_Behavious
{
    public class Transitions
    {
        public Func<bool> Condition { get; }
        
        public State ToState { get; }

        public Transitions(State toState, Func<bool> condition)
        {
            this.ToState = toState;
            this.Condition = condition;
        }
    }
}
