using System;
using System.Collections.Generic;
using Script.Design_Pattern.StateMachine.Base;
using Script.Design_Pattern.StateMachine.Player.Base;
using Script.Design_Pattern.StateMachine.Player.Main;
using UnityEngine;

namespace Script.Design_Pattern.Tree_Behavious
{
    public class PlayerBehavious : MonoBehaviour
    {
        private PlayerStateMachine _sm;
        // private InputReader inputReader;

        ICombatInput inputHandler;

        private State freeLookState;
        private State attackState;
        private State hitState;
        private State deathState;
        private State jumpState;
        private State landingState;
        private State targetState;
        private State skillState;

        private readonly Dictionary<State, List<Transitions>> transitions = new Dictionary<State, List<Transitions>>();
        private readonly List<Transitions> anyStateTransitions = new List<Transitions>();

        private bool isTarget = false;

        private void Awake()
        {
            inputHandler = GetComponent<ICombatInput>();
            _sm = GetComponent<PlayerStateMachine>();
            freeLookState = new FreeLookState(_sm);
            attackState = new PlayerAttackState(_sm, 0);
            hitState = new PlayerHitState(_sm, false);
            deathState = new PlayerDeathState(_sm);
            jumpState = new PlayerStartJumpState(_sm);
            landingState = new PlayerLandingState(_sm);
            targetState = new PlayerTargetState(_sm);
            skillState = new PlayerUseSkillState(_sm);
            
            _sm.SwitchState(freeLookState);
            SetupTransition();
        }


        private void OnEnable()
        {
            _sm.Health.HitAction += HandleHitEvent;
            _sm.Health.DeathAction += HandleDeathEvent;
            inputHandler.JumpAction += HandleJump;
            inputHandler.TargetAction += HandleTargetState;
            inputHandler.SkillAction += HandleSkillEvent;
        }

        private void OnDisable()
        {
            _sm.Health.HitAction -= HandleHitEvent;
            _sm.Health.DeathAction -= HandleDeathEvent;
            inputHandler.JumpAction -= HandleJump;
            inputHandler.TargetAction -= HandleTargetState;
            inputHandler.SkillAction -= HandleSkillEvent;

        }

        private void Update()
        {
            foreach (var transition in anyStateTransitions)
            {
                if (transition.Condition())
                {
                    _sm.SwitchState(transition.ToState);
                }
            }

            if (transitions.TryGetValue(_sm.currentState, out var curretnListTransition))
            {
                foreach (var transition in curretnListTransition)
                {
                    if (!transition.Condition()) continue;
                    _sm.SwitchState(transition.ToState);
                    break;
                }
            }
        }

        private void SetupTransition()
        {
            void AddTransition(State from, State to, Func<bool> condition)
            {
                if (!transitions.TryGetValue(from, out var curretnListTransition))
                {
                    curretnListTransition = new List<Transitions>();
                    transitions.Add(from, curretnListTransition);
                }

                curretnListTransition.Add(new Transitions(to, condition));
            }

            void AddAnyTransition(State to, Func<bool> condition)
            {
                anyStateTransitions.Add(new Transitions(to, condition));
            }

            AddTransition(freeLookState, attackState, () => inputHandler.IsAttack);
            
            AddTransition(targetState, attackState, () => inputHandler.IsAttack);


            // AddTransition(targetState, () => inputHandler.);
            
            AddAnyTransition(freeLookState, () => _sm.currentState.IsFinished && !isTarget);
            
            AddAnyTransition(targetState, () => _sm.currentState.IsFinished && isTarget);
        }

        private void HandleHitEvent()
        {
            _sm.SwitchState(hitState);
        }

        private void HandleDeathEvent()
        {
            _sm.SwitchState(deathState);
        }

        private void HandleJump()
        {
            if (_sm.currentState == freeLookState || _sm.currentState == targetState) return;
            _sm.SwitchState(jumpState);
        }

        private void HandleTargetState()
        {
            if (!_sm.Targeter.SelectedTarget())
            {
                return;
            }
            
            if (!isTarget)
            {
                isTarget = true;
                _sm.SwitchState(targetState);
            }
            else
            {
                isTarget = false;
                _sm.SwitchState(freeLookState);
            }
        }

        private void HandleSkillEvent(int skillNumber)
        {
            if (_sm.Invincible) {return;}
            
            if (_sm.Mana.currentMana <= 0)
            {
                return;
            }

            _sm.SkillNumber = skillNumber;

            _sm.SwitchState(skillState);
        }
    }
}