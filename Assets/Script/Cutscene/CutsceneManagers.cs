using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class CutsceneManagers : MonoBehaviour
{
    [SerializeField] private GameObject BossContainer;


    private BoxCollider boxCollider;


    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (!other.TryGetComponent<CharacterController>(out _)) return;

        BossContainer.SetActive(true);
        boxCollider.enabled = false;
        TimelineEvent.Instance.CallTimelineAction();
        AudioManagers.Instance.PlayerBackgroundMusic(true);
    }
}
