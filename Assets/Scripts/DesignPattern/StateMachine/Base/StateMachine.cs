using UnityEngine;

namespace DesignPattern.StateMachine.Base
{
    public class StateMachine : MonoBehaviour
    {
        public State currentState { get; private set; }

        public void SwitchState(State newState)
        {
            currentState?.Exit();
            currentState = newState;
            currentState?.Enter();
        }


        protected void Update()
        {
            currentState?.Tick(Time.deltaTime);
        }
    }
}
