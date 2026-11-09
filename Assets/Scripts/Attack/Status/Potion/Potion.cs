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
            if (_playerStateMachine.IsCanNotSubSpiritual || _playerStateMachine.PlayerSpiritualPower <= 0)
            {
                _playerStateMachine.IsCanNotSubSpiritual = false;
                return;
            }
            MaxPotion += maxPotionAdded;
            CurrentPotion = MaxPotion;
            OnChangePotion?.Invoke(CurrentPotion / MaxPotion);
        }

        public void SubtractMaxPotion()
        {
            if (MaxPotion == _minimumPotion)
            {
                _playerStateMachine.IsCanNotAddSpiritual = true;
                return;
            }
            MaxPotion -= maxPotionAdded;
            CurrentPotion = MaxPotion;
            OnChangePotion?.Invoke(CurrentPotion / MaxPotion);
        }

        public void AddUsagePotion()
        {
            if (_playerStateMachine.IsCanNotSubSpiritual || _playerStateMachine.PlayerSpiritualPower <= 0)
            {
                _playerStateMachine.IsCanNotSubSpiritual = false;
                return;
            }
            potionUsage += usageAdded;
        }

        public void SubtractUsagePotion()
        {
            if (potionUsage == _minimumPotionUsage)
            {
                _playerStateMachine.IsCanNotAddSpiritual = true;
                return;
            }
            potionUsage -= usageAdded;
        }

        public void Reset()
        {
            CurrentPotion = MaxPotion;
            OnChangePotion?.Invoke(CurrentPotion / MaxPotion);
        }
    }
}