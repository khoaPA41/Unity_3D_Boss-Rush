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
    [SerializeField] float timeToChangeAttack = 1f;

    FinalBossStateMachine finalBossStateMachine;

    void Start()
    {
        finalBossStateMachine = GetComponent<FinalBossStateMachine>();
    }

    void ChangeState()
    {

    }
}
