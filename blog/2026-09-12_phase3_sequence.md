# 2026-09-12 — Phase 3 Wafer Transfer Sequence

## 오늘의 학습 목표

Equipment Controller의 상태 관리와 Sequence의 작업 순서 관리를 구분하고, 실제 Device를 구현하기 전에 Wafer 이송 단계가 정해진 순서로 진행되는 구조를 만든다.

## 새로 이해한 개발 환경과 파일

- `.cs`는 C# 소스 코드 파일이다. 같은 C# Project 폴더의 `.cs` 파일은 기본적으로 함께 빌드된다.
- `.csproj`는 C# Project 하나의 대상 .NET 버전, 결과물 종류와 의존성 같은 빌드 설정을 관리한다.
- `.sln`은 여러 Project를 묶어 관리한다.
- `Program.cs`의 `Main()`은 프로그램 실행 시작 지점이다. 현재는 객체를 만들고 명령을 호출하여 결과를 출력하는 학습용 실행 코드다.
- `bin/`의 `.deps.json` 같은 파일은 .NET 빌드 과정에서 자동 생성된다. 직접 편집하지 않는다.
- C#은 .NET SDK로 빌드하고 .NET Runtime에서 실행된다. 일반적인 C++ 코드는 C++ 컴파일러와 링커로 Native DLL 또는 실행 파일을 만든다.

이 프로젝트에서는 C#이 Equipment Controller, Sequence와 HMI 같은 상위 흐름을 담당하고, 이후 C++ Device Module이 장치 제조사 API나 구체적인 장치 제어 처리를 담당하도록 역할을 나눈다. 이는 언어 자체의 규칙이 아니라 프로젝트 설계이다.

## Sequence의 역할

Equipment State의 `Running`만으로는 Wafer 이송 작업의 어느 단계인지 알 수 없다. Sequence Step을 사용하여 작업 내부의 진행 위치를 표현했다.

```text
Equipment State: Running

Sequence Step:
Preparing → PickingWafer → MovingWafer → PlacingWafer → Completed
```

`SequenceStep` 열거형은 가능한 단계 이름을 정의할 뿐 자동으로 순서를 실행하지 않는다. `WaferTransferSequence`가 현재 단계와 변경 규칙을 관리한다.

## 구현 내용

### Sequence 시작

새 Sequence는 `NotStarted`에서 시작한다. `Start()`는 `NotStarted`에서만 허용하고 `Preparing`으로 변경한다. 이미 시작한 상태에서는 `false`를 반환하고 현재 단계를 유지한다.

### 단계 진행

`MoveNext()`에서 `switch`를 사용해 현재 단계마다 다음 단계를 지정했다. 한 번 호출할 때 하나의 `case`만 실행되고 `return`에서 메서드가 종료되므로 한 단계씩 진행한다.

```text
Preparing → PickingWafer
PickingWafer → MovingWafer
MovingWafer → PlacingWafer
PlacingWafer → Completed
```

`NotStarted`와 `Completed`는 진행할 수 없으므로 `default`에서 `false`를 반환한다. 반환 전에 단계 대입이 없기 때문에 현재 단계는 그대로 유지된다.

### 다음 작업 준비

`Reset()`은 `Completed`에서만 허용하고 Sequence를 `NotStarted`로 되돌린다. Equipment Controller가 다음 작업을 시작할 때 이전 Sequence가 완료 상태라면 초기화한 뒤 다시 시작한다.

### Equipment Controller와 연결

초기 구현에서는 Program이 Equipment Controller와 Wafer Transfer Sequence를 각각 생성하고 호출했다. 최종 구현에서는 Equipment Controller가 Sequence 객체를 내부에 보관하고 두 상태의 관계를 관리한다.

- `Start()` 성공 시 Equipment State는 `Running`, Sequence Step은 `Preparing`이 된다.
- `MoveNextSequenceStep()`은 Equipment State가 `Running`일 때만 실행된다.
- Sequence가 `Completed`에 도달하면 `Complete()`를 호출하여 Equipment State를 `Idle`로 변경한다.
- 외부 코드는 `CurrentSequenceStep`으로 현재 단계만 읽을 수 있다.

`private readonly` 필드로 Sequence 객체를 내부에 저장했다. `private`은 Equipment Controller 내부에서만 접근하게 하고, `readonly`는 생성된 Sequence 객체를 다른 객체로 다시 대입하지 못하게 한다.

## 실행 결과

```text
Initial | Equipment: Uninitialized | Sequence: NotStarted
Initialize: True | Equipment: Idle | Sequence: NotStarted
Start: True | Equipment: Running | Sequence: Preparing
MoveNext: True | Equipment: Running | Sequence: PickingWafer
MoveNext: True | Equipment: Running | Sequence: MovingWafer
MoveNext: True | Equipment: Running | Sequence: PlacingWafer
MoveNext: True | Equipment: Idle | Sequence: Completed
```

`while` 반복문은 Equipment State가 `Running`인 동안 Sequence를 한 단계씩 진행한다. 마지막 `MoveNextSequenceStep()`에서 Sequence가 완료되고 Equipment State가 `Idle`로 바뀌므로 반복문도 종료된다.

## 실제 Device가 연결되면

현재는 Device가 없어서 `MoveNext()` 호출 즉시 다음 단계로 변경한다. 실제 구조에서는 C# Sequence가 Device에 Command를 전달하고, Device 또는 C++ Device Module이 제공하는 완료 Status를 확인한 뒤 다음 단계로 이동한다.

```text
C# Sequence가 동작 요청
→ Device Module이 장치 명령 처리
→ 완료 Status 전달
→ C# Sequence가 다음 단계 결정
```

완료 전에는 현재 단계를 유지한다. 장치 오류나 제한 시간 초과 시 Equipment State를 `Error`로 바꾸는 처리는 이후 Interlock, Alarm, Logging 단계에서 구체화한다.

## 현재 한계와 다음 단계

- 실제 Robot, Door, Sensor, Motor와 Cylinder가 없다.
- 각 단계는 실제 동작이나 완료 신호 없이 즉시 진행된다.
- 장비 오류가 Sequence 진행 중 발생했을 때의 Sequence 정리 규칙은 아직 없다.
- 시간 초과, Alarm과 자동 테스트는 아직 구현하지 않았다.

Phase 3의 기본 Sequence 구현과 실행 확인을 완료했다. 다음은 Phase 4 Device Layer에서 Device의 공통 역할과 Robot Device부터 공부한다.
