using System.Collections.Generic;
using UnityEngine;
public class WeaponDealDamage : MonoBehaviour
{
    [SerializeField] GameObject myCollider;
    List<GameObject> alreadyDealDamage = new List<GameObject>();

    int damage;

    void OnEnable()
    {
        alreadyDealDamage.Clear();
    }

    public void SetDamage(int weaponDamage)
    {
        damage = weaponDamage;
    }

    void OnTriggerEnter(Collider other)
    {
        if (alreadyDealDamage.Contains(other.gameObject)) { return; }
        if (other.gameObject == myCollider) { return; }

        alreadyDealDamage.Add(other.gameObject);

        if (other.TryGetComponent<Health>(out Health enemy))
        {
            enemy.DealDamage(damage);
            enemy.HitStop();
        }
    }
}
