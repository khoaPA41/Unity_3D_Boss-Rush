using System;
using System.Collections;
using System.Collections.Generic;
using Script.Design_Pattern.Tree_Behavior;
using UnityEngine;
using UnityEngine.Playables;

public class BossPhaseManager : MonoBehaviour
{
    [SerializeField] private List<BossBehaviorBrain> phases;
    private int currentPhaseIndex = 0;

    private bool firstTime;
    private bool isActiveCutscene;

    public void Start()
    {
        GetEventInPhases();
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


    private void CallTimelineAction()
    {
        TimelineEvent.Instance.CallTimelineAction();
    }

    private void HandleDeadPhaseAction()  // Inactive phase is dead and active next phase if has
    {
        if (TimelineEvent.Instance.Timeline.state == PlayState.Paused)
        {
            isActiveCutscene = false;
            return;
        }

        if (isActiveCutscene) return;

        if ((float)TimelineEvent.Instance.Timeline.time / TimelineEvent.Instance.Timeline.duration < .5f) return;

        if (!firstTime)
        {
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
            this.gameObject.SetActive(false);
            return;
        }

        phases[currentPhaseIndex].gameObject.SetActive(true);

    }
}
