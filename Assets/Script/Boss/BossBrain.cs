using UnityEngine;

public class BossBrain : MonoBehaviour
{
    FinalBossStateMachine finalBossStateMachine;
    void Start()
    {
        finalBossStateMachine = GetComponent<FinalBossStateMachine>();
    }

    void Update()
    {

    }
}
