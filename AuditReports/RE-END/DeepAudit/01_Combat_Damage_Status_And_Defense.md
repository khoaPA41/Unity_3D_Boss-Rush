# RE-END — Deep Technical Audit: Subsystem Group 01
## Combat Hit Detection, Damage Pipeline, Status/Potions & Defensive Mechanics

- **Phạm vi kiểm toán (Scope):**
  - **Hit Detection & Weapon Logic:** [`WeaponTrail.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/WeaponTrail.cs), [`WeaponHanldler.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/WeaponHanldler.cs), [`AttackData.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/AttackData.cs), [`AttackDealsDamge.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/AttackDealsDamge.cs), [`ManageAttackAnimation.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/ManageAttackAnimation.cs), [`AttackDatas.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/ScriptableObject/AttackDatas.cs).
  - **Status & Resource Systems:** [`Health.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/Status/Health.cs), [`Mana.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/Status/Mana.cs), [`Stamina.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/Status/Stamina.cs).
  - **Potions & Buffs:** [`Potion.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/Status/Potion/Potion.cs), [`HealthPotion.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/Status/Potion/HealthPotion.cs), [`ManaPotion.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/Status/Potion/ManaPotion.cs), [`SubPotion.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/Status/Potion/SubPotion.cs), [`PlayerUsePotionState.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Main/PlayerUsePotionState.cs), [`PlayerUseSubPotionState.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Main/PlayerUseSubPotionState.cs).
  - **Defensive Mechanics:** [`DodgeAward.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/Status/Award/DodgeAward.cs), [`PlayerCounterAttackState.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Main/PlayerCounterAttackState.cs), [`PlayerDodgeState.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Main/PlayerDodgeState.cs).
  - **Serialized Assets Checked:** [`Player_Model.prefab`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Prefabs/Player/Player_Model.prefab), [`Player.prefab`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Prefabs/Player/Player.prefab), [`Main.unity`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scenes/Main.unity), và toàn bộ `.anim` trong `Assets/Character/`.

---

## 1. Danh sách Phát hiện Kỹ thuật (Detailed Findings)

### [COMBAT-01] Tên `potionName` trong Prefab/Scene không khớp với `switch(potionName)` khiến toàn bộ 3 Sub-Potion không bao giờ kích hoạt hiệu ứng buff
- **ID:** `COMBAT-01`
- **Subsystem:** Combat / Potions (`SubPotion`, `PlayerUseSubPotionState`)
- **Category:** `Bug`
- **Severity:** `Critical`
- **Confidence:** `Confirmed`
- **Location:**
  - [`PlayerUseSubPotionState.cs:15-21, 37-52`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Main/PlayerUseSubPotionState.cs#L37-L52) — `PlayerUseSubPotionState.Enter()` & `UsePotion(string potionName)`
  - [`SubPotion.cs:68-102`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/Status/Potion/SubPotion.cs#L68-L102) — `SubtractPotion`, `ReduceDame`, `ReduceStamina`, `IncreaseDame`
  - [`Player_Model.prefab:10489-10501`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Prefabs/Player/Player_Model.prefab#L10489-L10501) — `SubPotion.subPotionList`
- **Evidence & Technical Explanation:**
  Trong [`Player_Model.prefab:10489-10501`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Prefabs/Player/Player_Model.prefab#L10489-L10501) (prefab thực tế được sử dụng trong `Main.unity`) và [`Player.prefab:13568-13580`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Prefabs/Player/Player.prefab#L13568-L13580), danh sách `subPotionList` (`List<SubPotionValue>`) gồm 3 phần tử có trường `potionName` được serialize là:
  - Index 0: `"EmeraldGuava"` (`quantity: 1`, `timeToDone: 20`, `hexColor: '#0CE400'`)
  - Index 1: `"GoldenPear"` (`quantity: 1`, `timeToDone: 20`, `hexColor: '#E47A00'`)
  - Index 2: `"BloodPomegranate"` (`quantity: 1`, `timeToDone: 20`, `hexColor: '#E4009F'`)

  Khi người chơi bấm phím dùng Sub-Potion, [`PlayerUseSubPotionState.Enter()`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Main/PlayerUseSubPotionState.cs#L18) gọi:
  ```csharp
  UsePotion(playerStateMachine.SubPotion.currentPotion.potionName);
  ```
  Bên trong [`UsePotion(string potionName)`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Main/PlayerUseSubPotionState.cs#L37-L52):
  ```csharp
  private void UsePotion(string potionName)
  {
      playerStateMachine.SubPotion.SubtractPotion(potionName, 1);
      switch (potionName)
      {
          case "ReduceStamina":
              playerStateMachine.SubPotion.ReduceStamina("ReduceStamina");
              return;
          case "IncreaseDame":
              playerStateMachine.SubPotion.IncreaseDame("IncreaseDame");
              return;
          case "ReduceDame":
              playerStateMachine.SubPotion.ReduceDame("ReduceDame");
              return;
      }
  }
  ```
  1. Dòng 39 (`SubtractPotion(potionName, 1)`) tìm trong `subPotionList` theo `potionName` (`"EmeraldGuava"`, `"GoldenPear"`, `"BloodPomegranate"`), khớp với dữ liệu Prefab và **trừ đi 1 bình thuốc** của người chơi.
  2. Dòng 40 (`switch (potionName)`) lại so sánh với `"ReduceStamina"`, `"IncreaseDame"`, `"ReduceDame"`. Do `potionName` thực tế là `"EmeraldGuava"`, `"GoldenPear"`, `"BloodPomegranate"`, **không có nhánh `case` nào khớp**.
  3. Hơn nữa, ngay cả khi gọi trực tiếp `ReduceStamina("ReduceStamina")`, bên trong [`SubPotion.cs:81, 89, 97`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/Status/Potion/SubPotion.cs#L81-L97), hàm `FindPotion("ReduceStamina")` sẽ trả về `null` và ném `NullReferenceException` ngay tại `.timeToDone`!
- **Trigger Conditions:**
  Mọi lần người chơi sử dụng Sub-Potion trong suốt quá trình chơi.
- **Impact / Consequence:**
  **100% hệ thống Sub-Potion bị vô hiệu hóa về mặt hiệu ứng chiến đấu.** Người chơi mất bình thuốc, nhân vật phát animation uống thuốc và đổi màu đèn `PotionLight`, nhưng không bao giờ nhận được hiệu ứng giảm tiêu hao Stamina (`EmeraldGuava`), giảm sát thương nhận vào (`GoldenPear`), hay tăng sát thương (`BloodPomegranate`).
- **Remediation Plan:**
  - **Root Cause:** Sử dụng magic strings không đồng nhất giữa dữ liệu hiển thị/prefab (`"EmeraldGuava"`, `"GoldenPear"`, `"BloodPomegranate"`) và tên hiệu ứng trong code (`"ReduceStamina"`, `"IncreaseDame"`, `"ReduceDame"`).
  - **Minimal Safe Fix (`Bug Fix`):**
    Trong [`PlayerUseSubPotionState.cs:40-51`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Main/PlayerUseSubPotionState.cs#L40-L51), đổi các nhánh `case` và đối số truyền vào thành:
    ```csharp
    switch (potionName)
    {
        case "EmeraldGuava":
        case "ReduceStamina":
            playerStateMachine.SubPotion.ReduceStamina(potionName);
            return;
        case "BloodPomegranate":
        case "IncreaseDame":
            playerStateMachine.SubPotion.IncreaseDame(potionName);
            return;
        case "GoldenPear":
        case "ReduceDame":
            playerStateMachine.SubPotion.ReduceDame(potionName);
            return;
    }
    ```
  - **Long-term Refactor (`Refactor`):** Thêm trường `SubPotionEffectType effectType` (enum) vào class [`SubPotionValue`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/Status/Potion/SubPotion.cs#L12-L18).
  - **Pros / Cons / Cost:** Minimal fix chỉ sửa 6 dòng trong `PlayerUseSubPotionState.cs`, tương thích ngược 100% với Prefab và Save (`Cost: Trivial`).
  - **Affected Subsystems:** `PlayerUseSubPotionState`, `SubPotion`.
- **Verification Method:**
  Trong Play Mode, chọn lần lượt từng loại Sub-Potion và bấm phím sử dụng; kiểm tra trên Inspector của `Player` xem các cờ `Stamina.isReduceStamina`, `PlayerStateMachine.IsIncreaseDamePotion`, `Health.isReduceDame` có chuyển sang `true` trong `20s` (`timeToDone`) hay không.
- **Regression Risk:** Thấp.

---

### [COMBAT-02] Công thức sát thương cộng trực tiếp `resistance` khiến đòn đánh nhỏ hơn `resistance` hồi máu cho mục tiêu
- **ID:** `COMBAT-02`
- **Subsystem:** Combat / Status (`Health`)
- **Category:** `Bug`
- **Severity:** `High`
- **Confidence:** `Confirmed`
- **Location:**
  - [`Health.cs:79-91`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/Status/Health.cs#L79-L91) — `Health.DealDamage(float damage)`
  - [`PlayerHeavyAttackState.cs:80-84`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Main/PlayerHeavyAttackState.cs#L80-L84) — `CaculateDamage()`
  - [`Player_Model.prefab:10221`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Prefabs/Player/Player_Model.prefab#L10221) — `Health.resistance = 10`
- **Evidence & Technical Explanation:**
  Trong [`Health.DealDamage(float damage)`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/Status/Health.cs#L79-L91):
  ```csharp
  public void DealDamage(float damage)
  {
      if (isReduceDame) damage /= reduceHealth;

      currentHealth = Mathf.Clamp(currentHealth - damage + resistance, 0, maxHealth);
      ...
  }
  ```
  Biểu thức `currentHealth - damage + resistance` tương đương với `currentHealth - (damage - resistance)`, nhưng **không chặn biên dưới của sát thương thực tế (`Mathf.Max(0f, damage - resistance)`)**.
  Khi `damage < resistance`, lượng `-damage + resistance` mang dấu dương (`> 0`), dẫn đến `currentHealth` **tăng lên (hồi máu)** khi bị đánh trúng!
  Cụ thể:
  1. `Player_Model.prefab` có `resistance = 10` (và có thể nâng cấp cao hơn qua `Health.AddResistance()` tại Checkpoint). Khi `isReduceDame` bật (`damage /= reduceHealth`) hoặc khi trúng đòn sát thương nhỏ (`< resistance`), Player được hồi máu mỗi lần trúng đòn.
  2. Trong [`PlayerHeavyAttackState.cs:82`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Main/PlayerHeavyAttackState.cs#L82), nếu người chơi nhả chuột sớm (`_holdTime` nhỏ), `_holdDamage = AttackDamage * _holdTime * 3f` nhỏ hơn `resistance` của Boss sẽ hồi máu cho Boss.
- **Trigger Conditions:**
  Bất kỳ va chạm gây sát thương nào có `damage < resistance` (hoặc `damage / reduceHealth < resistance`).
- **Impact / Consequence:**
  Phá vỡ tính đúng đắn của hệ thống chiến đấu: sát thương nhỏ hơn giáp biến thành hồi máu.
- **Remediation Plan:**
  - **Root Cause:** Thiếu `Mathf.Max(0f, damage - resistance)` khi tính sát thương thực tế.
  - **Minimal Safe Fix (`Bug Fix`):**
    ```csharp
    float effectiveDamage = Mathf.Max(0f, damage - resistance);
    currentHealth = Mathf.Clamp(currentHealth - effectiveDamage, 0f, maxHealth);
    ```
  - **Pros / Cons / Cost:** Sửa 1 dòng trong [`Health.cs:83`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/Status/Health.cs#L83) (`Cost: Trivial`).
  - **Affected Subsystems:** `Health`.
- **Verification Method:**
  Gọi `DealDamage(5f)` lên `Player` (có `resistance = 10`) và kiểm tra `currentHealth` giữ nguyên thay vì tăng thêm 5 máu.
- **Regression Risk:** Không có.

---

### [COMBAT-03] Đòn đánh kết liễu kích hoạt đồng thời cả `HitAction` và `DeathAction`, và mục tiêu đã chết vẫn tiếp tục nhận sát thương
- **ID:** `COMBAT-03`
- **Subsystem:** Combat / Status (`Health`, `PlayerStateMachine`, `BossStateMachine`)
- **Category:** `Bug`
- **Severity:** `High`
- **Confidence:** `Confirmed`
- **Location:**
  - [`Health.cs:79-91`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/Status/Health.cs#L79-L91) — `Health.DealDamage(float damage)`
  - [`PlayerStateMachine.cs:185-186, 344-354`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Base/PlayerStateMachine.cs#L344-L354)
  - [`BossStateMachine.cs:119-120, 160-168`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Boss/Base/BossStateMachine.cs#L160-L168)
- **Evidence & Technical Explanation:**
  Xem [`Health.DealDamage(float damage)`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/Status/Health.cs#L79-L91):
  ```csharp
  public void DealDamage(float damage)
  {
      if (isReduceDame) damage /= reduceHealth;

      currentHealth = Mathf.Clamp(currentHealth - damage + resistance, 0, maxHealth);
      HitAction?.Invoke();

      if (currentHealth <= 0)
      {
          DeathAction?.Invoke();
      }
      OnChangeHealth?.Invoke(currentHealth / maxHealth);
  }
  ```
  1. **Không có guard kiểm tra đã chết (`if (currentHealth <= 0f) return;`) ở đầu hàm:** Khi Player hoặc Boss đã chết (`currentHealth <= 0`), nếu một đòn đánh khác tiếp tục quét trúng cơ thể họ, `DealDamage` vẫn bắn `HitAction?.Invoke()` (đưa xác chết về `HitState`) rồi bắn `DeathAction?.Invoke()` (vào lại `DeathState` từ đầu).
  2. **Kích hoạt cả `HitAction` lẫn `DeathAction` trong cùng một cú đánh kết liễu:** Khi `currentHealth` giảm về `0`, dòng 84 gọi `HitAction?.Invoke()` (chuyển sang `PlayerHitState` / `BossHitState` và tăng `BossBehaviorBrain.HitReceived++`), rồi ngay dòng 88 gọi `DeathAction?.Invoke()` (chuyển tiếp sang `DeathState`).
- **Trigger Conditions:**
  Mọi đòn đánh làm `currentHealth` của Player hoặc Boss về `0`, hoặc bất kỳ đòn đánh nào quét qua mục tiêu sau khi `currentHealth <= 0`.
- **Impact / Consequence:**
  Chuyển state kép (`HitState` -> `DeathState`) trong cùng 1 frame khi chết; đánh vào xác chết làm phát lại animation chết.
- **Remediation Plan:**
  - **Minimal Safe Fix (`Bug Fix`):**
    ```csharp
    public void DealDamage(float damage)
    {
        if (noDamage || currentHealth <= 0f) return;
        if (isReduceDame) damage /= reduceHealth;
        float effectiveDamage = Mathf.Max(0f, damage - resistance);
        currentHealth = Mathf.Clamp(currentHealth - effectiveDamage, 0f, maxHealth);
        if (currentHealth <= 0f)
        {
            DeathAction?.Invoke();
        }
        else
        {
            HitAction?.Invoke();
        }
        OnChangeHealth?.Invoke(currentHealth / maxHealth);
    }
    ```
- **Verification Method:**
  Đánh Boss hoặc Player về `0` HP, sau đó tiếp tục vung kiếm trúng mục tiêu đang phát animation `Death` và xác nhận `DeathAction` / `HitAction` không bị gọi lại.
- **Regression Risk:** Thấp.

---

### [COMBAT-04] Đột biến vĩnh viễn chỉ số `AttackData` và khóa vĩnh viễn kỹ năng sau khi dùng `WorldBreaker` (`Invincible = true`)
- **ID:** `COMBAT-04`
- **Subsystem:** Combat / Skill / Player (`PlayerStateMachine`, `Skill`, `GameManagers`)
- **Category:** `Bug`
- **Severity:** `High`
- **Confidence:** `Confirmed`
- **Location:**
  - [`PlayerStateMachine.cs:289-299, 367-376`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Base/PlayerStateMachine.cs#L289-L299) — `HandleSkillEvent(int index)` & `InvincibleState()`
  - [`Skill.cs:135-161`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/Skill%20Factory/Skill.cs#L135-L161) — `WorldBreaker.Cast(ICaster caster)`
  - [`GameManagers.cs:227`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Managers/GameManagers.cs#L227) — `AutoSave()`
- **Evidence & Technical Explanation:**
  Khi người chơi kích hoạt kỹ năng `WorldBreaker`, [`WorldBreaker.Cast()`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/Skill%20Factory/Skill.cs#L144-L150) thực thi:
  ```csharp
  var player = caster.GetTransform().GetComponent<PlayerStateMachine>();
  if (player == null) return;
  player.Invincible = true;
  player.InvincibleState();
  ```
  Hàm [`PlayerStateMachine.InvincibleState()`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Base/PlayerStateMachine.cs#L367-L376) sửa trực tiếp các thuộc tính gốc:
  ```csharp
  public void InvincibleState()
  {
      FreeLookMovementSpeed *= 1.5f;
      FreeLookMovementSprintSpeed *= 0.8f;
      for (int i = 0; i < AttackData.Length; i++)
      {
          AttackData[i].AttackDamage *= 2;
      }
  }
  ```
  1. **Không có bất kỳ đoạn code nào đặt lại `player.Invincible = false`** hoặc hoàn trả `FreeLookMovementSpeed`, `FreeLookMovementSprintSpeed`, và `AttackData[i].AttackDamage`!
  2. Trong [`PlayerStateMachine.HandleSkillEvent(int index)`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Base/PlayerStateMachine.cs#L291):
     ```csharp
     if (!isAttackState) return;
     if (Invincible) return;
     ```
     Vì `Invincible` giữ giá trị `true` mãi mãi, sau khi dùng `WorldBreaker` 1 lần, **người chơi bị khóa hoàn toàn khả năng sử dụng mọi kỹ năng trong suốt phần còn lại của phiên chơi**!
  3. [`GameManagers.AutoSave()`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Managers/GameManagers.cs#L227) lưu trực tiếp `stateMachine.AttackData[0].AttackDamage` vào `ReSave.json`. Nếu lưu game sau khi dùng `WorldBreaker`, sát thương gấp đôi (`* 2`) bị **ghi vĩnh viễn vào file save**!
  4. `FreeLookMovementSprintSpeed *= 0.8f` làm tốc độ chạy nước rút giảm xuống thay vì tăng lên.
- **Trigger Conditions:**
  Người chơi sử dụng kỹ năng `WorldBreaker` 1 lần.
- **Impact / Consequence:**
  Khóa chết hệ thống Skill của người chơi và làm hỏng vĩnh viễn dữ liệu sát thương trong `ReSave.json`.
- **Remediation Plan:**
  - **Minimal Safe Fix (`Bug Fix`):** Dùng hệ số nhân tạm thời (`_worldBreakerMultiplier`) có thời hạn (Coroutine 15s) thay vì nhân trực tiếp vào `AttackData[i].AttackDamage`, và đặt lại `Invincible = false` khi hết hạn.
- **Verification Method:**
  Kích hoạt `WorldBreaker`, kiểm tra sau khi hết thời gian hiệu lực thì `Invincible == false`, `AttackDamage` vẫn giữ giá trị gốc và người chơi tiếp tục dùng được kỹ năng.
- **Regression Risk:** Thấp.

---

### [COMBAT-05] Khai thác lỗi nâng cấp chỉ số ngược (Stat Underflow Exploit) tại Checkpoint do hardcode mốc tối thiểu `1000` không khớp Prefab (`500` / `100`)
- **ID:** `COMBAT-05`
- **Subsystem:** Combat / Status & Potions (`Health`, `Mana`, `Stamina`, `Potion`, `PlayerStateMachine`)
- **Category:** `Bug`
- **Severity:** `High`
- **Confidence:** `Confirmed`
- **Location:**
  - [`Health.cs:143-153`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/Status/Health.cs#L143-L153) — `SubHealth()`
  - [`Mana.cs:45-55`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/Status/Mana.cs#L45-L55) — `SubMana()`
  - [`Stamina.cs:78-87`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/Status/Stamina.cs#L78-L87) — `SubStamina()`
  - [`Potion.cs:21, 23-29, 72-80`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/Status/Potion/Potion.cs#L72-L80) — `SubtractUsagePotion()`
  - [`Player_Model.prefab:10220, 10282`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Prefabs/Player/Player_Model.prefab#L10220) — `maxHealth: 500`, `maxMana: 100`
- **Evidence & Technical Explanation:**
  1. Trong [`Health.SubHealth()`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/Status/Health.cs#L143-L153): kiểm tra `if (maxHealth == 1000) return;`, nhưng trên `Player_Model.prefab` thì `maxHealth` khởi điểm là **`500`**! Do `500 == 1000` là `false`, người chơi bấm nút `-` sẽ trừ `maxHealth` từ `500` xuống `0`, `-1`, `-2`... và nhận **vô hạn `PlayerSpiritualPower`**.
  2. Trong [`Mana.SubMana()`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/Status/Mana.cs#L45-L55): kiểm tra `if (currentMana == 1000) return;`, nhưng `maxMana` trên `Player_Model.prefab` khởi điểm là **`100`** (và lại so sánh `currentMana` thay vì `maxMana`).
  3. Trong [`Potion.cs:21, 23-29, 72-80`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/Status/Potion/Potion.cs#L72-L80): biến `private float minimumPotionUsage;` không được gán trong `Awake()` (mặc định bằng `0f`), cho phép `SubtractUsagePotion()` trừ `potionUsage` về `0` để lấy thêm điểm `PlayerSpiritualPower`.
- **Trigger Conditions:**
  Mở UI tại bất kỳ Checkpoint nào và bấm nút `-` ở dòng Health, Mana hoặc Potion Usage.
- **Impact / Consequence:**
  Farm vô hạn `PlayerSpiritualPower` ngay tại Checkpoint đầu tiên và làm âm máu/mana tối đa.
- **Remediation Plan:**
  - **Minimal Safe Fix (`Bug Fix`):** Lưu giá trị khởi tạo trong `Awake()` (`_initialMaxHealth = maxHealth;`, `_initialMaxMana = maxMana;`, `_initialMaxStamina = maxStamina;`, `minimumPotionUsage = potionUsage;`) và kiểm tra `<=` thay vì `== 1000`.
- **Verification Method:**
  Mở Checkpoint UI khi vừa vào game mới, bấm nút `-` ở tất cả các mục và xác nhận không có chỉ số nào giảm xuống dưới mức khởi điểm.
- **Regression Risk:** Không có.

---

### [COMBAT-06] Xung đột trên cờ dùng chung `Health.noDamage` và `Time.timeScale` giữa Perfect Dodge, HitStop (`TimeFreeHit`), và Skill `Indestructible`
- **ID:** `COMBAT-06`
- **Subsystem:** Combat / Defense & Status (`Health`, `PlayerDodgeState`, `Skill.Indestructible`, `PlayerUseSkillState`)
- **Category:** `Bug`
- **Severity:** `High`
- **Confidence:** `Confirmed`
- **Location:**
  - [`Health.cs:99-127`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/Status/Health.cs#L99-L127) — `TimeFreeHit()`, `SlowTime()`, `PerfectDodgeAward()`
  - [`PlayerDodgeState.cs:71-76`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Main/PlayerDodgeState.cs#L71-L76) — `PlayerDodgeState.Exit()`
  - [`Skill.cs:51-57`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/Skill%20Factory/Skill.cs#L51-L57) — `Indestructible.Cast()`
  - [`PlayerUseSkillState.cs:74-80`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Main/PlayerUseSkillState.cs#L74-L80)
- **Evidence & Technical Explanation:**
  1. **Xung đột `Health.noDamage`:** Kỹ năng `Indestructible` bật `noDamage = true` trong 1s (`PlayerUseSkillState.cs:74-80`), nhưng nếu trong thời gian đó người chơi thực hiện `Dodge`, khi kết thúc cú lộn nhào [`PlayerDodgeState.Exit()`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Main/PlayerDodgeState.cs#L74) gán thẳng `playerStateMachine.Health.noDamage = false;`, làm mất hiệu ứng bất tử của `Indestructible`! Ngược lại, `Health.SlowTime()` (`Health.cs:109`) gán `noDamage = true` nhưng không tự tắt `noDamage = false` khi kết thúc Coroutine.
  2. **Xung đột `Time.timeScale`:** `Health.SlowTime()` đặt `Time.timeScale = 0.5f` trong `0.8s` realtime. Nếu ngay sau khi Perfect Dodge người chơi chém trúng Boss, `Health.TimeFreeHit()` trên Boss đặt `Time.timeScale = timeFreeze` trong `0.05s` rồi trả thẳng về `Time.timeScale = 1f` (`Health.cs:104`), hủy sớm `0.75s` Slow Motion còn lại của Perfect Dodge!
- **Trigger Conditions:**
  - Dùng `Indestructible` rồi bấm Dodge.
  - Thực hiện Perfect Dodge rồi đánh trúng Boss ngay trong lúc đang Slow Motion.
- **Impact / Consequence:**
  Mất bất tử của kỹ năng `Indestructible` khi Dodge; mất hiệu ứng Slow Motion của Perfect Dodge ngay khi chém trúng Boss.
- **Remediation Plan:**
  - **Minimal Safe Fix (`Bug Fix`):** Thay `bool noDamage` bằng bộ đếm nguồn bất tử (`_invulnerabilityCount > 0`) và quản lý `Time.timeScale` qua bộ điều phối tập trung theo thời điểm kết thúc (`Time.unscaledTime`).
- **Verification Method:**
  Bật `Indestructible` -> Dodge -> kiểm tra vẫn còn bất tử; Perfect Dodge -> chém Boss -> kiểm tra Slow Motion vẫn duy trì đủ `0.8s`.
- **Regression Risk:** Thấp.

---

### [COMBAT-07] `PlayerCounterAttackState` thiếu kích hoạt `WeaponTrail` / `ActiveSlashVfxAction`, và các Attack State có thể để kẹt collider vũ khí nếu bị ngắt giữa chừng
- **ID:** `COMBAT-07`
- **Subsystem:** Combat / Weapon & Player FSM (`PlayerCounterAttackState`, `PlayerAttackState`, `WeaponHanldler`)
- **Category:** `Bug`
- **Severity:** `Medium`
- **Confidence:** `Confirmed`
- **Location:**
  - [`PlayerCounterAttackState.cs:12-34`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Main/PlayerCounterAttackState.cs#L12-L34)
  - [`PlayerAttackState.cs:27-31, 80-84`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Main/PlayerAttackState.cs#L27-L31)
  - [`WeaponHanldler.cs:23-53`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/WeaponHanldler.cs#L23-L53)
- **Evidence & Technical Explanation:**
  1. Khác với [`PlayerAttackState.Enter()`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Main/PlayerAttackState.cs#L29-L30) và [`PlayerHeavyAttackState.Enter()`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Main/PlayerHeavyAttackState.cs#L32-L33), [`PlayerCounterAttackState.Enter()`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Main/PlayerCounterAttackState.cs#L12-L17) không bật `playerStateMachine.WeaponTrail.SetActive(true)` và không đăng ký `ActiveSlashVfxAction`.
  2. Việc bật/tắt GameObject chứa hitbox `WeaponTrail` (`weaponRightLogic` / `weaponLeftLogic`) trong [`WeaponHanldler.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/WeaponHanldler.cs#L23-L53) hoàn toàn phụ thuộc vào Animation Event `OnActiveWeaponRightCollider` và `OnUnActiveWeaponRightCollider` trên clip `.anim`. Nếu nhân vật bị đánh trúng (`HitState`) hoặc hủy đòn (Dodge/Jump) đúng vào khoảng giữa 2 event này, `weaponRightLogic` vẫn giữ trạng thái `activeSelf == true` sau khi đã rời khỏi Attack State!
- **Trigger Conditions:**
  - Thực hiện Counter Attack sau khi Perfect Dodge.
  - Bị đánh ngắt đòn hoặc cancel đòn đánh ngay trong các frame active của vũ khí.
- **Impact / Consequence:**
  Counter Attack thiếu hiệu ứng Trail/Slash VFX; vũ khí có nguy cơ bị kẹt hitbox đang bật sau khi đòn đánh bị ngắt giữa chừng.
- **Remediation Plan:**
  - **Minimal Safe Fix (`Bug Fix`):** Bổ sung `WeaponTrail.SetActive(true/false)` vào `PlayerCounterAttackState` và gọi hàm tắt toàn bộ `weaponRightLogic`/`weaponLeftLogic`/`otherDamageLogic` của `WeaponHandler` trong `Exit()` của mọi Attack State.
- **Verification Method:**
  Trong lúc đang vung kiếm (khi `weaponRightLogic` vừa bật), bấm Dodge ngay lập tức và kiểm tra `weaponRightLogic` tự động tắt trong `Exit()`.
- **Regression Risk:** Không có.

---

### [COMBAT-08] Ba Animation Clip của Boss gọi Animation Event `OnActiveWeaponCollider` không tồn tại trong `WeaponHandler`
- **ID:** `COMBAT-08`
- **Subsystem:** Combat / Weapon (`WeaponHanldler`, Animation Clips)
- **Category:** `Bug`
- **Severity:** `Medium`
- **Confidence:** `Confirmed`
- **Location:**
  - [`WeaponHanldler.cs:23-53`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/WeaponHanldler.cs#L23-L53)
  - `Assets/Character/Animation/Animation File/Boss/Melee_Hook.anim:25150`
  - `Assets/Character/Animation/Animation File/Boss/Sword_Dash_Boss 1.anim:48334`
  - `Assets/Character/Animation/Animation File/Boss/slash9 1.anim:13494`
- **Evidence & Technical Explanation:**
  [`WeaponHanldler.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/WeaponHanldler.cs) định nghĩa `OnActiveWeaponRightCollider()` và `OnActiveWeaponLeftCollider()`, nhưng trong 3 file `.anim` (`Melee_Hook.anim`, `Sword_Dash_Boss 1.anim`, `slash9 1.anim`), Animation Event lại gọi `functionName: OnActiveWeaponCollider` (thiếu chữ `Right` hoặc `Left`).
- **Trigger Conditions:**
  Khi Animator phát một trong 3 animation clip trên.
- **Impact / Consequence:**
  Unity báo lỗi `AnimationEvent 'OnActiveWeaponCollider' has no receiver!` và không bật hitbox vũ khí ở đòn đánh đó.
- **Remediation Plan:**
  - **Minimal Safe Fix (`Bug Fix`):** Thêm hàm chuyển tiếp `public void OnActiveWeaponCollider() => OnActiveWeaponRightCollider();` vào [`WeaponHanldler.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/WeaponHanldler.cs).
- **Verification Method:**
  Phát các clip trên và xác nhận Console không còn báo lỗi `has no receiver`.
- **Regression Risk:** Không có.

---

### [COMBAT-09] `WeaponTrail.LateUpdate()` cấp phát mảng mới mỗi frame qua `Physics.OverlapBox` và `Physics.BoxCastAll`
- **ID:** `COMBAT-09`
- **Subsystem:** Combat / Hit Detection (`WeaponTrail`)
- **Category:** `Performance`
- **Severity:** `Medium`
- **Confidence:** `Confirmed`
- **Location:**
  - [`WeaponTrail.cs:40-64`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/WeaponTrail.cs#L40-L64) — `WeaponTrail.LateUpdate()`
- **Evidence & Technical Explanation:**
  Trong [`WeaponTrail.LateUpdate()`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/WeaponTrail.cs#L48-L61):
  ```csharp
  var overlaps = Physics.OverlapBox(currentPosition, halfExtend, transform.rotation, _layerMask);
  foreach (var overlap in overlaps)
  {
      DealDamage(overlap.gameObject);
  }

  if (sweepDistance > 0.01f)
  {
      var raycastHits = Physics.BoxCastAll(previousPosition, halfExtend, direction, transform.rotation, sweepDistance, _layerMask);
      foreach (var hit in raycastHits)
      {
          DealDamage(hit.collider.gameObject);
      }
  }
  ```
  Mỗi frame `LateUpdate()` khi hitbox vũ khí hoặc chiêu thức đang bật, `Physics.OverlapBox` cấp phát một mảng `Collider[]` mới và `Physics.BoxCastAll` cấp phát một mảng `RaycastHit[]` mới trên managed heap.
- **Trigger Conditions:**
  Mọi đòn tấn công cận chiến hoặc kỹ năng sử dụng `WeaponTrail` của Player, Clone và Boss.
- **Impact / Consequence:**
  Tạo rác bộ nhớ (GC Allocation) liên tục ở mỗi frame trong suốt thời gian giao chiến.
- **Remediation Plan:**
  - **Minimal Safe Fix (`Optimization`):** Thay bằng `Physics.OverlapBoxNonAlloc` và `Physics.BoxCastNonAlloc` sử dụng mảng đệm cấp phát sẵn (`private readonly Collider[] _overlapBuffer = new Collider[16];`, `private readonly RaycastHit[] _hitBuffer = new RaycastHit[16];`).
- **Verification Method:**
  Kiểm tra trên Unity Profiler: cột `GC.Alloc` của `WeaponTrail.LateUpdate()` giảm về `0 B`.
- **Regression Risk:** Không có.

---

## 2. Đánh giá Kiến trúc, Hiệu năng & Điểm mạnh Kỹ thuật (Group 01)

- **Điểm mạnh Kỹ thuật:**
  1. **Quét va chạm kép (`OverlapBox` + `BoxCastAll`) chống xuyên thấu (Anti-Tunneling) trong [`WeaponTrail.cs:40-64`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/WeaponTrail.cs#L40-L64):** Giải quyết triệt để hiện tượng vung kiếm quá nhanh ở FPS thấp làm lưỡi kiếm nhảy qua người mục tiêu giữa 2 frame.
  2. **Danh sách `alreadyObjectHit` (`WeaponTrail.cs:17, 31, 75-77`):** Tự động xóa khi `OnEnable()` và hỗ trợ `ResetObjectHitList()` cho các chiêu nhiều đợt sát thương, đảm bảo mỗi nhát chém chỉ gây sát thương đúng 1 lần lên mỗi mục tiêu.
  3. **Ưu tiên xét `PerfectDodgeAward()` trước khi kiểm tra `noDamage` (`WeaponTrail.cs:81-83`):** Cho phép đòn đánh của Boss khi chạm vào Player đang ở cửa sổ `isPerfectDodge` sẽ kích hoạt phần thưởng Perfect Dodge và bật `noDamage = true` ngay trước dòng kiểm tra `if (health.noDamage) return;`.
- **Điểm yếu Kiến trúc:**
  - [`Health.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/Status/Health.cs) trộn lẫn 3 trách nhiệm: quản lý máu của thực thể, điều khiển `Time.timeScale` toàn cục, và xử lý nâng/hạ cấp chỉ số gắn chặt với `PlayerStateMachine`.
