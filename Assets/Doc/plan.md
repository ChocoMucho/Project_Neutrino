# 적과 플레이어 충돌(데미지 판정) 상세 구현 계획

본 계획서는 `research.md`에서 분석된 내용을 바탕으로 **"모든 적들이 플레이어와 충돌 시 데미지를 주는 기능"**을 실제 프로젝트에 구현하는 구체적인 절차를 다룹니다.

## 1. 개요
현재 `Enemy.cs` 스크립트에는 고유 데미지(`damage`)가 할당되어 있지만 물리적 충돌 판정(`OnTriggerEnter2D`)이 없어 플레이어에게 타격을 주지 못하고 있습니다. 
따라서 `Enemy.cs`에 유니티 내장 물리 충돌 이벤트를 추가하여, 충돌체 태그가 `Player`일 때 데미지를 입히도록 만듭니다.

---

## 2. 세부 구현 단계

### 단계 1: Unity 에디터 컴포넌트 세팅 (매우 중요)
스크립트만 수정해서는 충돌 이벤트가 발생하지 않습니다. 적 몬스터와 플레이어 프리팹(Prefab) 모두 기초적인 물리 컴포넌트를 가지고 있어야 합니다.

1. **Player 프리팹 설정**
   - `Player` 게임 오브젝트 선택
   - Inspector에서 `Tag`가 **Player**로 설정되어 있는지 확인합니다. (`Enemy.cs` 내부적으로 `GameObject.FindWithTag("Player")`를 사용 중이므로 이미 설정되어 있을 확률이 높습니다.)
   - `BoxCollider2D` 또는 캡슐/원형 콜라이더 등 `Collider2D` 컴포넌트 추가
   - `Rigidbody2D` 추가 (중력을 받지 않도록 `Gravity Scale` = 0 설정 및 회전하지 않도록 Z축 `Freeze Rotation` 설정)

2. **Enemy 프리팹 설정 (총알, 돌진형 등 모든 적)**
   - 각 `Enemy` 프리팹 선택
   - `Collider2D` 컴포넌트 추가 후 **`Is Trigger` 체크박스 켜기** (물리적으로 부딪혀서 밀려나는 것을 방지하고 판정만 얻기 위함)
   - `Rigidbody2D` 컴포넌트 추가 (충돌 이벤트 발생을 위해 최소 한 쪽에 Rigidbody가 필요. 주로 `Kinematic` 타입 권장)
   - `Enemy.cs` 컴포넌트가 부착되어 있는지 재확인 (기본적으로 부착되어 있음)

---

### 단계 2: `Enemy.cs` 스크립트 수정
모든 적 프리팹에 달린 `Enemy` 클래스 하단에 충돌 감지 코드를 추가합니다. `Assets/Scripts/Enemy/Enemy.cs` 파일을 엽니다.

**수정 전 (기존 구성)**:
`Enemy.cs` 최하단에 `TryStartBulletAttack()` 등의 메서드가 있고 클래스가 닫혀있습니다.

**수정/추가할 코드**:
다음과 같은 유니티 생명주기 메서드를 클래스 내부에 추가합니다.
```csharp
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. 충돌한 대상이 플레이어 태그를 가지고 있는지 확인합니다.
        if (collision.CompareTag("Player"))
        {
            // 2. 플레이어 오브젝트로부터 IDamageable 인터페이스를 가져옵니다. (Health.cs가 이를 상속받고 있음)
            IDamageable damageable = collision.GetComponent<IDamageable>();
            
            // 3. 인터페이스를 성공적으로 가져왔다면 데미지를 적용합니다.
            if (damageable != null)
            {
                // Init()에서 EnemyDataSO를 통해 할당받은 자신의 데미지(damage)를 전달합니다.
                damageable.OnDamage(this.damage);
                
                // (선택 사항) 적이 플레이어와 부딪힌 다음 즉시 사라지게 하고 싶다면 아래 주석을 해제합니다.
                // RequestDespawn();
            }
        }
    }
```

---

### 단계 3: 동작 원리 및 검증 로직 연결선
- 위 수정을 적용하고 플레이 모드에 진입하면 다음 흐름이 발생합니다:
  1. `Dash`나 `Bullet` 타입 적이 플레이어의 콜라이더 안으로 들어감(Transform이나 Rigidbody 기반 이동).
  2. `Enemy.cs`의 `OnTriggerEnter2D` 발동.
  3. `if(CompareTag("Player"))` 통과.
  4. 플레이어의 `Health.cs` 컴포넌트(`IDamageable`) 감지.
  5. `damageable.OnDamage(damage)` 호출.
  6. `Health.cs`에서 `Player.cs`의 `IsInvincible` 상태를 체크하여, 무적이 아니라면 체력 감소 및 `OnHit` 이벤트 발생.
  7. `Player.cs`가 `OnHit` 발생을 감지하고 무적 코루틴(글리치 효과 포함) 실행.

## 3. 요약 (Action Item)
- [ ] 에디터상에서 Player 오브젝트 Component 검토 (Collider2D, Rigidbody2D).
- [ ] 에디터상에서 Enemy 프리팹들 Component 검토 (Collider2D Trigger 상태 여부, Rigidbody2D).
- [완료] `Assets/Scripts/Enemy/Enemy.cs` 파일을 열어 `OnTriggerEnter2D` 로직 삽입.
