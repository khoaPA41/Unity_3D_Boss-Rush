using UnityEngine;

namespace Script.Design_Pattern.StateMachine.Base
{
    public class StateMachine : MonoBehaviour
    {
        private State currentState;

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
