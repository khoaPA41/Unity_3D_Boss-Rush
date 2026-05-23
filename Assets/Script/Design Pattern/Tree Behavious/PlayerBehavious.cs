using System;
using System.Collections.Generic;
using System.Linq;
using Script.Design_Pattern.StateMachine.Base;
using Script.Design_Pattern.StateMachine.Player.Base;
using Script.Design_Pattern.StateMachine.Player.Main;
using Script.Design_Pattern.Tree_Behavious.Dependency_Injection;
using UnityEngine;

namespace Script.Design_Pattern.Tree_Behavious
{
    public class PlayerBehavious : BaseBrain
    {
        private PlayerStateMachine _sm;
        ICombatInput inputHandler;

        private State freeLookState;
        private State attackState;
        private State hitState;
        private State deathState;
        private State jumpState;
        private State landingState;
        private State targetState;
        private State skillState;
        private State dodgeState;


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
            dodgeState = new PlayerDodgeState(_sm);

            _sm.SwitchState(freeLookState);
            SetupTransitions();
        }


        private void OnEnable()
        {
            _sm.Health.HitAction += HandleHitEvent;
            _sm.Health.DeathAction += HandleDeathEvent;
            inputHandler.JumpAction += HandleJump;
            inputHandler.TargetAction += HandleTargetState;
            inputHandler.SkillAction += HandleSkillEvent;
            inputHandler.DodgeAction += HandleDodgeEvent;
        }

        private void OnDisable()
        {
            _sm.Health.HitAction -= HandleHitEvent;
            _sm.Health.DeathAction -= HandleDeathEvent;
            inputHandler.JumpAction -= HandleJump;
            inputHandler.TargetAction -= HandleTargetState;
            inputHandler.SkillAction -= HandleSkillEvent;
            inputHandler.DodgeAction -= HandleDodgeEvent;
        }

        private void Update()
        {
            foreach (var transition in anyStateTransitions.Where(transition => transition.Condition()))
            {
                _sm.SwitchState(transition.ToState);
            }

            // if (_sm.currentState == null) return;
            if (!transitions.TryGetValue(_sm.currentState, out var currentListTransition)) return;

            foreach (var transition in currentListTransition.Where(transition => transition.Condition()))
            {
                _sm.SwitchState(transition.ToState);
            }
        }

        protected override void SetupTransitions()
        {
            AddTransitions(freeLookState, attackState, () => inputHandler.IsAttack);
            AddTransitions(targetState, attackState, () => inputHandler.IsAttack);
            /*******************************************************************************************/
            AddAnyTransitions(freeLookState, () => _sm.currentState.IsFinished && !isTarget);
            AddAnyTransitions(targetState, () => _sm.currentState.IsFinished && isTarget);
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
            if (_sm.currentState != freeLookState && _sm.currentState != targetState) return;
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
            if (_sm.Invincible)
            {
                return;
            }

            if (_sm.Mana.currentMana <= 0)
            {
                return;
            }

            _sm.SkillNumber = skillNumber;

            _sm.SwitchState(skillState);
        }

        private void HandleDodgeEvent()
        {
            if (_sm.currentState != freeLookState && _sm.currentState != targetState) return;
            _sm.SwitchState(dodgeState);
        }
    }
}