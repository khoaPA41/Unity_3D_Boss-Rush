using Attack.Status.Potion;
using DesignPattern.StateMachine.Player.Base;

using UnityEngine;

namespace DesignPattern.StateMachine.Player.Main
{
    public class PlayerUseSubPotionState : PlayerBaseState
    {
        private readonly int _subPotionAnimation = Animator.StringToHash("UseSubPotion");
        private const string AnimationTag = "UseSubPotion";

        public PlayerUseSubPotionState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
        }

        public override void Enter()
        {
            ChangeColorEffect(playerStateMachine.SubPotion.currentPotion.hexColor);
            UsePotion(playerStateMachine.SubPotion.currentPotion.subPotionId);
            playerStateMachine.PotionLight.SetActive(true);
            playerStateMachine.Animator.CrossFadeInFixedTime(_subPotionAnimation, playerStateMachine.AnimationCrossFade);
        }

        public override void Tick(float deltaTime)
        {
            var normalizeTime = GetNormalizeTime(playerStateMachine.Animator, AnimationTag, 0);
            if (normalizeTime > .8f)
            {
                playerStateMachine.ReturnLocomotion();
                return;
            }
        }

        public override void Exit()
        {
            playerStateMachine.PotionLight.SetActive(false);
        }

        private void UsePotion(SubPotionId subPotionId)
        {
            playerStateMachine.SubPotion.SubtractPotion(subPotionId, 1);
            playerStateMachine.SubPotion.ApplyTheEffect(subPotionId);
        }
    }
}
