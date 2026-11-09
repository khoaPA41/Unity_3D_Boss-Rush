using UnityEngine;


namespace Attack
{
    public class WeaponHandler : MonoBehaviour
    {
        [Header("Collider")]

        [SerializeField] private GameObject _weaponRightLogic;
        [SerializeField] private GameObject _weaponLeftLogic;
        [SerializeField] private GameObject _otherDamageLogic;

        [SerializeField] private GameObject _weaponMain;
        [SerializeField] private GameObject _weaponStore;

        [SerializeField] private GameObject _leftSingleSword;
        [SerializeField] private GameObject _rightSingleSword;
        [SerializeField] private GameObject _doubleEdgedSword;

        /***********Collider***********/

        public void ActivateLeftWeaponCollider()
        {
            _weaponLeftLogic?.SetActive(true);
        }

        public void DeactivateLeftWeaponCollider()
        {
            if (_weaponLeftLogic == null) return;
            _weaponLeftLogic?.SetActive(false);
        }

        public void ActivateRightWeaponCollider()
        {
            _weaponRightLogic?.SetActive(true);
        }

        public void DeactivateRightWeaponCollider()
        {
            _weaponRightLogic?.SetActive(false);
        }


        public void ActivateOtherCollider()
        {
            _otherDamageLogic?.SetActive(true);
        }

        public void DeactivateOtherCollider()
        {
            _otherDamageLogic?.SetActive(false);
        }


        /***********Game Object***********/
        public void OnGetWeapon()
        {
            _weaponMain?.SetActive(true);
            if (_weaponStore)
            {
                _weaponStore.SetActive(false);
            }
        }

        public void OnStoreWeapon()
        {
            _weaponMain?.SetActive(false);
            if (_weaponStore)
            {
                _weaponStore.SetActive(true);
            }
        }

        // Left Single Sword
        public void ActivateLeftSingleSword()
        {
            _leftSingleSword?.SetActive(true);
        }

        public void DeactivateLeftSingleSword()
        {
            _leftSingleSword?.SetActive(false);
        }
        // Right Single Sword
        public void ActivateRightSingleSword()
        {
            _rightSingleSword?.SetActive(true);
        }

        public void DeactivateRightSingleSword()
        {
            _rightSingleSword?.SetActive(false);
        }

        public void ActivateDoubleEdgedSword()
        {
            _doubleEdgedSword?.SetActive(true);
        }

        public void DeactivateDoubleEdgedSword()
        {
            _doubleEdgedSword?.SetActive(false);
        }
    }
}