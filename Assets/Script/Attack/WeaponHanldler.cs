using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] private GameObject weaponLogic;
    [SerializeField] private GameObject weaponMain;
    [SerializeField] private GameObject weaponStore;
    public void OnActiveWeaponCollider()
    {
        weaponLogic?.SetActive(true);
    }

    public void OnUnActiveWeaponCollider()
    {
        weaponLogic?.SetActive(false);
    }

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
}
