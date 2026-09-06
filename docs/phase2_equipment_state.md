# Phase 2 — Equipment State와 Equipment Controller

## 구현 범위

현재 상태에 따라 명령을 허용하거나 거부하는 기본 State Machine을 Equipment Controller 내부에 구현한다. 별도의 State Machine 클래스는 만들지 않는다.

실제 장비 동작, Sequence, 비동기 처리, 오류 감지와 정지, 오류 원인 해결 여부 검사는 아직 구현하지 않았다. 초기화는 즉시 성공한다고 가정한다.

## 구성 요소

| 파일 | 책임 |
|---|---|
| [EquipmentState.cs](../src/WaferTransferEquipmentSimulator/EquipmentState.cs) | 장비 상태 목록 정의 |
| [EquipmentController.cs](../src/WaferTransferEquipmentSimulator/EquipmentController.cs) | 현재 상태 저장, 명령 허용 조건과 상태 변경 |
| [Program.cs](../src/WaferTransferEquipmentSimulator/Program.cs) | 명령 호출과 반환값·상태 출력 |

상태 이름은 [TERMINOLOGY](../TERMINOLOGY.md)의 장비 상태 용어를 따른다. 객체 생성 시 상태는 `Uninitialized`이며 외부에서는 직접 변경할 수 없다.

## 상태 변경 규칙

| 명령 | 허용 상태 | 변경 후 상태 | 반환값 |
|---|---|---|---|
| `Initialize()` | `Uninitialized` | `Initializing`을 거쳐 `Idle` | 성공 시 `true` |
| `Start()` | `Idle` | `Running` | 성공 시 `true` |
| `Complete()` | `Running` | `Idle` | 성공 시 `true` |
| `SetError()` | 모든 상태 | `Error` | 없음 |
| `Reset()` | `Error` | `Uninitialized` | 성공 시 `true` |

`bool` 메서드의 허용 조건을 만족하지 않으면 `false`를 반환하고 상태를 유지한다. 중복 시작 요청이 기존 `Running` 상태를 해제하지 않는다. `Reset()` 후에는 다시 초기화해야 시작할 수 있다.

`Complete()`는 완료 사실을, `SetError()`는 오류 상태를 반영한다. 실제 작업을 끝내거나 장비를 정지시키는 동작은 수행하지 않는다.

## 콘솔 실행

저장소 루트에서 실행한다.

```text
dotnet run --project src/WaferTransferEquipmentSimulator
```

전체 예상 출력:

```text
Uninitialized
True
Idle
True
Running
False
Running
True
Idle
Error
False
Error
True
Uninitialized
False
Uninitialized
True
Idle
True
Running
```

객체 생성 직후 상태를 출력하고, 각 `bool` 명령의 반환값과 상태를 두 줄씩 출력한다. `SetError()`는 반환값이 없으므로 상태만 출력한다. 예제는 작업 완료 후 `Idle`에서 오류를 설정하고, 복구 후 다시 시작한 `Running` 상태에서 프로그램을 종료한다.

이 예제는 정상 흐름, 중복 시작 거부, 오류 상태에서 시작 거부, 리셋 후 재초기화 필요성을 확인한다. 모든 상태와 명령의 조합을 검증하는 자동 테스트는 아니다.
