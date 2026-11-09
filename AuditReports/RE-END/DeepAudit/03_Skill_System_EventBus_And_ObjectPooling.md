# RE-END — Deep Technical Audit: Subsystem Group 03
## Skill System, Projectiles/Triggers, Event Bus & Object Pooling

- **Phạm vi kiểm toán (Scope):**
  - **Skill Factory & 15 Skills:** [`ICaster.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/Skill%20Factory/ICaster.cs), [`ISkill.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/Skill%20Factory/ISkill.cs), [`SkillFactory.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/Skill%20Factory/SkillFactory.cs), [`Skill.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/Skill%20Factory/Skill.cs), [`SkillActive.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/Skill%20Factory/SkillActive.cs).
  - **Projectiles & Skill Triggers:** [`SwordSkill.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/Separate/SwordSkill.cs), [`Bullet.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/Separate/Bullet.cs), [`TriggerSkillForBoss.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/Separate/TriggerSkillForBoss.cs).
  - **Event Bus & Animation Event Bridge:** [`GameEventManagers.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/EventBus/GameEventManagers.cs), [`ManageAnimationSkillEvent.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/EventBus/ManageAnimationSkillEvent.cs), [`ChangeState.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/UI/ChangeState.cs), [`TickSystem.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/Observer/TickSystem.cs).
  - **Object Pooling:** [`ObjectPooling.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/Object%20Pooling/ObjectPooling.cs), [`PooledObject.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/Object%20Pooling/PooledObject.cs), [`GetSkill.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/Object%20Pooling/GetSkill.cs), [`SkillPooledObject.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/SkillPooledObject.cs).

---

## 1. Danh sách Phát hiện Kỹ thuật (Detailed Findings)

### [SKILL-01] Kỹ năng `Invisible` gán ngược `player.Invisible = true` khi kết thúc và `PlayerUseSkillState` khôi phục nhầm Mesh của `Indestructible`
- **ID:** `SKILL-01`
- **Subsystem:** Skill System (`Skill.Invisible`, `PlayerUseSkillState`)
- **Category:** `Bug`
- **Severity:** `High`
- **Confidence:** `Confirmed`
- **Location:**
  - [`Skill.cs:111-116`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/Skill%20Factory/Skill.cs#L111-L116) — `Invisible.Cast(ICaster caster)`
  - [`PlayerUseSkillState.cs:82-88, 103-113`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Main/PlayerUseSkillState.cs#L82-L113) — `ResetAfterSkill()` & `ResetToMainMaterial()`
- **Evidence & Technical Explanation:**
  1. Trong [`Skill.cs:109-116`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/Skill%20Factory/Skill.cs#L109-L116), callback kết thúc tàng hình sau 4 giây viết:
     ```csharp
     player.WaitToRun(4f, () =>
     {
         GameEventManagers.Instance.TriggerSkillCasted(caster, SkillEffect.None);
         player.Invisible = false;
         ResetMaterial(player);
         player.Invisible = true;
     });
     ```
     Dòng 113 vừa đặt `player.Invisible = false;` thì ngay dòng 115 lại ghi đè `player.Invisible = true;`!
  2. Trong [`PlayerUseSkillState.cs:82-88`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Main/PlayerUseSkillState.cs#L82-L88), một Coroutine thứ hai chạy sau `5f` giây gán lại `playerStateMachine.Invisible = false;` và gọi `ResetToMainMaterial()`. Tuy nhiên `ResetToMainMaterial()` (`:103-113`) chỉ tắt `HeadIndestructible` / `ArmIndestructible` / `TorsoIndestructible` chứ không tắt các mesh `*Invisible`. Nếu kỹ năng `Invisible` được kích hoạt, trong khoảng từ giây thứ 4 đến giây thứ 5 trạng thái `player.Invisible` bị bật ngược lại thành `true`.
- **Trigger Conditions:**
  Người chơi sử dụng kỹ năng `Invisible`.
- **Impact / Consequence:**
  Dư thừa 2 bộ hẹn giờ chồng chéo (4s trong `Skill.cs` và 5s trong `PlayerUseSkillState.cs`) và dòng lệnh `player.Invisible = true;` ở dòng 115 làm sai lệch trạng thái tàng hình.
- **Remediation Plan:**
  - **Root Cause:** Lỗi copy-paste ở dòng 115 của `Skill.cs` và phân tán trách nhiệm quản lý thời gian hiệu lực kỹ năng giữa `Skill.cs` và `PlayerUseSkillState.cs`.
  - **Minimal Safe Fix (`Bug Fix`):** Xóa dòng 115 (`player.Invisible = true;`) trong [`Skill.cs`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/Skill%20Factory/Skill.cs#L115) và thống nhất quản lý kết thúc hiệu ứng tại một nơi duy nhất.
- **Verification Method:**
  Dùng kỹ năng `Invisible`, kiểm tra sau 4 giây `player.Invisible == false`, mesh tàng hình tắt, mesh thường bật và Boss đuổi đánh trở lại.
- **Regression Risk:** Không có.

---

### [SKILL-02] Rò rỉ và kích hoạt nhầm closure trên `ManageAnimationSkillEvent.SituationEvent` khi Boss bị ngắt đòn giữa chừng
- **ID:** `SKILL-02`
- **Subsystem:** Skill System & Animation Event Bridge (`Skill.cs`, `ManageAnimationSkillEvent`, `ChangeState`)
- **Category:** `Bug`
- **Severity:** `High`
- **Confidence:** `Confirmed`
- **Location:**
  - [`Skill.cs:233-250, 299-313, 343-358, 412-427, 441-455`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/Skill%20Factory/Skill.cs#L233-L250) — `ThrowSword`, `PullBack`, `FirstAoe`, `TransformToTwoSword`, `TransformToBlade`
  - [`ManageAnimationSkillEvent.cs:9-31`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/EventBus/ManageAnimationSkillEvent.cs#L9-L31)
- **Evidence & Technical Explanation:**
  Trong `ThrowSword.Cast()`, `PullBack.Cast()`, `FirstAoe.Cast()`, `TransformToTwoSword.Cast()`, và `TransformToBlade.Cast()`, kỹ năng đăng ký một hàm nặc danh (`Action situationAction`) vào `manageEvent.SituationEvent += situationAction;`, và chỉ tự gỡ (`manageEvent.SituationEvent -= situationAction;`) **bên trong thân của `situationAction` khi nó được gọi**.
  Nếu Boss bắt đầu vung chiêu (đã chạy `Cast()` và đăng ký `+= situationAction`), nhưng **trước khi** animation đạt tới mốc `triggerSkillSituation` để kích hoạt `SendSituationEvent()`, Boss bị đánh trúng (`Health.HitAction` chuyển sang `BossHitState`) hoặc chuyển phase, thì `situationAction` cũ **vẫn nằm trong `SituationEvent`**.
  Khi Boss thực hiện một kỹ năng khác ở lượt sau và animation mới phát `SendSituationEvent()`, **cả kỹ năng cũ lẫn kỹ năng mới sẽ nổ cùng một lúc**!
- **Trigger Conditions:**
  Boss đang khởi động một kỹ năng có dùng `SituationEvent` nhưng bị ngắt sang `BossHitState` trước khung hình phát sự kiện.
- **Impact / Consequence:**
  Kích hoạt chồng chéo nhiều kỹ năng Boss trong một đòn đánh duy nhất ở lượt tiếp theo, hoặc rò rỉ closure giữ tham chiếu đối tượng.
- **Remediation Plan:**
  - **Root Cause:** Đăng ký `+=` tích lũy trên `SituationEvent` mà không dọn dẹp khi thoát `BossAttackState` / `BossUltimateState`.
  - **Minimal Safe Fix (`Bug Fix`):** Gọi `manageEvent.ClearSituationEvent()` ở đầu mỗi hàm `Cast()` trước khi đăng ký `+= situationAction`, đồng thời gọi `ClearSituationEvent()` trong `BossAttackState.Exit()` và `BossUltimateState.Exit()`.
- **Verification Method:**
  Ngắt đòn Boss khi đang ra chiêu `FirstAoe` hoặc `ThrowSword`, sau đó quan sát đòn tiếp theo của Boss chỉ phát đúng 1 hiệu ứng duy nhất.
- **Regression Risk:** Không có.

---

### [SKILL-03] `SwordSkill.OnEnable()` chạy trước khi `TargetPosition` được gán từ `ObjectPooling` và lấy nhầm Boss Phase I đang inactive
- **ID:** `SKILL-03`
- **Subsystem:** Skill Projectile & Object Pooling (`SwordSkill`, `ObjectPooling`, `Skill.ThrowSword`)
- **Category:** `Bug`
- **Severity:** `High`
- **Confidence:** `Confirmed`
- **Location:**
  - [`SwordSkill.cs:27-57`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/Separate/SwordSkill.cs#L27-L57)
  - [`ObjectPooling.cs:78-102`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/Object%20Pooling/ObjectPooling.cs#L78-L102)
  - [`Skill.cs:233-248`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/Skill%20Factory/Skill.cs#L233-L248)
- **Evidence & Technical Explanation:**
  1. Trong [`ObjectPooling.GetPooledObject()`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/Object%20Pooling/ObjectPooling.cs#L94-L100), `obj.gameObject.SetActive(true)` được gọi **ngay bên trong `GetPooledObject`**, kích hoạt `SwordSkill.OnEnable()` **trước khi** [`ThrowSword`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/Skill%20Factory/Skill.cs#L241-L245) kịp gán `swordSkill.TargetPosition = targetPosition`!
  2. Hơn nữa, trong [`SwordSkill.OnEnable()`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/Separate/SwordSkill.cs#L29-L31):
     ```csharp
     GameObject container = GameObject.FindGameObjectWithTag("Boss");
     _bossStateMachine = container.GetComponentInChildren<BossStateMachine>(true);
     ```
     Vì truyền `includeInactive: true`, `GetComponentInChildren<BossStateMachine>(true)` luôn trả về phần tử đầu tiên trong cây Hierarchy của Boss Container (`Nordic_Phase_I`, ngay cả khi `Nordic_Phase_I` đã chết và đang `SetActive(false)` ở Phase II hoặc Phase III!).
- **Trigger Conditions:**
  Boss thực hiện kỹ năng ném kiếm (`ThrowSword`), đặc biệt ở Phase II/III hoặc lần tái sử dụng từ Pool.
- **Impact / Consequence:**
  `_bossStateMachine` trỏ nhầm vào Boss Phase I đã tắt (`inactive`), làm sai lệch `TriggerSkillCasted` và tính toán vị trí ném kiếm.
- **Remediation Plan:**
  - **Root Cause:** Tìm kiếm phụ thuộc bằng `FindGameObjectWithTag` + `GetComponentInChildren(true)` trong `OnEnable()` thay vì truyền trực tiếp `BossStateMachine` vào hàm `InitializeBullet(BossStateMachine caster, Vector3 targetPos)`.
  - **Minimal Safe Fix (`Bug Fix`):** Truyền trực tiếp `bossStateMachine` và `targetPosition` vào `swordSkill.InitializeBullet(bossStateMachine, targetPosition)` và bỏ logic tìm kiếm trong `OnEnable()`.
- **Verification Method:**
  Kiểm tra kỹ năng `ThrowSword` ở cả Phase I và Phase II của Boss Nordic.
- **Regression Risk:** Không có.

---

### [SKILL-04] `PhantomRetreat` dịch chuyển các `Rigidbody` con bằng `Bullet.MovePosition` nhưng không bao giờ reset `localPosition` khi tái sử dụng từ Pool
- **ID:** `SKILL-04`
- **Subsystem:** Skill System & Object Pooling (`Skill.PhantomRetreat`, `Bullet`, `SkillPooledObject`)
- **Category:** `Bug`
- **Severity:** `Medium`
- **Confidence:** `Confirmed`
- **Location:**
  - [`Skill.cs:164-184`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/Skill%20Factory/Skill.cs#L164-L184) — `PhantomRetreat.Cast()`
  - [`Bullet.cs:10-40`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/Separate/Bullet.cs#L10-L40)
- **Evidence & Technical Explanation:**
  Khi người chơi dùng `PhantomRetreat`, prefab `PhantomRetreat` được lấy từ `ObjectPooling` và `PhantomRetreat.Cast()` lấy toàn bộ `Rigidbody[]` con bên trong prefab rồi gọi `bullet.Shoot(rig, direction, 25f)`.
  [`Bullet.MovePosition`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/Separate/Bullet.cs#L29-L39) di chuyển từng `Rigidbody` con ra xa trong 1 giây (`rig.MovePosition(...)`). Khi ParticleSystem dừng, `SkillPooledObject` trả root prefab `PhantomRetreat` về Pool, nhưng **vị trí `localPosition` của các `Rigidbody` con đã bay xa hàng chục mét khỏi tâm prefab gốc và không bao giờ được đặt lại về `Vector3.zero`**! Ở các lần dùng `PhantomRetreat` tiếp theo, các đường đạn xuất phát từ vị trí lệch hoàn toàn so với nhân vật.
- **Trigger Conditions:**
  Sử dụng kỹ năng `PhantomRetreat` từ lần thứ 2 trở đi trong cùng một màn chơi.
- **Impact / Consequence:**
  Đạn của `PhantomRetreat` xuất hiện lệch vị trí nhân vật từ lần dùng thứ 2 trở đi.
- **Remediation Plan:**
  - **Root Cause:** Không reset `localPosition = Vector3.zero` và `linearVelocity = Vector3.zero` cho các `Rigidbody` con trước khi bắn hoặc khi trả về Pool.
  - **Minimal Safe Fix (`Bug Fix`):** Trước khi gọi `bullet.Shoot(rig, ...)` trong `PhantomRetreat.Cast()`, đặt `rig.transform.localPosition = Vector3.zero;` (hoặc lưu vị trí `localPosition` ban đầu trong `Bullet.Awake()` và khôi phục trong `OnEnable()`).
- **Verification Method:**
  Dùng `PhantomRetreat` 3 lần liên tiếp ở các vị trí khác nhau trên bản đồ và kiểm tra các tia đạn luôn phóng ra từ đúng vị trí của Player.
- **Regression Risk:** Không có.

---

### [SKILL-05] Kỹ năng `ChokeNeck` là hàm rỗng (Stub) và `PlayerStateMachine.OnDisable()` hủy đăng ký `OnSkillCasted` nhưng `OnEnable()` không đăng ký lại
- **ID:** `SKILL-05`
- **Subsystem:** Skill System & Event Bus (`Skill.ChokeNeck`, `PlayerStateMachine`, `GameEventManagers`)
- **Category:** `Bug` / `Missing Safeguard`
- **Severity:** `Medium`
- **Confidence:** `Confirmed`
- **Location:**
  - [`Skill.cs:317-329`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Attack/Skill%20Factory/Skill.cs#L317-L329) — `ChokeNeck.Cast()`
  - [`PlayerStateMachine.cs:183, 190-198`](file:///C:/Users/Official_41/Documents/Unity_Project/Unity_3D_Boss-Rush/Assets/Scripts/Design%20Pattern/StateMachine/Player/Base/PlayerStateMachine.cs#L183-L198)
- **Evidence & Technical Explanation:**
  1. `ChokeNeck.Cast(ICaster caster)` (`Skill.cs:324-328`) chỉ lấy biến `bossStateMachine` và `player` rồi kết thúc mà không gọi Timeline hay gây hiệu ứng gì.
  2. `PlayerStateMachine` đăng ký `GameEventManagers.Instance.OnSkillCasted += HandleEffectedState;` trong `Start()` (`:183`), nhưng lại hủy đăng ký `-= HandleEffectedState` trong `OnDisable()` (`:196`). Nếu GameObject `Player` bị `SetActive(false)` rồi `SetActive(true)` (ví dụ trong cutscene hoặc chuyển trạng thái), `Start()` sẽ không chạy lại lần thứ 2, khiến Player mất vĩnh viễn kết nối với `OnSkillCasted`.
- **Trigger Conditions:**
  - Boss gọi kỹ năng `ChokeNeck`.
  - GameObject `Player` bị disable rồi enable lại.
- **Impact / Consequence:**
  `ChokeNeck` không có tác dụng; bất đối xứng vòng đời `Start()` vs `OnDisable()`.
- **Remediation Plan:**
  - **Minimal Safe Fix (`Bug Fix`):** Chuyển việc đăng ký `OnSkillCasted += HandleEffectedState` sang `OnEnable()` (kèm kiểm tra tránh đăng ký trùng) để đối xứng với `OnDisable()`, hoặc chuyển hủy đăng ký sang `OnDestroy()`.

---

## 2. Đánh giá Kiến trúc, Hiệu năng & Điểm mạnh (Group 03)

- **Điểm mạnh:**
  - **Object Pooling (`ObjectPooling.cs` + `PooledObject.cs` + `SkillPooledObject.cs`):** Sử dụng `Dictionary<string, Stack<PooledObject>>` với cờ bảo vệ `isReleased` trong `PooledObject.Release()` ngăn chặn lỗi Push trùng một object 2 lần vào Stack (Double-Free). Tích hợp callback `OnParticleSystemStopped()` của Unity trong `SkillPooledObject` để tự động thu hồi VFX về Pool mà không cần cấp phát Coroutine đếm giờ cho từng hạt hiệu ứng.
  - **Tách biệt Cooldown theo Slot (`SkillActive.cs`):** Quản lý 3 nhóm kỹ năng (`ChangingTheGame`, `Escape`, `Response`) rõ ràng, cập nhật fillAmount của UI mượt mà qua Coroutine.
- **Điểm yếu kiến trúc:**
  - **Vi phạm Liskov Substitution / Dependency Inversion trong `ISkill.Cast(ICaster)`:** Dù đã thiết kế interface `ICaster`, toàn bộ 15 lớp trong `Skill.cs` đều ép kiểu ngược về `GetComponent<PlayerStateMachine>()` hoặc `GetComponent<BossStateMachine>()`.
  - **Dead Code:** `TickSystem.cs` phát sự kiện `OnUpdateSystem` mỗi frame nhưng không có bất kỳ subscriber nào trong toàn dự án.
