using System;
using DesignPattern.StateMachine.Player.Base;
using UnityEngine;

namespace Attack.Status
{
    [RequireComponent(typeof(PlayerStateMachine))]
    public class Stamina : MonoBehaviour
    {
        [field: SerializeField] public float MaxStamina { get; set; }
        [field: SerializeField] public float DodgeReduce { get; set; }
        [field: SerializeField] public float walkReduce { get; set; }
        [field: SerializeField] public float runReduce { get; set; }
        [field: SerializeField] public float lightAttackReduce { get; set; }
        [field: SerializeField] public float heavyAttackReduce { get; set; }
        [field: SerializeField] public float jumpReduce { get; set; }
        [SerializeField] private float _reduceStamina;
        [SerializeField] private float _recoveryStamina;

        public float CurrentStamina { get; set; }
        public bool IsCombat;
        public event Action<float> OnChangeStamina = delegate { };
        public event Action<float> OnRecoveryStamina = delegate { };

        public bool isReduceStamina { get; set; } // If use potion

        private PlayerStateMachine _playerStateMachine;

        private void Awake()
        {
            _playerStateMachine = GetComponent<PlayerStateMachine>();
            CurrentStamina = MaxStamina;
        }

        public void ChangeStamina(float amount)
        {
            if (!IsCombat) return;
            if (isReduceStamina) amount /= _reduceStamina;

            CurrentStamina -= amount * Time.deltaTime;
            CurrentStamina = Mathf.Clamp(CurrentStamina, 0, MaxStamina);

            OnChangeStamina?.Invoke(CurrentStamina / MaxStamina);
        }

        public void RecoveryStamina()
        {
            if (CurrentStamina < MaxStamina)
            {
                CurrentStamina += _recoveryStamina * Time.deltaTime;
                CurrentStamina = Mathf.Clamp(CurrentStamina, 0, MaxStamina);
            }

            OnRecoveryStamina?.Invoke(CurrentStamina / MaxStamina);
        }

        public void DodgeAwardStamina()
        {
            var staminaLost = MaxStamina - CurrentStamina;
            CurrentStamina += staminaLost * .5f;
            OnRecoveryStamina?.Invoke(CurrentStamina / MaxStamina);
        }

        public void AddStamina()
        {
            if (_playerStateMachine.IsCanNotSubSpiritual)
            {
                _playerStateMachine.IsCanNotSubSpiritual = false;
                return;
            }

            MaxStamina += 1;
            CurrentStamina = MaxStamina;
            OnChangeStamina?.Invoke(CurrentStamina / MaxStamina);
        }

        public void SubStamina()
        {
            if (MaxStamina == 1000)
            {
                _playerStateMachine.IsCanNotAddSpiritual = true;
                return;
            }

            MaxStamina = Mathf.Max(0, MaxStamina - 1);
            CurrentStamina = MaxStamina;
            OnChangeStamina?.Invoke(CurrentStamina / MaxStamina);
        }

        public void Reset()
        {
            CurrentStamina = MaxStamina;
            OnChangeStamina?.Invoke(CurrentStamina / MaxStamina);
        }
    }
}