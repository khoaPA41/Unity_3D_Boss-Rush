using System;
using DesignPattern.StateMachine.Player.Base;
using UnityEngine;

namespace Attack.Status.Potion
{
    public class Potion : MonoBehaviour
    {
        [field: SerializeField] public float MaxPotion;
        [field: SerializeField] public float potionUsage { get; set; }
        [field: SerializeField] public float reduceDuration { get; set; }
        [field: SerializeField] public float maxPotionAdded { get; set; }
        [field: SerializeField] public float usageAdded { get; set; }

        public float CurrentPotion { get; private set; }
        public event Action<float> OnChangePotion = delegate { };
        public float PossibleUsage { get; private set; }

        private PlayerStateMachine _playerStateMachine;
        private float _minimumPotion;
        private float _minimumPotionUsage;

        private void Awake()
        {
            _playerStateMachine = GetComponent<PlayerStateMachine>();
            _minimumPotion = MaxPotion;
            CurrentPotion = MaxPotion;
            PossibleUsage = potionUsage;
        }

        public void ChangePotion()
        {
            PossibleUsage = potionUsage < CurrentPotion ? potionUsage : CurrentPotion;
            CurrentPotion = Mathf.Max(CurrentPotion - PossibleUsage, 0f);
            OnChangePotion?.Invoke(CurrentPotion / MaxPotion);
        }

        public void AddMaxPotion()
        {
            if (!_playerStateMachine.CheckHasSpiritualPower()) return;

            _playerStateMachine.SubSpiritualPower();

            MaxPotion += maxPotionAdded;
            CurrentPotion = MaxPotion;

            OnChangePotion?.Invoke(CurrentPotion / MaxPotion);
        }

        public void SubtractMaxPotion()
        {
            if (MaxPotion == _minimumPotion) return;

            _playerStateMachine.AddSpiritualPower();

            MaxPotion = Mathf.Max(0, MaxPotion - maxPotionAdded);
            CurrentPotion = MaxPotion;

            OnChangePotion?.Invoke(CurrentPotion / MaxPotion);
        }

        public void AddUsagePotion()
        {
            if (!_playerStateMachine.CheckHasSpiritualPower()) return;

            _playerStateMachine.SubSpiritualPower();
            potionUsage += usageAdded;
        }

        public void SubtractUsagePotion()
        {
            if (potionUsage == _minimumPotionUsage) return;

            _playerStateMachine.AddSpiritualPower();
            potionUsage -= usageAdded;
        }

        public void Reset()
        {
            CurrentPotion = MaxPotion;
            OnChangePotion?.Invoke(CurrentPotion / MaxPotion);
        }
    }
}