using UnityEngine;
using UnityEngine.Playables;

namespace Cutscene
{
    public class TimelineEvent : MonoBehaviour
    {
        public static TimelineEvent Instance;

        [field: SerializeField] public PlayableDirector Timeline { get; private set; }
        [field: SerializeField] public PlayableDirector EndTimeline { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }

            Instance = this;
        }

        public void CallTimelineAction()
        {
            PlayTimeline();
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
}
