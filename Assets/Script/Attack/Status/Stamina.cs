using System;
using Design_Pattern.StateMachine.Player;
using UnityEngine;

namespace Status
{
    [RequireComponent(typeof(PlayerStateMachine))]
    public class Stamina : MonoBehaviour
    {
        [field: SerializeField] public float maxStamina { get; set; }
        [field: SerializeField] public float dodgeReduce { get; set; }
        [field: SerializeField] public float walkReduce { get; set; }
        [field: SerializeField] public float runReduce { get; set; }
        [field: SerializeField] public float lightAttackReduce { get; set; }
        [field: SerializeField] public float heavyAttackReduce { get; set; }
        [field: SerializeField] public float jumpReduce { get; set; }
        [SerializeField] private float reduceStamina;
        [SerializeField] private float recoveryStamina;

        public float CurrentStamina { get; set; }
        public bool IsCombat;
        public event Action<float> OnChangeStamina = delegate { };
        public event Action<float> OnRecoveryStamina = delegate { };

        public bool isReduceStamina { get; set; } // If use potion

        private PlayerStateMachine _playerStateMachine;

        private void Awake()
        {
            _playerStateMachine = GetComponent<PlayerStateMachine>();
            CurrentStamina = maxStamina;
        }

        public void ChangeStamina(float amount)
        {
            if (!IsCombat) return;
            if (isReduceStamina) amount /= reduceStamina;

            CurrentStamina -= amount * Time.deltaTime;
            CurrentStamina = Mathf.Clamp(CurrentStamina, 0, maxStamina);

            OnChangeStamina?.Invoke(CurrentStamina / maxStamina);
        }

        public void RecoveryStamina()
        {
            if (CurrentStamina < maxStamina)
            {
                CurrentStamina += recoveryStamina * Time.deltaTime;
                CurrentStamina = Mathf.Clamp(CurrentStamina, 0, maxStamina);
            }

            OnRecoveryStamina?.Invoke(CurrentStamina / maxStamina);
        }

        public void DodgeAwardStamina()
        {
            var staminaLost = maxStamina - CurrentStamina;
            CurrentStamina += staminaLost * .5f;
            OnRecoveryStamina?.Invoke(CurrentStamina / maxStamina);
        }

        public void AddStamina()
        {
            if (_playerStateMachine.isCanNotSubSpiritual)
            {
                _playerStateMachine.isCanNotSubSpiritual = false;
                return;
            }

            maxStamina += 1;
            CurrentStamina = maxStamina;
            OnChangeStamina?.Invoke(CurrentStamina / maxStamina);
        }

        public void SubStamina()
        {
            if (maxStamina == 1000)
            {
                _playerStateMachine.isCanNotAddSpiritual = true;
                return;
            }
            maxStamina -= 1;
            CurrentStamina = maxStamina;
            OnChangeStamina?.Invoke(CurrentStamina / maxStamina);
        }

        public void Reset()
        {
            CurrentStamina = maxStamina;
            OnChangeStamina?.Invoke(CurrentStamina / maxStamina);
        }
    }
}