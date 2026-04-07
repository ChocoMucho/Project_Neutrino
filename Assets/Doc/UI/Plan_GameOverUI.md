# Implementation Plan: Game Over UI

본 문서는 `Research_GameOverUI.md`의 조사 내용과 **사용자 메모(User Memo)**를 완벽히 반영한 상세 구현 계획서입니다.
*(참고: UI 시스템 구현 등은 다른 세션에서 진행될 예정이므로, 본 계획은 게임오버의 핵심 로직과 씬 구조 설정에 집중합니다.)*

## 1. InGameScene.cs 스크립트 작성 및 구조 설정
게임오버 UI의 로드 및 비활성화를 `InGameScene` 객체에서 미리 처리하도록 설정합니다.

*   **상속 구조 변경**: `InGameScene.cs` 스크립트는 `BaseScene`을 상속받도록 작성합니다.
*   **오브젝트 구성**: 씬 하이라키(Hierarchy) 상에 `InGameScene` 게임 오브젝트를 생성하고, 컴포넌트로 `InGameScene.cs`를 부착합니다.
*   **UI 풀링 및 관리**: 게임 시작(초기화) 시 게임오버 UI를 미리 생성(Instantiate)해둔 뒤 즉시 비활성화(`SetActive(false)`) 상태로 둡니다. 플레이어 사망 이벤트 발생 시 이를 찾아 활성화하게 됩니다.

## 2. Player.cs 연동 (사망 처리 로직)
플레이어 사망(Health <= 0) 발생 시 시간 정지 및 생성해둔 UI 활성화를 담당합니다.

```csharp
// Player.cs 내 코드 로직

private void Start()
{
    // ... 중략 ...
    // Health 컴포넌트를 참조하는 코드는 이미 존재하므로, 아래 구독 줄만 추가합니다.
    if (health != null)
    {
        health.OnDeath += HandleDeath; 
    }
}

private void HandleDeath()
{
    // 1. 게임오버 시 물리 연산을 중지합니다.
    Time.timeScale = 0f;

    // 2. InGameScene.cs에서 미리 생성해 둔 UI를 찾아 활성화합니다.
    // (InGameScene 내에 활성화 메서드를 마련하거나 객체를 캐싱하여 사용)
    var inGameScene = FindObjectOfType<InGameScene>(); // 혹은 매니저를 통한 접근
    if (inGameScene != null)
    {
        inGameScene.ShowGameOverUI(); // 미리 생성 후 비활성화해둔 UI 활성화 메서드 (가칭)
    }
}
```

## 3. UI 리소스 구성 관련 
> [!NOTE] 
> **디자인 설계 보류사항**
> UI 시스템에 해당하는 프론트엔드 작업(캔버스 세팅, 버튼 이벤트 연결 등 `UI_GameOver`의 구체적인 구현)은 다른 세션에서 진행합니다. 본 세션에서는 게임 흐름상 활성화 및 비활성화만 관리합니다.