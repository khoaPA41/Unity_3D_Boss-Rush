# RE-END — 06. Developer Technical Skill Assessment & Learning Gap Analysis (`06_Developer_Skill_Assessment.md`)

- **Ngày đánh giá:** 2026-11-09
- **Nguyên tắc đánh giá (Mục 4 & Mục 8):**
  - Đánh giá khách quan, dựa hoàn toàn trên bằng chứng kỹ thuật thu được từ mã nguồn và cấu hình dự án **RE-END**.
  - **Không suy luận năng lực cá nhân toàn diện** chỉ từ một dự án, và **không giả định** một người duy nhất tự viết toàn bộ mã nguồn (dự án có thể có nhiều thành viên hoặc tái sử dụng tài nguyên học tập).
  - Phân biệt minh bạch 3 mức độ tin cậy của nhận định:
    1. **[Mức 1 — Bằng chứng trực tiếp trong Source Code]:** Khẳng định chắc chắn từ code/YAML thực tế.
    2. **[Mức 2 — Nhận định có cơ sở nhưng chưa chắc chắn]:** Suy luận hợp lý từ quy luật xuất hiện của code, nhưng có thể do áp lực thời gian (deadline) chứ không hẳn do thiếu kiến thức nền tảng.
    3. **[Mức 3 — Chưa đủ bằng chứng để đánh giá từ Repository]:** Những kỹ năng không xuất hiện trong phạm vi dự án này.

---

## 1. Đánh giá Năng lực Kỹ thuật Thể hiện qua Dự án (Technical Competency Evaluation)

### 1.1. Những kiến thức đã vận dụng tốt ([Mức 1 — Bằng chứng trực tiếp trong Source Code])
1. **Lập trình Gameplay 3D Action & Toán học Vector 3D:**
   - Hiểu và triển khai đúng phép chiếu vector hướng nhìn Camera (`forward`, `right`) lên mặt phẳng ngang (`y = 0`) trong [`PlayerBaseState.cs:53-74`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Base/PlayerBaseState.cs#L53-L74) để tính hướng di chuyển tự do và di chuyển strafe khóa mục tiêu.
   - Triển khai tốt cơ chế xoay VFX vết chém bám theo quỹ đạo vung kiếm trong không gian 3D bằng `Quaternion.FromToRotation(Vector3.right, slashDir.normalized)` trong [`PlayerAttackState.cs:110-121`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Main/PlayerAttackState.cs#L110-L121).
   - Giải quyết bài toán va chạm vũ khí cận chiến tốc độ cao bằng sự kết hợp giữa `Physics.OverlapBox` (vị trí hiện tại) và `Physics.BoxCastAll` (quét đoạn đường từ `previousPosition` đến `currentPosition`) trong [`WeaponTrail.cs:40-64`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/WeaponTrail.cs#L40-L64), kèm vẽ Gizmos bằng `Matrix4x4.TRS` (`:93-105`).
2. **Vận dụng các Design Pattern cốt lõi của Game Development vào bài toán thực tế:**
   - **State Pattern:** Xây dựng thành công hệ thống FSM cho 3 nhóm thực thể (`PlayerStateMachine` với 17 states, `BossStateMachine` với 6 states, `PlayerCloneStateMachine` với 4 states).
   - **Behavior Tree:** Tự cài đặt bộ khung `BehaviorNode`, `BehaviorSelector`, `BehaviorSequence` và kết hợp với FSM để điều khiển 3 Boss có hành vi đa dạng ([`BossBehaviorBrain.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/Tree%20Behavior/Brain/BossBehaviorBrain.cs)).
   - **Object Pooling:** Cài đặt [`ObjectPooling.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/Object%20Pooling/ObjectPooling.cs) dựa trên `Dictionary<string, Stack<PooledObject>>`, có cờ chống trả trùng `isReleased` ([`PooledObject.cs:9-23`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/Object%20Pooling/PooledObject.cs#L9-L23)) và tích hợp callback native `OnParticleSystemStopped` ([`SkillPooledObject.cs:19-22`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/SkillPooledObject.cs#L19-L22)).
   - **Input Buffering:** Cài đặt hàng đợi đệm phím bấm theo mốc thời gian `Time.time` gọn nhẹ, độ phức tạp $O(1)$ trong [`InputBuffering.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Input/InputBuffering.cs).
3. **Sử dụng hệ sinh thái Package hiện đại của Unity 6:**
   - Tích hợp trơn tru Unity Input System mới (`IPlayerActions` trong [`InputReader.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Input/InputReader.cs)), Cinemachine 3 (`CinemachineTargetGroup` trong [`Targeter.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Target/Targeter.cs)), Timeline (`PlayableDirector` trong [`TimelineEvent.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Cutscene/TimelineEvent.cs)), và URP Post-Processing (`VolumeProfile` trong [`GraphicManager.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Managers/GraphicManager.cs)).

---

### 1.2. Những kiến thức có dấu hiệu hiểu chưa đầy đủ hoặc áp dụng chưa phù hợp ([Mức 1 & Mức 2])

1. **Tính bất biến của Luồng điều khiển (Control-Flow Invariants) trong State Machine & Behavior Tree ([Mức 1 — Bằng chứng trực tiếp]):**
   - Trong [`StateMachine.SwitchState(newState)`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Base/StateMachine.cs#L11-L16), hàm `Exit()` của state cũ và `Enter()` của state mới chạy **ngay lập tức và đồng bộ**. Tuy nhiên ở nhiều nơi (`PlayerTargetState.Enter:28-31`, `PlayerTargetState.Tick:36-39`, `PlayerAttackState.Tick:53-56`, `PlayerDodgeState.Tick:42, 64-67`, `PlayerCloneIdleState.Tick:25-30`), lập trình viên gọi `SwitchState(...)` nhưng **không `return;`**, để mặc các dòng code bên dưới của state đã thoát tiếp tục chạy và ghi đè lên state mới.
   - Trong [`BehaviorSequence.Evaluate()`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/Tree%20Behavior/Base/BehaviorSequence.cs#L25-L27), nhánh `case NodeState.Running:` lại dùng `continue;` thay vì `return NodeState.Running;`, cho thấy sự nhầm lẫn giữa việc "duyệt hết danh sách" và "chờ hành động tuần tự hoàn tất".
2. **Quản lý Trạng thái Chia sẻ (Shared Mutable State) & Các Tác vụ Bất đồng bộ theo Thời gian (Coroutines/Timers) ([Mức 1 — Bằng chứng trực tiếp]):**
   - Khi nhiều hệ thống cùng tác động lên một thuộc tính (`Health.noDamage`, `Time.timeScale`, `PlayerStateMachine.Invisible`, `AttackData[i].AttackDamage`), code hiện tại gán đè trực tiếp giá trị boolean/float (`noDamage = false`, `Time.timeScale = 1f`, `AttackDamage *= 2`) từ nhiều Coroutine hoặc `Exit()` khác nhau mà không tính đến trường hợp **2 hiệu ứng diễn ra chồng lấn (overlapping lifetimes)**.
3. **Trừu tượng hóa hướng đối tượng (Interface Design & SOLID) ([Mức 1 & Mức 2]):**
   - Đã tạo interface [`ICaster.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/Skill%20Factory/ICaster.cs) và [`ICombatInput.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/Tree%20Behavior/Dependency%20Injection/ICombatInput.cs), nhưng:
     - `ICombatInput` không được bất kỳ class nào gọi tới.
     - Trong [`Skill.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/Skill%20Factory/Skill.cs), gần như toàn bộ 15 lớp implement `ISkill.Cast(ICaster caster)` đều lập tức gọi `caster.GetTransform().GetComponent<PlayerStateMachine>()` hoặc `GetComponent<BossStateMachine>()`. Điều này cho thấy người viết hiểu cú pháp `interface` và `Factory`, nhưng chưa nắm vững cách thiết kế hợp đồng giao tiếp (contract) trên interface để tránh phụ thuộc ngược vào lớp cụ thể (Downcasting / Concrete Coupling).
4. **Xác suất & Lấy mẫu Ngẫu nhiên trong Vòng lặp Frame (`Update`) ([Mức 1 — Bằng chứng trực tiếp]):**
   - Trong [`CheckCounterAttackNode.Evaluate()`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/Tree%20Behavior/Boss%20Leaf%20Node/CheckCounterAttackNode.cs#L22-L23), việc gọi `Random.Range(...)` ở mỗi frame `Update()` và kiểm tra `hitToNeed - HitReceived == 0` thể hiện sự nhầm lẫn giữa **biến ngẫu nhiên quyết định 1 lần theo sự kiện (Event-driven random roll)** và **biến ngẫu nhiên lấy mẫu lại 60 lần mỗi giây (Per-frame polling)**.

---

### 1.3. Những thói quen lập trình cần cải thiện ([Mức 1 & Mức 2])
1. **Sử dụng Magic Strings & Hardcoded Numbers thiếu đối chiếu với Serialized Data ([Mức 1]):**
   - Dùng chuỗi `"ReduceStamina"`, `"IncreaseDame"`, `"ReduceDame"` trong [`PlayerUseSubPotionState.cs:40-51`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Main/PlayerUseSubPotionState.cs#L40-L51) trong khi Prefab lưu `"EmeraldGuava"`, `"GoldenPear"`, `"BloodPomegranate"` (`[COMBAT-01]`).
   - Hardcode điều kiện chặn dưới `if (maxHealth == 1000)` / `if (currentMana == 1000)` trong [`Health.cs:145`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/Status/Health.cs#L145) và [`Mana.cs:47`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/Status/Mana.cs#L47) trong khi Prefab thực tế có `maxHealth = 500` và `maxMana = 100` (`[COMBAT-05]`).
2. **Copy-Paste Code thay vì Tái sử dụng Component / Hàm dùng chung ([Mức 1]):**
   - Nhân đôi toàn bộ biến UI cho `*NonCheckpoint` trong [`GraphicsUI.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/UI/GraphicsUI.cs) và [`UIPlayerManagers.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/UI/UIPlayerManagers.cs), dẫn tới quên gắn event listener cho nhánh `*NonCheckpoint` (`[LIFE-04]`).
   - Các lỗi nhỏ do copy-paste còn sót lại: gọi `UpdateFreeLookAnimation` 2 lần ở [`PlayerChangeAction.cs:87`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Main/PlayerChangeAction.cs#L87), gán `player.Invisible = false;` rồi ngay dòng dưới gán `player.Invisible = true;` ở [`Skill.cs:113-115`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/Skill%20Factory/Skill.cs#L113-L115).
3. **Thiếu kiểm tra biên (Boundary Checks) cho các phép toán Gameplay, Camera & Save/Load ([Mức 1]):**
   - Quên `Mathf.Max(0f, damage - resistance)` trong `Health.DealDamage` (`[COMBAT-02]`), quên guard `if (currentHealth <= 0f) return;` (`[COMBAT-03]`), quên kiểm tra `viewPoint.z > 0` trong `Targeter.SelectedTarget()` (`[PLAYER-06]`), và quên tạo bản lưu khởi điểm khi bắt đầu `New Game` (`[LIFE-01]`).

---

### 1.4. Những kỹ năng chưa có đủ bằng chứng để đánh giá ([Mức 3 — Unverifiable from Repository])
Từ repository hiện tại, **không thể kết luận** về năng lực của người phát triển trong các lĩnh vực sau (do dự án không sử dụng hoặc không thuộc phạm vi yêu cầu của đồ án):
- Kỹ năng viết Automated Unit Tests / PlayMode Integration Tests (`NUnit` / Unity Test Framework).
- Kỹ năng tối ưu hóa Shader chuyên sâu (HLSL / ShaderGraph nâng cao), DOTS/ECS/Jobs/Burst, hoặc Networking/Multiplayer.
- Kỹ năng làm việc nhóm với Git branching, CI/CD, hoặc phân rã module bằng `.asmdef` cho dự án quy mô lớn.

---

## 2. Phân tích Khoảng trống Kiến thức (Learning Gap Analysis — Mục 5)

Dưới đây là **5 chủ đề học tập trọng tâm nhất**, được trích xuất trực tiếp từ các lỗi và thiếu sót đã xác minh trong **RE-END** (mỗi chủ đề trả lời đầy đủ 6 câu hỏi bắt buộc):

---

### Chủ đề 1: Thiết kế State Machine, Behavior Tree & Bất biến Luồng điều khiển (FSM/BT Control-Flow Invariants)
1. **Tại sao cần học?**
   - Trong game hành động thời gian thực, State Machine và Behavior Tree chạy ở tần suất 60–144 lần/giây. Chỉ một câu lệnh chạy rớt lại sau khi State đã chuyển đổi hoặc một node trả về sai trạng thái `Running` sẽ gây ra các lỗi ghi đè trạng thái (State Clobbering) cực kỳ khó tái hiện bằng mắt thường.
2. **Thiếu sót nào trong RE-END cho thấy nhu cầu đó?**
   - `[PLAYER-01]`: `PlayerTargetState.Enter()` gọi `EnterChangeAction(true)` (bên trong gọi `SwitchState`) làm chạy `PlayerTargetState.Exit()` ngay khi vừa vào state; `PlayerAttackState.Tick()`, `PlayerDodgeState.Tick()`, `PlayerTargetState.Tick()` và `PlayerCloneIdleState.Tick()` (`[BOSS-01]`) gọi `SwitchState` nhưng thiếu `return;`.
   - `[BOSS-03]`: `BehaviorSequence.Evaluate()` dùng `continue;` khi gặp `NodeState.Running` thay vì dừng lại chờ frame sau.
   - `[BOSS-02]`: `CheckCounterAttackNode` gọi `Random.Range` mỗi frame trong `Evaluate()`.
3. **Cần nắm những khái niệm cụ thể nào?**
   - **Atomic State Transition & Reentrancy Guard:** Nguyên tắc "Một khi `SwitchState` được gọi, frame hiện tại của State cũ phải chấm dứt ngay lập tức" (hoặc dùng cơ chế **Deferred State Transition**: `RequestTransition(newState)` chỉ đổi state ở đầu/cuối `Update`).
   - **Vòng đời của Behavior Tree Node (`OnInit` / `OnEnter` -> `OnUpdate` -> `OnExit`):** Phân biệt rõ logic chạy 1 lần khi bắt đầu Node (như chọn số ngẫu nhiên `_targetHitCount`) và logic kiểm tra mỗi frame trong `Evaluate()`.
   - **Ngữ nghĩa chuẩn của Composite Nodes (`Sequence`, `Selector`)** khi xử lý trạng thái `Running`.
4. **Có thể luyện tập bằng bài tập nào?**
   - Viết một `StateMachine` nhỏ có cơ chế phát hiện lỗi Reentrancy (báo `Debug.LogError` nếu `SwitchState` bị gọi 2 lần trong cùng 1 frame hoặc nếu `SwitchState` được gọi bên trong `Enter()`/`Exit()`).
   - Cài đặt một `BehaviorSequence` chuẩn và viết kịch bản 3 bước tuần tự cho AI: `Chờ 1 giây` -> `Lướt tới mục tiêu` -> `Đánh 1 đòn`.
5. **Có thể áp dụng lại vào RE-END như thế nào?**
   - Rà soát và thêm `return;` vào toàn bộ các điểm gọi `SwitchState` trong 17 Player States, 6 Boss States và 4 Clone States của RE-END; sửa `BehaviorSequence.cs` và `CheckCounterAttackNode.cs`.
6. **Tiêu chí nào cho thấy đã nắm vững kiến thức?**
   - Không còn bất kỳ trường hợp nào trong dự án mà 2 State khác nhau cùng gọi `Enter()` trong cùng 1 frame `Update()`, và mọi biến ngẫu nhiên của AI chỉ được gieo xúc xắc đúng 1 lần cho mỗi quyết định.

---

### Chủ đề 2: Quản lý Trạng thái Thời gian Thực, Buff/Modifier & Tác vụ Bất đồng bộ (Buff Architecture & Overlapping Lifetimes)
1. **Tại sao cần học?**
   - Trong game RPG/Action, nhân vật luôn chịu tác động của nhiều hiệu ứng chồng lấn nhau theo thời gian (vừa bật kỹ năng bất tử `Indestructible`, vừa lộn nhào `Dodge`; vừa kích hoạt Slow Motion của `Perfect Dodge`, vừa chém trúng Boss gây `HitStop`; vừa bật `WorldBreaker` tăng sát thương, vừa uống thuốc `IncreaseDame`). Nếu dùng biến `bool` đơn lẻ hoặc nhân trực tiếp vào chỉ số gốc (`AttackDamage *= 2`), các hiệu ứng sẽ triệt tiêu nhau hoặc làm hỏng vĩnh viễn dữ liệu gốc.
2. **Thiếu sót nào trong RE-END cho thấy nhu cầu đó?**
   - `[COMBAT-04]`: `WorldBreaker` nhân trực tiếp `AttackData[i].AttackDamage *= 2` và bật `Invincible = true` vĩnh viễn, làm hỏng cả file save `ReSave.json`.
   - `[COMBAT-06]`: `PlayerDodgeState.Exit()` gán `noDamage = false`, vô tình tắt luôn bất tử của kỹ năng `Indestructible`; `TimeFreeHit()` gán `Time.timeScale = 1f` sau `0.05s`, hủy luôn `0.8s` Slow Motion của Perfect Dodge.
   - `[SKILL-01]`: Kỹ năng `Invisible` có 2 bộ hẹn giờ chồng chéo ở `Skill.cs` (4s) và `PlayerUseSkillState.cs` (5s).
   - `[SKILL-02]`: Các kỹ năng Boss đăng ký `+= situationAction` vào `ManageAnimationSkillEvent.SituationEvent` nhưng không hủy khi Boss bị đánh ngắt đòn.
3. **Cần nắm những khái niệm cụ thể nào?**
   - **Base Stat vs Runtime Stat Modifiers:** Không bao giờ ghi đè lên `baseDamage`. Luôn tính:
     $$\text{FinalDamage} = (\text{BaseDamage} + \sum \text{FlatBonus}) \times \prod \text{Multipliers}$$
   - **Reference Counting / Token-based State Locking:** Thay `bool noDamage` bằng bộ đếm số nguồn đang kích hoạt (`int _invulnerableSources > 0`) hoặc danh sách `HashSet<string> _activeImmunities`.
   - **Priority/Stack-based TimeScale Controller:** Quản lý các yêu cầu thay đổi `Time.timeScale` theo danh sách có thời hạn (`Time.unscaledTime`), luôn lấy mức `timeScale` ưu tiên cao nhất đang còn hạn, và chỉ trả về `1.0f` khi mọi yêu cầu đã hết hạn.
   - **Cancellation Lifecycle cho Coroutine / Event Callbacks:** Luôn lưu tham chiếu `Coroutine` hoặc dọn dẹp callback trong `Exit()` / `OnDisable()` khi hành động bị ngắt.
4. **Có thể luyện tập bằng bài tập nào?**
   - Xây dựng một class độc lập `StatFloat` (có `BaseValue`, `AddModifier(Modifier)`, `RemoveModifier(source)`) và một `TimeScaleController` xử lý đúng kịch bản: Bật Slow-Mo `0.5x` trong `1.0s`, ở giây thứ `0.2s` kích hoạt HitStop `0.0x` trong `0.05s`, kiểm tra ở giây `0.25s` thời gian quay về `0.5x` và ở giây `1.0s` quay về `1.0x`.
5. **Có thể áp dụng lại vào RE-END như thế nào?**
   - Áp dụng trực tiếp để sửa `[COMBAT-04]`, `[COMBAT-06]`, `[SKILL-01]`, `[SKILL-02]` trong `Health.cs`, `PlayerStateMachine.cs`, `Skill.cs` và `ManageAnimationSkillEvent.cs`.
6. **Tiêu chí nào cho thấy đã nắm vững kiến thức?**
   - Có thể kích hoạt đồng thời bất kỳ tổ hợp Kỹ năng + Sub-Potion + Dodge + HitStop nào rồi lưu game ngay giữa trận mà chỉ số gốc trong `ReSave.json` không bao giờ bị sai lệch.

---

### Chủ đề 3: Thiết kế Dữ liệu Bền vững, Serialization & Ràng buộc Toàn vẹn (Data Integrity, Serialization & Boundary Validation)
1. **Tại sao cần học?**
   - Mọi dữ liệu đi qua ranh giới Serialization (Inspector Prefab YAML, Scene `OnClick` Events, JSON Save File) đều có thể bị lệch tên, thiếu khởi tạo, hoặc vượt biên toán học nếu không có lớp kiểm tra toàn vẹn (Validation & Defensive Programming).
2. **Thiếu sót nào trong RE-END cho thấy nhu cầu đó?**
   - `[COMBAT-01]`: Lệch chuỗi `potionName` giữa Prefab (`"EmeraldGuava"`) và code (`"ReduceStamina"`).
   - `[LIFE-01]`: `new SaveData()` tạo các trường bằng `0`, gây lỗi hồi sinh toàn chỉ số `0` khi chết trước Checkpoint đầu tiên.
   - `[LIFE-02]` & `[LIFE-03]`: `AutoSave()` gọi trước khi cộng `spiritualPower`; `SaveData` thiếu trường `spiritualPower` và ghi đè 4 đòn combo thành 1 giá trị; `CompletedList` không đồng bộ với `CurrentSaveData.completedTriggerBoss`.
   - `[COMBAT-02]` & `[COMBAT-05]`: Thiếu chặn biên dưới trong công thức `damage - resistance` và trong hàm trừ chỉ số `SubHealth`/`SubMana`/`SubtractUsagePotion`.
3. **Cần nắm những khái niệm cụ thể nào?**
   - **Type-Safe Identifiers (Enums / ScriptableObject IDs thay cho Magic Strings):** Không dùng `string` rời rạc cho logic `switch/case`.
   - **Single Source of Truth (SSOT) trong Save System:** Không duy trì 2 danh sách rời rạc (`CompletedList` và `completedTriggerBoss`) mà không đóng gói qua một hàm truy cập chung (`MarkBossCompleted(id)`).
   - **Default Initialization & Schema Validation cho DTO (`SaveData`):** Bước `CreateNewSaveGame()` phải ghi nhận dữ liệu chuẩn từ cấu hình gốc (Default Config / Initial AutoSave), không bao giờ để giá trị `0` chưa khởi tạo.
   - **Mathematical Clamping & Precondition Guards:** Luôn đặt `Guard Clauses` ở đầu các hàm thay đổi trạng thái (`if (currentHealth <= 0f) return;`, `if (maxHealth <= _initialMaxHealth) return;`).
4. **Có thể luyện tập bằng bài tập nào?**
   - Viết một module `SaveLoadService` nhỏ với `PlayerSaveDTO`, kiểm thử các kịch bản biên: (a) Chết ngay giây thứ 1 của New Game, (b) Nâng cấp rồi hạ cấp chỉ số liên tục 100 lần, (c) Lưu/Tải mảng sát thương nhiều đòn đánh.
5. **Có thể áp dụng lại vào RE-END như thế nào?**
   - Thay chuỗi loại Sub-Potion bằng `enum SubPotionType`, sửa `SaveData.cs`, `SaveManagers.cs`, `GameManagers.cs` và các hàm nâng cấp trong `Health`/`Mana`/`Stamina`/`Potion`.
6. **Tiêu chí nào cho thấy đã nắm vững kiến thức?**
   - Không còn bất kỳ magic string nào trong câu lệnh `switch` của gameplay logic; mọi hàm nhận sát thương và nâng/hạ cấp chỉ số đều có bất biến biên dưới/biên trên chặt chẽ.

---

### Chủ đề 4: Nguyên lý SOLID thực chiến trong Unity (SRP, DIP) & Tái sử dụng UI/Component
1. **Tại sao cần học?**
   - Khi dự án lớn dần, việc nhồi nhét logic vào một class trung tâm (`PlayerStateMachine`, `Health`) hoặc nhân đôi code UI (`Checkpoint` vs `NonCheckpoint`) khiến mỗi lần sửa một tính năng lại làm hỏng một tính năng khác.
2. **Thiếu sót nào trong RE-END cho thấy nhu cầu đó?**
   - [`Health.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/Status/Health.cs) vừa tính máu, vừa điều khiển `Time.timeScale`, vừa xử lý nút nâng cấp chỉ số tại Checkpoint, vừa kiểm tra `if (gameObject.tag != "Boss")`.
   - [`GraphicsUI.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/UI/GraphicsUI.cs) và [`UIPlayerManagers.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/UI/UIPlayerManagers.cs) nhân đôi toàn bộ các trường UI cho `*NonCheckpoint`, dẫn đến quên gọi `AddListener` cho 11 control đồ họa ngoài Checkpoint (`[LIFE-04]`).
   - `ISkill.Cast(ICaster)` ép kiểu ngược về `PlayerStateMachine` / `BossStateMachine` ở cả 15 kỹ năng.
3. **Cần nắm những khái niệm cụ thể nào?**
   - **Single Responsibility Principle (SRP):** Tách biệt **Data/Logic Component** (`Health` chỉ quản lý HP và phát sự kiện `OnDamaged`, `OnDied`) khỏi **Upgrade Presenter** (`StatusUpgradePresenter`) và **Global Feedback** (`TimeScaleManager`).
   - **Dependency Inversion & Meaningful Interfaces:** Thiết kế `ICaster` cung cấp đủ các phương thức mà `ISkill` thực sự cần (hoặc tách `IPlayerSkill` và `IBossSkill`) thay vì truyền một interface rỗng rồi gọi `GetComponent<ConcreteClass>()`.
   - **Programmatic UI Binding & Single-Instance Panel Reuse:** Dùng chung 1 Panel UI cho nhiều ngữ cảnh, và ưu tiên đăng ký sự kiện nút bấm bằng code (`button.onClick.AddListener(...)`) thay vì kéo thả thủ công hàng chục `m_PersistentCalls` trong Scene YAML dễ bị đứt tham chiếu (`m_Target: {fileID: 0}`).
4. **Có thể luyện tập bằng bài tập nào?**
   - Refactor `Health.cs` và `GraphicsUI.cs` sao cho `Health.cs` không còn tham chiếu tới `PlayerStateMachine` và `GraphicsUI.cs` chỉ dùng 1 bộ biến UI duy nhất.
5. **Có thể áp dụng lại vào RE-END như thế nào?**
   - Thực hiện trong giai đoạn `P2-02` của Roadmap cải thiện RE-END.
6. **Tiêu chí nào cho thấy đã nắm vững kiến thức?**
   - Có thể gắn `Health.cs` lên một đối tượng bất kỳ (Player, Boss, Thùng gỗ, Clone) mà không dư thừa bất kỳ logic nào của UI Checkpoint hay `CompareTag("Boss")`.

---

### Chủ đề 5: Quản lý Bộ nhớ Unity, Vòng đời Object Pool & Physics NonAlloc (Memory & GC Optimization)
1. **Tại sao cần học?**
   - Trong game hành động 3D, việc cấp phát mảng mới trên Heap ở mỗi frame (`LateUpdate`) hoặc tạo mới các đối tượng bọc tài nguyên Native (`AnimatorOverrideController`) sẽ gây ra hiện tượng giật khung hình (GC Spike) hoặc rò rỉ bộ nhớ sau 15–30 phút chơi.
2. **Thiếu sót nào trong RE-END cho thấy nhu cầu đó?**
   - `[COMBAT-09]`: [`WeaponTrail.LateUpdate()`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/WeaponTrail.cs#L48-L61) gọi `Physics.OverlapBox` và `Physics.BoxCastAll` cấp phát mảng mới mỗi frame.
   - `[PLAYER-02]`: [`PlayerBaseState.ChangeSwordIdle()`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Base/PlayerBaseState.cs#L76-L81) gọi `new AnimatorOverrideController(playerStateMachine.Animator.runtimeAnimatorController)` bọc chồng lên nhau mỗi lần rút kiếm hoặc cất kiếm.
   - `[SKILL-03]` & `[SKILL-04]`: Hiểu chưa trọn vẹn thứ tự thực thi của `SetActive(true)` -> `OnEnable()` trong `ObjectPooling` và việc phải reset trạng thái con (`Rigidbody.localPosition`) khi tái sử dụng đối tượng từ Pool.
3. **Cần nắm những khái niệm cụ thể nào?**
   - **Managed Heap vs Native C++ Memory trong Unity:** Tại sao `new AnimatorOverrideController` bọc lồng nhau làm rò rỉ tài nguyên và tăng chi phí tra cứu clip của Animator.
   - **Zero-Allocation Physics Queries:** Cách sử dụng `Physics.OverlapBoxNonAlloc` và `Physics.BoxCastNonAlloc` với mảng đệm tĩnh/thành viên (`Collider[]`, `RaycastHit[]`).
   - **Poolable Object Lifecycle (`IPoolable.OnSpawn()` / `OnDespawn()`):** Tại sao `SetActive(true)` kích hoạt `OnEnable()` ngay lập tức trước khi dòng code tiếp theo của caller kịp chạy, và cách truyền dữ liệu khởi tạo đúng thứ tự.
4. **Có thể luyện tập bằng bài tập nào?**
   - Mở Unity Profiler (chế độ Deep Profile), thực hiện 20 nhát chém + 10 lần rút/cất kiếm + 5 lần dùng kỹ năng trong RE-END, ghi lại cột `GC Alloc`, sau đó sửa sang `NonAlloc` + cached `AnimatorOverrideController` và đo lại để thấy `GC Alloc = 0 B`.
5. **Có thể áp dụng lại vào RE-END như thế nào?**
   - Áp dụng trực tiếp vào `WeaponTrail.cs`, `PlayerBaseState.cs`, `SwordSkill.cs` và `Bullet.cs`.
6. **Tiêu chí nào cho thấy đã nắm vững kiến thức?**
   - Biết sử dụng Unity Profiler để tự kiểm chứng mọi hàm `Update()` / `LateUpdate()` trong dự án không phát sinh cấp phát bộ nhớ rác (`0 B GC.Alloc/frame`).
