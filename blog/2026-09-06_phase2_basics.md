# 2026-09-06 — Phase 2 기초 학습, 환경 설정과 상태 관리 구현

## 오늘의 학습 목표

Equipment State, State Machine, Equipment Controller의 관계를 이해하고 C# 학습 코드를 실행할 환경을 준비한다.

## 장비 상태와 명령 규칙

- Equipment State는 장비의 현재 상태를 나타낸다.
- State Machine은 현재 상태에서 허용할 명령과 상태 변경 규칙을 관리한다.
- Equipment Controller는 명령을 받아 규칙에 따라 처리한다.
- 모든 잘못된 상황을 나열하기보다, 명령이 허용되는 상태를 정한다. 예를 들어 시작은 `Idle`에서만 허용한다.
- 작업 중 중복 시작 요청은 거부하되 기존 작업 상태는 유지한다. 잘못된 명령 요청과 실제 장비 고장은 구분한다.

최종 구현한 규칙은 다음과 같다. 상태 목록과 다섯 명령을 구현하고 콘솔에서 실행 결과를 확인했다.

| 명령 | 허용 상태 | 처리 |
|---|---|---|
| `Initialize()` | `Uninitialized` | `Initializing`을 거쳐 `Idle`로 변경. 즉시 성공한다고 가정 |
| `Start()` | `Idle` | `Running`으로 변경 |
| `Complete()` | `Running` | 작업 완료 사실을 반영해 `Idle`로 변경 |
| `SetError()` | 제한 없음 | `Error`로 변경. 반환값 없음 |
| `Reset()` | `Error` | `Uninitialized`로 변경. 다시 초기화해야 시작 가능 |

`bool` 메서드는 허용 조건을 만족하면 상태를 변경하고 `true`를 반환한다. 조건을 만족하지 않으면 `false`를 반환하고 상태를 유지한다. 실제 장비 동작, 정지, 오류 감지와 오류 원인 해결 여부 검사는 이 예제에 포함되지 않는다.

## 헷갈렸던 부분과 정리

### 비교와 대입

```csharp
State = EquipmentState.Idle;       // 값을 변경한다.
bool isNotIdle = State != EquipmentState.Idle; // 다른지 비교한다. State를 변경하지 않는다.
```

`Start()`에서 `Idle`과 비교했다고 상태가 `Idle`로 바뀌는 것은 아니다. `return false;`를 만나면 메서드가 종료되어 뒤의 상태 변경 코드는 실행되지 않는다.

### 반환값과 상태는 별개

- `Reset()` 후 `Start()`: 시작 결과는 `false`, 상태는 `Uninitialized`.
- `Initialize()` → `Start()` → `Initialize()`: 마지막 초기화 결과는 `false`, 상태는 `Running`.
- 변수 `result`에는 해당 호출의 반환값이 들어간다. 이전 호출의 반환값이나 현재 상태가 자동으로 들어가지 않는다.

### C# 기초 문법

- `class`: 관련 데이터와 메서드를 묶는 타입 정의.
- `new EquipmentController()`: 객체 생성.
- `enum`: 이름을 붙인 상수들의 집합. 장비 상태 목록을 표현하는 데 사용.
- `public EquipmentState State { get; private set; }`: 외부에서 읽을 수 있고, 같은 클래스 내부에서만 변경할 수 있는 프로퍼티.
- `bool`: `true` 또는 `false`를 표현. `void`: 반환값 없음.
- `namespace`: 파일 공유가 아니라 관련 타입을 이름 아래에 묶어 구분하는 공간. 폴더 위치와 별개이며 C++에도 있다.
- `Console.WriteLine()`: 콘솔에 값을 출력하는 메서드.

C의 조건문, 반복문, 대입과 비교 지식을 연결해서 배우되 C#의 클래스와 프로퍼티는 별도로 익힌다. C++는 Device Module 단계에서 다룬다.

### Program과 Equipment Controller의 역할

- `Program.Main()`은 실행 시작 지점이다. 객체를 만들고 명령을 요청한 뒤 결과를 출력한다.
- Equipment Controller는 현재 상태를 저장하고 명령의 허용 조건을 검사한다.
- `= EquipmentState.Uninitialized;`는 객체 생성 시 초기값을 정한다. 계속 고정되는 값은 아니다.
- `private set`은 같은 파일이 아니라 같은 클래스 내부에서만 변경할 수 있다는 뜻이다.
- `Initializing`은 `Initialize()` 내부에서 거친다. 메서드 종료 후 출력하므로 콘솔에서는 최종 상태인 `Idle`이 보인다.

## .NET과 빌드 과정

- C#은 언어이고 .NET은 프로그램 개발과 실행을 지원하는 플랫폼이다.
- .NET SDK에는 컴파일러, 빌드 도구와 Runtime 등이 포함된다.
- .NET Runtime은 프로그램 실행과 메모리 관리 등을 지원한다.
- 현재 C# Project는 빌드한 결과를 .NET Runtime으로 실행한다. `dotnet run`은 필요한 빌드와 실행을 이어서 수행한다.
- 일반적인 C++ 프로그램은 컴파일과 링크를 거쳐 실행 파일을 만들며 .NET Runtime을 사용하지 않는다. 별도 C++ 런타임 라이브러리가 필요할 수 있다.
- Python과 비교할 때 Runtime은 인터프리터, 기본 라이브러리는 표준 라이브러리와 역할이 비슷하지만 완전히 일대일 대응하지는 않는다. .NET은 `venv` 같은 가상환경이 아니다.
- `bin/`은 최종 빌드 결과, `obj/`는 중간 파일을 담는다. 빌드 도구가 자동 생성하며 직접 편집하지 않는다.
- `.csproj`는 Project의 빌드 설정이고 `.sln`은 Project들을 묶는 Solution 파일이다.

## 실제 작업과 확인 결과

- Solution과 기본 Console Project를 생성했다.
- 사용자 폴더에 .NET SDK `10.0.400`을 설치했다.
- VS Code에 C# Dev Kit와 C# 확장을 설치했다.
- 사용자 PATH와 `DOTNET_ROOT`를 설정했다. 기존 시스템 .NET 5가 먼저 선택될 수 있어 실행 검증에는 새 SDK의 전체 경로를 사용했다.
- `.csproj`의 `TargetFramework`를 `net5.0`에서 `net10.0`으로 변경했다.
- 아래 명령으로 빌드·실행하여 `Hello World!` 출력을 확인했다.

```powershell
& "$env:LOCALAPPDATA\Microsoft\dotnet\dotnet.exe" run --project src/WaferTransferEquipmentSimulator
```

- C++ 확장과 Visual Studio 2019의 MSVC 컴파일러 설치를 확인했다. C++ 빌드·실행은 아직 검증하지 않았다.
- `EquipmentState.cs`에 장비 상태 다섯 개를 정의했다. 기존 `Program.cs`와 같은 namespace를 사용한다.
- `EquipmentController.cs`를 생성하고 `State` 프로퍼티를 추가했다. 초기값은 `Uninitialized`이며 외부에서는 읽기만 가능하다.
- `Initialize()`, `Start()`, `Complete()`, `SetError()`, `Reset()`을 차례로 구현했다.
- `Program.cs`에서 명령을 순서대로 호출하고 반환값과 현재 상태를 출력했다.
- 초기화와 중복 시작은 도구로도 실행 확인했다. 이후 완료, 오류 상태에서 시작 거부, 리셋 후 시작 거부와 재초기화 후 시작 성공은 사용자가 직접 실행하여 예상 출력과 일치함을 확인했다.
- Phase 2의 기본 구현과 실행 확인을 완료했다. 모든 상태·명령 조합에 대한 자동 테스트는 작성하지 않았다.

## 실행 중 문제와 해결

1. cmd에 PowerShell용 명령을 입력하여 `&은(는) 예상되지 않았습니다` 오류가 발생했다. cmd에서는 `%LOCALAPPDATA%`를 사용하며 앞의 `&`를 붙이지 않는다.
2. `dotnet run`이 기존 SDK `5.0.200`을 선택해 `NETSDK1045`가 발생했다. .NET 10 대상 Project를 .NET 5 SDK로 빌드할 수 없었기 때문이다.
3. 로컬 `.vscode/settings.json`의 `terminal.integrated.env.windows`에 PATH와 `DOTNET_ROOT`를 설정했다. 새 터미널에서 사용자 폴더의 .NET 10을 먼저 찾도록 했다.
4. 사용자가 기존 터미널을 종료하고 새 터미널에서 다음 명령의 정상 실행을 확인했다.

```cmd
dotnet run --project src/WaferTransferEquipmentSimulator
```

`.vscode/`는 `.gitignore`에 등록되어 다른 PC로 복제되지 않는다. 다른 환경에서는 SDK 경로를 다시 확인해야 한다.

## 최종 실행 결과

| 호출 | 반환값 | 호출 후 상태 |
|---|---|---|
| 객체 생성 | 해당 없음 | `Uninitialized` |
| `Initialize()` | `True` | `Idle` |
| `Start()` | `True` | `Running` |
| `Start()` 재요청 | `False` | `Running` |
| `Complete()` | `True` | `Idle` |
| `SetError()` | 없음 | `Error` |
| `Start()` | `False` | `Error` |
| `Reset()` | `True` | `Uninitialized` |
| `Start()` | `False` | `Uninitialized` |
| `Initialize()` | `True` | `Idle` |
| `Start()` | `True` | `Running` |

현재 예제에서는 완료 후 `Idle`에서 오류를 설정한다. 작업 중 실제 고장을 재현한 것은 아니다. 전체 예상 출력과 설계 규칙은 [Phase 2 설계](../docs/phase2_equipment_state.md)에 정리했다.

## 다음 학습

1. 상태 변경과 반환값의 차이를 복습한다.
2. Phase 3의 Sequence 역할을 공부한다.
3. Equipment Controller의 상태 관리와 Sequence의 작업 순서 관리를 구분하고 필요한 최소 동작부터 설계한다.

코드를 한꺼번에 적용하지 않고 설명 → 작성 → 실행 확인 순서로 진행한다. 연습 문제에서는 호출 코드와 해당 메서드 정의를 함께 보여 준다.

학습 로그는 하루 학습을 마칠 때 한 번에 최종 정리한다. README에는 날짜별 작업 기록을 넣지 않고, TERMINOLOGY는 장비·설계 용어 중심으로 유지한다. Git push는 사용자 검토 후 별도 요청을 받아 진행한다.
