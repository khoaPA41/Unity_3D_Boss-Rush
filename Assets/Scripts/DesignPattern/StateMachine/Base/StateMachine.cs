using UnityEngine;

namespace DesignPattern.StateMachine.Base
{
    public class StateMachine : MonoBehaviour
    {
        public State CurrentState { get; private set; }

        public void SwitchState(State newState)
        {
            CurrentState?.Exit();
            CurrentState = newState;
            CurrentState?.Enter();
        }


        protected void Update()
        {
            CurrentState?.Tick(Time.deltaTime);
        }
    }
}
