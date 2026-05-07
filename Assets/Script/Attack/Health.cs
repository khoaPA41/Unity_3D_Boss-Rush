using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int maxHealth;
    public int currentHealth { get; private set; }

    public event Action DeathAction;
    public event Action HitAction;
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void DealDamage(int damage)
    {
        currentHealth = Mathf.Max(currentHealth - damage, 0);

        HitAction?.Invoke();

        if (currentHealth <= 0)
        {
            DeathAction?.Invoke();
        }
    }


}
