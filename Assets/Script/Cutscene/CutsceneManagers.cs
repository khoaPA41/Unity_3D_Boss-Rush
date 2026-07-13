using System;
using Script.Attack;
using Script.Design_Pattern.StateMachine.Boss.Base;
using Script.Design_Pattern.StateMachine.Boss.Main;
using Script.Design_Pattern.StateMachine.Player.Base;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class CutsceneManagers : MonoBehaviour
{
    [SerializeField] private GameObject cutsceneObject;
    [SerializeField] private GameObject Boss;
    [SerializeField] private Health Health;
    [SerializeField] private bool isTriggerCutScene;
    [SerializeField] private bool isEndGame;

    [SerializeField] private float holdTimeLimit;
    [SerializeField] private float timeToSkip;
    [SerializeField] private InputActionReference skipAction;
    private PlayableDirector _playableDirector;
    private PlayerStateMachine _playerStateMachine;
    private bool _isActiveCutscene;
    private bool _isSkip;
    private float _holdTime;
    private void Awake()
    {
        _playableDirector = GetComponent<PlayableDirector>();
        _playerStateMachine = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStateMachine>();
    }

    private void OnEnable()
    {
        _playableDirector.stopped += UnActiveCutscene;
        if(!isTriggerCutScene)
        {
            if (isEndGame)
            {
                Health.EndGameAction += ActiveEndGameScene;
            }
            else
            {
                Health.FinalPhaseAction += ActiveCutscene;
            }
            
        }

        if (skipAction != null && skipAction.action != null)
        {
            skipAction.action.performed += OnSkipActionPerformed;
            skipAction.action.canceled += OnCancelSkipActionPerformed;
            skipAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        _playableDirector.stopped -= UnActiveCutscene;
        if(!isTriggerCutScene)
        {
            if (isEndGame)
            {
                Health.EndGameAction -= ActiveEndGameScene;
            }
            else
            {
                Health.FinalPhaseAction -= ActiveCutscene;
            }
        }
        
        if (skipAction != null && skipAction.action != null)
        {
            skipAction.action.performed -= OnSkipActionPerformed;
            skipAction.action.canceled -= OnCancelSkipActionPerformed;
        }
    }

    private void Update()
    {
        if (!_isSkip) return;
        Debug.Log(_holdTime);
        HoldToSkip();
    }
    
    private void OnSkipActionPerformed(InputAction.CallbackContext ctx)
    {
        if(!_isActiveCutscene) return;
        _isSkip = true;
    }
    
    private void OnCancelSkipActionPerformed(InputAction.CallbackContext ctx)
    {
        if(!_isActiveCutscene) return;
        _isSkip = false;
    }

    private void UnActiveCutscene(PlayableDirector director)
    {
        if (isEndGame)
        {
            _playerStateMachine.InputReader.SetCursor(false);
            GameManagers.Instance.ReturnTitle();
            return;
        }
        
        cutsceneObject.SetActive(false);
        Boss.SetActive(true);
        if (!isTriggerCutScene)
        {
            Boss.GetComponent<FinalBossStateMachine>().SwitchState(new FinalBossEnterPhaseState(Boss.GetComponent<FinalBossStateMachine>(), 2, 0));
        }
    }

    private void ActiveCutscene()
    {
        if(isTriggerCutScene) return;
        
        Time.timeScale = 1;
        Boss.SetActive(false);
        _playableDirector.Play();
        _isActiveCutscene = true;
        AudioManagers.Instance.PlayerBackgroundMusic(false);
    }

    private void ActiveEndGameScene()
    {
        if(isTriggerCutScene) return;
        
        Time.timeScale = 1;
        _playableDirector.Play();
        _isActiveCutscene = true;
        GameManagers.Instance.AutoSave();
    }

    private void HoldToSkip()
    {
        if (skipAction is null || !_isActiveCutscene) return;

        _holdTime += Time.deltaTime;
        _holdTime = Mathf.Clamp(_holdTime, 0f, holdTimeLimit);
        
        if (_holdTime >= holdTimeLimit)
        {
            _playableDirector.time = timeToSkip;
            _playableDirector.Evaluate();   
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isActiveCutscene) return;
        if (other.tag != "Player") return;
        
        _playableDirector.Play();
        _isActiveCutscene = true;
        AudioManagers.Instance.PlayerBackgroundMusic(true);
    }
}
