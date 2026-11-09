using Audio;
using Cutscene;
using System;
using System.Collections;
using System.Collections.Generic;

using DesignPattern.StateMachine.Player.Base;
using DesignPattern.TreeBehavior.Brain;

using TMPro;
using UnityEngine;
using UnityEngine.Playables;

namespace Managers
{
    public class BossPhaseManager : MonoBehaviour
    {
        public string TriggerId;
        [SerializeField] private List<BossBehaviorBrain> phases;
        [SerializeField] private List<ParticleSystem> lockGateParticle;
        [SerializeField] private GameObject healthUi;
        [SerializeField] private bool isFinalBoss;
        [SerializeField] private int spiritualPower;

        public event Action Finished = delegate { };

        public PlayerStateMachine Player { get; set; }
        private int currentPhaseIndex = 0;

        private bool firstTime;
        private bool isActiveCutscene;

        public void Start()
        {
            GetEventInPhases();
        }

        private void OnEnable()
        {
            healthUi.SetActive(true);
        }

        private void Update()
        {
            HandleDeadPhaseAction();
        }

        private void GetEventInPhases()
        {
            foreach (var phase in phases)
            {
                phase.DeadPhaseAction += CallTimelineAction;
                phase.gameObject.SetActive(false);
            }
        }

        public void SetPlayer(PlayerStateMachine player)
        {
            Player = player;
        }

        private void CallTimelineAction()
        {
            TimelineManager.Instance.CallTimelineAction();
        }

        private void HandleDeadPhaseAction()  // Inactive phase is dead and active next phase if has
        {
            if (TimelineManager.Instance.Timeline.state == PlayState.Paused)
            {
                isActiveCutscene = false;
                return;
            }

            if (isActiveCutscene) return;

            if ((float)TimelineManager.Instance.Timeline.time / TimelineManager.Instance.Timeline.duration < .5f) return;

            if (!firstTime)
            {
                ActivateLockGateParticle();
                isActiveCutscene = true;
                phases[currentPhaseIndex].gameObject.SetActive(true);
                firstTime = true;
                return;
            }

            isActiveCutscene = true;
            phases[currentPhaseIndex].DeadPhaseAction -= CallTimelineAction;
            phases[currentPhaseIndex].gameObject.SetActive(false);

            currentPhaseIndex++;

            if (currentPhaseIndex == phases.Count)
            {
                Finished?.Invoke();
                healthUi.SetActive(false); // Off Boss health
                Player.Stamina.IsCombat = false;
                AudioManager.Instance.StopBackgroundMusic();
                this.gameObject.SetActive(false);

                Player.AddBossSpiritualPower(spiritualPower); // add money
                if (!isFinalBoss) return;
                TimelineManager.Instance.PlayEndTimeline();
                return;
            }

            phases[currentPhaseIndex].gameObject.SetActive(true);
        }

        private void ActivateLockGateParticle()
        {
            foreach (var particle in lockGateParticle)
            {
                particle.gameObject.SetActive(true);
                particle.Play();
            }
        }

        public void DeactivateLockGateParticle()
        {
            foreach (var particle in lockGateParticle)
            {
                particle.Stop();
                particle.gameObject.SetActive(false);
            }
        }
    }
}