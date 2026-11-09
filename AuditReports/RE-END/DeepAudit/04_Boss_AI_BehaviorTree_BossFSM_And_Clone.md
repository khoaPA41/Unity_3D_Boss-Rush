# RE-END — Deep Technical Audit: Subsystem Group 04
## Boss AI (Behavior Tree), Boss FSM, PlayerClone AI & Boss Prefabs

- **Phạm vi kiểm toán (Scope):**
  - **Behavior Tree Core & Brain:** [`BehaviorNode.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/Tree%20Behavior/Base/BehaviorNode.cs), [`BehaviorSelector.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/Tree%20Behavior/Base/BehaviorSelector.cs), [`BehaviorSequence.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/Tree%20Behavior/Base/BehaviorSequence.cs), [`BossBehaviorBrain.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/Tree%20Behavior/Brain/BossBehaviorBrain.cs), [`ICombatInput.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/Tree%20Behavior/Dependency%20Injection/ICombatInput.cs).
  - **11 Boss Leaf Nodes:** `CheckBossHealthNode`, `CheckPlayerHealthNode`, `CheckChangePhaseNode`, `CheckUltimateNode`, `CheckCounterAttackNode`, `CheckAttackRangeNode`, `TaskChangePhaseNode`, `TaskUltimateAttackNode`, `TaskCounterAttackNode`, `TaskAttackingNode`, `TaskChasingNode`.
  - **Boss FSM:** [`BossStateMachine.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Boss/Base/BossStateMachine.cs), [`BossBaseState.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Boss/Base/BossBaseState.cs), `BossLocomotionState`, `BossAttackState`, `BossUltimateState`, `BossCounterAttackState`, `BossHitState`, `BossDeathState`.
  - **PlayerClone AI:** [`PlayerCloneStateMachine.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/PlayerClone/Base/PlayerCloneStateMachine.cs), [`PlayerCloneBaseState.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/PlayerClone/Base/PlayerCloneBaseState.cs), `PlayerCloneIdleState`, `PlayerCloneChasingState`, `PlayerCloneAttackState`, `PlayerCloneHitState`, [`CloneBehaviour.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/Tree%20Behavior/CloneBehaviour.cs).

---

## 1. Danh sách Phát hiện Kỹ thuật (Detailed Findings)

### [BOSS-01] `PlayerCloneIdleState.Tick()` thiếu `return;` khiến `PlayerCloneAttackState` bị ghi đè tức thì bởi `PlayerCloneChasingState`
- **ID:** `BOSS-01`
- **Subsystem:** PlayerClone AI (`PlayerCloneIdleState`)
- **Category:** `Bug`
- **Severity:** `Medium`
- **Confidence:** `Confirmed`
- **Location:**
  - [`PlayerCloneIdleState.cs:19-31`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/PlayerClone/Main/PlayerCloneIdleState.cs#L19-L31)
- **Evidence & Technical Explanation:**
  ```csharp
  if (_Timer >= cloneStateMachine.ChangeChasingState)
  {
      if (IsAttackRange())
      {
          cloneStateMachine.SwitchState(new PlayerCloneAttackState(cloneStateMachine, 0));
      }
      cloneStateMachine.SwitchState(new PlayerCloneChasingState(cloneStateMachine));
  }
  ```
  Khi `_Timer >= ChangeChasingState` và Clone đang đứng ngay trong tầm đánh Boss (`IsAttackRange() == true`), dòng 27 chuyển Clone sang `PlayerCloneAttackState`, nhưng do **thiếu `return;`** (hoặc `else`), ngay dòng 29 lại gọi đè `SwitchState(new PlayerCloneChasingState(cloneStateMachine))`!
- **Trigger Conditions:**
  Triệu hồi `PlayerClone` ngay sát Boss (`IsAttackRange() == true`).
- **Impact / Consequence:**
  Chuyển state thừa (`Idle` -> `Attack` -> `Chasing`) trong cùng 1 frame, hủy đòn đánh đầu tiên của Clone.
- **Remediation Plan:**
  - **Minimal Safe Fix (`Bug Fix`):** Thêm `return;` sau dòng 27 (hoặc dùng `else`) trong [`PlayerCloneIdleState.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/PlayerClone/Main/PlayerCloneIdleState.cs#L25-L30).

---

### [BOSS-02] `CheckCounterAttackNode.Evaluate()` quay số ngẫu nhiên mỗi frame và dùng phép so sánh `== 0`, dễ làm mất vĩnh viễn khả năng Counter Attack của Boss
- **ID:** `BOSS-02`
- **Subsystem:** Boss AI Behavior Tree (`CheckCounterAttackNode`, `BossHitState`)
- **Category:** `Bug`
- **Severity:** `High`
- **Confidence:** `Confirmed`
- **Location:**
  - [`CheckCounterAttackNode.cs:18-30`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/Tree%20Behavior/Boss%20Leaf%20Node/CheckCounterAttackNode.cs#L18-L30)
  - [`BossHitState.cs:16-21`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Boss/Main/BossHitState.cs#L16-L21)
- **Evidence & Technical Explanation:**
  Trong [`CheckCounterAttackNode.Evaluate()`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/Tree%20Behavior/Boss%20Leaf%20Node/CheckCounterAttackNode.cs#L20-L29):
  ```csharp
  int hitToNeed = Random.Range(bossBehaviorBrain.EnduredTimes / 2, bossBehaviorBrain.EnduredTimes + 1);
  if (hitToNeed - bossBehaviorBrain.HitReceived == 0)
  {
      bossBehaviorBrain.HitReceived = 0;
      state = NodeState.Success;
      return state;
  }
  ```
  1. `Random.Range(...)` được gọi ở **mỗi frame `Update()`** (60–144 lần/giây). Chỉ cần `HitReceived >= EnduredTimes / 2`, hầu như ngay ở frame đầu tiên hoặc vài frame sau đó, `Random.Range` sẽ quay trúng đúng số `HitReceived`, khiến ngưỡng ngẫu nhiên thực chất luôn tụ về giá trị nhỏ nhất `EnduredTimes / 2`!
  2. Ngược lại, nếu người chơi đánh trúng Boss liên tiếp khi Boss đang thực hiện Ultimate hoặc đang ở nhánh ưu tiên cao hơn trong Behavior Tree khiến `HitReceived` tăng vượt quá `EnduredTimes` (`HitReceived > EnduredTimes`), thì `hitToNeed - HitReceived` sẽ luôn `< 0` (vì `hitToNeed <= EnduredTimes`). Khi đó điều kiện `== 0` **không bao giờ đúng nữa**, `HitReceived` không bao giờ được reset về `0`, và **Boss mất vĩnh viễn khả năng Counter Attack trong suốt phần còn lại của trận đấu**!
- **Trigger Conditions:**
  Boss nhận số đòn đánh `HitReceived > EnduredTimes` trong lúc đang ở nhánh ưu tiên cao hơn của Behavior Tree.
- **Impact / Consequence:**
  1. Xác suất ngẫu nhiên bị hỏng do lấy mẫu lại 60 lần/giây.
  2. Boss bị khóa vĩnh viễn đòn Counter Attack nếu `HitReceived > EnduredTimes`.
- **Remediation Plan:**
  - **Root Cause:** Không lưu ngưỡng `targetHitCount` một lần cho mỗi chu kỳ Counter Attack và dùng phép so sánh `==` thay vì `>=`.
  - **Minimal Safe Fix (`Bug Fix`):**
    1. Lưu một biến `_currentHitThreshold` được random **1 lần duy nhất** khi khởi tạo và mỗi khi Counter Attack kích hoạt.
    2. Đổi điều kiện thành `if (bossBehaviorBrain.HitReceived >= _currentHitThreshold)`.

---

### [BOSS-03] `BehaviorSequence.Evaluate()` không dừng lại khi gặp node con trả về `NodeState.Running`
- **ID:** `BOSS-03`
- **Subsystem:** Boss AI Behavior Tree (`BehaviorSequence`)
- **Category:** `Bug` / `Architecture`
- **Severity:** `Medium`
- **Confidence:** `Confirmed`
- **Location:**
  - [`BehaviorSequence.cs:14-34`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/Tree%20Behavior/Base/BehaviorSequence.cs#L14-L34)
- **Evidence & Technical Explanation:**
  ```csharp
  switch (node.Evaluate())
  {
      case NodeState.Failure:
          state = NodeState.Failure;
          return state;
      case NodeState.Success:
          continue;
      case NodeState.Running:
          anyChildIsRunning = true;
          continue;
  }
  ```
  Theo chuẩn kiến trúc Behavior Tree, một node `Sequence` (AND-gate tuần tự) khi gặp một node con trả về `Running` thì **bắt buộc phải dừng lại ngay (`state = NodeState.Running; return state;`)** để chờ hành động đó hoàn tất ở các frame sau rồi mới chạy sang bước tiếp theo trong chuỗi.
  Việc dùng `continue;` ở nhánh `NodeState.Running` khiến mọi node phía sau một node đang `Running` trong cùng `BehaviorSequence` vẫn bị gọi `Evaluate()` ngay trong cùng 1 frame!
- **Trigger Conditions:**
  Bất kỳ `BehaviorSequence` nào có nhiều hơn 1 task node hoặc có node phía sau một node `Running`.
- **Impact / Consequence:**
  Phá vỡ tính tuần tự của `BehaviorSequence` nếu mở rộng thêm các chuỗi hành động đa bước cho Boss.
- **Remediation Plan:**
  - **Minimal Safe Fix (`Bug Fix`):** Đổi nhánh `case NodeState.Running:` trong [`BehaviorSequence.cs:25-27`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/Tree%20Behavior/Base/BehaviorSequence.cs#L25-L27) thành `state = NodeState.Running; return state;`.

---

### [BOSS-04] `BossAttackState` và `BossUltimateState` comment dòng `_previousTime = 0f` và có nguy cơ kẹt State khi `normalizeTime > 1f`
- **ID:** `BOSS-04`
- **Subsystem:** Boss FSM (`BossAttackState`, `BossUltimateState`)
- **Category:** `Bug`
- **Severity:** `High`
- **Confidence:** `Confirmed`
- **Location:**
  - [`BossAttackState.cs:26, 40-59`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Boss/Main/BossAttackState.cs#L40-L59)
  - [`BossUltimateState.cs:22, 37-55`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Boss/Main/BossUltimateState.cs#L37-L55)
- **Evidence & Technical Explanation:**
  Trong [`BossAttackState.Tick()`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Boss/Main/BossAttackState.cs#L44-L57):
  ```csharp
  var normalizeTime = GetNormalizeTime(bossStateMachine.Animator, AnimationTag, 0);
  if (normalizeTime >= _previousTime && normalizeTime <= 1f)
  {
      if (normalizeTime >= _attackData.ForceTime) TryApplyForce();
      if (normalizeTime >= _attackData.AttackAnimationTime)
      {
          TryCombo(normalizeTime);
      }
  }
  else
  {
      bossStateMachine.IsFinishedAttack = true;
      ReturnLocomotion();
  }
  _previousTime = normalizeTime;
  ```
  - Trong `Enter()`, dòng `// _previousTime = 0f;` và `// _alreadyApplyForce = false;` đã bị comment out (mặc dù hiện tại `BossLocomotionState` tạo `new BossAttackState(...)` mỗi lần nên biến thành viên mới khởi tạo bằng `0`, nhưng nếu tái sử dụng instance state đã cache trong `BossStateMachine.SetupState()`, `_previousTime` và `_alreadyApplyForce` sẽ giữ giá trị cũ!).
  - Nếu trong `AttackData`, nhà thiết kế cấu hình nhầm `AttackAnimationTime >= 1.0f`, hoặc một cú frame spike làm `normalizeTime` nhảy vọt từ `0.85f` qua `1.02f` chỉ trong 1 frame, nhánh `if (normalizeTime <= 1f)` bị bỏ qua và rơi thẳng vào `else`, khiến Boss **bỏ dở chuỗi combo giữa chừng** (`IsFinishedAttack = true`, thoát về `Locomotion` mà không đánh các đòn tiếp theo trong combo)!
- **Trigger Conditions:**
  Frame spike khiến `normalizeTime` nhảy qua `1.0f` trước khi `TryCombo` kịp chạy, hoặc cấu hình `AttackAnimationTime` sát `1.0f`.
- **Impact / Consequence:**
  Ngắt chuỗi combo của Boss sớm hơn thiết kế.
- **Remediation Plan:**
  - **Minimal Safe Fix (`Bug Fix`):** Bỏ comment `_previousTime = 0f; _alreadyApplyForce = false;` trong `Enter()`, và khi `normalizeTime > 1f` thì vẫn kiểm tra `if (_attackData.NextAttackDataIndex != -1) TryCombo(normalizeTime);` trước khi kết thúc combo.

---

## 2. Đánh giá Kiến trúc, Hiệu năng & Điểm mạnh (Group 04)

- **Điểm mạnh:**
  - **Kiến trúc AI 2 tầng (Behavior Tree + Finite State Machine):** Tách biệt rất tốt giữa **"Cái gì cần làm tiếp theo"** (`BossBehaviorBrain` + 11 Leaf Nodes) và **"Làm điều đó như thế nào trong từng frame"** (`BossStateMachine` + 6 Boss States). Cấu trúc này giúp thêm một Boss mới (như Juggernaut, Underwater, Nordic) chỉ bằng cách thay đổi mảng `NormalCombo` / `UltimateCombo` trên Inspector mà không cần viết lại code AI cho từng Boss.
- **Điểm yếu kiến trúc:**
  - Interface [`ICombatInput.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/Tree%20Behavior/Dependency%20Injection/ICombatInput.cs) được tạo ra trong thư mục `Dependency Injection` với ý định tốt (giúp Behavior Tree điều khiển bất kỳ thực thể nào qua interface), nhưng thực tế toàn bộ 11 Leaf Nodes đều giữ tham chiếu cứng tới `BossStateMachine` và `BossBehaviorBrain`, khiến `ICombatInput` trở thành dead abstraction.
  - `PlayerCloneStateMachine.OnEnable()` gọi `GameObject.FindGameObjectWithTag("Boss")` để tìm mục tiêu thay vì nhận mục tiêu từ `PlayerStateMachine` lúc triệu hồi.
