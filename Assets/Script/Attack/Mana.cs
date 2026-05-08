using UnityEngine;

public class Mana : MonoBehaviour
{
    [SerializeField] int maxMana;

    public int currentMana;
    void Start()
    {
        currentMana = maxMana;
    }
}
