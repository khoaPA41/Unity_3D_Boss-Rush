using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] private GameObject weaponRightLogic;
    [SerializeField] private GameObject weaponLeftLogic;
    [SerializeField] private GameObject otherDamageLogic;
    [SerializeField] private GameObject specialDamageLogic;
    
    [SerializeField] private GameObject weaponMain;
    [SerializeField] private GameObject weaponStore;
    
    [SerializeField] private GameObject LeftSingleSword;
    [SerializeField] private GameObject RightSingleSword;
    [SerializeField] private GameObject DoubleEdgedSword;
    
    /***********Collider***********/
    public void OnActiveWeaponCollider()
    {
        specialDamageLogic?.SetActive(true);
    }

    public void OnUnActiveWeaponCollider()
    {
        specialDamageLogic?.SetActive(false);
    }
    
    public void OnActiveWeaponLeftCollider()
    {
        weaponLeftLogic?.SetActive(true);
    }

    public void OnUnActiveWeaponLeftCollider()
    {
        weaponLeftLogic?.SetActive(false);
    }
    
    public void OnActiveWeaponRightCollider()
    {
        weaponRightLogic?.SetActive(true);
    }

    public void OnUnActiveWeaponRightCollider()
    {
        weaponRightLogic?.SetActive(false);
    }

    
    public void OnActiveOtherCollider()
    {
        otherDamageLogic?.SetActive(true);
    }

    public void OnUnActiveOtherCollider()
    {
        otherDamageLogic?.SetActive(false);
    }

    
    /***********Game Object***********/
    public void OnGetWeapon()
    {
        weaponMain?.SetActive(true);
        if (weaponStore)
        {
            weaponStore.SetActive(false);
        }
    }

    public void OnStoreWeapon()
    {
        weaponMain?.SetActive(false);
        if (weaponStore)
        {
            weaponStore.SetActive(true);
        }
    }
    
    // Left Single Sword
    public void OnActiveLeftSingleSword()
    {
        LeftSingleSword?.SetActive(true);
    }

    public void OnUnActiveLeftSingleSword()
    {
        LeftSingleSword?.SetActive(false);
    }
    // Right Single Sword
    public void OnActiveRightSingleSword()
    {
        RightSingleSword?.SetActive(true);
    }

    public void OnUnActiveRightSingleSword()
    {
        RightSingleSword?.SetActive(false);
    }
    
    public void OnActiveDoubleEdgedSword()
    {
        DoubleEdgedSword?.SetActive(true);
    }

    public void OnUnActiveDoubleEdgedSword()
    {
        DoubleEdgedSword?.SetActive(false);
    }
}
