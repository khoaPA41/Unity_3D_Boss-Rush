using System.Collections;
using UnityEngine;

public class TriggerActiveTutorials : MonoBehaviour
{
    private readonly int tutorialAppearAnimationHash = Animator.StringToHash("Appear");
    private readonly int tutorialDisappearAnimationHash = Animator.StringToHash("Disappear");

    [Header("Tutorials UI")]
    [SerializeField] private GameObject tutorial_I;

    [Header("Time To Disappear")]
    [SerializeField] private float time;

    private Animator animator;
    private void Start()
    {
        if (SaveManagers.Instance.CurrentSaveData.hasSaveData)
        {
            gameObject.SetActive(false);
        }
        animator = tutorial_I.GetComponent<Animator>();
    }

    private IEnumerator ActiveTutorial()
    {
        animator.SetTrigger(tutorialAppearAnimationHash);
        yield return new WaitForSecondsRealtime(time);
        animator.SetTrigger(tutorialDisappearAnimationHash);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(ActiveTutorial());
        }
    }
}
