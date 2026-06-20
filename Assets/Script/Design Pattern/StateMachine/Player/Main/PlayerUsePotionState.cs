using Script.Design_Pattern.StateMachine.Player.Base;
using UnityEngine;


namespace Script.Design_Pattern.StateMachine.Player.Main
{
    public class PlayerUsePotionState : PlayerBaseState
    {
        private readonly int _usePotionAnimationHash = Animator.StringToHash("UseMainPotion");
        private const string UsePotionAnimationTag = "UseMainPotion";
        private float _previousTime;
        private string hexColorEffect;
        public PlayerUsePotionState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
        }

        public override void Enter()
        {
            hexColorEffect = playerStateMachine.IsHealthPotion ? "#C92800" : "#0026C1";
            ChangeColorEffect(hexColorEffect);
            UseMainPotion();
            
            playerStateMachine.Animator.CrossFadeInFixedTime(_usePotionAnimationHash,
                playerStateMachine.AnimationCrossFade);
        }

        public override void Tick(float deltaTime)
        {
            var normalizeTime = GetNormalizeTime(playerStateMachine.Animator, UsePotionAnimationTag, 0);

            if (normalizeTime >= .8f)
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
            playerStateMachine.HealthParticle.Stop();

            playerStateMachine.PotionLight.SetActive(false);
            playerStateMachine.ManaParticle.Stop();
        }


        private void UseMainPotion()
        {
            if (playerStateMachine.IsHealthPotion)
            {
                UseHealthPotion();
            }
            else
            {
                UseManaPotion();
            }
        }

        private void UseHealthPotion()
        {
            playerStateMachine.Health.RecoveryHealth(playerStateMachine.HealthPotion.potionUsage);
            playerStateMachine.HealthPotion.ChangePotion(playerStateMachine.HealthPotion.potionUsage);
            playerStateMachine.PotionLight.SetActive(true);
            playerStateMachine.HealthParticle.Play();
        }

        private void UseManaPotion()
        {
            playerStateMachine.Mana.RecoveryMana(playerStateMachine.ManaPotion.potionUsage);
            playerStateMachine.ManaPotion.ChangePotion(playerStateMachine.ManaPotion.potionUsage);
            playerStateMachine.PotionLight.SetActive(true);
            playerStateMachine.ManaParticle.Play();
        }
    }
}