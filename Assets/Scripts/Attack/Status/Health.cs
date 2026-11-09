using Audio;
using System;
using System.Collections;
using DesignPattern.StateMachine.Player.Base;
using UnityEngine;


namespace Attack.Status
{
    public class Health : MonoBehaviour
    {
        [Header("Health Value")]
        [field: SerializeField] public float MaxHealth { get; set; }
        [field: SerializeField] public float resistance { get; set; }
        [field: SerializeField] public float reduceHealthFollowingDuration { get; set; }
        [field: SerializeField] public float reduceHealPrevDuration { get; set; }

        [Header("Health Special Value")]
        [SerializeField] private float _reduceHealth;
        [SerializeField] private float _timeFreeze;
        [SerializeField] private float _timeToBackNormal;

        [field: Header("Hit Sound")]
        [field: SerializeField] public PlayerSFX PlayerSFX { get; private set; }

        public float CurrentHealth;
        public bool isPerfectDodge;
        public bool isReduceDamage;
        private PlayerStateMachine _playerStateMachine;
        private PlayerSFX _playerSFX;
        public bool noDamage { get; set; }

        public event Action DeathAction;
        public event Action HitAction;
        public event Action DodgeAwardAction;
        public event Action<float> OnChangeHealth = delegate { };

        public event Action FinalPhaseAction;

        public event Action EndGameAction;

        private bool _isPhaseII;
        private bool _isEndGame;


        private void Awake()
        {
            if (gameObject.tag != "Boss")
            {
                _playerStateMachine = GetComponent<PlayerStateMachine>();
                _playerSFX = GetComponent<PlayerSFX>();
            }

            CurrentHealth = MaxHealth;
            noDamage = false;
        }

        private void OnEnable()
        {
            OnChangeHealth?.Invoke(CurrentHealth / MaxHealth);
        }


        private void Update()
        {
            if (gameObject.tag != "Boss") return;
            if (CurrentHealth / MaxHealth <= .5f && !_isPhaseII)
            {
                FinalPhaseAction?.Invoke();
                _isPhaseII = true;
            }

            if (CurrentHealth / MaxHealth <= 0f && !_isEndGame)
            {
                EndGameAction?.Invoke();
                _isEndGame = true;
            }
        }

        public void DealDamage(float damage)
        {
            if (isReduceDamage) damage /= _reduceHealth;

            // CurrentHealth = Mathf.Clamp(CurrentHealth - damage + resistance, 0, MaxHealth);
            CurrentHealth = Mathf.Max(0, CurrentHealth - damage + resistance);
            HitAction?.Invoke();

            if (CurrentHealth <= 0)
            {
                DeathAction?.Invoke();
            }
            OnChangeHealth?.Invoke(CurrentHealth / MaxHealth);
        }

        public void RecoveryHealth(float amount)
        {
            CurrentHealth = Mathf.Min(CurrentHealth + amount, MaxHealth);
            OnChangeHealth?.Invoke(CurrentHealth / MaxHealth);
        }

        private IEnumerator TimeFreeHit()
        {
            Time.timeScale = _timeFreeze;

            yield return new WaitForSecondsRealtime(_timeToBackNormal);
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
            if (_playerStateMachine.IsCanNotSubSpiritual)
            {
                _playerStateMachine.IsCanNotSubSpiritual = false;
                return;
            }
            MaxHealth += 1;
            CurrentHealth = MaxHealth;
            OnChangeHealth?.Invoke(CurrentHealth / MaxHealth);
        }

        public void SubHealth()
        {
            if (MaxHealth == 1000)
            {
                _playerStateMachine.IsCanNotAddSpiritual = true;
                return;
            }

            MaxHealth = Mathf.Max(0, MaxHealth - 1); ;
            CurrentHealth = MaxHealth;
            OnChangeHealth?.Invoke(CurrentHealth / MaxHealth);
        }

        public void AddResistance()
        {
            if (_playerStateMachine.IsCanNotSubSpiritual)
            {
                _playerStateMachine.IsCanNotSubSpiritual = false;
                return;
            }
            resistance++;
        }

        public void SubResistance()
        {
            if (resistance == 0)
            {
                _playerStateMachine.IsCanNotAddSpiritual = true;
                return;
            }
            resistance = Mathf.Max(0, resistance - 1);
        }

        public void Reset()
        {
            CurrentHealth = MaxHealth;
            OnChangeHealth?.Invoke(CurrentHealth / MaxHealth);
        }
    }
}