using System.Collections.Generic;
using Script.Attack;
using UnityEngine;

public class WeaponDealDamage : MonoBehaviour
{
    [SerializeField] private GameObject myCollider;
    private readonly List<GameObject> alreadyDealDamage = new List<GameObject>();

    private int damage;

    private void OnEnable()
    {
        alreadyDealDamage.Clear();
    }

    public void SetDamage(int weaponDamage)
    {
        damage = weaponDamage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (alreadyDealDamage.Contains(other.gameObject))
        {
            return;
        }

        if (other.gameObject == myCollider)
        {
            return;
        }

        alreadyDealDamage.Add(other.gameObject);

        if (!other.TryGetComponent<Health>(out var enemy)) return;

        if (enemy.noDamage)
        {
            return;
        }
Debug.Log("Bullet");

        enemy.DealDamage(damage);
        enemy.HitStop();
    }
}