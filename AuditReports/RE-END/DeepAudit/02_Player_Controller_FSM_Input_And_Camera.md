# RE-END — Deep Technical Audit: Subsystem Group 02
## Player Controller, Hierarchical FSM, Input Buffering, Physics & Lock-On Camera

- **Phạm vi kiểm toán (Scope):**
  - **Core FSM:** [`State.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Base/State.cs), [`StateMachine.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Base/StateMachine.cs).
  - **Player State Machine & Base State:** [`PlayerStateMachine.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Base/PlayerStateMachine.cs), [`PlayerBaseState.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Base/PlayerBaseState.cs).
  - **All 17 Player States:** [`FreeLookState.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Main/FreeLookState.cs), [`PlayerTargetState.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Main/PlayerTargetState.cs), [`PlayerChangeAction.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Main/PlayerChangeAction.cs), [`PlayerAttackState.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Main/PlayerAttackState.cs), [`PlayerHeavyAttackState.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Main/PlayerHeavyAttackState.cs), [`PlayerCounterAttackState.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Main/PlayerCounterAttackState.cs), [`PlayerDodgeState.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Main/PlayerDodgeState.cs), [`PlayerStartJumpState.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Main/PlayerStartJumpState.cs), [`PlayerFallState.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Main/PlayerFallState.cs), [`PlayerLandingState.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Main/PlayerLandingState.cs), [`PlayerHitState.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Main/PlayerHitState.cs), [`PlayerAffectedState.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Main/PlayerAffectedState.cs), [`PlayerDeathState.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Main/PlayerDeathState.cs), [`PlayerActiveCheckPointState.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Main/PlayerActiveCheckPointState.cs), [`PlayerUseSkillState.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Main/PlayerUseSkillState.cs), [`PlayerUsePotionState.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Main/PlayerUsePotionState.cs), [`PlayerUseSubPotionState.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Main/PlayerUseSubPotionState.cs).
  - **Physics, Input & Camera:** [`ForceReceiver.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Physics/ForceReceiver.cs), [`InputReader.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Input/InputReader.cs), [`InputBuffering.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Input/InputBuffering.cs), [`InputController.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Input/InputController.cs), [`Targeter.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Target/Targeter.cs), [`Target.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Target/Target.cs).
  - **Serialized Assets Checked:** [`Player_Model.prefab`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Prefabs/Player/Player_Model.prefab), [`Player.prefab`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Prefabs/Player/Player.prefab), [`Player.controller`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Character/Animation/Player/Player.controller).

---

## 1. Danh sách Phát hiện Kỹ thuật (Detailed Findings)

### [PLAYER-01] Chuyển State lồng nhau ngay trong `PlayerTargetState.Enter()` và thiếu `return;` sau khi `SwitchState` trong `Tick()` của nhiều Player State
- **ID:** `PLAYER-01`
- **Subsystem:** Player FSM (`StateMachine`, `PlayerTargetState`, `PlayerAttackState`, `PlayerDodgeState`)
- **Category:** `Bug`
- **Severity:** `High`
- **Confidence:** `Confirmed`
- **Location:**
  - [`StateMachine.cs:11-16`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Base/StateMachine.cs#L11-L16) — `StateMachine.SwitchState(State newState)`
  - [`PlayerTargetState.cs:15-40, 69-74`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Main/PlayerTargetState.cs#L15-L40) — `Enter()` & `Tick(float deltaTime)`
  - [`PlayerAttackState.cs:47-78`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Main/PlayerAttackState.cs#L47-L78) — `Tick(float deltaTime)`
  - [`PlayerDodgeState.cs:42-69, 88-94`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Main/PlayerDodgeState.cs#L42-L69) — `Tick(float deltaTime)` & `CheckCounterattack()`
- **Evidence & Technical Explanation:**
  Hàm [`StateMachine.SwitchState(State newState)`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Base/StateMachine.cs#L11-L16) thực thi đồng bộ `currentState?.Exit(); currentState = newState; currentState?.Enter();`.
  Có 4 điểm trong Player FSM vi phạm tính nguyên tử của chuyển trạng thái:
  1. **Trong [`PlayerTargetState.Enter()`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Main/PlayerTargetState.cs#L15-L32):**
     Khi người chơi bấm Lock-On lúc chưa rút kiếm (`!playerStateMachine.isAttackState`), `PlayerTargetState.Enter()` vừa bật `TargetPoint.gameObject.SetActive(true)` và đăng ký 6 sự kiện input thì dòng 30 gọi `playerStateMachine.EnterChangeAction(true)` -> gọi `SwitchState(new PlayerChangeAction(this, true))` ngay bên trong `Enter()`, làm chạy `PlayerTargetState.Exit()` (tắt ngược `TargetPoint.gameObject.SetActive(false)` trong suốt thời gian rút kiếm).
  2. **Trong [`PlayerTargetState.Tick()`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Main/PlayerTargetState.cs#L36-L74):**
     ```csharp
     if (playerStateMachine.Targeter.currentTarget == null)
     {
         OutTargetState(); // Gọi EnterChangeAction(false) -> SwitchState(PlayerChangeAction)
     }
     ...
     playerStateMachine.HandleAttackState();
     playerStateMachine.HandleHeavyAttackState();
     Move(_movement * speed, deltaTime);
     UpdateAnimation(deltaTime);
     FaceTarget(deltaTime);
     ```
     Khi `currentTarget == null`, `OutTargetState()` đã chuyển sang `PlayerChangeAction` nhưng **thiếu `return;`**, khiến `HandleAttackState()`, `HandleHeavyAttackState()`, `Move()`, `UpdateAnimation()`, `FaceTarget()` của `PlayerTargetState` đã thoát vẫn tiếp tục chạy! Tương tự, `HandleAttackState()` ở dòng 69 nếu chuyển sang `AttackState1` thì dòng 70 (`HandleHeavyAttackState()`) và 72 (`UpdateAnimation`) vẫn chạy tiếp!
  3. **Trong [`PlayerAttackState.Tick()`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Main/PlayerAttackState.cs#L53-L62):**
     ```csharp
     if (playerStateMachine.InputBuffering.TryConsume(ActionType.Attack))
     {
         TryCombo();
     }
     if (playerStateMachine.InputBuffering.TryConsume(ActionType.Jump))
     {
         playerStateMachine.SwitchState(new PlayerStartJumpState(playerStateMachine));
         return;
     }
     ```
     Khi `TryCombo()` chuyển sang đòn combo tiếp theo, do thiếu `return;`, nếu trong `InputBuffering` cũng đang có `ActionType.Jump`, dòng 60 sẽ chạy tiếp và **ghi đè ngay lập tức** đòn combo mới bằng `PlayerStartJumpState` trong cùng 1 frame!
  4. **Trong [`PlayerDodgeState.Tick()`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Main/PlayerDodgeState.cs#L42-L69):**
     Dòng 42 gọi `CheckCounterattack()`. Nếu điều kiện thỏa mãn, `CheckCounterattack()` gọi `SwitchState(new PlayerCounterAttackState(playerStateMachine))` nhưng `Tick()` không `return;` mà tiếp tục chạy `remainingTime -= deltaTime`, và nếu `remainingTime <= 0f` (dòng 56) hoặc `TryConsume(ActionType.Attack)` (dòng 64) thỏa mãn ở cùng frame đó, `PlayerDodgeState.Tick()` sẽ gọi tiếp `ReturnLocomotion()` hoặc `SwitchState(new PlayerAttackState(0))`, **hủy bỏ đòn `PlayerCounterAttackState` vừa mới khởi tạo**!
- **Trigger Conditions:**
  - Mất mục tiêu Lock-on (`currentTarget == null`) hoặc bấm Attack khi đang ở `PlayerTargetState`.
  - Thực hiện Counter Attack ở cuối thời gian Dodge hoặc khi có Attack/Jump trong Input Buffer.
- **Impact / Consequence:**
  Đè trạng thái (State Clobbering), mất đòn Counter Attack hoặc Combo ngay trong cùng một frame.
- **Remediation Plan:**
  - **Minimal Safe Fix (`Bug Fix`):** Thêm `return;` ngay sau mọi nhánh gọi `SwitchState` / `OutTargetState` / `TryCombo` / `CheckCounterattack` trong `PlayerTargetState.Tick()`, `PlayerAttackState.Tick()`, và `PlayerDodgeState.Tick()`.
- **Verification Method:**
  Kiểm tra hủy Lock-on khi Boss biến mất và kích hoạt Counter Attack ở cuối cú Dodge.
- **Regression Risk:** Không có.

---

### [PLAYER-02] Lỗi gọi đè `UpdateFreeLookAnimation` trong `PlayerChangeAction.Tick()` và cấp phát lồng nhau `AnimatorOverrideController` mỗi lần rút/cất kiếm
- **ID:** `PLAYER-02`
- **Subsystem:** Player Animation & FSM (`PlayerChangeAction`, `PlayerBaseState`)
- **Category:** `Bug` (kèm `Performance` / `Memory Leak`)
- **Severity:** `High`
- **Confidence:** `Confirmed`
- **Location:**
  - [`PlayerChangeAction.cs:78-101`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Main/PlayerChangeAction.cs#L78-L101) — `PlayerChangeAction.Tick()` & `Exit()`
  - [`PlayerBaseState.cs:76-81`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Base/PlayerBaseState.cs#L76-L81) — `ChangeSwordIdle(string idleAnimationName, AnimationClip animationClip)`
- **Evidence & Technical Explanation:**
  1. **Lỗi logic trong [`PlayerChangeAction.Tick(float deltaTime)`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Main/PlayerChangeAction.cs#L78-L88):**
     ```csharp
     if (isTarget)
     {
         UpdateTargetAnimation(deltaTime);
     }
     else
     {
         UpdateFreeLookAnimation(deltaTime);
     }

     UpdateFreeLookAnimation(deltaTime);
     ```
     Dòng 87 gọi `UpdateFreeLookAnimation(deltaTime)` **không điều kiện** ngay bên dưới khối `if/else`!
  2. **Lỗi bọc lồng nhau `AnimatorOverrideController` trong [`PlayerBaseState.ChangeSwordIdle()`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Base/PlayerBaseState.cs#L76-L81):**
     ```csharp
     protected void ChangeSwordIdle(string idleAnimationName, AnimationClip animationClip)
     {
         overrideController = new AnimatorOverrideController(playerStateMachine.Animator.runtimeAnimatorController);
         playerStateMachine.Animator.runtimeAnimatorController = overrideController;
         overrideController[idleAnimationName] = animationClip;
     }
     ```
     Mỗi khi người chơi rút kiếm hoặc cất kiếm (`PlayerChangeAction.Exit()`), hàm `ChangeSwordIdle` tạo mới một `new AnimatorOverrideController(...)` bọc chồng lên `runtimeAnimatorController` hiện tại (vốn đã là một `AnimatorOverrideController` từ lần rút kiếm trước), gây rò rỉ các instance `AnimatorOverrideController` cũ trong bộ nhớ native.
- **Trigger Conditions:**
  Mọi lần nhân vật rút kiếm hoặc tra kiếm vào vỏ (`PlayerChangeAction`).
- **Impact / Consequence:**
  Tính toán thừa tham số BlendTree trong lúc rút/cất kiếm và cấp phát lồng nhau `AnimatorOverrideController` mỗi lần đổi tư thế kiếm.
- **Remediation Plan:**
  - **Minimal Safe Fix (`Bug Fix` / `Optimization`):**
    1. Xóa dòng 87 (`UpdateFreeLookAnimation(deltaTime);` dư thừa) trong [`PlayerChangeAction.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Main/PlayerChangeAction.cs#L87).
    2. Khởi tạo `AnimatorOverrideController` **1 lần duy nhất** trên `PlayerStateMachine` và tái sử dụng.
- **Verification Method:**
  Rút/cất kiếm 20 lần liên tiếp và kiểm tra `Animator.runtimeAnimatorController` không tạo mới object.
- **Regression Risk:** Không có.

---

### [PLAYER-03] `New State` trên `UpBodyLayer` của `Player.controller` bị gán nhầm `m_Tag: SwordChange` và `GetNormalizeTime()` đọc nhầm tiến độ khi chuyển giữa 2 State cùng Tag
- **ID:** `PLAYER-03`
- **Subsystem:** Player FSM & Animation (`State.GetNormalizeTime`, `PlayerChangeAction`, `Player.controller`)
- **Category:** `Bug`
- **Severity:** `High`
- **Confidence:** `Confirmed`
- **Location:**
  - [`State.cs:11-28`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Base/State.cs#L11-L28) — `GetNormalizeTime(Animator animator, string animationTag, int layer)`
  - [`PlayerChangeAction.cs:56-65`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Main/PlayerChangeAction.cs#L56-L65)
  - [`Player.controller:414-415`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Character/Animation/Player/Player.controller#L414-L415) — `m_Name: New State`, `m_Tag: SwordChange`
- **Evidence & Technical Explanation:**
  1. Trong [`Player.controller:414-415`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Character/Animation/Player/Player.controller#L414-L415), state mặc định rỗng `New State` trên layer 1 (`UpBodyLayer`) lại được gán `m_Tag: SwordChange` (cùng tag với `Sword_Enter` và `Sword_Exit`). Do đó khi `PlayerChangeAction.Tick()` gọi `GetNormalizeTime(playerStateMachine.Animator, "SwordChange", 1)`, cả `currentState` (`New State`) và `nextState` (`Sword_Enter`) đều có `IsTag("SwordChange") == true`!
  2. Tương tự, trong [`State.GetNormalizeTime()`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Base/State.cs#L11-L28), nếu chuyển tiếp giữa 2 state có cùng Tag (ví dụ `UseSkill` -> `UseSkill` hoặc `Hit` -> `Hit`), ở frame đầu tiên trước khi Animator kịp vào transition (hoặc nếu `AnimationCrossFade == 0`), `currentState` vẫn mang Tag đó với `normalizedTime >= 1.0f`, làm state mới thoát ra ngay lập tức!
- **Trigger Conditions:**
  Rút/cất kiếm (`PlayerChangeAction`) hoặc chuyển tiếp liên tiếp giữa 2 animation có cùng `m_Tag`.
- **Impact / Consequence:**
  Thoát sớm khỏi `PlayerChangeAction` hoặc các Action State có cùng Tag.
- **Remediation Plan:**
  - **Minimal Safe Fix (`Bug Fix`):** Xóa `m_Tag: SwordChange` khỏi `New State` trong `Player.controller` và kiểm tra thêm `shortNameHash` của animation đích trong `GetNormalizeTime`.
- **Verification Method:**
  Kiểm tra animation rút kiếm (`Sword_Enter`) và cất kiếm (`Sword_Exit`) phát đủ 90% thời lượng trước khi thoát `PlayerChangeAction`.
- **Regression Risk:** Thấp.

---

### [PLAYER-04] Tồn tại bản sao Prefab lỗi thời `Player.prefab` song song với `Player_Model.prefab` gây nhầm lẫn cấu hình
- **ID:** `PLAYER-04`
- **Subsystem:** Player Prefab & Configuration (`Player.prefab`, `Player_Model.prefab`)
- **Category:** `Maintainability`
- **Severity:** `Medium`
- **Confidence:** `Confirmed`
- **Location:**
  - [`Player_Model.prefab`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Prefabs/Player/Player_Model.prefab) (`guid: 08ff67a053377ec4d88b0a99775f764c` — **62 tham chiếu trong `Main.unity`**)
  - [`Player.prefab`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Prefabs/Player/Player.prefab) (`guid: d2f09a87a713ec945a99e0911e1a7998` — **0 tham chiếu trong toàn bộ Scenes**)
- **Evidence & Technical Explanation:**
  `Main.unity` chỉ sử dụng `Player_Model.prefab` (`maxHealth = 500`), trong khi `Player.prefab` (`maxHealth = 1000`) là bản cũ không còn được dùng trong bất kỳ Scene nào. Sự tồn tại song song này đã trực tiếp khiến lập trình viên viết nhầm `if (maxHealth == 1000)` trong `[COMBAT-05]` và làm đứt tham chiếu `m_Target: {fileID: 0}` của 6 nút UI Sub-Potion trong `[LIFE-04]`.
- **Trigger Conditions:**
  Chỉnh sửa nhầm `Player.prefab` thay vì `Player_Model.prefab`.
- **Impact / Consequence:**
  Gây nhầm lẫn nguồn dữ liệu chuẩn (Single Source of Truth) của Player.
- **Remediation Plan:**
  - **Minimal Safe Fix (`Refactor`):** Xóa hoặc di chuyển `Player.prefab` vào thư mục lưu trữ cũ.
- **Verification Method:**
  Kiểm tra `Main.unity` hoạt động bình thường với `Player_Model.prefab`.
- **Regression Risk:** Không có.

---

### [PLAYER-05] `ForceReceiver.Update()` cộng dồn `VerticalVelocity` không có giới hạn tốc độ rơi tối đa (Terminal Velocity)
- **ID:** `PLAYER-05`
- **Subsystem:** Player / Boss Physics (`ForceReceiver`)
- **Category:** `Missing Safeguard`
- **Severity:** `Low`
- **Confidence:** `Confirmed`
- **Location:**
  - [`ForceReceiver.cs:21-33`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Physics/ForceReceiver.cs#L21-L33)
- **Evidence & Technical Explanation:**
  Khi nhân vật ở trên không (`!characterController.isGrounded`), `VerticalVelocity += UnityEngine.Physics.gravity.y * Time.deltaTime;` giảm tuyến tính không có ngưỡng chặn dưới (Terminal Velocity), có nguy cơ gây xuyên sàn nếu rơi từ độ cao lớn hoặc khi `isGrounded` tạm thời `false` trên dốc.
- **Trigger Conditions:**
  Nhân vật rơi lâu hoặc trượt trên địa hình dốc.
- **Impact / Consequence:**
  Vận tốc rơi âm quá lớn.
- **Remediation Plan:**
  - **Minimal Safe Fix (`Bug Fix`):** Giới hạn `VerticalVelocity = Mathf.Max(VerticalVelocity + UnityEngine.Physics.gravity.y * Time.deltaTime, -40f);`.
- **Verification Method:**
  Kiểm tra nhảy và rơi trên địa hình trong `Main.unity`.
- **Regression Risk:** Không có.

---

### [PLAYER-06] `Targeter.CancelTarget()` bị khóa chết khi `targetList.Count == 0` và `SelectedTarget()` có thể khóa nhầm mục tiêu phía sau lưng Camera
- **ID:** `PLAYER-06`
- **Subsystem:** Combat / Targeting & Camera (`Targeter`, `Target`)
- **Category:** `Bug`
- **Severity:** `Medium`
- **Confidence:** `Confirmed`
- **Location:**
  - [`Targeter.cs:39-43, 45-80`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Target/Targeter.cs#L39-L80)
- **Evidence & Technical Explanation:**
  1. **Lỗi kẹt `currentTarget` trong [`Targeter.CancelTarget()`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Target/Targeter.cs#L75-L80):**
     ```csharp
     private void OnTriggerExit(Collider other)
     {
         if (!other.TryGetComponent<Target>(out var target)) { return; }
         targetList.Remove(target);
     }
     ...
     public void CancelTarget()
     {
         if (targetList.Count == 0) { return; }
         cinemachineTargetGroup.RemoveMember(currentTarget?.transform);
         currentTarget = null;
     }
     ```
     Khi Boss lùi ra ngoài vùng `SphereCollider` của `Targeter`, `OnTriggerExit` xóa Boss khỏi `targetList` (`targetList.Count` trở về `0`) nhưng **không hủy sự kiện `target.CancelTargetEvent -= RemoveTarget`** và **không hủy Lock-on**. Nghiêm trọng hơn, khi người chơi bấm nút tắt Lock-on (gọi `CancelTarget()`), dòng 77 kiểm tra `if (targetList.Count == 0) { return; }` sẽ **thoát hàm ngay lập tức**, khiến `currentTarget` không bao giờ được gỡ khỏi `cinemachineTargetGroup` và không bao giờ về `null`!
  2. **Lỗi chọn mục tiêu sau lưng Camera trong [`Targeter.SelectedTarget()`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Target/Targeter.cs#L54-L59):**
     ```csharp
     Vector2 viewPoint = mainCamera.WorldToViewportPoint(target.transform.position);
     if (viewPoint.x < 0 || viewPoint.x > 1 || viewPoint.y < 0 || viewPoint.y > 1) continue;
     ```
     Hàm `Camera.WorldToViewportPoint` trả về `Vector3`, trong đó `z < 0` nghĩa là điểm đó nằm **phía sau lưng Camera**. Việc ép kiểu trực tiếp sang `Vector2` làm mất thành phần `z`, khiến mục tiêu đứng ngay sau lưng Camera vẫn có `0 <= x, y <= 1` và bị Lock-on!
- **Trigger Conditions:**
  - Đang Lock-on mà Boss lùi ra ngoài tầm `Targeter` (`targetList.Count == 0`) rồi người chơi bấm tắt Lock-on.
  - Bấm Lock-on khi Boss đang đứng phía sau lưng Camera.
- **Impact / Consequence:**
  Kẹt cứng chế độ Lock-on khi Boss ra khỏi vùng `Targeter`; khóa mục tiêu ngược phía sau Camera.
- **Remediation Plan:**
  - **Minimal Safe Fix (`Bug Fix`):**
    1. Trong `CancelTarget()`, đổi `if (targetList.Count == 0) { return; }` thành `if (currentTarget == null) { return; }`.
    2. Trong `OnTriggerExit()`, gọi `RemoveTarget(target);` thay vì chỉ `targetList.Remove(target);`.
    3. Trong `SelectedTarget()`, giữ kiểu `Vector3 viewPoint = mainCamera.WorldToViewportPoint(...)` và kiểm tra `if (viewPoint.z <= 0f || viewPoint.x < 0f || viewPoint.x > 1f || viewPoint.y < 0f || viewPoint.y > 1f) continue;`.
- **Verification Method:**
  Đứng quay lưng lại phía Boss và bấm Lock-on -> xác nhận không khóa mục tiêu sau lưng; khi đang Lock-on mà Boss nhảy ra xa khỏi vùng `Targeter` -> tự động thoát Lock-on an toàn.
- **Regression Risk:** Không có.

---

## 2. Đánh giá Kiến trúc, Hiệu năng & Điểm mạnh Kỹ thuật (Group 02)

- **Điểm mạnh Kỹ thuật:**
  1. **Thiết kế `InputBuffering.cs` độ phức tạp $O(1)$ không cấp phát bộ nhớ:** Lưu mốc thời gian nhấn phím cuối cùng (`_lastInputTimeDictionary`) và kiểm tra `Time.time - lastTime <= bufferWindow` trong `TryConsume(ActionType)` mà không cần vòng lặp `Update()`.
  2. **Tiền tính toán `Animator.StringToHash` nhất quán** trên toàn bộ 17 Player States.
- **Điểm yếu Kiến trúc:**
  - `PlayerStateMachine.cs` (444 dòng) đóng vai trò God Class vừa điều khiển State Machine, vừa chứa toàn bộ tham chiếu UI/Audio/Mesh/Status/Upgrade.
