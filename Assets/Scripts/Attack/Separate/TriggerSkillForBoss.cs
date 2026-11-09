using System.Collections;
using Attack.SkillFactory;
using DesignPattern.EventBus;
using UnityEngine;

namespace Attack.Separate
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
            StartCoroutine(WaitToActivateTrigger());
        }

        private IEnumerator WaitToActivateTrigger()
        {
            yield return new WaitForSeconds(timeToActiveTrigger);
            damageLogic.SetActive(true);
        }


        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            GameEventManager.Instance.TriggerSkillCasted(Caster, effect);
        }
    }
}