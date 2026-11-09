# RE-END — 00. Project Inventory & Repository Discovery

- **Ngày khảo sát:** 2026-11-09
- **Unity Project Root:** `C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush`
- **Audit Workspace Root:** `C:/Users/Official_41/Desktop/Audit Re-End`
- **Phương pháp:** Khảo sát tĩnh (Static Architecture Discovery) trên toàn bộ mã nguồn C#, cấu hình project, Scene YAML, Prefab YAML và Animator Controller metadata. Không chạy Play Mode, không build, không can thiệp bất kỳ file nào của dự án.

---

## 1. Phiên bản Unity, Render Pipeline & Dependencies

### 1.1. Phiên bản Unity
- **Editor Version:** `6000.3.8f1` (`6000.3.8f1 (1c7db571dde0)`) — xác minh tại `ProjectSettings/ProjectVersion.txt:1-2`.

### 1.2. Các Package quan trọng (`Packages/manifest.json`)
| Nhóm chức năng | Package ID | Phiên bản | Trạng thái sử dụng thực tế trong RE-END |
|---|---|---|---|
| **Input** | `com.unity.inputsystem` | `1.18.0` | **Đang dùng chính:** `Assets/Scripts/Input/InputController.inputactions` + class sinh tự động `InputController.cs` + `InputReader.cs`. |
| **Camera** | `com.unity.cinemachine` | `3.1.7` | **Đang dùng chính:** Lock-on target group (`CinemachineTargetGroup` trong `Targeter.cs:2,9`) và camera điều khiển trong `Main.unity`. |
| **Cutscene / Director** | `com.unity.timeline` | `1.8.13` | **Đang dùng chính:** 6 file `.playable` trong `Assets/TimeLine/`, điều phối bởi `TimelineEvent.cs` và `BossPhaseManager.cs`. |
| **Render Pipeline** | `com.unity.render-pipelines.universal` | `17.3.0` | **Đang dùng chính:** Các asset URP trong `Assets/Settings/` (`PC_RPAsset.asset`, `Mobile_RPAsset.asset`, `Web_RPAsset.asset`, `URP.asset`) và Post-Processing (`Bloom`, `MotionBlur`) trong `GraphicManager.cs:6-8`. |
| **Render Pipeline (Phụ)** | `com.unity.render-pipelines.high-definition` | `17.3.0` | **Có cài đặt & import namespace dư:** `Assets/HDRPDefaultResources/` tồn tại và `GraphicManager.cs:5` có `using UnityEngine.Rendering.HighDefinition;`, nhưng pipeline vận hành thực tế của `GraphicManager` dùng `UnityEngine.Rendering.Universal`. |
| **UI & Text** | `com.unity.ugui` | `2.0.0` | **Đang dùng chính:** Toàn bộ HUD, Checkpoint System UI, Settings UI, Start Menu, Progress UI, TextMeshPro (`Assets/TextMesh Pro/`). |
| **AI / Navigation** | `com.unity.ai.navigation` | `2.0.16` | **Không dùng trong gameplay script:** Boss và PlayerClone di chuyển trực tiếp qua `CharacterController.Move` (`BossBaseState.cs:24`, `PlayerCloneBaseState.cs:19`), không dùng `NavMeshAgent`. |
| **Animation Rigging** | `com.unity.animation.rigging` | `1.4.1` | Có trong `manifest.json`, không có script custom gọi trực tiếp trong `Assets/Scripts/`. |
| **VFX / Shader** | `com.unity.visualeffectgraph` (`17.3.0`), `com.unity.shadergraph` (`17.3.0`), `com.unity.toonshader` (`0.11.0-preview`) | Nhiều bản | Dùng cho tài nguyên hiệu ứng/shader; VFX runtime trong code chủ yếu điều khiển qua `ParticleSystem` (`SkillPooledObject.cs:11`). |
| **Visual Scripting** | `com.unity.visualscripting` | `1.9.9` | Có trong `manifest.json`, không dùng trong gameplay architecture (toàn bộ logic viết bằng C#). |
| **Testing** | `com.unity.test-framework` | `1.6.0` | Chỉ có package mặc định; **không có** thư mục `Tests/` hay bất kỳ unit/playmode test custom nào trong project. |
| **Services / Khác** | `com.unity.ads` (`4.16.4`), `com.unity.analytics` (`3.8.2`), `com.unity.recorder` (`5.1.6`), `com.unity.formats.alembic` (`2.4.4`), `com.unity.formats.fbx` (`5.1.5`) | Nhiều bản | Có cài đặt (`Assets/Resources/BillingMode.json` tồn tại), không tham gia vào gameplay loop. |

---

## 2. Cấu trúc Repository & Assembly Definitions

### 2.1. Assembly Definition (`.asmdef`)
- **Kết quả kiểm tra:** Có **0 file `.asmdef`** trong toàn bộ `Assets/`.
- **Hệ quả kiến trúc:**
  - Toàn bộ **110 script runtime** trong `Assets/Scripts/` (cùng các script của `Assets/TextMesh Pro/` và runtime `Assets/Third_Assets/`) đều biên dịch chung vào một assembly mặc định duy nhất: **`Assembly-CSharp.csproj`**.
  - Script editor duy nhất `Assets/Editor/FindMissingScripts.cs` (cùng 2 script editor trong `Third_Assets`) biên dịch vào **`Assembly-CSharp-Editor.csproj`**.
  - Không có ranh giới module ở tầng compiler (mọi namespace trong `Assets/Scripts/` có thể tham chiếu chéo lẫn nhau tự do).
  - **Phát hiện file rác ở root:** Các file `ALP8310_ControllerGlobal*.csproj` ở root project là project file cũ còn sót lại từ ngày `2026-06-17` trỏ tới `Assets/ALP_Assets/Big Oak Tree FREE/...` (thư mục này hiện không còn tồn tại trong `Assets/`).

### 2.2. Cấu trúc thư mục `Assets/`
```text
Assets/
├── _Recovery/                  # Scene backup tự động (0.unity)
├── Character/                  # Animator Controllers (.controller), Animation Clips, Render Textures
│   ├── Animation/
│   │   ├── Boss/               # 7 controllers: BossCutscene, Final_Boss, Juggernaut, Nordic_Phase_I..III, Underwater
│   │   ├── Player/             # 4 controllers: Player, PlayerClone, Player_Proxy, Player_UI
│   │   └── UI/                 # 1 controller: TutorialAnimation
│   └── Render Texture/
├── Compositor/                 # DefaultCompositionGraph.asset
├── Editor/                     # 1 editor script: FindMissingScripts.cs
├── HDRPDefaultResources/       # Tài nguyên mặc định của HDRP package
├── Media/                      # Audio clips / AudioResources / Video / Sprites
├── MobileDependencyResolver/   # Plugin Google Play Services resolver
├── Prefabs/                    # 39 prefabs của dự án
│   ├── Audio/                  # Audio.prefab (PooledObject cho SFX)
│   ├── Boss/                   # Armature/Enemy, Juggernaut, Nordic (Phase I/II/III), Underwater, Phase_II_AOE, PullBack
│   ├── Player/                 # Player, Player_Clone, Player_Model, PlayerModelUI
│   ├── Proxy/                  # Enemy_Proxy, Player_Proxy (dùng cho Timeline/Cutscene)
│   ├── Weapon/                 # FireBullet, FireExplosion, PhantomRetreat, SoulSword, Sword, Twinblade, Katana/*, VFX/*
│   └── (Root Prefabs)          # Cube, Global Volume, Lock Gate, Plane (1), Sphere, Continue Button.controller
├── Resources/                  # Chỉ chứa BillingMode.json (không dùng Resources.Load trong gameplay)
├── Scenes/                     # 3 scenes chính trong Build Settings
│   ├── Start.unity             # Scene index 0: Menu khởi đầu, nơi khởi tạo GameManagers & SaveManagers
│   ├── Main.unity              # Scene index 1: Thế giới chính, toàn bộ Boss Rush encounters, Player, UI, Pooling
│   └── LoadScene.unity         # Scene index 2: Màn hình loading bất đồng bộ (ProgressUI)
├── Scripts/                    # 110 C# scripts chia theo 12 thư mục chức năng
│   ├── Attack/                 # Combat hit detection, WeaponHandler, Skill Factory, Status (HP/MP/Stamina/Potion/Award), Separate projectiles
│   ├── Audio/                  # AudioManagers, PlayerSFX, ButtonSound
│   ├── Cutscene/               # CutsceneManagers, TimelineEvent
│   ├── Design Pattern/         # EventBus, Object Pooling, Observer (TickSystem), StateMachine (Base/Player/Boss/PlayerClone), Tree Behavior
│   ├── Environment/            # ActiveCheckPoint
│   ├── Input/                  # InputController.inputactions, InputController.cs, InputReader.cs, InputBuffering.cs
│   ├── Interact/               # CheckPoint
│   ├── Managers/               # GameManagers, SaveManagers, SaveData, BossPhaseManager, GraphicManager, AudioSetting, GameButtonManagers
│   ├── Physics/                # ForceReceiver
│   ├── ScriptableObject/       # AttackDatas.cs (không có instance .asset thực tế)
│   ├── Target/                 # Target, Targeter
│   └── UI/                     # UIPlayerManagers, WorldUIManager, GraphicsUI, StartMenuFeatures, ProgressUI, TriggerActiveTutorials, Billboard, ChangeState
├── Settings/                   # Cấu hình URP Render Pipeline & Volume Profiles (PC, Mobile, Web, Default)
├── Terrain/                    # New Terrain.asset
├── TextMesh Pro/               # Tài nguyên và 34 script ví dụ của TMP
├── Third_Assets/               # Tài nguyên bên thứ ba (Character models, Hovl/Vefects VFX, UI icons, Weapons) - 5 scripts
└── TimeLine/                   # 6 Playable assets (.playable): BeforeCombat, BossChokeNeck, EndGame, Fade Cutscene, PhaseTwoCutScene, PlayerChokeNecked
```

---

## 3. Thống kê File Mã Nguồn & Phạm Vi Đã Kiểm Tra

| Hạng mục | Số lượng tìm thấy | Số lượng đã đọc & phân tích | Tỷ lệ khảo sát | Ghi chú |
|---|---:|---:|---:|---|
| **Custom Gameplay Scripts (`Assets/Scripts/*.cs`)** | **110** | **110** | **100%** | Đã đọc toàn bộ 109 file viết tay + xác minh file sinh tự động `InputController.cs` (2.331 dòng). |
| **Custom Editor Scripts (`Assets/Editor/*.cs`)** | **1** | **1** | **100%** | `Assets/Editor/FindMissingScripts.cs`. |
| **Third-Party / TMP Scripts (`TextMesh Pro`, `Third_Assets`)** | **39** | **5 (liệt kê & phân loại)** | **100% phạm vi** | 34 file ví dụ TextMesh Pro + 5 file demo/shader utility của `Third_Assets` (không thuộc gameplay RE-END). |
| **Tổng số file `.cs` trong `Assets/`** | **150** | **111 custom + 39 phân loại** | **100%** | Không bỏ sót bất kỳ script custom nào. |
| **Scenes dự án (`Assets/Scenes/*.unity`)** | **3** | **3 (YAML & BuildSettings)** | **100%** | `Start.unity`, `Main.unity`, `LoadScene.unity` — đã quét toàn bộ script GUID và missing script (`0` null `m_Script`). |
| **Prefabs dự án (`Assets/Prefabs/**/*.prefab`)** | **39** | **39 (YAML & GUID check)** | **100%** | Đã kiểm tra cấu trúc script đính kèm, Prefab Variant chain và missing script (`0` null `m_Script`). |
| **Animator Controllers dự án (`Assets/Character/**/*.controller`)** | **12** (+1 ở `Prefabs`) | **13 (GUID check)** | **100%** | Đã quét StateMachineBehaviour GUID (`ChangeState.cs`) và xác nhận không có missing script. |
| **Tài liệu lịch sử trong Workspace (`Audit Re-End/*.md`)** | **3** | **3** | **100%** | Dùng để đối chiếu sự khác biệt giữa tài liệu cũ (20/10 & 29/10) với mã nguồn thực tế hiện tại (09/11). |

---

## 4. Danh Mục Hệ Thống (Phân Loại Theo Nguyên Tắc 7)

### A. Hệ thống ĐÃ XÁC MINH tồn tại và hoạt động trong mã nguồn hiện tại
1. **Lifecycle, Scene & Progression Orchestration (`Manager`):**
   - `GameManagers.cs` (`DontDestroyOnLoad` singleton khởi tạo từ `Start.unity`), điều phối New Game / Continue / Respawn / AutoSave / Scene transitions (`Start` ↔ `LoadScene` ↔ `Main`).
   - `BossPhaseManager.cs` điều phối các phase boss (`List<BossBehaviorBrain> phases`), cổng khóa arena (`lockGateParticle`), phần thưởng `spiritualPower` và kết thúc game (`PlayEndTimeline`).
   - `CutsceneManagers.cs` và `TimelineEvent.cs` điều phối trigger vào boss arena và chạy `PlayableDirector`.
2. **JSON Save/Load System (`Manager`):**
   - `SaveManagers.cs` (`DontDestroyOnLoad` singleton) đọc/ghi `Application.persistentDataPath/ReSave.json` thông qua `JsonUtility` với DTO `SaveData.cs`.
3. **Input System & Input Buffering (`PlayerInput` & Global):**
   - `InputController.inputactions` + `InputReader.cs` (`InputController.IPlayerActions`) chuyển đổi callback của Unity Input System thành C# events/properties.
   - `InputBuffering.cs` lưu timestamp lần nhấn cuối (`Dictionary<ActionType, float>`) cho `Attack`, `Jump`, `Dodge` và tiêu thụ qua `TryConsume(ActionType)`.
4. **Player Controller & Flat Finite State Machine (`Design_Pattern.StateMachine.Player`):**
   - Kế thừa từ `StateMachine.cs` (`Base`) → `PlayerStateMachine.cs` (`ICaster`) và `PlayerBaseState.cs` (`State`).
   - Bao gồm **17 state cụ thể** (`FreeLookState`, `PlayerTargetState`, `PlayerAttackState`, `PlayerHeavyAttackState`, `PlayerCounterAttackState`, `PlayerDodgeState`, `PlayerStartJumpState`, `PlayerFallState`, `PlayerLandingState`, `PlayerUseSkillState`, `PlayerUsePotionState`, `PlayerUseSubPotionState`, `PlayerChangeAction`, `PlayerHitState`, `PlayerAffectedState`, `PlayerActiveCheckPointState`, `PlayerDeathState`).
5. **Boss AI: Behavior Tree + Boss State Machine (`Design_Pattern.Tree_Behavior` & `Design_Pattern.StateMachine.Boss`):**
   - **Bộ não quyết định (Behavior Tree):** `BossBehaviorBrain.cs` chạy cây `BehaviorSelector` / `BehaviorSequence` gồm **11 leaf nodes** (`CheckBossHealthNode`, `CheckPlayerHealthNode`, `CheckChangePhaseNode`, `CheckUltimateNode`, `CheckCounterAttackNode`, `CheckAttackRangeNode`, `TaskChangePhaseNode`, `TaskUltimateAttackNode`, `TaskCounterAttackNode`, `TaskAttackingNode`, `TaskChasingNode`).
   - **Bộ thực thi (Boss FSM):** `BossStateMachine.cs` (`ICaster`, `ICombatInput`) và `BossBaseState.cs` điều khiển **6 state** (`BossLocomotionState`, `BossAttackState`, `BossUltimateState`, `BossCounterAttackState`, `BossHitState`, `BossDeathState`).
   - Áp dụng trên 3 Boss trong `Main.unity` / `Assets/Prefabs/Boss/`: **Juggernaut**, **Underwater**, và **Nordic** (3 Phase: `Nordic_Phase_I`, `Nordic_Phase_II`, `Nordic_Phase_III`).
6. **PlayerClone AI State Machine (`Design_Pattern.StateMachine.PlayerClone`):**
   - Triệu hồi bởi kỹ năng `PhantomMirage`, điều khiển bởi `PlayerCloneStateMachine.cs` + `CloneBehaviour.cs` (tự trả về pool sau `timeToRelease`), gồm **4 state** (`PlayerCloneIdleState`, `PlayerCloneChasingState`, `PlayerCloneAttackState`, `PlayerCloneHitState`).
7. **Combat Hit Detection (`Attack`):**
   - `WeaponTrail.cs` thực hiện quét va chạm mỗi `LateUpdate()` kết hợp `Physics.OverlapBox` (tại vị trí hiện tại) và `Physics.BoxCastAll` (quét từ `previousPosition` đến `currentPosition` khi `sweepDistance > 0.01f`).
   - `WeaponHandler.cs` (`WeaponHanldler.cs`) bật/tắt GameObject chứa collider vũ khí và mesh vũ khí thông qua Animation Events.
8. **Perfect Dodge & Counter Attack (`Status.Award`, `Status`, `Player`):**
   - Trong `PlayerDodgeState.cs`, `Health.isPerfectDodge = true` được bật từ đầu cú né cho tới khi `remainingTime <= 0.3f`.
   - Khi `WeaponTrail.DealDamage` chạm mục tiêu đang có `isPerfectDodge == true`, `Health.PerfectDodgeAward()` kích hoạt bất tử tạm thời (`noDamage = true`), slow-motion (`Time.timeScale = 0.5f` trong `0.8s` realtime) và gọi `DodgeAward.DodgeAwardActive()` (1 trong 3 nhánh: tăng tốc `IsMovementPush`, hồi 50% stamina đã mất `IsRecoveryStamina`, hoặc mở cờ `IsCounterAttack` để nhấn Attack vào `PlayerCounterAttackState`).
9. **Skill System, Skill Factory, Cooldown & Selection (`Attack.Skill_Factory`, `Attack`):**
   - `SkillFactory.cs` (static switch factory) khởi tạo **15 lớp `ISkill`** trong `Skill.cs` (6 skill của Player: `Inescapable`, `Indestructible`, `Invisible`, `WorldBreaker`, `PhantomRetreat`, `PhantomMirage`; 9 skill của Boss: `ThrowSword`, `JumpToSword`, `SwordAround`, `PullBack`, `ChokeNeck`, `FirstAoe`, `FireBullet`, `TransformToTwoSword`, `TransformToBlade`).
   - `SkillActive.cs` quản lý 3 nhóm kỹ năng của Player (`ChangingTheGame`, `Escape`, `Response`), chọn kỹ năng từ UI và đếm ngược cooldown bằng Coroutine (`CountCoolDownCoroutine`).
10. **Event Bus & Animation Event Bridge (`Design_Pattern.EventBus`):**
    - `GameEventManagers.cs`: Singleton Event Bus phát sự kiện `OnSkillCasted(ICaster, SkillEffect)` giữa Player và Boss.
    - `ManageAnimationSkillEvent.cs` + `ChangeState.cs` (`StateMachineBehaviour` gắn trên các state của Animator Controller): Cầu nối truyền sự kiện theo mốc `normalizedTime` (`SituationEvent`, `NextActionEvent`, `SlashWeaponEvent`, `ReleasePoolObjectEvent`) từ Animator sang Skill/FSM.
11. **Object Pooling (`Script.Design_Pattern.Object_Pooling`, `Attack`):**
    - `ObjectPooling.cs` quản lý `Dictionary<string, Stack<PooledObject>>`, cấp phát VFX, Projectile (`SwordSkill`, `Bullet`), `PlayerClone`, và `Audio` (`Audio.prefab`).
    - Tự động thu hồi khi ParticleSystem dừng qua `SkillPooledObject.cs` (`OnParticleSystemStopped`) hoặc theo thời gian qua `CloneBehaviour.cs`.
12. **Status, Potions, Lock-on Camera, Audio & UI Systems:**
    - `Health.cs`, `Mana.cs`, `Stamina.cs`, `Potion.cs` (`HealthPotion`, `ManaPotion`), `SubPotion.cs`.
    - `Targeter.cs` + `Target.cs` (tích hợp `CinemachineTargetGroup`).
    - `AudioManagers.cs`, `PlayerSFX.cs`, `AudioSetting.cs`.
    - `UIPlayerManagers.cs`, `WorldUIManager.cs`, `BossHealthUIManagers.cs`, `GraphicsUI.cs`, `StartMenuFeatures.cs`, `ProgressUI.cs`, `TriggerActiveTutorials.cs`, `Billboard.cs`.

---

### B. Hệ thống / Thành phần CHỈ XUẤT HIỆN TRONG TÀI LIỆU, STUB HOẶC BỎ DỞ (Dead / Unused / Stub Code)
1. **ScriptableObject `AttackDatas.cs` (`Assets/Scripts/ScriptableObject/AttackDatas.cs`):**
   - Có `[CreateAssetMenu]` và kế thừa `ScriptableObject`, **nhưng**:
     - Các auto-property (`AnimationName`, `Force`, v.v.) không có `[field: SerializeField]` nên Unity Inspector không thể serialize.
     - Không có bất kỳ file `.asset` nào được tạo từ `AttackDatas` trong toàn project.
     - Hệ thống thực tế dùng class `[Serializable] public class AttackData` (`Assets/Scripts/Attack/AttackData.cs`) nhúng trực tiếp trong Inspector của `PlayerStateMachine`, `BossStateMachine` và `PlayerCloneStateMachine`.
2. **Observer `TickSystem.cs` (`Assets/Scripts/Design Pattern/Observer/TickSystem.cs`):**
   - Được gắn vào một GameObject trong `Main.unity` và phát `OnUpdateSystem?.Invoke(Time.deltaTime)` mỗi `Update()`, **nhưng không có bất kỳ class nào trong toàn bộ project đăng ký (`+=`) vào `TickSystem.OnUpdateSystem`**.
3. **Class rỗng / Stub không có thân hàm:**
   - `AttackDealsDamge.cs` (`Assets/Scripts/Attack/AttackDealsDamge.cs`): Chỉ có `Start()` và `Update()` rỗng, không được gắn vào prefab/scene nào.
   - `ButtonSound.cs` (`Assets/Scripts/Audio/ButtonSound.cs`): Chỉ khai báo 3 trường `AudioSource` serialized, không có bất kỳ method nào (âm thanh nút bấm thực tế được xử lý bởi `WorldUIManager.PlayClickSound1..4` và `AudioManagers.PlayUISound`).
   - `ChokeNeck` skill (`Assets/Scripts/Attack/Skill Factory/Skill.cs:317-329`): Method `Cast(ICaster caster)` chỉ lấy biến cục bộ `bossStateMachine` và `player` rồi kết thúc, không thực hiện hành động nào (dù dự án có `BossChokeNeckTimeline.playable` và `PlayerChokeNeckedTimeline.playable` trong `Assets/TimeLine/`).
4. **Interface `ICombatInput.cs` (`Assets/Scripts/Design Pattern/Tree Behavior/Dependency Injection/ICombatInput.cs`):**
   - Được implement bởi `BossStateMachine` và `PlayerCloneStateMachine`, nhưng **không có bất kỳ class nào (kể cả các node trong Behavior Tree) gọi thông qua interface `ICombatInput`** — tất cả các node đều giữ tham chiếu trực tiếp tới kiểu cụ thể `BossStateMachine`.
5. **Trạng thái / Trường khai báo nhưng không bao giờ khởi tạo hoặc sử dụng:**
   - `PlayerStateMachine.AttackState5` và `PlayerStateMachine.ChangeAction` (`PlayerStateMachine.cs:147,149`): Khai báo property nhưng không được khởi tạo trong `SetupState()`.
   - `PlayerHitState` có nhánh `_isKnockBack` (`PlayerHitState.cs:14,28`) và `PlayerStateMachine.TimeToGetKnockBackHit` (`:55`), nhưng `SetupState()` luôn khởi tạo `new PlayerHitState(this, false)` và không có nơi nào tạo `PlayerHitState(this, true)`.
   - `PlayerCloneHitState.cs`: Được định nghĩa nhưng không có nơi nào trong `PlayerCloneStateMachine` hoặc `Health` chuyển sang `PlayerCloneHitState`.
   - `ChangeState.cs:79-83`: Logic gán `isRightWeaponVFX` / `isBothWeaponVFX` cho `BossStateMachine` đã bị comment out.

---

### C. Hệ thống CHƯA TÌM THẤY (Hoặc Đã Thay Đổi So Với Tài Liệu Cũ)
1. **Không có Hierarchical State Machine (HFSM) hay FixedUpdate State Tick:**
   - Base `StateMachine.cs` là FSM phẳng (1 biến `currentState` duy nhất) và chỉ gọi `currentState?.Tick(Time.deltaTime)` trong `Update()`.
2. **Không có hệ thống Parry / Posture / Stagger bar / Loadout đổi nhiều vũ khí của Player:**
   - Những mục này chỉ xuất hiện trong kế hoạch nâng cấp (`RE-END_Audit_And_Upgrade_Plan_2026-10-20.md` mục N05–N07), chưa tồn tại trong mã nguồn. (Chỉ Boss Nordic có kỹ năng chuyển đổi model kiếm `TransformToTwoSword` / `TransformToBlade` qua `WeaponHandler`).
3. **Các lỗi trong tài liệu cũ (20/10 & 29/10) ĐÃ ĐƯỢC SỬA trong mã nguồn hiện tại (cần lưu ý để tránh báo cáo nhầm):**
   - `PlayerAttackState.Enter()` (`PlayerAttackState.cs:19-20`) **đã có** `_previousTime = 0f; _alreadyApplyForce = false;`.
   - `PlayerStateMachine.OnDisable()` (`PlayerStateMachine.cs:196`) **đã dùng** `-= HandleEffectedState` (không còn dùng `+=`).
   - `PlayerDeathState.Tick()` (`PlayerDeathState.cs:35-45`) **đã có** cờ `_hasFinished` chặn gọi fade lặp lại và bỏ chặn trên `<= 1f` ở điều kiện `normalizeTime > .99f`.
   - `GraphicManager.LoadApplyAll()` (`GraphicManager.cs:188`) **đã đảo lại** điều kiện `if (!SaveManagers.Instance.HaveSaveData())`.

---

## 5. Những Khu Vực Chưa Thể Khảo Sát (Giới Hạn Giai Đoạn 1)

1. **Hành vi Runtime / Play Mode & Frame Timing thực tế:**
   - Theo nguyên tắc bắt buộc số 2, không chạy Unity Editor Play Mode, không build và không chạy Profiler. Do đó các hiện tượng phụ thuộc tốc độ khung hình (30/60/144 FPS), va chạm vật lý liên tục, và animation transition blending chỉ được xác minh trên cơ sở logic tĩnh.
2. **Chi tiết cấu hình sâu bên trong file `.unity` / `.playable` / `.controller` lớn:**
   - Đã quét tự động toàn bộ `Start.unity`, `LoadScene.unity`, `Main.unity`, 39 Prefabs và 13 Animator Controllers để lập bản đồ script GUID và kiểm tra Missing Script (`0` missing script).
   - Chưa đọc từng dòng YAML của `Main.unity` (file scene rất lớn) cho mọi thông số serialized chi tiết của từng Boss/UI Button hay từng track của 6 file `.playable`, trừ các liên kết kiến trúc cốt lõi. Những điểm cần kiểm chứng sâu trong YAML serialized (như `m_OnClick` của UI Button, `PoolItem` list của `ObjectPooling` trong `Main.unity`, hay Animation Events trong các clip `.anim`) được liệt kê rõ trong `02_Handoff_To_Opus.md` để Giai đoạn 2 kiểm tra có trọng tâm.
