using System.Collections;
using Attack.Skill_Factory;
using Design_Pattern.EventBus;
using UnityEngine;

namespace Attack
{
    public class TriggerSkillForBoss : MonoBehaviour
    {
        [SerializeField] private float timeToActiveTrigger;
        [SerializeField] private SkillEffect effect;
        [SerializeField] private GameObject damageLogic;
        public ICaster Caster { get; set; }

        private void OnEnable()
        {
            damageLogic.SetActive(false);
            StartCoroutine(WaitToActiveTrigger());
        }

        private IEnumerator WaitToActiveTrigger()
        {
            yield return new WaitForSeconds(timeToActiveTrigger);
            damageLogic.SetActive(true);
        }


        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            GameEventManagers.Instance.TriggerSkillCasted(Caster, effect);
        }
    }
}