**P1 — Có thể làm kẹt vòng chơi hoặc sai combat/tiến trình**

| Ưu tiên | Vấn đề | Khi nào xảy ra, hậu quả | Hướng sửa |
|---|---|---|---|
| 1 | Hồi sinh phụ thuộc cửa sổ animation quá hẹp | Chỉ hồi sinh khi `normalizedTime > .99 && <= 1`. Một frame nhảy qua khoảng này có thể khiến player nằm chết mãi. Fade cũng được khởi động nhiều lần. | Kiểm tra vượt ngưỡng, chạy đúng một lần; có đường phục hồi khi animation không hoàn thành như dự kiến. |
| 2 | State đánh được dùng lại nhưng không reset | `AttackState1` được cache; `_previousTime` và `_alreadyApplyForce` giữ giá trị cũ. Lần đánh sau có thể thoát ngay hoặc mất lực lao tới. | Reset toàn bộ dữ liệu của một lần đánh trong `Enter`. |
| **3** | Event đăng ký sai vòng đời | Player dùng `+=` trong `OnDisable`; boss đăng ký ở `Start` nhưng gỡ ở `OnDisable`. Bật/tắt có thể tạo listener trùng hoặc làm boss mất listener. | Ghép cặp đăng ký/gỡ theo cùng vòng đời, sau khi dependency đã sẵn sàng. |
| **4** | Ngắt đòn chưa dọn skill callback và hitbox | Boss bị đánh ngắt trước lúc tung skill: callback cũ có thể chạy theo animation tiếp theo. Collider phụ như kick chưa được mọi đường ngắt đóng lại. | Mỗi đòn sở hữu callback/hitbox; hủy đòn phải dọn hết phần thuộc đòn đó. |
| **5** | Damage có thể hồi máu; death chưa được bảo vệ | Công thức `HP - damage + resistance` làm tăng HP nếu kháng lớn hơn damage. Hit/death có thể phát lặp, skill effect có thể chuyển state khi đã chết. | Clamp damage trước khi trừ HP; death chạy một lần và chặn transition combat tiếp theo. |
| **6** | Chết trước lần save đầu có thể nạp dữ liệu bằng 0 | New Game tạo `SaveData` trống; respawn lại áp nó vào max HP/MP/SP, damage và vị trí. | Tạo snapshot khởi đầu hợp lệ; không áp dữ liệu chưa validate. |
| **7** | Boss đã hạ có hai bản dữ liệu lệch nhau | Ghi vào list trong save nhưng kiểm tra bằng HashSet khác. Hạ boss rồi respawn có thể gặp lại; Continue rồi New Game có thể giữ trạng thái cũ. | Một nguồn tiến trình và một API đánh dấu hoàn thành; reset/rebuild index khi New/Load. |
| **8** | Save làm mất hoặc biến đổi tiến trình | Không lưu linh lực và lượng hồi potion. Chỉ lưu damage đòn đầu rồi áp cho mọi đòn. Prefab đang có **10/20/30/40**, nên load có thể san bằng cả combo. | Lưu currency, upgrade level và ID; tính damage từng đòn từ definition và modifier. |

Ba mẫu sửa quan trọng nhất:

```
EnterAttack:
    previousTime = 0
    alreadyAppliedForce = false
    executionId = nextId()

CancelAttack:
    cancelOwnedSkillCallbacks()
    closeAllOwnedHitWindows()
    clearPendingRelease()
```
    ResolveDamage(hit):
    if dead or immuneTo(hit): return

    damage = max(0, mitigatedDamage - resistance)
    hp = clamp(hp - damage, 0, maxHp)

    if hp == 0:
        dead = true
        cancelCurrentAction()
        emitDeathOnce()
    else if damage > 0:
        requestHitReaction()
```
CompleteEncounter(bossId, reward):
    if alreadyCompleted(bossId): return

    markCompleted(bossId)
    currency += reward
    saveValidatedSnapshot()
```

**P2 — Cần củng cố ngay sau nhóm trên**

***Các mục này được phân tích và có mã giả riêng trong báo cáo:***

- Skill thiếu target validation: PhantomRetreat có thể lỗi khi không lock-on; cooldown/mana đã bị tác động trước khi cast thất bại.
- Effect chưa gắn với hit thực tế: PullBack phát hiệu ứng ngay lúc bắt đầu cast; event toàn cục thiếu receiver cụ thể.
- Tick tiếp tục sau SwitchState: input hoặc cờ đồng thời có thể ghi đè state vừa vào.
- Buff, miễn nhiễm và timeScale tranh quyền điều khiển: timer cũ có thể tắt hiệu ứng mới; hit-stop có thể kết thúc slow-motion.
- Counter boss random mỗi lần Evaluate: hành vi phụ thuộc số lần Tick; vượt ngưỡng hit có thể không counter được nữa.
- Pooling chưa có bước configure trước activate: projectile có thể chạy với target/caster cũ; sword tự tìm boss bằng tag; thiếu TTL được thực thi.
- Lock-on cleanup chưa đầy đủ: target cuối rời vùng thì danh sách rỗng, nhưng CancelTarget lại không xóa target hiện tại.
- Phase phụ thuộc polling Timeline chung: dễ thiếu đường hoàn tất khi dừng/skip; combo cursor cần reset khi đổi phase.
- UI/progression: ba nút mua sub-potion mất target callback; refund mana kiểm tra currentMana thay vì biên maxMana.
- Settings/save: nhánh load đồ họa bị đảo; file save thiếu validation, backup và phục hồi khi ghi lỗi.
Điểm chung của các lỗi: quyền sở hữu trạng thái đang phân tán giữa FSM, coroutine, event và UnityEvent. Đây là phần cần làm vững.