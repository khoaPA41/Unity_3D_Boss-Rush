using System;
using Script.Attack;
using Script.Design_Pattern.StateMachine.Boss.Base;
using Script.Design_Pattern.StateMachine.Boss.Main;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneManagers : MonoBehaviour
{
    [SerializeField] private GameObject cutsceneObject;
    [SerializeField] private GameObject Boss;
    [SerializeField] private Health Health;

    [SerializeField] private bool isTriggerCutScene;

    private PlayableDirector _playableDirector;
    private bool _isActiveCutscene;
    
    
    
    private void Awake()
    {
        _playableDirector = GetComponent<PlayableDirector>();
    }

    private void OnEnable()
    {
        _playableDirector.stopped += UnActiveCutscene;
        if(!isTriggerCutScene)
        {
            Health.FinalPhaseAction += ActiveCutscene;
        }

    }

    private void OnDisable()
    {
        _playableDirector.stopped -= UnActiveCutscene;
        if(!isTriggerCutScene)
        {
            Health.FinalPhaseAction -= ActiveCutscene;
        }
    }

    private void UnActiveCutscene(PlayableDirector director)
    {
        cutsceneObject.SetActive(false);
        Boss.SetActive(true);
        if (!isTriggerCutScene)
        {
            Boss.GetComponent<FinalBossStateMachine>().SwitchState(new FinalBossEnterPhaseState(Boss.GetComponent<FinalBossStateMachine>(), 2, 0));
        }
    }

    public void ActiveCutscene()
    {
        Time.timeScale = 1;
        Boss.SetActive(false);
        _playableDirector.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isActiveCutscene) return;
        if (other.tag != "Player") return;
        
        _playableDirector.Play();
        _isActiveCutscene = true;
    }
}
