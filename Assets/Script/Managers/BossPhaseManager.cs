using System;
using System.Collections;
using System.Collections.Generic;
using Script.Design_Pattern.Tree_Behavior;
using UnityEngine;

public class BossPhaseManager : MonoBehaviour
{
    [SerializeField] private List<BossBehaviorBrain> phases;

    private int currentPhaseIndex = 0;

    public void Start()
    {
        GetEventInPhases();
        phases[currentPhaseIndex].gameObject.SetActive(true);
    }


    private void GetEventInPhases()
    {
        foreach (var phase in phases)
        {
            phase.DeadPhaseAction += HandleDeadPhaseAction;
            phase.gameObject.SetActive(false);
        }
    }

    // Inactive phase is dead and active next phase if has
    private void HandleDeadPhaseAction()
    {
        phases[currentPhaseIndex].gameObject.SetActive(false);
        currentPhaseIndex++;

        if (currentPhaseIndex == phases.Count)
        {
            return;
        }

        phases[currentPhaseIndex].gameObject.SetActive(true);
    }
}
