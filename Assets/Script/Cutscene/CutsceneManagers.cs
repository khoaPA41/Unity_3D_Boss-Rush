using System;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneManagers : MonoBehaviour
{
    [SerializeField] private GameObject cutsceneObject;
    private PlayableDirector _playableDirector;
    private BoxCollider _boxCollider;
    private bool _isActiveCutscene;
    private void Awake()
    {
        _playableDirector = GetComponent<PlayableDirector>();
        _boxCollider =  GetComponent<BoxCollider>();
    }

    private void OnEnable()
    {
        _playableDirector.stopped += UnActiveCutscene;
    }

    private void OnDisable()
    {
        _playableDirector.stopped -= UnActiveCutscene;
    }

    private void UnActiveCutscene(PlayableDirector director)
    {
        cutsceneObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isActiveCutscene) return;
        if (other.tag != "Player") return;
        
        _playableDirector.Play();
        _isActiveCutscene = true;
    }
}
