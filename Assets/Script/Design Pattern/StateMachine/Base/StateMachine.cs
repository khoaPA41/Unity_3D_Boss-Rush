using UnityEngine;

namespace Script.Design_Pattern.StateMachine.Base
{
    public class StateMachine : MonoBehaviour
    {
        public State currentState { get; set; }

        public void SwitchState(State newState)
        {
            currentState?.Exit();
            currentState = newState;
            currentState?.Enter();
        }


        private void Update()
        {
            currentState?.Tick(Time.deltaTime);
        }
        

        private void FixedUpdate()
        {
            currentState?.PhysicTick(Time.fixedDeltaTime);
        }
    }
}
