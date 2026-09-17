using System;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineEvent : MonoBehaviour
{
    public static TimelineEvent Instance;

    [field: SerializeField] public PlayableDirector Timeline { get; private set; }
    [field: SerializeField] public PlayableDirector EndTimeline { get; private set; }

    public event Action PlayTimelineAction;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        Instance = this;
    }

    private void OnEnable()
    {
        PlayTimelineAction += PlayTimeline;
        // PlayEndTimelineAction += PlayEndTimeline;
    }

    private void OnDisable()
    {
        // PlayEndTimelineAction -= PlayEndTimeline;
        PlayTimelineAction -= PlayTimeline;
    }

    public void CallTimelineAction()
    {
        PlayTimelineAction?.Invoke();
    }

    private void PlayTimeline()
    {
        Time.timeScale = 1;
        Timeline.Play();
    }

    public void PlayEndTimeline()
    {
        Time.timeScale = 1;
        EndTimeline.Play();
    }
}
