using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Script.Attack
{
    public class Health : MonoBehaviour
    {
        [field: SerializeField] public float maxHealth { get; set; }
        [field: SerializeField] public float resistance { get; set; }
        [field: SerializeField] public float reduceHealthFollowingDuration { get; set; }
        [field: SerializeField] public float reduceHealPrevDuration { get; set; }

        [SerializeField] private float reduceHealth;
        [SerializeField] private float timeFreeze;
        [SerializeField] private float timeToBackNormal;
        
        public float currentHealth;
        public bool isPerfectDodge;
        public bool isReduceDame;
        
        public bool noDamage { get; set; }

        public event Action DeathAction;
        public event Action HitAction;
        public event Action DodgeAwardAction;
        public event Action<float> OnChangeHealth = delegate { };

        private void Awake()
        {
            currentHealth = maxHealth;
            noDamage = false;
        }
        
        public void DealDamage(float damage)
        {
            if(isReduceDame) damage /= reduceHealth;
            
            currentHealth = Mathf.Max(currentHealth - damage + resistance, 0);
            HitAction?.Invoke();

            if (currentHealth <= 0)
            {
                DeathAction?.Invoke();
            }
            OnChangeHealth?.Invoke(currentHealth / maxHealth);
        }

        public void RecoveryHealth(float amount)
        {
            currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
            OnChangeHealth?.Invoke(currentHealth / maxHealth);
        }

        private IEnumerator TimeFreeHit()
        {
            Time.timeScale = timeFreeze;
            yield return new WaitForSecondsRealtime(timeToBackNormal);
            Time.timeScale = 1f;
        }
        
        private IEnumerator SlowTime()
        {
            noDamage = true;
            Time.timeScale = .5f;
            yield return new WaitForSecondsRealtime(.8f);
            Time.timeScale = 1f;
        }
        
        public void PerfectDodgeAward()
        {
            if (!isPerfectDodge) return;
            noDamage = true;
            DodgeAwardAction?.Invoke();
            StartCoroutine(SlowTime());
        }

        public void HitStop()
        {
            StartCoroutine(TimeFreeHit());
        }
        
        public void AddHealth()
        {
            maxHealth += 1;
            currentHealth = maxHealth;
            OnChangeHealth?.Invoke(currentHealth / maxHealth);
        }
        
        public void SubHealth()
        {
            if (maxHealth == 1000) return;
            maxHealth -= 1;
            currentHealth = maxHealth;
            OnChangeHealth?.Invoke(currentHealth / maxHealth);
        }

        public void AddResistance()
        {
            resistance++;
        }
        
        public void SubResistance()
        {
            resistance = Mathf.Max(10, resistance - 1);
        }
    }
}