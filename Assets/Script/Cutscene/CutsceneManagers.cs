using System.Collections.Generic;
using Design_Pattern.StateMachine.Player;
using Manager;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class CutsceneManagers : MonoBehaviour
{
    [SerializeField] private GameObject BossContainer;

    private BoxCollider boxCollider;
    private BossPhaseManager bossPhaseManager;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        bossPhaseManager = BossContainer.GetComponent<BossPhaseManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (!other.TryGetComponent<CharacterController>(out _)) return;

        var player = other.GetComponent<PlayerStateMachine>();
        bossPhaseManager.SetPlayer(player);
        player.Stamina.IsCombat = true;


        BossContainer.SetActive(true);
        boxCollider.enabled = false;
        TimelineEvent.Instance.CallTimelineAction();
        AudioManagers.Instance.PlayerBackgroundMusic(true);
    }
}
