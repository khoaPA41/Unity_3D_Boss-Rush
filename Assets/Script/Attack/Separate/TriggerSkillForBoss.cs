using System.Collections;
using Script.Attack.Skill_Factory;
using Script.Design_Pattern.EventBus;
using Script.Design_Pattern.StateMachine.Boss.Base;
using Script.Design_Pattern.StateMachine.Player.Base;
using UnityEngine;

public class TriggerSkillForBoss : MonoBehaviour
{
    [SerializeField] private float timeToActiveTrigger;
    [SerializeField] private SkillEffect effect;
    private BoxCollider _collider;
    public ICaster Caster {get; set;}

    private void Start()
    {
        _collider = GetComponent<BoxCollider>();
        _collider.enabled = false;
    }

    private void OnEnable()
    {
        StartCoroutine(WaitToActiveTrigger());
    }

    private IEnumerator WaitToActiveTrigger()
    {
        yield return new WaitForSeconds(timeToActiveTrigger);
        _collider.enabled = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        GameEventManagers.Instance.TriggerSkillCasted(Caster, effect); 
    }
}
