using UnityEngine;

public enum BossState
{
    Locomotion,
    Chasing,
    Attack
}

public class BossBrain : MonoBehaviour
{
    [Header("Time to change state")]
    [SerializeField] float timeToChangeIdle = 1f;
    [SerializeField] float timeToChangeChasing = 1f;

    [SerializeField] Transform limitZone;
    [SerializeField] float timeToChangeAttack = 1f;

    Health health;
    FinalBossStateMachine finalBossStateMachine;

    void Start()
    {
        finalBossStateMachine = GetComponent<FinalBossStateMachine>();
        health = GetComponent<Health>();
    }

    void ChangeState()
    {

    }
}
