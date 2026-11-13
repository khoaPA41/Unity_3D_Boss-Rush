using UnityEngine;
namespace DesignPattern.StateMachine.Base
{
    public class SafeStateMachine : MonoBehaviour
    {
        public State CurrentState { get; set; }

        private int _lastTransitionFrame = -1;
        private bool _isInEnterOrExit = false;


        public void SwitchState(State newState)
        {
            if (_isInEnterOrExit)
            {
                Debug.LogError("Reentrancy detected: SwitchState called inside Enter/Exit!");
                return;
            }

            if (Time.frameCount == _lastTransitionFrame)
            {
                Debug.LogError($"Double transition in frame {Time.frameCount}!");
                return;
            }

            if (newState == null) return;

            if (CurrentState == newState) return;

            if (CurrentState != null)
            {
                _isInEnterOrExit = true;
                try
                {
                    CurrentState?.Exit();
                }
                finally
                {
                    _isInEnterOrExit = false;
                }
            }

            _lastTransitionFrame = Time.frameCount;
            CurrentState = newState;
            _isInEnterOrExit = true;
            try
            {
                CurrentState.Enter();
            }
            finally
            {

                _isInEnterOrExit = false;
            }
        }

        private void Update()
        {
            CurrentState?.Tick(Time.deltaTime);
        }

    }
}