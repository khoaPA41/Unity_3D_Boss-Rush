using System;
using System.Collections;
using UnityEngine;

namespace Script.Attack
{
    public class Health : MonoBehaviour
    {
        [field: SerializeField] public int maxHealth { get; private  set; }
        [SerializeField] private float timeFreeze;
        [SerializeField] private float timeToBackNormal;

        public int currentHealth;

        public bool noDamage { get; set; }

        public event Action DeathAction;
        public event Action HitAction;

        private void Start()
        {
            currentHealth = maxHealth;
            noDamage = false;
        }

        public void DealDamage(int damage)
        {
            currentHealth = Mathf.Max(currentHealth - damage, 0);
            HitAction?.Invoke();

            if (currentHealth <= 0)
            {
                DeathAction?.Invoke();
            }
        }

        private IEnumerator TimeFreeHit()
        {
            Time.timeScale = timeFreeze;
            yield return new WaitForSecondsRealtime(timeToBackNormal);
            Time.timeScale = 1f;
        }

        public void HitStop()
        {
            StartCoroutine(TimeFreeHit());
        }
    }
}