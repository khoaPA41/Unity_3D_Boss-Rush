using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int maxHealth;


    int currentHealth;
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void DealDamage(int damage)
    {
        currentHealth = Mathf.Max(currentHealth - damage, 0);

        Debug.Log(currentHealth);

        if (currentHealth <= 0)
        {
            Debug.Log("Die!");
        }
    }
}
