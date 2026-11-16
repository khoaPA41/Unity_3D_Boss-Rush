using UnityEngine;
namespace DesignPattern.StateMachine.Base
{
    public class SafeStateMachine : MonoBehaviour
    {
        private State _currentState { get; set; }
        private State _pendingState { get; set; }

        private int _lastTransitionFrame = -1;
        private bool _isInEnterOrExit = false;


        public void SwitchState(State newState)
        {
            if (newState == null) return;
            if (_currentState == newState) return;

            if (_isInEnterOrExit || Time.frameCount == _lastTransitionFrame)
            {

                string _currentStateName = _currentState?.GetType().Name ?? "Null";
                string newStateName = newState.GetType().Name;
                string pendingStateName = _pendingState?.GetType().Name ?? "Null";

                string reason = _isInEnterOrExit ? "Reentrancy" : "Double transition";

                Debug.LogWarning(
                    $"[StateMachine Warning]\n" +
                    $"Reason: {reason}\n" +
                    $"FrameTime: {Time.frameCount}\n" +
                    $"Current State: {_currentStateName}\n" +
                    $"Next State: {newStateName}\n" +
                    $"Pending State: {pendingStateName}\n" +
                    $"Call Stack: {StackTraceUtility.ExtractStackTrace()}"
                    );

                _pendingState = newState;
                return;
            }

            if (_currentState != null)
            {
                _isInEnterOrExit = true;
                try
                {
                    _currentState?.Exit();
                }
                finally
                {
                    _isInEnterOrExit = false;
                }
            }

            _lastTransitionFrame = Time.frameCount;
            _currentState = newState;
            _isInEnterOrExit = true;
            try
            {
                _currentState.Enter();
            }
            finally
            {
                _isInEnterOrExit = false;
            }
        }

        private void Update()
        {
            ProcessPendingState();
            _currentState?.Tick(Time.deltaTime);
        }

        protected void ProcessPendingState()
        {
            if (_pendingState == null || _isInEnterOrExit || _lastTransitionFrame == Time.frameCount) return;

            State nextState = _pendingState;
            _pendingState = null;

            SwitchState(nextState);
        }
    }
}