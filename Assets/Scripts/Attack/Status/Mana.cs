using System;
using DesignPattern.StateMachine.Player.Base;
using UnityEngine;

namespace Attack.Status
{
    public class Mana : MonoBehaviour
    {
        [Header("Mana Value")]
        private const float _minLimitMana = 100f;
        [field: SerializeField] public float MaxMana { get; set; }
        [field: SerializeField] public float reduceManaDuration { get; set; }
        public event Action<float> OnChangeMana = delegate { };
        public float CurrentMana;
        private PlayerStateMachine _playerStateMachine;

        private void Awake()
        {
            _playerStateMachine = GetComponent<PlayerStateMachine>();
            CurrentMana = MaxMana;
        }

        public void ChangeMana(int amount)
        {
            CurrentMana = Mathf.Max(CurrentMana - amount, 0);
            OnChangeMana?.Invoke(CurrentMana / MaxMana);
        }

        public void RecoveryMana(float amount)
        {
            CurrentMana = Mathf.Min(CurrentMana + amount, MaxMana);
            OnChangeMana?.Invoke(CurrentMana / MaxMana);
        }

        public void AddMana()
        {
            if (!_playerStateMachine.CheckHasSpiritualPower()) return;

            _playerStateMachine.SubSpiritualPower();
            MaxMana += 1;
            CurrentMana = MaxMana;
            OnChangeMana?.Invoke(CurrentMana / MaxMana);
        }

        public void SubMana()
        {
            if (CurrentMana <= _minLimitMana) return;

            _playerStateMachine.AddSpiritualPower();
            MaxMana = Mathf.Max(0, MaxMana - 1);
            CurrentMana = MaxMana;
            OnChangeMana?.Invoke(CurrentMana / MaxMana);
        }

        public void Reset()
        {
            CurrentMana = MaxMana;
            OnChangeMana?.Invoke(CurrentMana / MaxMana);
        }
    }
}