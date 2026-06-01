using System.Collections;
using Script.Attack.Skill_Factory;
using Script.Design_Pattern.EventBus;
using UnityEngine;

public class TriggerSkillForBoss : MonoBehaviour
{
    [SerializeField] private float timeToActiveTrigger;
    [SerializeField] private SkillEffect effect;
    private BoxCollider _collider;
    // public GameObject Caster {get; set;}
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
        Debug.Log(other.name);
        // Caster.TryGetComponent(out ICaster caster);
        GameEventManagers.TriggerSkillCasted(Caster, effect);
        SkillSituationEvent.Instance.SendNextActionEvent();
        
    }
}
