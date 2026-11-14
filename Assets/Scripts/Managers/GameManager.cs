using DesignPattern.StateMachine.Player.Base;
using Attack.Status;
using Attack.Status.Potion;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    /// <summary>
    /// This class will control the NEW/ CONTINUE/ SAVE/ RESPAWN
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        //Checkpoint
        private string currentCheckpointID = "start";
        private Vector3 checkpointPosition;

        // false is new save - true is load existed save

        private enum ReasonLoadScene
        {
            Start,
            New,
            Continue,
            Respawn,
            Exit
        };

        private ReasonLoadScene loadReason = ReasonLoadScene.New;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;

        private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

        // ----- New Game / Continue / Exit -----
        public void StartNewGame(string sceneName)
        {
            SaveManager.Instance.CreateNewSaveGame();
            loadReason = ReasonLoadScene.New;
            currentCheckpointID = "start";
            SceneManager.LoadScene(sceneName);
        }

        public void ContinueGame()
        {
            var saveData = SaveManager.Instance.LoadGame();

            if (saveData is null)
            {
                Debug.LogWarning("[GameManager] No save data found");
                StartNewGame("Main");
                return;
            }

            loadReason = ReasonLoadScene.Continue;

            currentCheckpointID = saveData.currentCheckpointID;
            checkpointPosition = new Vector3(saveData.posX, saveData.posY, saveData.posZ);
            SceneManager.LoadScene("LoadScene");
        }

        public void ExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        public void ExitToTitle()
        {
            loadReason = ReasonLoadScene.Exit;
            SceneManager.LoadScene("Start");
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "Start") return;
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player == null) return;

            switch (loadReason)
            {
                case ReasonLoadScene.New:
                    checkpointPosition = player.transform.position;
                    AutoSave();
                    break;
                case ReasonLoadScene.Continue:
                    ApplySaveData(player);
                    ResetStatus();
                    break;
                case ReasonLoadScene.Respawn:
                    ApplySaveData(player);
                    ResetStatus();
                    break;
                case ReasonLoadScene.Exit:
                    break;
            }
        }

        private void ApplySaveData(GameObject player)
        {
            var data = SaveManager.Instance.CurrentSaveData;
            var audio = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioSetting>();
            if (data == null) return;

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
            health.MaxHealth = data.currentHealth;
            health.resistance = data.currentResistance;
            mana.MaxMana = data.currentMana;
            stamina.MaxStamina = data.currentStamina;
            foreach (var damage in stateMachine.AttackData)
            {
                damage.AttackDamage = data.currentDamage;
            }

            // Attach potion
            healthPotion.MaxPotion = data.currentHealthPotion;
            manaPotion.MaxPotion = data.currentManaPotion;
            subPotion.subPotionList[0].quantity = data.emeraldGuavaQuantity;
            subPotion.subPotionList[1].quantity = data.goldenPearQuantity;
            subPotion.subPotionList[2].quantity = data.bloodPomegranateQuantity;

            // Dodge Award
            stateMachine.DodgeAward.IsMovementPush = data.isMovementPush;
            stateMachine.DodgeAward.IsRecoveryStamina = data.isRecoveryStamina;
            stateMachine.DodgeAward.IsCounterAttack = data.isCounterAttack;

            // Skill
            stateMachine.SkillActive.changingTheGameSkill = data.changingTheGameSkill;
            stateMachine.SkillActive.escapeSkill = data.escapeSkill;
            stateMachine.SkillActive.responseSkill = data.responseSkill;

            // Audio
            audio.MasterVolume = data.masterVolume;
            audio.BgmVolume = data.BGMVolume;
            audio.SfxVolume = data.SFXVolume;
            audio.UiVolume = data.UIVolume;
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
            if (player == null) return;
            loadReason = ReasonLoadScene.Respawn;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void ReturnTitle()
        {
            loadReason = ReasonLoadScene.Start;
            SceneManager.LoadScene("Start");
        }

        public void AutoSave()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            var audio = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioSetting>();

            if (player == null) return;

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
                completedTriggerBoss = SaveManager.Instance.CurrentSaveData.completedTriggerBoss,
                currentHealth = health.MaxHealth,
                currentMana = mana.MaxMana,
                currentStamina = stamina.MaxStamina,
                currentResistance = health.resistance,
                currentDamage = stateMachine.AttackData[0].AttackDamage,
                currentHealthPotion = healthPotion.MaxPotion,
                currentManaPotion = manaPotion.MaxPotion,
                isRecoveryStamina = stateMachine.DodgeAward.IsRecoveryStamina,
                isCounterAttack = stateMachine.DodgeAward.IsCounterAttack,
                isMovementPush = stateMachine.DodgeAward.IsMovementPush,
                emeraldGuavaQuantity = subPotion.subPotionList[0].quantity,
                goldenPearQuantity = subPotion.subPotionList[1].quantity,
                bloodPomegranateQuantity = subPotion.subPotionList[2].quantity,
                changingTheGameSkill = stateMachine.SkillActive.changingTheGameSkill,
                escapeSkill = stateMachine.SkillActive.escapeSkill,
                responseSkill = stateMachine.SkillActive.responseSkill,
                masterVolume = audio.MasterVolume,
                BGMVolume = audio.BgmVolume,
                SFXVolume = audio.SfxVolume,
                UIVolume = audio.UiVolume,

                resolutionIndex = GraphicManager.Instance.ResolutionIndex,
                displayModeIndex = GraphicManager.Instance.DisplayModeIndex,
                fps = GraphicManager.Instance.Fps,
                vsync = GraphicManager.Instance.Vsync,
                qualityPresetIndex = GraphicManager.Instance.QualityPresetIndex,
                shadow = GraphicManager.Instance.Shadow,
                antiAliasingIndex = GraphicManager.Instance.AntiAliasingIndex,
                textureQualityIndex = GraphicManager.Instance.TextureQualityIndex,
                bloom = GraphicManager.Instance.BloomData,
                motionBlur = GraphicManager.Instance.MotionBlurData,
                ambientOcclusion = GraphicManager.Instance.AmbientOcclusion,
            };
            SaveManager.Instance.SaveGame(saveData);
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

            // Reset Stats
            health.Reset();
            mana.Reset();
            stamina.Reset();

            // Reset potion
            healthPotion.Reset();
            manaPotion.Reset();


            // Update Dodge Award
            stateMachine.DodgeAward.CallUIEvent();

            // Update Skill
            stateMachine.SkillActive.UpdateSkillUIByType();
        }
    }
}