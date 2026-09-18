using UnityEngine;


namespace Design_Pattern.StateMachine.Player
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
            _previousTime = 0f;
            hexColorEffect = playerStateMachine.IsHealthPotion ? "#C92800" : "#0026C1";
            ChangeColorEffect(hexColorEffect);
            UseMainPotion();
            playerStateMachine.InputReader.UsePotionAction += playerStateMachine.HandleUsePotionState;
            playerStateMachine.Animator.CrossFadeInFixedTime(_usePotionAnimationHash, playerStateMachine.AnimationCrossFade);
        }

        public override void Tick(float deltaTime)
        {
            var normalizeTime = GetNormalizeTime(playerStateMachine.Animator, UsePotionAnimationTag, 0);

            if (normalizeTime > _previousTime && normalizeTime >= .8f)
            {
                playerStateMachine.ReturnLocomotion();
            }

            if (normalizeTime > _previousTime && normalizeTime < .8f)
            {

            }

            _previousTime = normalizeTime;
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

            // playerStateMachine.InputReader.UsePotionAction -= UseMainPotion;

            playerStateMachine.InputReader.UsePotionAction -= playerStateMachine.HandleUsePotionState;

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
            playerStateMachine.HealthPotion.ChangePotion();
            playerStateMachine.Health.RecoveryHealth(playerStateMachine.HealthPotion.PossibleUsage);
            playerStateMachine.PotionLight.SetActive(true);
            playerStateMachine.HealthParticle.Play();
        }

        private void UseManaPotion()
        {
            playerStateMachine.ManaPotion.ChangePotion();
            playerStateMachine.Mana.RecoveryMana(playerStateMachine.ManaPotion.PossibleUsage);
            playerStateMachine.PotionLight.SetActive(true);
            playerStateMachine.ManaParticle.Play();
        }
    }
}