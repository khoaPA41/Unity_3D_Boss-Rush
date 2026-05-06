using UnityEngine;

public class WeaponHanldler : MonoBehaviour
{
    [SerializeField] GameObject weaponLogic;
    [SerializeField] GameObject weaponMain;
    [SerializeField] GameObject weaponStore;
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
        weaponStore?.SetActive(false);
    }

    public void OnStoreWeapon()
    {
        weaponMain?.SetActive(false);
        weaponStore?.SetActive(true);
    }
}
