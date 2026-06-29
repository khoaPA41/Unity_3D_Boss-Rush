using System;
using System.Collections;
using Script.Design_Pattern.StateMachine.Player.Base;
using UnityEngine;
using UnityEngine.UI;

namespace Script.Attack
{
    public class Health : MonoBehaviour
    {
        [Header("Health Value")]
        [field: SerializeField] public float maxHealth { get; set; }
        [field: SerializeField] public float resistance { get; set; }
        [field: SerializeField] public float reduceHealthFollowingDuration { get; set; }
        [field: SerializeField] public float reduceHealPrevDuration { get; set; }

        [Header("Health Special Value")]
        [SerializeField] private float reduceHealth;
        [SerializeField] private float timeFreeze;
        [SerializeField] private float timeToBackNormal;
        
        
        public float currentHealth;
        public bool isPerfectDodge;
        public bool isReduceDame;
        private PlayerStateMachine _playerStateMachine;
        private PlayerSFX _playerSFX;
        public bool noDamage { get; set; }

        public event Action DeathAction;
        public event Action HitAction;
        public event Action DodgeAwardAction;
        public event Action<float> OnChangeHealth = delegate { };

        public event Action FinalPhaseAction;

        private void Awake()
        {
            if (gameObject.tag != "Boss")
            {
                _playerStateMachine = GetComponent<PlayerStateMachine>();
                _playerSFX = GetComponent<PlayerSFX>();
            }

            currentHealth = maxHealth;
            noDamage = false;
        }


        private void Update()
        {
            if (gameObject.tag != "Boss") return;
            if (currentHealth / maxHealth <= .5f)
            {
                FinalPhaseAction?.Invoke();
            }

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
            _playerSFX.PlayPerfectDodgeSound();
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
            if (_playerStateMachine.isCanNotSubSpiritual || _playerStateMachine.PlayerSpiritualPower <= 0)
            { 
                _playerStateMachine.isCanNotSubSpiritual = false;
                return;
            }
            maxHealth += 1;
            currentHealth = maxHealth;
            OnChangeHealth?.Invoke(currentHealth / maxHealth);
        }
        
        public void SubHealth()
        {
            if (maxHealth == 1000)
            {
                _playerStateMachine.isCanNotAddSpiritual = true;
                return;
            }
            maxHealth -= 1;
            currentHealth = maxHealth;
            OnChangeHealth?.Invoke(currentHealth / maxHealth);
        }

        public void AddResistance()
        {
            if (_playerStateMachine.isCanNotSubSpiritual || _playerStateMachine.PlayerSpiritualPower <= 0)
            { 
                _playerStateMachine.isCanNotSubSpiritual = false;
                return;
            }
            resistance++;
        }
        
        public void SubResistance()
        {
            if (resistance == 0)
            {
                _playerStateMachine.isCanNotAddSpiritual = true;
                return;
            }
            resistance = Mathf.Max(0, resistance - 1);
        }
    }
}