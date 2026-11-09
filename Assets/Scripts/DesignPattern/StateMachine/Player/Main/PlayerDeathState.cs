using DesignPattern.StateMachine.Player.Base;
using Managers;
using UI;
using UnityEngine;

namespace DesignPattern.StateMachine.Player.Main
{
    public class PlayerDeathState : PlayerBaseState
    {
        readonly int _deathAnimationOneHash = Animator.StringToHash("Death_1");
        readonly int _deathAnimationTwoHash = Animator.StringToHash("Death_2");
        readonly string _deathAnimationTag = "Death";

        int _randomAnimation;
        bool _hasFinished;
        public PlayerDeathState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {

        }

        public override void Enter()
        {
            _hasFinished = false;

            _randomAnimation = Random.Range(0, 2);
            playerStateMachine.Animator.CrossFadeInFixedTime(
                _randomAnimation == 0 ? _deathAnimationOneHash : _deathAnimationTwoHash,
                playerStateMachine.AnimationCrossFade);
            playerStateMachine.CharacterController.enabled = false;
        }

        public override void Tick(float deltaTime)
        {
            var normalizeTime = GetNormalizeTime(playerStateMachine.Animator, _deathAnimationTag, 0);

            if (!_hasFinished && normalizeTime >= .5f)
            {
                WorldUIManager.Instance.StartCoroutineTitleFade(0, 1, 1f);
                WorldUIManager.Instance.StartCoroutineBackgroundFade(0, 1, 1f);
                _hasFinished = true;
            }

            if (_hasFinished && normalizeTime > .99f)
            {
                GameManager.Instance.ReturnCheckpoint();
            }
        }

        public override void Exit()
        {
        }
    }
}