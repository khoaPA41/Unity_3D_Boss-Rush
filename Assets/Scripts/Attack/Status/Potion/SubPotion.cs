using Attack.Status;
using System;
using System.Collections;
using System.Collections.Generic;
using DesignPattern.StateMachine.Player.Base;
using Input;
using UnityEngine;


namespace Attack.Status.Potion
{
    [Serializable]
    public class SubPotionValue
    {
        public string potionName;
        public int quantity;
        public float timeToDone;
        public string hexColor;
    }


    public class SubPotion : MonoBehaviour
    {
        [field: SerializeField] public List<SubPotionValue> subPotionList { get; set; }
        private Health _health;
        private Stamina _stamina;

        public SubPotionValue currentPotion { get; set; }
        private int _currentIndex = 1;

        private InputReader _inputReader;
        private PlayerStateMachine _playerStateMachine;

        private void Awake()
        {
            _health = GetComponent<Health>();
            _stamina = GetComponent<Stamina>();
            _playerStateMachine = GetComponent<PlayerStateMachine>();
            _inputReader = GetComponent<InputReader>();
            currentPotion = subPotionList[_currentIndex];
        }

        private void OnEnable()
        {
            _inputReader.ChangeNextSubPotionAction += ChangeNextSubPotion;
            _inputReader.ChangePrevSubPotionAction += ChangePreviousSubPotion;
        }

        private void OnDisable()
        {
            _inputReader.ChangeNextSubPotionAction -= ChangeNextSubPotion;
            _inputReader.ChangePrevSubPotionAction -= ChangePreviousSubPotion;
        }

        private void ChangeNextSubPotion()
        {
            _currentIndex += 1;
            if (_currentIndex >= subPotionList.Count) _currentIndex = 0;
            currentPotion = subPotionList[_currentIndex];
        }

        private void ChangePreviousSubPotion()
        {
            _currentIndex -= 1;
            if (_currentIndex < 0) _currentIndex = subPotionList.Count - 1;
            currentPotion = subPotionList[_currentIndex];
        }

        public void SubtractPotion(string potionName, int amount)
        {
            var targetPotion = subPotionList.Find(potion => potion.potionName == potionName);
            targetPotion.quantity = Mathf.Max(targetPotion.quantity - amount, 0);
        }

        private SubPotionValue FindPotion(string potionName)
        {
            return subPotionList.Find(potion => potion.potionName == potionName);
        }

        public void ReduceDamage(string potionName)
        {
            StartCoroutine(SubPotionCoroutine(FindPotion(potionName).timeToDone,
                () => _health.isReduceDamage = true,
                () => _health.isReduceDamage = false
            ));
        }

        public void ReduceStamina(string potionName)
        {
            StartCoroutine(SubPotionCoroutine(FindPotion(potionName).timeToDone,
                () => _stamina.isReduceStamina = true,
                () => _stamina.isReduceStamina = false
            ));
        }

        public void IncreaseDamage(string potionName)
        {
            StartCoroutine(SubPotionCoroutine(FindPotion(potionName).timeToDone,
                () => _playerStateMachine.IsIncreaseDamagePotion = true,
                () => _playerStateMachine.IsIncreaseDamagePotion = false
            ));
        }

        private IEnumerator SubPotionCoroutine(float timeToDone, Action callback1, Action callback2)
        {
            callback1?.Invoke();
            yield return new WaitForSecondsRealtime(timeToDone);
            callback2?.Invoke();
        }

        public void AddSubPotionQuantity(string potionName)
        {
            var targetPotion = FindPotion(potionName);
            if (_playerStateMachine.IsCanNotSubSpiritual || _playerStateMachine.PlayerSpiritualPower <= 0)
            {
                _playerStateMachine.IsCanNotSubSpiritual = false;
                return;
            }
            targetPotion.quantity++;
        }

        public void SubtractSubPotionQuantity(string potionName)
        {
            var targetPotion = FindPotion(potionName);

            if (targetPotion.quantity == 0)
            {
                _playerStateMachine.IsCanNotAddSpiritual = true;
                return;
            }

            targetPotion.quantity--;
        }
    }
}