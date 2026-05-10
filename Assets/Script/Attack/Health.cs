using System;
using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int maxHealth;
    [SerializeField] float timeFreeze;
    [SerializeField] float timeToBackNormal;

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

    IEnumerator TimeFreeHit()
    {
        Time.timeScale = timeFreeze;
        yield return new WaitForSecondsRealtime(timeToBackNormal);
        Time.timeScale = 1f;
    }

    public void HitStop()
    {
        StartCoroutine(TimeFreeHit());
    }

}
