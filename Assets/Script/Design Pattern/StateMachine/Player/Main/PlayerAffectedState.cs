using System;
using System.Collections;
using System.Collections.Generic;
using Script.Attack.Skill_Factory;
using Script.Design_Pattern.StateMachine.Player.Base;
using UnityEngine;

public class PlayerAffectedState : PlayerBaseState
{
    readonly int AffectedAnimationHash = Animator.StringToHash("Affected");
    readonly string AffectedAnimationTag = "Affected";
    private float previousTime;

    private Transform caster;
    private SkillEffect playerEffected;

    public PlayerAffectedState(PlayerStateMachine playerStateMachine, ICaster caster, SkillEffect skillEffect) : base(
        playerStateMachine)
    {
        this.caster = caster.GetTransform();
        playerEffected = skillEffect;
    }

    public override void Enter()
    {
        Debug.Log("Enter: " + playerStateMachine.IsAttractiveForce );
        ActiveSkillEvent(playerEffected).Invoke();
        playerStateMachine.Animator.CrossFadeInFixedTime(AffectedAnimationHash, playerStateMachine.AnimationCrossFade);
    }

    public override void Tick(float deltaTime)
    {
        Debug.Log("Tick: " + playerStateMachine.IsAttractiveForce );
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

    public override void PhysicTick(float fixedDeltaTime)
    {
    }

    public override void Exit()
    {
        playerStateMachine.InputReader.OnEnableInput();
        playerStateMachine.IsAttractiveForce = false;
    }

    private Action ActiveSkillEvent(SkillEffect skillEffect)
    {
        return skillEffect switch
        {
            SkillEffect.NonEffect => () => { Debug.Log("NonEffect"); },
            SkillEffect.Stunned => () => StartEffectCoroutine(1f, StunnedEvent, ResetSpeed),
            SkillEffect.PullBack => () => StartEffectCoroutine(2f, PullBackEvent, ResetInput),
            SkillEffect.PushOut => () => { Debug.Log("PushOut"); },
            SkillEffect.AttractiveForce => () => playerStateMachine.IsAttractiveForce = true,
            _ => playerStateMachine.ReturnLocomotion
        };
    }

    private IEnumerator Coroutine(float time, Action action1, Action action2)
    {
        action1?.Invoke();
        yield return new WaitForSecondsRealtime(time);
        action2?.Invoke();
    }

    private void StartEffectCoroutine(float time, Action action1, Action action2)
    {
        playerStateMachine.StartCoroutine(Coroutine(time, action1, action2));
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
        playerStateMachine.InputReader.OnEnableInput();
    }

    private void HandleAttractiveForce(float deltaTime)
    {
        var dir = (playerStateMachine.Boss.transform.position - playerStateMachine.transform.position).normalized;
        Move(dir * 20f, deltaTime);
    }
}