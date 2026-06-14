using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Script.Attack
{
    public class Health : MonoBehaviour
    {
        [field: SerializeField] public int maxHealth { get; private  set; }
        [field: SerializeField] public float reduceHealthFollowingDuration { get; set; }
        [field: SerializeField] public float reduceHealPrevDuration { get; set; }
        [SerializeField] private float timeFreeze;
        [SerializeField] private float timeToBackNormal;
        
        public int currentHealth;
        public bool isPerfectDodge;
        public bool noDamage { get; set; }

        public event Action DeathAction;
        public event Action HitAction;
        
        public event Action<float> OnChangeHealth = delegate { };

        private void Awake()
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
            OnChangeHealth?.Invoke((float)currentHealth / maxHealth);
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
            if (isPerfectDodge)
            {
                noDamage = true;
                Debug.Log("Perfect Dodge");
                StartCoroutine(SlowTime());
            }
        }

        public void HitStop()
        {
            StartCoroutine(TimeFreeHit());
        }
    }
}