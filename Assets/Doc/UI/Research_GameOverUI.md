# Research Report: Game Over UI Implementation

본 보고서는 <Project Neutrino>의 기술 스택 분석과 플레이어 사망 시 게임오버 UI를 띄우기 위한 연동 방안을 정리한 문서입니다.

## 1. 프로젝트 기술 스택 분석
- **Engine**: Unity (C#)
- **Architecture**: 싱글톤 패턴 기반의 매니저 관리 (`UIManager`, `ResourceManager`, `SceneManagerEx` 등)
- **UI System**: `BaseUI`를 상속받은 커스텀 UI 시스템. `ResourceManager`를 통해 프리팹을 동적으로 로드 및 생성.
- **HP System**: `Health.cs` 컴포넌트가 체력을 관리하며, `OnHit`, `OnDeath`, `OnHealthDecreased` 등의 Action 멤버를 통해 이벤트를 전파.

## 2. 관련 파일 분석 결과

### [Health.cs](file:///c:/UnityProjects/Project_Neutrino/Assets/Scripts/Health.cs)
- `currentHealth`가 0 이하일 때 `Die()` 메소드 호출 및 `OnDeath?.Invoke()` 수행.
- 현재 이 `OnDeath`를 구독하여 게임오버 UI를 띄우는 객체가 없음.

### [Player.cs](file:///c:/UnityProjects/Project_Neutrino/Assets/Scripts/Player/Player.cs)
- 플레이어 캐릭터의 핵심 로직 담당.
- 현재 `Health.OnHit`에만 연동되어 무적 상태(Invincibility) 처리를 수행 중.
- `Health.OnDeath`에 연동되어 게임오버 시퀀스를 시작할 핵심 장소.

### [UI_PlayerHealth.cs](file:///c:/UnityProjects/Project_Neutrino/Assets/Scripts/UI/UI_PlayerHealth.cs)
- 화면상의 하트 UI 표시 담당.
- `Health.OnHealthDecreased`를 구독하여 하트 개수를 감소시킴.

### [UIManager.cs](file:///c:/UnityProjects/Project_Neutrino/Assets/Scripts/Managers/UIManager.cs) 및 [BaseUI.cs](file:///c:/UnityProjects/Project_Neutrino/Assets/Scripts/UI/BaseUI.cs)
- UI 생성 및 관리를 담당하는 싱글톤 매니저와 베이스 클래스.
- 새로운 `UI_GameOver` 생성 시 `BaseUI`를 상속받아 구현하는 것이 구조적으로 적합함.

### [SceneManagerEx.cs](file:///c:/UnityProjects/Project_Neutrino/Assets/Scripts/Managers/SceneManagerEx.cs)
- `SceneType` 열거형(`LobbyScene`, `InGameScene`)을 정의함.
- 게임 재시작 및 메인 메뉴 이동 시 활용 예정.

## 3. 구현 로직 제안 (Proposed Logic)

### A. UI_GameOver Script 신규 생성
- 재시작(Restart) 버튼: `SceneManagerEx.Instance.LoadScene(SceneType.InGameScene)` 호출.
- 메인메뉴(Main Menu) 버튼: `SceneManagerEx.Instance.LoadScene(SceneType.LobbyScene)` 호출.

### B. Player - Health 연동
- `Player.Start()`에서 `health.OnDeath += ShowGameOverUI` 등록.
- `ShowGameOverUI()`에서 `UIManager.Instance`를 통해 해당 UI 프리팹 생성.

### C. 프리팹 구성
- `Assets/Resources/Prefabs/UI/` 하위에 `UI_GameOver` 프리팹 필요. (추후 작업 예정)

## 4. 향후 작업 순서
1. `UI_GameOver.cs` 스크립트 작성.
2. `Player.cs`에 사망 시 UI 팝업 연동 코드 추가.
3. 유니티 에디터 상에서 `UI_GameOver` 프리팹 제작 및 버튼 바인딩.
4. 최종 빌드 및 테스트 확인.
