# PatternTester × PatternDataSO 총알 발사 테스트 – 참고 정리

## 1. 사용할 클래스/에셋

| 대상 | 경로 | 역할 |
|------|------|------|
| **PatternDataSO** | `Assets/Scripts/SO/PatternDataSO.cs` | 발사 패턴 설정 (한 번에 몇 발, 각도, 연사 등) |
| **BulletDataSO** | `Assets/Scripts/SO/BulletDataSO.cs` | 총알 하나의 속성 (스프라이트, 속도, 데미지) |
| **PatternTester** | `Assets/Scripts/BulletPatternTest/PatternTester.cs` | 패턴 테스트용 MonoBehaviour (발사 로직 구현 대상) |
| **Bullet** | `Assets/Scripts/Bullet.cs` | 풀에서 스폰되는 총알 (Init 후 자동 이동) |
| **PoolManager** | `Assets/Scripts/Managers/PoolManager.cs` | 총알 풀 스폰/디스폰 |

---

## 2. PatternDataSO 필드 (패턴 설정)

```csharp
public int shotCount = 3;        // 한 번에 쏘는 총알 수
public float angleGap = 10f;     // 총알 간 각도 (도)
public float startAngle = 0f;    // 시작 각도 (도, 기본: Vector2.up 기준)
public float rotateSpeed = 0f;   // 발사 시 회전 속도 (나선형용, 도/초)
public int waveCount = 1;        // 발사 웨이브 횟수
public float fireInterval = 0.5f; // 웨이브 간 연사 간격 (초)
```

- **각도**: `startAngle`은 **도(degree)**. Unity에서 방향으로 쓰려면 `Vector2.up`을 기준으로 `Quaternion.Euler(0,0,angle)` 등으로 변환.
- **나선형**: `rotateSpeed`만큼 매 웨이브마다 `startAngle`(또는 현재 기준각)을 더해가면 됨.

---

## 3. BulletDataSO 필드 (총알 하나 설정)

```csharp
public Sprite bulletSprite;  // 총알 스프라이트 (선택)
public float speed = 10f;    // 총알 속도
public int damage = 1;       // 총알 데미지
```

- **Bullet.Init** 시 `speed`만 사용됨. `damage`는 현재 Bullet 쪽에서 미사용(트리거에서 1 고정).

---

## 4. Bullet 사용 방법 (풀 스폰 + Init)

```csharp
// 프리팹은 Bullet 컴포넌트가 붙은 GameObject
var bullet = PoolManager.Instance.Spawn(bulletPrefab.GetComponent<Bullet>());

// 시그니처
bullet.Init(Vector2 position, Vector2 direction, float speed, int layer);
```

- **position**: 발사 위치 (예: `PatternTester`의 `transform.position` 또는 muzzle).
- **direction**: 단위 방향 벡터 (예: `Vector2.up`을 각도만큼 회전).
- **speed**: `BulletDataSO.speed` 전달.
- **layer**: 테스트용이면 `LayerMask.NameToLayer("EnemyBullet")` 또는 별도 테스트 레이어.

풀 없으면 `PoolManager.Spawn`이 자동으로 풀 생성 (`CreatePool`).

---

## 5. 각도 → 방향 변환

- `startAngle`(도)를 **Vector2 방향**으로 바꾸는 예:

```csharp
// 도 → 라디안 후 Vector2 (Unity 기본: 위가 0도, 반시계 양의 각도)
float rad = (pattern.startAngle - 90f) * Mathf.Deg2Rad;  // Unity 2D 좌표계에 맞추려면
Vector2 baseDir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

// 또는 Quaternion 사용 (Vector2.up 기준 회전)
Vector2 baseDir = (Quaternion.Euler(0, 0, pattern.startAngle) * Vector2.up).normalized;
```

- **shotCount**, **angleGap**으로 여러 방향 만들기:
  - **부채꼴 (기본 권장)**: `startAngle`을 아래 수식으로 두면 총알이 부채꼴로 퍼짐.
    ```csharp
    // 부채꼴용 startAngle (예: shotCount=2, angleGap=20 → -20°~0°)
    float fanStartAngle = -(angleGap * shotCount) / 2f;
    angle[i] = fanStartAngle + i * angleGap;  // i = 0..shotCount-1
    ```
    - 검증: shotCount=2, angleGap=20 → startAngle=-20 → 각도 -20°, 0° ✓  
    - 검증: shotCount=3, angleGap=10 → startAngle=-15 → 각도 -15°, -5°, 5° ✓  
    - 검증: shotCount=4, angleGap=15 → startAngle=-30 → 각도 -30°~15° ✓
  - **직접 지정**: `startAngle`을 SO에서 수동으로 넣고 `angle = startAngle + i * angleGap` 사용.
  - **중앙 대칭 (다른 방식)**:  
    `angle = startAngle + (i - (shotCount-1)/2f) * angleGap` (i = 0..shotCount-1)

---

## 6. PatternTester에 넣을 시리얼 필드 제안

- `PatternDataSO pattern` — 패턴 설정.
- `BulletDataSO bulletData` — 속도(및 나중에 스프라이트/데미지).
- `GameObject bulletPrefab` — `Bullet` 컴포넌트가 붙은 프리팹 (PoolManager가 사용).

(기존 `BulletDataSO data` + `GameObject bullet`를 위 이름으로 정리해도 됨.)

---

## 7. Fire() 로직 흐름 제안

1. **웨이브 루프** `waveCount`번:
   - 현재 기준각 = `startAngle + (waveIndex * rotateSpeed)` (나선용).
2. **각 웨이브에서** `shotCount`번:
   - 각도 계산 (위 5번 참고).
   - 각도 → `Vector2` 방향.
   - `PoolManager.Instance.Spawn(bulletPrefab.GetComponent<Bullet>())`.
   - `bullet.Init(발사위치, direction, bulletData.speed, layer)`.
3. **웨이브 간 대기** `fireInterval` 초 (코루틴 `WaitForSeconds(fireInterval)` 권장).

---

## 8. 테스트 트리거

- **Update**에서 키 입력(예: 스페이스/마우스 클릭) 시 `Fire()` 한 번 호출하거나,
- **주기적 자동 발사**로 테스트하려면 타이머로 `fireInterval`마다 `Fire()` 호출.

---

## 9. 참고 코드 위치

- 풀 스폰 예: `PlayerShooter.Shoot()`, `EnemyShooter.Shoot()`.
- Bullet 이동/디스폰: `Bullet.Update()`, `CheckOffScreen()` → `PoolManager.Despawn(this)`.

---

## 10. 현재 Bullet 제한 사항

- `Init`에 **damage 인자 없음**. 트리거에서 `damageable.OnDamage(1)` 고정.
- PatternTester에서 데미지를 쓰려면 나중에 `Bullet.Init`에 damage 추가하거나, Bullet이 BulletDataSO를 참조하도록 확장 필요.

이 문서만 보고도 PatternDataSO + BulletDataSO + PoolManager + Bullet을 이용해 PatternTester에 발사 로직을 구현할 수 있습니다.
