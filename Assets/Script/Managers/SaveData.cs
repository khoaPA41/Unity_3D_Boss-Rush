using System;
using UnityEngine;


/// <summary>
/// This all data need to save in once playing
/// This class need to mark Serializable to JsonUtility convert JSON
/// </summary>
[Serializable]
public class SaveData
{
    // --------- Checkpoint ---------
    public string currentCheckpointID = "start";
    public string sceneName;

    public float posX;
    public float posY;
    public float posZ;

    // --------- Stats ---------
    public float currentHealth;
    public float currentMana;
    public float currentStamina;
    public float currentResistance;
    public float currentDamage;

    // --------- Potion ---------
    public float currentHealthPotion;
    public float currentManaPotion;
    public int emeraldGuavaQuantity;
    public int goldenPearQuantity;
    public int bloodPomegranateQuantity;

    // --------- Power ---------
    public bool isRecoveryStamina;
    public bool isCounterAttack;
    public bool isMovementPush;
    public SkillActiveType changingTheGameSkill;
    public SkillActiveType escapeSkill;
    public SkillActiveType responseSkill;
    
    // --------- Sub Information ---------
    public string saveDateTime;
    public bool hasSaveData;
    
    // --------- Sound Settings ---------
    public float masterVolume;
    public float BGMVolume;
    public float SFXVolume;
    public float UIVolume;

    // --------- Graphic Settings ---------

}