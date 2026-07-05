using System;
using Script.Attack;
using Script.Design_Pattern.StateMachine.Player.Base;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class will control the NEW/ CONTINUE/ SAVE/ RESPAWN
/// </summary>
public class GameManagers : MonoBehaviour
{
    public static GameManagers Instance { get; private set; }

    //Checkpoint
    private string currentCheckpointID = "start";
    private Vector3 checkpointPosition;

    // false is new save - true is load existed save

    private enum ReasonLoadScene
    {
        New,
        Continue,
        Respawn
    };
    
    private ReasonLoadScene loadReason = ReasonLoadScene.New;
    // private bool isLoadingGameFromSave = false;

    private void Awake()
    {
        if (Instance is not null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;


    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;



    // ----- New Game / Continue -----
    public void StartNewGame(string sceneName)
    {
        SaveManagers.Instance.CreateNewSaveGame();
        loadReason = ReasonLoadScene.New;
        currentCheckpointID = "start";
        SceneManager.LoadScene(sceneName);
    }

    public void ContinueGame()
    {
        var saveData = SaveManagers.Instance.LoadGame();

        if (saveData is null)
        {
            Debug.LogWarning("[SaveManagers] Don't have save data]");
            StartNewGame("Main");
            return;
        }
        loadReason = ReasonLoadScene.Continue;

        // isLoadingGameFromSave = true;
        currentCheckpointID = saveData.currentCheckpointID;
        checkpointPosition = new Vector3(saveData.posX, saveData.posY, saveData.posZ);
        SceneManager.LoadScene(saveData.sceneName);
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player is null) return;

        switch (loadReason)
        {
            case ReasonLoadScene.New:
            checkpointPosition = player.transform.position;
            break;
            case ReasonLoadScene.Continue:
                ApplySaveData(player);
                ResetStatus();
                break;
            case ReasonLoadScene.Respawn:
                ApplySaveData(player);
                ResetStatus();
                break;
        }
    }


    private void ApplySaveData(GameObject player)
    {
        var data = SaveManagers.Instance.CurrentSaveData;
        if (data is null) return;

        player.transform.position = new Vector3(data.posX, data.posY, data.posZ);

        // Stats
        var health = player.GetComponent<Health>(); // include resistance
        var mana = player.GetComponent<Mana>();
        var stamina = player.GetComponent<Stamina>();
        var stateMachine = player.GetComponent<PlayerStateMachine>();
        // Potion
        var healthPotion = player.GetComponent<HealthPotion>();
        var manaPotion = player.GetComponent<ManaPotion>();
        var subPotion = player.GetComponent<SubPotion>();


        // Attach Stats
        health.maxHealth = data.currentHealth;
        health.resistance = data.currentResistance;
        mana.maxMana = data.currentMana;
        stamina.maxStamina = data.currentStamina;
        foreach (var damage in stateMachine.AttackData)
        {
            damage.AttackDamage = data.currentDamage;
        }

        // Attach potion
        healthPotion.maxPotion = data.currentHealthPotion;
        manaPotion.maxPotion = data.currentManaPotion;
        subPotion.subPotionList[0].quantity = data.emeraldGuavaQuantity;
        subPotion.subPotionList[1].quantity = data.goldenPearQuantity;
        subPotion.subPotionList[2].quantity = data.bloodPomegranateQuantity;

        // Dodge Award

        // Skill
    }

    // ----- Checkpoint / Respawn / Auto save -----

    public void SetCheckpoint(string checkpointID, Vector3 checkpointPosition)
    {
        currentCheckpointID = checkpointID;
        this.checkpointPosition = checkpointPosition;
    }

    public void ReturnCheckpoint()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player is null) return;
        loadReason = ReasonLoadScene.Respawn;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void AutoSave()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player is null) return;

        // Stats
        var health = player.GetComponent<Health>(); // include resistance
        var mana = player.GetComponent<Mana>();
        var stamina = player.GetComponent<Stamina>();
        var stateMachine = player.GetComponent<PlayerStateMachine>();
        // Potion
        var healthPotion = player.GetComponent<HealthPotion>();
        var manaPotion = player.GetComponent<ManaPotion>();
        var subPotion = player.GetComponent<SubPotion>();

        var saveData = new SaveData
        {
            currentCheckpointID = currentCheckpointID,
            sceneName = SceneManager.GetActiveScene().name,
            posX = checkpointPosition.x,
            posY = checkpointPosition.y,
            posZ = checkpointPosition.z,
            currentHealth = health.maxHealth,
            currentMana = mana.maxMana,
            currentStamina = stamina.maxStamina,
            currentHealthPotion = healthPotion.maxPotion,
            currentManaPotion = manaPotion.maxPotion,
            emeraldGuavaQuantity = subPotion.subPotionList[0].quantity,
            goldenPearQuantity = subPotion.subPotionList[1].quantity,
            bloodPomegranateQuantity = subPotion.subPotionList[2].quantity,
        };
        SaveManagers.Instance.SaveGame(saveData);
    }

    private void ResetStatus()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        // stats
        var health = player.GetComponent<Health>(); // include resistance
        var mana = player.GetComponent<Mana>();
        var stamina = player.GetComponent<Stamina>();
        var stateMachine = player.GetComponent<PlayerStateMachine>();
        
        // Potion
        var healthPotion = player.GetComponent<HealthPotion>();
        var manaPotion = player.GetComponent<ManaPotion>();
        // var subPotion = player.GetComponent<SubPotion>();
        
        
        // Reset Stats
        health.Reset();
        mana.Reset();
        stamina.Reset();
        
        // Reset potion
        healthPotion.Reset();
        manaPotion.Reset();
    }
}