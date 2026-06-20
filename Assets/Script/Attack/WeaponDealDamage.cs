using System.Collections.Generic;
using Script.Attack;
using UnityEngine;

public class WeaponDealDamage : MonoBehaviour
{
    [SerializeField] private GameObject myCollider;
    private List<GameObject> alreadyDealDamage = new();

    private float damage;

    private void Start()
    {
        // alreadyDealDamage = new List<GameObject>();
    }

    private void OnEnable()
    {
        alreadyDealDamage.Clear();
    }

    public void SetDamage(float weaponDamage)
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

        enemy.DealDamage(damage);
        enemy.HitStop();
    }
}