# RE-END — 02. Technical Handoff Document (Phase 1 → Phase 2 Deep Audit)

- **Ngày bàn giao:** 2026-11-09
- **Unity Project Root:** `C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush`
- **Audit Workspace Root:** `C:/Users/Official_41/Desktop/Audit Re-End`
- **Trạng thái Giai đoạn 1:** Hoàn tất khảo sát kiến trúc tĩnh 100% mã nguồn custom (`111/111` file `.cs`), 3 Scenes, 39 Prefabs và 13 Animator Controllers.

---

## 1. Tóm Tắt Kỹ Thuật Dành Cho Model Phân Tích Giai Đoạn 2 (Opus)

1. **Quy mô & Cấu trúc biên dịch:**
   - Dự án Unity **6000.3.8f1** sử dụng **URP 17.3.0**, **Input System 1.18.0**, **Cinemachine 3.1.7** và **Timeline 1.8.13**.
   - **Không sử dụng `.asmdef`**: Toàn bộ 110 script runtime trong `Assets/Scripts/` nằm chung trong `Assembly-CSharp.csproj`.
2. **Kiến trúc Gameplay cốt lõi:**
   - **Player:** Điều khiển bởi Flat FSM (`StateMachine.cs` → `PlayerStateMachine.cs` + 17 states kế thừa `PlayerBaseState.cs`), nhận dữ liệu từ `InputReader.cs` + `InputBuffering.cs` và di chuyển qua `CharacterController` + `ForceReceiver.cs`.
   - **Boss AI:** Kiến trúc 2 tầng gồm **Behavior Tree** (`BossBehaviorBrain.cs` chạy `BehaviorSelector` / `BehaviorSequence` với 11 leaf nodes mỗi `Update`) điều khiển các cờ boolean trên **Boss FSM** (`BossStateMachine.cs` + 6 states kế thừa `BossBaseState.cs`).
   - **Combat & Hit Detection:** Không dùng trigger collider thụ động cho đòn chém thường mà dùng `WeaponTrail.cs` quét chủ động mỗi `LateUpdate()` bằng `Physics.OverlapBox` + `Physics.BoxCastAll`. Animation Events trên clip gọi `WeaponHandler.cs` để bật/tắt GameObject chứa `WeaponTrail`.
   - **Skill & Event Bus:** `SkillFactory.cs` khởi tạo 15 class `ISkill` (trong `Skill.cs`). Giao tiếp hiệu ứng khống chế giữa Player và Boss đi qua singleton `GameEventManagers.OnSkillCasted(ICaster, SkillEffect)`. Đồng bộ thời điểm tung chiêu với khung hình animation đi qua `ChangeState.cs` (`StateMachineBehaviour`) → `ManageAnimationSkillEvent.cs`.
   - **Persistence & Encounter:** `GameManagers.cs` và `SaveManagers.cs` (`DontDestroyOnLoad` từ `Start.unity`) lưu/đọc JSON tại `Application.persistentDataPath/ReSave.json` (`SaveData.cs`). Các trận đánh Boss trong `Main.unity` được kích hoạt bởi `CutsceneManagers.cs` → `TimelineEvent.cs` → `BossPhaseManager.cs`.
3. **Lưu ý quan trọng về lịch sử sửa code (Snapshot 09/11/2026 so với báo cáo cũ 20/10 & 29/10):**
   - Một số lỗi từng nêu trong tài liệu cũ ở thư mục `Audit Re-End` **đã được sửa trong code hiện tại** (ví dụ: `PlayerAttackState.Enter` đã reset `_previousTime = 0f` và `_alreadyApplyForce = false`; `PlayerStateMachine.OnDisable` đã dùng `-=`; `PlayerDeathState` đã có cờ `_hasFinished`; `GraphicManager.LoadApplyAll` đã sửa điều kiện `if (!SaveManagers.Instance.HaveSaveData())`). **Model Giai đoạn 2 cần đọc theo kết quả xác minh mới nhất trong báo cáo này, không sao chép nhận định cũ nếu code đã thay đổi.**

---

## 2. Danh Sách Subsystem Cần Phân Tích Chuyên Sâu & Các File Trọng Tâm

### Subsystem 1: Combat Hit Detection, Damage Resolution & Defense Windows
- **File cần tập trung phân tích:**
  - `Assets/Scripts/Attack/WeaponTrail.cs` (`LateUpdate:40-64`, `DealDamage:72-90`)
  - `Assets/Scripts/Attack/WeaponHanldler.cs` (`OnActive/UnActive*Collider:23-53`)
  - `Assets/Scripts/Attack/Status/Health.cs` (`DealDamage:79-91`, `TimeFreeHit:99-105`, `SlowTime:107-114`, `PerfectDodgeAward:116-122`)
  - `Assets/Scripts/Attack/Status/Award/DodgeAward.cs` (`DodgeAwardActive:52-87`)
  - `Assets/Scripts/Design Pattern/StateMachine/Player/Main/PlayerDodgeState.cs` (`Enter:21-36`, `Tick:38-69`, `Exit:71-76`)
  - `Assets/Scripts/Design Pattern/StateMachine/Player/Main/PlayerCounterAttackState.cs` (`Enter:12-17`, `Tick:19-28`)
- **Rủi ro ban đầu & Lý do cần kiểm tra sâu:**
  1. Công thức `currentHealth = Mathf.Clamp(currentHealth - damage + resistance, 0, maxHealth)` (`Health.cs:83`) làm **tăng máu** nếu `damage < resistance` (đặc biệt khi dùng `PlayerHeavyAttackState` nhả nút sớm khiến `_holdTime` nhỏ hoặc khi `isReduceDame` chia nhỏ damage).
  2. `Health.DealDamage` không kiểm tra trạng thái đã chết (`currentHealth <= 0`) ở đầu hàm và luôn bắn `HitAction` trước `DeathAction` (`Health.cs:84-89`).
  3. Trong `PlayerCounterAttackState.Enter()` (`:12-17`), `WeaponTrail` của Player **không được bật** (`playerStateMachine.WeaponTrail.SetActive(true)` bị thiếu, khác với `PlayerAttackState.Enter:30` và `PlayerHeavyAttackState.Enter:33`), cần kiểm tra xem Animation clip `"Attack_3"` có bật collider qua `WeaponHandler` hay đòn Counter Attack không gây được sát thương.
  4. `Health.noDamage` là một biến `bool` dùng chung bị ghi đè bởi cả `PlayerDodgeState.Exit()` (`noDamage = false`, `:74`), `Health.SlowTime()` (`noDamage = true`, `:109`), `Indestructible.Cast()` (`Skill.cs:51, 57`) và `PlayerUseSkillState.ResetAfterSkill()` (`:77`).

### Subsystem 2: Player FSM, Input Buffering & Combo Transitions
- **File cần tập trung phân tích:**
  - `Assets/Scripts/Design Pattern/StateMachine/Base/State.cs` & `StateMachine.cs`
  - `Assets/Scripts/Design Pattern/StateMachine/Player/Base/PlayerStateMachine.cs` (`SetupState:165-184`, `Handle*:238-340`, `InvincibleState:367-376`)
  - `Assets/Scripts/Design Pattern/StateMachine/Player/Main/PlayerAttackState.cs` (`Tick:33-78`, `TryCombo:86-97`, `ActiveVfx:110-121`)
  - `Assets/Scripts/Design Pattern/StateMachine/Player/Main/PlayerHeavyAttackState.cs` (`Tick:36-50`, `CountHoldTime:60-78`)
  - `Assets/Scripts/Design Pattern/StateMachine/Player/Main/PlayerChangeAction.cs` (`Tick:56-89`, `Exit:91-101`)
  - `Assets/Scripts/Input/InputReader.cs` & `Assets/Scripts/Input/InputBuffering.cs`
- **Rủi ro ban đầu & Lý do cần kiểm tra sâu:**
  1. Trong `PlayerAttackState.Tick()` (`:47-63`), nhánh `TryConsume(ActionType.Attack)` gọi `TryCombo()` (bên trong gọi `SwitchState`) nhưng **không có `return;`**, dẫn tới việc vẫn kiểm tra `TryConsume(ActionType.Jump)` và chạy tiếp phần thân dưới của `Tick()` trên state cũ.
  2. `PlayerChangeAction.Tick()` (`:78-88`): Sau khối `if (isTarget) UpdateTargetAnimation(deltaTime); else UpdateFreeLookAnimation(deltaTime);`, dòng `87` lại gọi thêm một lần `UpdateFreeLookAnimation(deltaTime);` vô điều kiện. Đồng thời mỗi lần `Exit()` (`PlayerBaseState.cs:76-81`) lại cấp phát `new AnimatorOverrideController(playerStateMachine.Animator.runtimeAnimatorController)` bọc chồng lên controller cũ.
  3. `PlayerStateMachine.InvincibleState()` (`:367-376`, gọi bởi skill `WorldBreaker` trong `Skill.cs:148-149`): Nhân `FreeLookMovementSpeed *= 1.5f`, `FreeLookMovementSprintSpeed *= 0.8f` và `AttackDamage *= 2` cho mọi đòn đánh, nhưng **không bao giờ đặt lại `Invincible = false` hay hoàn trả tốc độ/sát thương gốc**.

### Subsystem 3: Boss AI (Behavior Tree + Boss FSM) & PlayerClone FSM
- **File cần tập trung phân tích:**
  - `Assets/Scripts/Design Pattern/Tree Behavior/Brain/BossBehaviorBrain.cs` (`ConstructBehaviorTree:61-104`)
  - `Assets/Scripts/Design Pattern/Tree Behavior/Base/BehaviorSequence.cs` (`Evaluate:14-34`)
  - Toàn bộ 11 file trong `Assets/Scripts/Design Pattern/Tree Behavior/Boss Leaf Node/*.cs`
  - `Assets/Scripts/Design Pattern/StateMachine/Boss/Base/BossStateMachine.cs` & `Main/*.cs`
  - `Assets/Scripts/Design Pattern/StateMachine/PlayerClone/Main/PlayerCloneIdleState.cs` (`Tick:19-30`)
- **Rủi ro ban đầu & Lý do cần kiểm tra sâu:**
  1. **`PlayerCloneIdleState.Tick()` (`PlayerCloneIdleState.cs:25-30`):**
     ```csharp
     if (IsAttackRange())
     {
         cloneStateMachine.SwitchState(new PlayerCloneAttackState(cloneStateMachine, 0));
     }
     cloneStateMachine.SwitchState(new PlayerCloneChasingState(cloneStateMachine));
     ```
     Thiếu `return;` trong khối `if (IsAttackRange())`, khiến `PlayerClone` vừa vào `AttackState` bị ghi đè ngay lập tức sang `ChasingState` trong cùng 1 frame!
  2. **`CheckCounterAttackNode.Evaluate()` (`CheckCounterAttackNode.cs:20-29`):** Gọi `Random.Range(EnduredTimes / 2, EnduredTimes + 1)` mỗi frame và so sánh bằng tuyệt đối `hitToNeed - HitReceived == 0`. Nếu `HitReceived` vượt quá `EnduredTimes` (ví dụ khi Boss đang ở state khác hoặc random không trúng trong vài frame), điều kiện `== 0` vĩnh viễn không bao giờ thỏa mãn nữa cho đến khi có gì đó reset `HitReceived`.
  3. **`BossAttackState.cs:26` & `BossUltimateState.cs:22`:** Dòng `// _previousTime = 0f;` đang bị comment, mặt khác điều kiện thoát/combo trong `Tick()` (`BossAttackState.cs:44`) chỉ chạy khi `normalizeTime >= _previousTime && normalizeTime <= 1f`. Nếu một animation không có `NextAttackDataIndex` hoặc `normalizeTime` vượt quá `1f` trước khi `TryCombo` hoàn tất, Boss có nguy cơ kẹt trong `BossAttackState` / `BossUltimateState`.

### Subsystem 4: Skill Factory, Event Bus, Animation Callbacks & Object Pooling
- **File cần tập trung phân tích:**
  - `Assets/Scripts/Attack/Skill Factory/Skill.cs` (toàn bộ 457 dòng)
  - `Assets/Scripts/Attack/Skill Factory/SkillActive.cs`
  - `Assets/Scripts/Design Pattern/EventBus/GameEventManagers.cs` & `ManageAnimationSkillEvent.cs`
  - `Assets/Scripts/UI/ChangeState.cs`
  - `Assets/Scripts/Design Pattern/Object Pooling/ObjectPooling.cs` & `PooledObject.cs`
  - `Assets/Scripts/Attack/Separate/SwordSkill.cs`, `Bullet.cs`, `TriggerSkillForBoss.cs`
- **Rủi ro ban đầu & Lý do cần kiểm tra sâu:**
  1. **Lỗi logic trong `Invisible.Cast()` (`Skill.cs:111-131`) & `PlayerUseSkillState.cs:82-88, 103-113`:**
     - Callback kết thúc sau 4s của `Invisible.Cast` viết:
       ```csharp
       player.Invisible = false;
       player.Invisible = true;
       ```
       (Gán `false` rồi gán lại `true` ngay dòng dưới!).
     - Còn `PlayerUseSkillState.ResetAfterSkill` sau 5s gán `player.Invisible = false;` và gọi `ResetToMainMaterial()`, nhưng `ResetToMainMaterial()` chỉ bật/tắt `HeadIndestructible` / `ArmIndestructible` / `TorsoIndestructible` chứ không đụng tới các mesh `*Invisible`.
  2. **Rò rỉ / Xung đột closure trên `ManageAnimationSkillEvent.SituationEvent`:**
     - `ThrowSword`, `PullBack`, `FirstAoe`, `TransformToTwoSword`, `TransformToBlade` đăng ký `manageEvent.SituationEvent += situationAction;` trong `Cast()`, nhưng `JumpToSword` và `FireBullet` lại gọi `manageEvent.ClearSituationEvent();`. Nếu Boss bị đánh trúng (`BossHitState`) trước khi animation phát `SituationEvent`, closure không được gỡ và sẽ chạy chồng ở đòn sau.
  3. **`PhantomRetreat.Cast()` (`Skill.cs:164-184`) & `Bullet.cs`:**
     - `Bullet.MovePosition` di chuyển các `Rigidbody` con của prefab `PhantomRetreat` đi xa khỏi gốc parent, nhưng khi `SkillPooledObject` trả `PhantomRetreat` về pool và tái sử dụng lần sau, vị trí cục bộ (`localPosition`) của các `Rigidbody` con không bao giờ được reset về tâm!
  4. **`SwordSkill.OnEnable()` (`SwordSkill.cs:27-34`):**
     - Khi `ObjectPooling.GetPooledObject` gọi `SetActive(true)` (`ObjectPooling.cs:94, 100`), `SwordSkill.OnEnable()` chạy **trước khi** caller gán `TargetPosition` và gọi `InitializeBullet()`, đồng thời `container.GetComponentInChildren<BossStateMachine>(true)` với `includeInactive = true` sẽ luôn lấy `Nordic_Phase_I` (dù đang ở Phase II hoặc Phase III).

### Subsystem 5: Save/Load, Encounter Progression, Audio & UI Settings
- **File cần tập trung phân tích:**
  - `Assets/Scripts/Managers/GameManagers.cs`, `SaveManagers.cs`, `SaveData.cs`, `BossPhaseManager.cs`, `GraphicManager.cs`, `AudioSetting.cs`
  - `Assets/Scripts/Cutscene/CutsceneManagers.cs`, `TimelineEvent.cs`
  - `Assets/Scripts/UI/UIPlayerManagers.cs`, `WorldUIManager.cs`, `GraphicsUI.cs`, `TriggerActiveTutorials.cs`
  - `Assets/Scripts/Attack/Status/Mana.cs`, `Stamina.cs`, `Potion/Potion.cs`, `Potion/SubPotion.cs`
- **Rủi ro ban đầu & Lý do cần kiểm tra sâu:**
  1. **Đồng bộ dữ liệu Save (`GameManagers.cs`, `SaveManagers.cs`, `SaveData.cs`):**
     - `CompletedList` (`HashSet<string>`) không được cập nhật trong `SaveIsDefeat()` và không được xóa trong `CreateNewSaveGame()` / `DeteleSaveGame()`.
     - `AutoSave()` chạy trước `Player.AddBossSpiritualPower(spiritualPower)` trong `BossPhaseManager.cs:94-100`.
     - `SaveData` thiếu `PlayerSpiritualPower` và `potionUsage`; `ApplySaveData` ghi đè `AttackData[0].AttackDamage` lên toàn bộ 4 đòn đánh trong `AttackData[]`.
     - Nếu người chơi chọn **New Game** rồi chết trước khi kích hoạt bất kỳ Checkpoint nào (`AutoSave()` chưa từng chạy), `ReturnCheckpoint()` sẽ gọi `ApplySaveData()` đọc từ `CurrentSaveData = new SaveData()` (mọi chỉ số `currentHealth`, `currentMana`, `currentStamina`, `currentDamage`, `posX/Y/Z` đều bằng `0`!).
  2. **Logic nâng cấp / hoàn điểm chỉ số (`Health.cs`, `Mana.cs`, `Stamina.cs`, `Potion.cs`, `SubPotion.cs`):**
     - `Mana.SubMana()` (`Mana.cs:45-55`) kiểm tra `if (currentMana == 1000)` thay vì `maxMana == 1000` (nếu người chơi đã dùng một ít mana rồi vào nâng cấp/trừ điểm, `maxMana` có thể bị trừ xuống dưới 1000).
     - `Potion.Awake()` (`Potion.cs:23-29`) quên khởi tạo `minimumPotionUsage = potionUsage;` (mặc định bằng `0`), khiến `SubtractUsagePotion()` (`:72-80`) cho phép trừ `potionUsage` về tới `0`.
  3. **Đồng bộ UI Settings giữa 2 bảng (`Checkpoint System UI` vs `Non-Checkpoint Settings UI`):**
     - `GraphicsUI.BindListener()` (`GraphicsUI.cs:111-179`) chỉ gắn `onValueChanged.AddListener` cho các dropdown/toggle của bảng Checkpoint (`resolutionDropdown`, `vsyncToggle`, v.v.), **hoàn toàn không gắn listener cho các control `*NonCheckpoint`** (`resolutionDropdownNonCheckpoint`, `vsyncToggleNonCheckpoint`, v.v.)! Do đó mở menu Settings bằng phím ESC ngoài Checkpoint sẽ không thay đổi được cấu hình đồ họa bằng chuột.

---

## 3. Những Thông Tin Chưa Được Xác Minh (Cần Kiểm Chứng Trong Giai Đoạn 2)

1. **Liên kết `m_OnClick` của các Button mua/nâng cấp trong `Assets/Scenes/Main.unity`:**
   - Tài liệu lịch sử (20/10) từng ghi nhận 3 nút Add trong tab SubPotion (`Blood Pomegranate`, `Golden Pear`, `Emerald Guava`) có `m_Target: {fileID: 0}` trong `Main.unity`. Cần kiểm tra trực tiếp đoạn YAML của các button này trong `Main.unity` xem đã được nối lại trong Inspector hay chưa.
2. **Danh sách `poolItemList` của `ObjectPooling` trong `Assets/Scenes/Main.unity`:**
   - Cần đối chiếu các chuỗi tên gọi `GetPooledObject` / `SpawnSkill` trong code (`"Audio"`, `"Basic_Sword_Slash_I"`, `"Inescapable"`, `"Indestructible"`, `"WorldBreaker"`, `"PhantomRetreat"`, `"PhantomMirage"`, `"PlayerClone"`, `"ThrowSword"`, `"PullBack"`, `"FirstAoe"`, `"FireBullet"`, và `AttackData.VfxName`) với `poolItemList` được serialize trong `Main.unity` để xác nhận có tên nào bị lệch chính tả gây trả về `null` hay không.
3. **Animation Events & Tags trong Animator Controllers (`Assets/Character/Animation/**/*.controller`):**
   - Cần kiểm tra xem clip `"Attack_3"` (dùng bởi `PlayerCounterAttackState`) và các clip của Boss/PlayerClone có đầy đủ tag `"Attack"` / `"Hit"` / `"Death"` và Animation Event bật/tắt `WeaponHandler` hay không.

---

## 4. Đề Xuất Thứ Tự Phân Tích Giai Đoạn 2 (Phase 2 Execution Plan)

1. **Bước 2.1 — Kiểm toán Core Loop & Save/Progression Integrity:**
   - Phân tích sâu `GameManagers`, `SaveManagers`, `SaveData`, `CutsceneManagers`, `BossPhaseManager`, `CheckPoint`, `ActiveCheckPoint`.
2. **Bước 2.2 — Kiểm toán Combat Engine, Hit Detection & Status/Defense:**
   - Phân tích sâu `WeaponTrail`, `WeaponHandler`, `Health`, `Mana`, `Stamina`, `DodgeAward`, `Potion`, `SubPotion`.
3. **Bước 2.3 — Kiểm toán Player FSM, Input Buffering & Skills:**
   - Phân tích sâu `PlayerStateMachine`, 17 Player States, `InputReader`, `InputBuffering`, `SkillFactory`, `Skill`, `SkillActive`, `ObjectPooling`.
4. **Bước 2.4 — Kiểm toán Boss AI (BT + Boss FSM), PlayerClone & Cutscene/Timeline:**
   - Phân tích sâu `BossBehaviorBrain`, 11 BT Leaf Nodes, `BossStateMachine`, 6 Boss States, `PlayerCloneStateMachine`, `SwordSkill`, `TriggerSkillForBoss`, `ChangeState`, `ManageAnimationSkillEvent`.
5. **Bước 2.5 — Kiểm toán Scene/Prefab Serialized Wiring & UI/Audio:**
   - Kiểm tra có mục tiêu trong `Main.unity` (danh sách `ObjectPooling.poolItemList`, `m_OnClick` của UI nâng cấp/mua đồ, cấu hình `GraphicsUI` & `AudioSetting`).

---

## 5. Danh Sách Toàn Bộ File Đã Đọc Trong Giai Đoạn 1 (Tránh Đọc Lại Tuần Tự Không Cần Thiết)

Toàn bộ **111/111 file C# custom** dưới đây đã được đọc đầy đủ trong Giai đoạn 1. Model ở Giai đoạn 2 chỉ cần mở lại các file cụ thể khi cần trích xuất đoạn code sửa lỗi chi tiết:

### Cấu hình & Tài liệu
- `ProjectSettings/ProjectVersion.txt`
- `ProjectSettings/EditorBuildSettings.asset`
- `Packages/manifest.json`
- `.gitignore`
- `ALP8310_ControllerGlobal.csproj` (xác nhận project file rác cũ)
- `C:/Users/Official_41/Desktop/Audit Re-End/SOURCE_REVIEW.md`
- `C:/Users/Official_41/Desktop/Audit Re-End/UPDATE_NOTES_2026-10-29.md`
- `C:/Users/Official_41/Desktop/Audit Re-End/RE-END_Audit_And_Upgrade_Plan_2026-10-20.md`
- `C:/Users/Official_41/Desktop/Audit Re-End/interview_cheatsheet_concise.md` (120 dòng đầu)

### `Assets/Editor/` (1 file)
- `Assets/Editor/FindMissingScripts.cs`

### `Assets/Scripts/Managers/` (7 files)
- `Assets/Scripts/Managers/AudioSetting.cs`
- `Assets/Scripts/Managers/BossPhaseManager.cs`
- `Assets/Scripts/Managers/GameButtonManagers.cs`
- `Assets/Scripts/Managers/GameManagers.cs`
- `Assets/Scripts/Managers/GraphicManager.cs`
- `Assets/Scripts/Managers/SaveData.cs`
- `Assets/Scripts/Managers/SaveManagers.cs`

### `Assets/Scripts/Input/`, `Physics/`, `Target/`, `Environment/`, `Interact/`, `Cutscene/`, `Audio/`, `ScriptableObject/` (14 files)
- `Assets/Scripts/Input/InputBuffering.cs`
- `Assets/Scripts/Input/InputController.cs` (auto-generated header verified)
- `Assets/Scripts/Input/InputReader.cs`
- `Assets/Scripts/Physics/ForceReceiver.cs`
- `Assets/Scripts/Target/Target.cs`
- `Assets/Scripts/Target/Targeter.cs`
- `Assets/Scripts/Environment/ActiveCheckPoint.cs`
- `Assets/Scripts/Interact/CheckPoint.cs`
- `Assets/Scripts/Cutscene/CutsceneManagers.cs`
- `Assets/Scripts/Cutscene/TimelineEvent.cs`
- `Assets/Scripts/Audio/AudioManagers.cs`
- `Assets/Scripts/Audio/ButtonSound.cs`
- `Assets/Scripts/Audio/PlayerSFX.cs`
- `Assets/Scripts/ScriptableObject/AttackDatas.cs`

### `Assets/Scripts/Attack/` (23 files)
- `Assets/Scripts/Attack/AttackData.cs`
- `Assets/Scripts/Attack/AttackDealsDamge.cs`
- `Assets/Scripts/Attack/BossHealthUIManagers.cs`
- `Assets/Scripts/Attack/ManageAttackAnimation.cs`
- `Assets/Scripts/Attack/SkillPooledObject.cs`
- `Assets/Scripts/Attack/WeaponHanldler.cs`
- `Assets/Scripts/Attack/WeaponTrail.cs`
- `Assets/Scripts/Attack/Separate/Bullet.cs`
- `Assets/Scripts/Attack/Separate/SwordSkill.cs`
- `Assets/Scripts/Attack/Separate/TriggerSkillForBoss.cs`
- `Assets/Scripts/Attack/Skill Factory/ICaster.cs`
- `Assets/Scripts/Attack/Skill Factory/ISkill.cs`
- `Assets/Scripts/Attack/Skill Factory/Skill.cs`
- `Assets/Scripts/Attack/Skill Factory/SkillActive.cs`
- `Assets/Scripts/Attack/Skill Factory/SkillFactory.cs`
- `Assets/Scripts/Attack/Status/Health.cs`
- `Assets/Scripts/Attack/Status/Mana.cs`
- `Assets/Scripts/Attack/Status/Stamina.cs`
- `Assets/Scripts/Attack/Status/Award/DodgeAward.cs`
- `Assets/Scripts/Attack/Status/Potion/HealthPotion.cs`
- `Assets/Scripts/Attack/Status/Potion/ManaPotion.cs`
- `Assets/Scripts/Attack/Status/Potion/Potion.cs`
- `Assets/Scripts/Attack/Status/Potion/SubPotion.cs`

### `Assets/Scripts/Design Pattern/` (58 files)
- `Assets/Scripts/Design Pattern/EventBus/GameEventManagers.cs`
- `Assets/Scripts/Design Pattern/EventBus/ManageAnimationSkillEvent.cs`
- `Assets/Scripts/Design Pattern/Object Pooling/GetSkill.cs`
- `Assets/Scripts/Design Pattern/Object Pooling/ObjectPooling.cs`
- `Assets/Scripts/Design Pattern/Object Pooling/PooledObject.cs`
- `Assets/Scripts/Design Pattern/Observer/TickSystem.cs`
- `Assets/Scripts/Design Pattern/StateMachine/Base/State.cs`
- `Assets/Scripts/Design Pattern/StateMachine/Base/StateMachine.cs`
- `Assets/Scripts/Design Pattern/StateMachine/Boss/Base/BossBaseState.cs`
- `Assets/Scripts/Design Pattern/StateMachine/Boss/Base/BossStateMachine.cs`
- `Assets/Scripts/Design Pattern/StateMachine/Boss/Main/BossAttackState.cs`
- `Assets/Scripts/Design Pattern/StateMachine/Boss/Main/BossCounterAttackState.cs`
- `Assets/Scripts/Design Pattern/StateMachine/Boss/Main/BossDeathState.cs`
- `Assets/Scripts/Design Pattern/StateMachine/Boss/Main/BossHitState.cs`
- `Assets/Scripts/Design Pattern/StateMachine/Boss/Main/BossLocomotionState.cs`
- `Assets/Scripts/Design Pattern/StateMachine/Boss/Main/BossUltimateState.cs`
- `Assets/Scripts/Design Pattern/StateMachine/Player/Base/PlayerBaseState.cs`
- `Assets/Scripts/Design Pattern/StateMachine/Player/Base/PlayerStateMachine.cs`
- `Assets/Scripts/Design Pattern/StateMachine/Player/Main/FreeLookState.cs`
- `Assets/Scripts/Design Pattern/StateMachine/Player/Main/PlayerActiveCheckPointState.cs`
- `Assets/Scripts/Design Pattern/StateMachine/Player/Main/PlayerAffectedState.cs`
- `Assets/Scripts/Design Pattern/StateMachine/Player/Main/PlayerAttackState.cs`
- `Assets/Scripts/Design Pattern/StateMachine/Player/Main/PlayerChangeAction.cs`
- `Assets/Scripts/Design Pattern/StateMachine/Player/Main/PlayerCounterAttackState.cs`
- `Assets/Scripts/Design Pattern/StateMachine/Player/Main/PlayerDeathState.cs`
- `Assets/Scripts/Design Pattern/StateMachine/Player/Main/PlayerDodgeState.cs`
- `Assets/Scripts/Design Pattern/StateMachine/Player/Main/PlayerFallState.cs`
- `Assets/Scripts/Design Pattern/StateMachine/Player/Main/PlayerHeavyAttackState.cs`
- `Assets/Scripts/Design Pattern/StateMachine/Player/Main/PlayerHitState.cs`
- `Assets/Scripts/Design Pattern/StateMachine/Player/Main/PlayerLandingState.cs`
- `Assets/Scripts/Design Pattern/StateMachine/Player/Main/PlayerStartJumpState.cs`
- `Assets/Scripts/Design Pattern/StateMachine/Player/Main/PlayerTargetState.cs`
- `Assets/Scripts/Design Pattern/StateMachine/Player/Main/PlayerUsePotionState.cs`
- `Assets/Scripts/Design Pattern/StateMachine/Player/Main/PlayerUseSkillState.cs`
- `Assets/Scripts/Design Pattern/StateMachine/Player/Main/PlayerUseSubPotionState.cs`
- `Assets/Scripts/Design Pattern/StateMachine/PlayerClone/Base/PlayerCloneBaseState.cs`
- `Assets/Scripts/Design Pattern/StateMachine/PlayerClone/Base/PlayerCloneStateMachine.cs`
- `Assets/Scripts/Design Pattern/StateMachine/PlayerClone/Main/PlayerCloneAttackState.cs`
- `Assets/Scripts/Design Pattern/StateMachine/PlayerClone/Main/PlayerCloneChasingState.cs`
- `Assets/Scripts/Design Pattern/StateMachine/PlayerClone/Main/PlayerCloneHitState.cs`
- `Assets/Scripts/Design Pattern/StateMachine/PlayerClone/Main/PlayerCloneIdleState.cs`
- `Assets/Scripts/Design Pattern/Tree Behavior/Base/BehaviorNode.cs`
- `Assets/Scripts/Design Pattern/Tree Behavior/Base/BehaviorSelector.cs`
- `Assets/Scripts/Design Pattern/Tree Behavior/Base/BehaviorSequence.cs`
- `Assets/Scripts/Design Pattern/Tree Behavior/Boss Leaf Node/CheckAttackRangeNode.cs`
- `Assets/Scripts/Design Pattern/Tree Behavior/Boss Leaf Node/CheckBossHealthNode.cs`
- `Assets/Scripts/Design Pattern/Tree Behavior/Boss Leaf Node/CheckChangePhaseNode.cs`
- `Assets/Scripts/Design Pattern/Tree Behavior/Boss Leaf Node/CheckCounterAttackNode.cs`
- `Assets/Scripts/Design Pattern/Tree Behavior/Boss Leaf Node/CheckPlayerHealthNode.cs`
- `Assets/Scripts/Design Pattern/Tree Behavior/Boss Leaf Node/CheckUltimateNode.cs`
- `Assets/Scripts/Design Pattern/Tree Behavior/Boss Leaf Node/TaskAttackingNode.cs`
- `Assets/Scripts/Design Pattern/Tree Behavior/Boss Leaf Node/TaskChangePhaseNode.cs`
- `Assets/Scripts/Design Pattern/Tree Behavior/Boss Leaf Node/TaskChasingNode.cs`
- `Assets/Scripts/Design Pattern/Tree Behavior/Boss Leaf Node/TaskCounterAttackNode.cs`
- `Assets/Scripts/Design Pattern/Tree Behavior/Boss Leaf Node/TaskUltimateAttackNode.cs`
- `Assets/Scripts/Design Pattern/Tree Behavior/Brain/BossBehaviorBrain.cs`
- `Assets/Scripts/Design Pattern/Tree Behavior/CloneBehaviour.cs`
- `Assets/Scripts/Design Pattern/Tree Behavior/Dependency Injection/ICombatInput.cs`

### `Assets/Scripts/UI/` (8 files)
- `Assets/Scripts/UI/Billboard.cs`
- `Assets/Scripts/UI/ChangeState.cs`
- `Assets/Scripts/UI/GraphicsUI.cs`
- `Assets/Scripts/UI/ProgressUI.cs`
- `Assets/Scripts/UI/StartMenuFeatures.cs`
- `Assets/Scripts/UI/TriggerActiveTutorials.cs`
- `Assets/Scripts/UI/UIPlayerManagers.cs`
- `Assets/Scripts/UI/WorldUIManager.cs`
