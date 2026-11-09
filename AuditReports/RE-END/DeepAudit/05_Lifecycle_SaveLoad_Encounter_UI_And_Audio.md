# RE-END — Deep Technical Audit: Subsystem Group 05
## Game Lifecycle, Save/Load, Boss Phase/Cutscene, Checkpoint, UI & Audio

- **Phạm vi kiểm toán (Scope):**
  - **Game Lifecycle & Save/Load:** [`GameManagers.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Managers/GameManagers.cs), [`SaveManagers.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Managers/SaveManagers.cs), [`SaveData.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Managers/SaveData.cs), [`CheckPoint.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Interact/CheckPoint.cs), [`ActiveCheckPoint.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Environment/ActiveCheckPoint.cs).
  - **Encounter & Cutscene:** [`BossPhaseManager.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Managers/BossPhaseManager.cs), [`CutsceneManagers.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Cutscene/CutsceneManagers.cs), [`TimelineEvent.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Cutscene/TimelineEvent.cs).
  - **Graphics, Audio & UI:** [`GraphicManager.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Managers/GraphicManager.cs), [`GraphicsUI.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/UI/GraphicsUI.cs), [`AudioManagers.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Audio/AudioManagers.cs), [`AudioSetting.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Managers/AudioSetting.cs), [`PlayerSFX.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Audio/PlayerSFX.cs), [`ButtonSound.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Audio/ButtonSound.cs), [`UIPlayerManagers.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/UI/UIPlayerManagers.cs), [`WorldUIManager.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/UI/WorldUIManager.cs), [`BossHealthUIManagers.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/BossHealthUIManagers.cs), [`StartMenuFeatures.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/UI/StartMenuFeatures.cs), [`ProgressUI.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/UI/ProgressUI.cs), [`TriggerActiveTutorials.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/UI/TriggerActiveTutorials.cs), [`Billboard.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/UI/Billboard.cs), [`GameButtonManagers.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Managers/GameButtonManagers.cs).

---

## 1. Danh sách Phát hiện Kỹ thuật (Detailed Findings)

### [LIFE-01] Chết trước khi chạm Checkpoint đầu tiên trong New Game khiến nhân vật hồi sinh với `0` Máu, `0` Mana, `0` Stamina, `0` Damage tại tọa độ `(0, 0, 0)`
- **ID:** `LIFE-01`
- **Subsystem:** Game Lifecycle & Save/Load (`GameManagers`, `SaveManagers`, `SaveData`)
- **Category:** `Bug`
- **Severity:** `Critical`
- **Confidence:** `Confirmed`
- **Location:**
  - [`SaveManagers.cs:70-75`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Managers/SaveManagers.cs#L70-L75) — `CreateNewSaveGame()`
  - [`GameManagers.cs:50-56, 92-169, 179-185`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Managers/GameManagers.cs#L92-L169) — `StartNewGame()`, `OnSceneLoaded()`, `ApplySaveData()`, `ReturnCheckpoint()`
- **Evidence & Technical Explanation:**
  1. Khi người chơi bấm **New Game**, [`GameManagers.StartNewGame()`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Managers/GameManagers.cs#L50-L56) gọi [`SaveManagers.CreateNewSaveGame()`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Managers/SaveManagers.cs#L70-L75):
     ```csharp
     public void CreateNewSaveGame()
     {
         DeteleSaveGame();
         CurrentSaveData = new SaveData();
     }
     ```
     Trong `new SaveData()`, mọi trường kiểu số (`currentHealth`, `currentMana`, `currentStamina`, `currentDamage`, `posX`, `posY`, `posZ`, `rotX/Y/Z`) đều có giá trị mặc định của C# là **`0`**!
  2. Khi load vào `Main.unity` với `loadReason == ReasonLoadScene.New`, `OnSceneLoaded()` (`GameManagers.cs:92-114`) **không gọi `AutoSave()`** để ghi nhận chỉ số và vị trí xuất phát ban đầu của Player.
  3. Nếu người chơi chưa mở bất kỳ Checkpoint hay menu Pause nào (tức là `AutoSave()` chưa từng chạy) mà đi thẳng vào đánh Boss đầu tiên rồi **bị đánh chết**, `PlayerDeathState` sẽ gọi `GameManagers.Instance.ReturnCheckpoint()` (`loadReason = ReasonLoadScene.Respawn`).
  4. Khi `Main.unity` tải lại với `loadReason == Respawn`, `OnSceneLoaded()` gọi `ApplySaveData(player)` đọc từ `CurrentSaveData` (đang toàn số `0`):
     - `health.maxHealth = data.currentHealth;` -> **`0`**!
     - `mana.maxMana = data.currentMana;` -> **`0`**!
     - `stamina.maxStamina = data.currentStamina;` -> **`0`**!
     - Toàn bộ `AttackData[i].AttackDamage = data.currentDamage;` -> **`0`**!
     - Dịch chuyển Player về tọa độ `(0, 0, 0)` (rơi xuyên địa hình)!
- **Trigger Conditions:**
  Bấm New Game -> đi thẳng vào Boss (không mở Checkpoint/Pause) -> chết -> tự động hồi sinh (`ReturnCheckpoint`).
- **Impact / Consequence:**
  Hỏng hoàn toàn phiên chơi (Game-breaking bug): nhân vật hồi sinh tại `(0,0,0)` với `0` máu tối đa, `0` mana, `0` stamina và `0` sát thương.
- **Remediation Plan:**
  - **Root Cause:** Không thực hiện `AutoSave()` khởi tạo ngay khi bắt đầu `New Game` sau khi `Main.unity` đã tải xong.
  - **Minimal Safe Fix (`Bug Fix`):** Trong [`GameManagers.OnSceneLoaded()`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Managers/GameManagers.cs#L99-L103), khi `loadReason == ReasonLoadScene.New` (và `scene.name == "Main"`), gọi ngay `AutoSave();` để lưu vị trí xuất phát và chỉ số gốc từ Prefab vào `CurrentSaveData` và `ReSave.json`.
- **Verification Method:**
  Bấm New Game, chạy thẳng vào Boss để bị đánh chết mà không chạm vào Checkpoint, kiểm tra sau khi hồi sinh nhân vật đứng đúng vị trí xuất phát với đầy đủ `500` HP, `100` Mana, `1000` Stamina và sát thương gốc.
- **Regression Risk:** Không có.

---

### [LIFE-02] Mất dữ liệu `PlayerSpiritualPower`, làm phẳng sát thương 4 đòn combo thành 1 giá trị duy nhất, và lưu game TRƯỚC khi cộng thưởng hạ Boss
- **ID:** `LIFE-02`
- **Subsystem:** Save/Load & Progression (`BossPhaseManager`, `CutsceneManagers`, `GameManagers`, `SaveData`)
- **Category:** `Bug`
- **Severity:** `High`
- **Confidence:** `Confirmed`
- **Location:**
  - [`BossPhaseManager.cs:92-103`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Managers/BossPhaseManager.cs#L92-L103) — `HandleDeadPhaseAction()`
  - [`CutsceneManagers.cs:34-42`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Cutscene/CutsceneManagers.cs#L34-L42) — `SaveIsDefeat()`
  - [`GameManagers.cs:141-145, 227`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Managers/GameManagers.cs#L141-L145) — `ApplySaveData()` & `AutoSave()`
  - [`SaveData.cs:8-48`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Managers/SaveData.cs#L8-L48)
- **Evidence & Technical Explanation:**
  1. **Thứ tự thực thi ngược trong [`BossPhaseManager.cs:94-100`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Managers/BossPhaseManager.cs#L94-L100):**
     ```csharp
     Finished?.Invoke(); // Gọi CutsceneManagers.SaveIsDefeat() -> GameManagers.Instance.AutoSave()
     ...
     Player.AddBossSpiritualPower(spiritualPower);
     ```
     `AutoSave()` chạy ở dòng 94 **trước khi** `AddBossSpiritualPower` được gọi ở dòng 100!
  2. **Thiếu trường lưu `PlayerSpiritualPower` và `maximumPotionUsage` trong `SaveData.cs`:**
     Dù người chơi nhận `spiritualPower` khi hạ Boss, `SaveData.cs` hoàn toàn không có trường lưu `PlayerSpiritualPower` hay số lần sử dụng tối đa của bình thuốc (`maximumPotionUsage`). Khi thoát game vào lại (`Continue`) hoặc khi chết hồi sinh (`ReturnCheckpoint`), `PlayerSpiritualPower` bị reset về giá trị mặc định trên Prefab!
  3. **Ghi đè hỏng sát thương của chuỗi 4 đòn đánh (`AttackData[0..3]`):**
     Trong `Player_Model.prefab`, 4 đòn combo có sát thương tăng dần: `40`, `40`, `45`, `50`. Thế nhưng [`GameManagers.AutoSave()`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Managers/GameManagers.cs#L227) chỉ lưu mỗi đòn đầu tiên (`data.currentDamage = stateMachine.AttackData[0].AttackDamage`), và khi load lại trong [`ApplySaveData()`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Managers/GameManagers.cs#L142-L145):
     ```csharp
     for (int i = 0; i < stateMachine.AttackData.Length; i++)
     {
         stateMachine.AttackData[i].AttackDamage = data.currentDamage;
     }
     ```
     Đòn thứ 3 (`45`) và đòn thứ 4 (`50`) bị ép giảm xuống bằng đòn thứ 1 (`40`) ngay sau lần lưu/tải đầu tiên!
- **Trigger Conditions:**
  Mọi lần lưu và tải game (`AutoSave` / `ApplySaveData`) hoặc hạ gục Boss.
- **Impact / Consequence:**
  Mất điểm `PlayerSpiritualPower` khi tải lại game; mất sát thương thiết kế của đòn combo thứ 3 và thứ 4.
- **Remediation Plan:**
  - **Minimal Safe Fix (`Bug Fix`):**
    1. Đưa `Player.AddBossSpiritualPower(spiritualPower);` lên **trước** `Finished?.Invoke();` trong `BossPhaseManager.cs:94-100`.
    2. Thêm `public int spiritualPower;`, `public float[] attackDamages;`, `public float healthPotionMaxUsage;`, `public float manaPotionMaxUsage;` vào `SaveData.cs` và ánh xạ đầy đủ trong `AutoSave()` / `ApplySaveData()`.

---

### [LIFE-03] Bất đồng bộ giữa `SaveManagers.CompletedList` (`HashSet`) và `CurrentSaveData.completedTriggerBoss` (`List`)
- **ID:** `LIFE-03`
- **Subsystem:** Save/Load & Encounter Trigger (`SaveManagers`, `CutsceneManagers`)
- **Category:** `Bug`
- **Severity:** `High`
- **Confidence:** `Confirmed`
- **Location:**
  - [`SaveManagers.cs:15, 66, 70-85`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Managers/SaveManagers.cs#L66-L85)
  - [`CutsceneManagers.cs:28-42`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Cutscene/CutsceneManagers.cs#L28-L42)
- **Evidence & Technical Explanation:**
  - [`CutsceneManagers.CheckDefeat()`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Cutscene/CutsceneManagers.cs#L30) kiểm tra xem Boss đã bị tiêu diệt chưa bằng `SaveManagers.Instance.CompletedList.Contains(bossPhaseManager.TriggerId)`.
  - Tuy nhiên:
    1. Khi hạ xong một Boss, [`CutsceneManagers.SaveIsDefeat()`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Cutscene/CutsceneManagers.cs#L36-L39) chỉ thêm `TriggerId` vào `CurrentSaveData.completedTriggerBoss` chứ **không thêm vào `SaveManagers.Instance.CompletedList`**! Nếu sau đó người chơi chết ở Boss tiếp theo và `ReturnCheckpoint()` tải lại `Main.unity` (không gọi `LoadGame()`), `CompletedList` vẫn trống nên **các Boss đã bị hạ trước đó sẽ sống lại**!
    2. Ngược lại, khi người chơi về Menu và chọn **New Game**, `CreateNewSaveGame()` và `DeteleSaveGame()` **không gọi `CompletedList.Clear()`**! Do đó các Boss đã hạ ở bản chơi cũ vẫn nằm trong `CompletedList`, khiến **New Game bị mất các trận đánh Boss đó**!
- **Trigger Conditions:**
  - Hạ Boss 1 -> Chết ở Boss 2 -> Hồi sinh (`ReturnCheckpoint`).
  - Chơi Continue (đã hạ Boss) -> Về Menu bấm New Game.
- **Impact / Consequence:**
  Boss đã chết sống lại khi hồi sinh tại Checkpoint; hoặc Boss biến mất trong New Game.
- **Remediation Plan:**
  - **Minimal Safe Fix (`Bug Fix`):**
    1. Trong `CutsceneManagers.SaveIsDefeat()`, thêm `SaveManagers.Instance.CompletedList.Add(bossPhaseManager.TriggerId);`.
    2. Trong `SaveManagers.CreateNewSaveGame()` và `DeteleSaveGame()`, gọi `CompletedList.Clear();`.

---

### [LIFE-04] Sáu nút UI Cộng/Trừ Sub-Potion trong `Main.unity` bị đứt tham chiếu (`m_Target: {fileID: 0}`) và bảng `GraphicsUI` ngoài Checkpoint không gắn Event Listener
- **ID:** `LIFE-04`
- **Subsystem:** UI & Scene Wiring (`Main.unity`, `GraphicsUI`)
- **Category:** `Bug`
- **Severity:** `High`
- **Confidence:** `Confirmed`
- **Location:**
  - `Assets/Scenes/Main.unity` — Dòng `48322`, `114374`, `277537`, `397415`, `465642`, `499578` (`m_Target: {fileID: 0}` cho `SubPotion.AddSubPotionQuantity` và `SubtractSubPotionQuantity`)
  - [`GraphicsUI.cs:111-179`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/UI/GraphicsUI.cs#L111-L179) — `BindListener()`
- **Evidence & Technical Explanation:**
  1. **6 nút UI Sub-Potion trong `Main.unity` có `m_Target: {fileID: 0}`:**
     Qua kiểm tra trực tiếp YAML của `Main.unity`, cả 3 nút Add (`AddSubPotionQuantity`) và 3 nút Subtract (`SubtractSubPotionQuantity`) của 3 bình Sub-Potion (`EmeraldGuava`, `GoldenPear`, `BloodPomegranate`) đều đang trỏ tới `m_Target: {fileID: 0}` (rất có thể bị mất tham chiếu khi thay thế `Player.prefab` bằng `Player_Model.prefab` trong Scene). Bấm vào các nút này trên UI hoàn toàn không gọi hàm trong `SubPotion.cs`.
  2. **Bảng `GraphicsUI` ngoài Checkpoint (`*NonCheckpoint`) không thể tương tác:**
     Trong [`GraphicsUI.BindListener()`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/UI/GraphicsUI.cs#L111-L179), lập trình viên chỉ gắn `onValueChanged.AddListener(...)` cho các control thuộc bảng Checkpoint (`resolutionDropdown`, `qualityDropdown`, `vsyncToggle`, `bloomSlider`...), mà **quên gắn listener cho toàn bộ 11 control `*NonCheckpoint`** (`resolutionDropdownNonCheckpoint`, `vsyncToggleNonCheckpoint`, v.v.). Khi người chơi bấm `ESC` ngoài trận đấu để mở Settings và chỉnh đồ họa, không có sự kiện nào được kích hoạt!
- **Trigger Conditions:**
  - Bấm nút `+` / `-` mua Sub-Potion tại Checkpoint.
  - Bấm `ESC` ngoài Checkpoint để chỉnh cài đặt Đồ họa.
- **Impact / Consequence:**
  Không thể mua/trừ số lượng Sub-Potion trên UI; bảng cài đặt đồ họa khi nhấn ESC hoàn toàn liệt.
- **Remediation Plan:**
  - **Minimal Safe Fix (`Bug Fix`):**
    1. Kéo lại component `SubPotion` trên `Player_Model` trong `Main.unity` vào `OnClick` của 6 nút Add/Subtract Sub-Potion (hoặc tự động bind bằng code trong `UIPlayerManagers.Start()`).
    2. Bổ sung `AddListener` cho các control `*NonCheckpoint` trong `GraphicsUI.BindListener()`.

---

### [LIFE-05] `GraphicManager` và `TriggerActiveTutorials` gặp lỗi khi `CurrentSaveData` chưa được nạp hoặc khi vào `Main.unity` từ New Game khi đã có file save cũ
- **ID:** `LIFE-05`
- **Subsystem:** Graphics & Tutorials (`GraphicManager`, `GraphicsUI`, `TriggerActiveTutorials`, `SaveManagers`)
- **Category:** `Bug`
- **Severity:** `Medium`
- **Confidence:** `Confirmed`
- **Location:**
  - [`GraphicManager.cs:186-205`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Managers/GraphicManager.cs#L186-L205) — `LoadApplyAll()`
  - [`GraphicsUI.cs:90-109`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/UI/GraphicsUI.cs#L90-L109) — `LoadCurrentValuesToUI()`
  - [`TriggerActiveTutorials.cs:20-26`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/UI/TriggerActiveTutorials.cs#L20-L26) — `Start()`
- **Evidence & Technical Explanation:**
  - Trong `GraphicManager.LoadApplyAll()` (`:188`) và `GraphicsUI.LoadCurrentValuesToUI()` (`:93`), điều kiện kiểm tra là:
    ```csharp
    if (!SaveManagers.Instance.HaveSaveData()) { ... return; }
    var data = SaveManagers.Instance.CurrentSaveData;
    ```
    Hàm `HaveSaveData()` chỉ kiểm tra `File.Exists(saveFilePath)`. Nếu người chơi chạy `Main.unity` trực tiếp trong Editor (khi trên máy đã có sẵn `ReSave.json` từ lần chơi trước), `HaveSaveData()` trả về `true`, nhưng `SaveManagers.LoadGame()` chưa được gọi nên `CurrentSaveData` vẫn là **`null`**, gây `NullReferenceException` ngay tại dòng `data.resolutionIndex`!
  - Tương tự, `TriggerActiveTutorials.Start()` (`:22`) truy cập thẳng `SaveManagers.Instance.CurrentSaveData.completedTutorials` mà không kiểm tra `SaveManagers.Instance != null && SaveManagers.Instance.CurrentSaveData != null`.
- **Trigger Conditions:**
  Chạy `Main.unity` trực tiếp trong Editor hoặc khi `CurrentSaveData == null`.
- **Impact / Consequence:**
  `NullReferenceException` trong `GraphicManager.Start()`, `GraphicsUI.Start()` và `TriggerActiveTutorials.Start()`.
- **Remediation Plan:**
  - **Minimal Safe Fix (`Bug Fix`):** Trong `SaveManagers.Awake()`, khởi tạo mặc định `CurrentSaveData = new SaveData();` (hoặc tự động gọi `LoadGame()` nếu `HaveSaveData()` là `true`), và gán giá trị mặc định hợp lệ cho các trường đồ họa trong constructor của `SaveData` (`targetFPS = 60`, `renderScale = 1.0f`, v.v.).

---

## 2. Đánh giá Kiến trúc, Hiệu năng & Điểm mạnh (Group 05)

- **Điểm mạnh:**
  - **Tách biệt Scene trung gian `LoadScene.unity` (`ProgressUI.cs`):** Sử dụng `SceneManager.LoadSceneAsync` để hiển thị tiến trình tải mượt mà khi chuyển vào thế giới `Main.unity`.
  - **Quản lý Audio qua Object Pool (`AudioManagers.cs`):** Thay vì gọi `AudioSource.PlayClipAtPoint` (tạo và hủy GameObject liên tục gây rác GC), `AudioManagers` lấy `PooledObject("Audio")` từ `ObjectPooling` để phát SFX và tự trả về Pool sau `clip.length`.
  - **Tích hợp URP Volume & Quality Settings (`GraphicManager.cs`):** Cung cấp đầy đủ tùy chỉnh Resolution, Display Mode, VSync, Target FPS, Quality Level, Shadow Distance, Anti-Aliasing, Render Scale, Bloom, Motion Blur.
- **Điểm yếu kiến trúc:**
  - **Nhân đôi UI Hierarchy (`Checkpoint` vs `NonCheckpoint`):** `UIPlayerManagers.cs` và `GraphicsUI.cs` duy trì 2 bộ biến UI gần như giống hệt nhau cho trường hợp ngồi ở Checkpoint và trường hợp bấm ESC ngoài Checkpoint, làm tăng gấp đôi số lượng tham chiếu Inspector và dẫn trực tiếp tới lỗi quên gắn listener ở `[LIFE-04]`.
