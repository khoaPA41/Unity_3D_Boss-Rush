using UnityEngine;

public class Mana : MonoBehaviour
{
    [SerializeField] int maxMana;

    public int currentMana { get; set; }
    void Start()
    {
        currentMana = maxMana;
    }
}
