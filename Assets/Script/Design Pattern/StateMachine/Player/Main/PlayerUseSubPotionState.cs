
using Script.Design_Pattern.StateMachine.Player.Base;
using UnityEngine;

namespace Script.Design_Pattern.StateMachine.Player.Main
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
            UsePotion(playerStateMachine.SubPotion.currentPotion.potionName);
            playerStateMachine.PotionLight.SetActive(true);
            playerStateMachine.Animator.CrossFadeInFixedTime(_subPotionAnimation, playerStateMachine.AnimationCrossFade);
        }

        public override void Tick(float deltaTime)
        {
            var normalizeTime = GetNormalizeTime(playerStateMachine.Animator, AnimationTag, 0);
            if (normalizeTime > .8f)
            {
                playerStateMachine.ReturnLocomotion();
            }
        }

        public override void PhysicTick(float fixedDeltaTime)
        {
            
        }

        public override void Exit()
        {
            playerStateMachine.PotionLight.SetActive(false);
        }

        private void UsePotion(string potionName)
        {
            playerStateMachine.SubPotion.SubtractPotion(potionName, 1);
            switch (potionName)
            {
                case "ReduceStamina":
                    playerStateMachine.SubPotion.ReduceStamina("ReduceStamina");
                    return;
                case "IncreaseDame":
                    playerStateMachine.SubPotion.IncreaseDame("IncreaseDame");
                    return;
                case "ReduceDame":
                    playerStateMachine.SubPotion.ReduceDame("ReduceDame");
                    return;
            }
        }
    }
}
