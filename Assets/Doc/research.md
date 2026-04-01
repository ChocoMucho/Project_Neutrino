# 1. 목표
돌진형(Dash) 및 총알형(Bullet) 상관없이 모든 타입의 적이 플레이어와 직접 충돌(접촉) 시 플레이어의 체력을 감소시키기 위해, 현재 구조를 이해하고 통합적인 충돌 판정 메커니즘을 분석합니다.

# 2. 현재 구조 및 관계성

## 플레이어 (Player) 측 구조
- **플레이어에 부착된 컴포넌트**:
  - `Player.cs`: 무적 상태 로직과 피격 시의 글리치(Glitch) 효과를 관리합니다.
  - `PlayerController.cs`: InputSystem을 기반으로 플레이어의 이동을 처리합니다.
  - `Health.cs`: `IDamageable` 인터페이스를 구현합니다. `currentHealth`(현재 체력), `maxHealth`(최대 체력)를 관리하며, 상태 변화를 알리는 이벤트(`OnHit`, `OnDeath`, `OnHealthDecreased`)를 가지고 있습니다.
- **데미지 처리 흐름**: 플레이어에게 데미지를 주고자 하는 객체는 충돌 시 플레이어의 `IDamageable` 인터페이스를 가져와 `OnDamage(int amount)` 메서드를 호출해야 합니다. `Health.cs`는 이를 받아 무적 상태(`Player.cs` 참조)인지 확인한 후 체력을 감소시킵니다.

## 적 (Enemy) 측 구조
- **적에 부착된 컴포넌트**:
  - `Enemy.cs`: 스탯(속도, 데미지), 상태 머신(FSM) 초기화 및 객체 풀링(Object Pooling) 관리를 담당하는 핵심 스크립트입니다.
  - `EnemyStateMachine`, `EnemyAttackState` 등: 상태 머신(FSM)을 사용하여 적의 이동/공격 로직을 제어합니다.
  - `EnemyDataSO`: 적의 타입(`EnemyType.Dash`, `EnemyType.Bullet`), 스탯(Damage, HP, Speed 등), 발사 데이터(`Pattern`)를 정의하는 스크립터블 오브젝트입니다. 모든 적은 각자의 `Damage` 수치를 가지고 태어납니다.
  - `EnemyShooter.cs`: `EnemyType.Bullet` 타입인 경우 총알을 발사하는 역할을 담당합니다.

## 충돌(Collision) 구현 현황
- 현재 플레이어 본체(`Player.cs`)와 적 본체(`Enemy.cs`) 양쪽 모두 유니티의 물리 충돌 메서드(`OnTriggerEnter2D` 또는 `OnCollisionEnter2D`)가 구현되어 있지 않습니다.
- 현재 데미지 처리가 잘 구현된 부분은 적이 발사한 객체인 **`Bullet.cs`** 내부입니다.
  ```csharp
  // Bullet.cs의 충돌 구현 예시
  private void OnTriggerEnter2D(Collider2D collision)
  {
      IDamageable damageable = collision.GetComponent<IDamageable>();
      if (damageable != null)
      {
          damageable.OnDamage(1); // 고정 피해
          RequestDespawn();
      }
  }
  ```
- 위 코드처럼 **충돌한 객체에서 `IDamageable` 인터페이스를 찾아 데미지를 호출하는 방식**을 적 본체(`Enemy.cs`)에도 적용해야 합니다.

# 3. 모든 적의 통합 충돌 구현 방법 (향후 계획)
타입(Dash, Bullet)에 상관없이 적 몬스터 자체가 플레이어와 닿았을 때 체력을 깎으려면, 모든 적이 공통적으로 사용하는 `Enemy.cs`에 플레이어 감지 로직을 추가해야 합니다.

### 단계별 구현 계획:
1. **콜라이더(Collider) 및 리지드바디(Rigidbody) 설정 확인**:
   - 플레이어 오브젝트에 물리 감지를 위한 `Collider2D`와 `Rigidbody2D`(Kinematic 혹은 Dynamic)가 존재하는지 확인 및 설정합니다.
   - 적(Enemy) 프리팹 쪽에도 `isTrigger = true`로 설정된 `Collider2D`와 `Rigidbody2D`(충돌 이벤트를 받기 위함)가 있어야 합니다.
2. **`Enemy.cs`에 공통 `OnTriggerEnter2D` 로직 추가**:
   - `Enemy.cs`는 `EnemyDataSO`로부터 읽어온 `damage` 변수를 이미 가지고 있습니다. 이를 활용하여 충돌 코드를 작성합니다.
   ```csharp
   private void OnTriggerEnter2D(Collider2D collision)
   {
       // 충돌한 객체가 플레이어인지 태그로 확인 
       if (collision.CompareTag("Player"))
       {
           IDamageable damageable = collision.GetComponent<IDamageable>();
           if (damageable != null)
           {
               // EnemyDataSO에 정의된 해당 적 고유의 데미지(damage)만큼 피해를 줌
               damageable.OnDamage(this.damage); 
           }
       }
   }
   ```
3. **추가 고려사항 (충돌 후 적의 동작)**:
   - 적이 플레이어와 부딪힌 후 즉시 파괴(Despawn)될지 혹은 튕겨나가거나 관통할지 결정이 필요합니다.
   - 만약 자폭/접촉 직후 파괴되기를 원한다면 `damageable.OnDamage(this.damage);` 호출 직후 `RequestDespawn();` 함수를 호출하여 풀로 반환(Pool Return)시키면 됩니다.

# 4. 결론
플레이어가 데미지를 입는 기반 시스템(`Health.cs : IDamageable`)은 모든 준비가 되어 있습니다. 이전에 분석한 것과 마찬가지로, **`Enemy.cs` 내부에 물리 충돌 이벤트(`OnTriggerEnter2D`)를 작성하고 `damageable.OnDamage(this.damage)`를 호출해주기만 하면 Dash 타입, Bullet 타입에 상관없이 모든 적이 플레이어에게 직접 체력 피해를 입힐 수 있게 됩니다.**
