using UnityEngine;

public class WeaponHanldler : MonoBehaviour
{
    [SerializeField] GameObject weapon;

    public void OnActiveWeaponCollider()
    {
        weapon.SetActive(true);
    }

    public void OnUnActiveWeaponCollider()
    {
        weapon.SetActive(false);

    }
}
