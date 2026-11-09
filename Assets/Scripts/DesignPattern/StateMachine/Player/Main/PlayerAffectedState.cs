using Audio;
using DesignPattern.StateMachine.Player.Base;
using System;
using System.Collections;
using Attack.SkillFactory;
using UnityEngine;

namespace DesignPattern.StateMachine.Player.Main
{
    public class PlayerAffectedState : PlayerBaseState
    {
        readonly int AffectedAnimationHash = Animator.StringToHash("Affected");
        readonly string AffectedAnimationTag = "Affected";
        private float previousTime;

        private Transform caster;
        private SkillEffect playerAffected;

        private bool isPlaySound;

        public PlayerAffectedState(PlayerStateMachine playerStateMachine, ICaster caster, SkillEffect skillEffect) : base(
            playerStateMachine)
        {
            this.caster = caster.GetTransform();
            playerAffected = skillEffect;
        }

        public override void Enter()
        {
            ActiveSkillEvent(playerAffected).Invoke();
            playerStateMachine.Animator.CrossFadeInFixedTime(AffectedAnimationHash, playerStateMachine.AnimationCrossFade);
        }

        public override void Tick(float deltaTime)
        {
            if (playerStateMachine.IsAttractiveForce)
            {
                HandleAttractiveForce(deltaTime);
            }

            var normalizedTime = GetNormalizeTime(playerStateMachine.Animator, AffectedAnimationTag, 0);

            if (normalizedTime > previousTime && normalizedTime > .9f)
            {
                playerStateMachine.ReturnLocomotion();
            }

            previousTime = normalizedTime;
        }

        public override void Exit()
        {
            playerStateMachine.InputReader.EnableInput();
            playerStateMachine.IsAttractiveForce = false;
        }

        private Action ActiveSkillEvent(SkillEffect skillEffect)
        {
            return skillEffect switch
            {
                SkillEffect.NonEffect => () => { },
                SkillEffect.Stunned => () => StartEffectCoroutine(1f, StunnedEvent, ResetSpeed),
                SkillEffect.PullBack => () => StartEffectCoroutine(2f, PullBackEvent, ResetInput),
                SkillEffect.PushOut => () => { },
                SkillEffect.AttractiveForce => () => playerStateMachine.IsAttractiveForce = true,
                _ => playerStateMachine.ReturnLocomotion
            };
        }

        private IEnumerator StartTimedAction(float time, Action action1, Action action2)
        {
            action1?.Invoke();
            yield return new WaitForSecondsRealtime(time);
            action2?.Invoke();
        }

        private void StartEffectCoroutine(float time, Action action1, Action action2)
        {
            playerStateMachine.StartCoroutine(StartTimedAction(time, action1, action2));
        }

        private void ResetSpeed()
        {
            playerStateMachine.FreeLookMovementSpeed /= playerStateMachine.MovementSpeedStunnedCoefficient;
            playerStateMachine.FreeLookMovementSprintSpeed /= playerStateMachine.MovementSpeedStunnedCoefficient;
        }

        private void StunnedEvent()
        {
            playerStateMachine.FreeLookMovementSpeed *= playerStateMachine.MovementSpeedStunnedCoefficient;
            playerStateMachine.FreeLookMovementSprintSpeed *= playerStateMachine.MovementSpeedStunnedCoefficient;
        }

        private void PullBackEvent()
        {
            playerStateMachine.InputReader.DisableInput();
        }

        private void ResetInput()
        {
            playerStateMachine.InputReader.EnableInput();
        }

        private void HandleAttractiveForce(float deltaTime)
        {
            if (!isPlaySound)
            {
                AudioManager.Instance.PlaySound(playerStateMachine.transform, AudioManager.Instance.attractiveResource);
            }
            var dir = (playerStateMachine.Boss.transform.position - playerStateMachine.transform.position).normalized;
            Move(dir * 20f, deltaTime);
            isPlaySound = true;
        }
    }
}