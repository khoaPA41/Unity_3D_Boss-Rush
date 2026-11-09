using DesignPattern.StateMachine.Player.Base;
using Managers;
using UI;
using UnityEngine;

namespace DesignPattern.StateMachine.Player.Main
{
    public class PlayerDeathState : PlayerBaseState
    {
        readonly int DeathAnimationOneHash = Animator.StringToHash("Death_1");
        readonly int DeathAnimationTwoHash = Animator.StringToHash("Death_2");
        readonly string DeathAnimationTag = "Death";

        int randomAnimation;
        bool _hasFinished;
        public PlayerDeathState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {

        }

        public override void Enter()
        {
            _hasFinished = false;

            randomAnimation = Random.Range(0, 2);
            playerStateMachine.Animator.CrossFadeInFixedTime(
                randomAnimation == 0 ? DeathAnimationOneHash : DeathAnimationTwoHash,
                playerStateMachine.AnimationCrossFade);
            playerStateMachine.CharacterController.enabled = false;
        }

        public override void Tick(float deltaTime)
        {
            var normalizeTime = GetNormalizeTime(playerStateMachine.Animator, DeathAnimationTag, 0);

            if (!_hasFinished && normalizeTime >= .5f)
            {
                WorldUIManager.instance.StartCoroutineTitleFade(0, 1, 1f);
                WorldUIManager.instance.StartCoroutineBackgroundFade(0, 1, 1f);
                _hasFinished = true;
            }

            if (_hasFinished && normalizeTime > .99f)
            {
                GameManagers.Instance.ReturnCheckpoint();
            }
        }

        public override void Exit()
        {
        }
    }
}