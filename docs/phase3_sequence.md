# Phase 3 — Wafer Transfer Sequence

## 목적

Equipment State가 장비 전체 상태를 나타낸다면 Sequence Step은 `Running` 상태 안에서 현재 수행 중인 작업을 나타낸다. Phase 3에서는 실제 Device 없이 Wafer 이송 단계와 순서, 잘못된 진행 요청의 거부, Sequence 완료와 Equipment State의 연결을 구현한다.

## 책임 분리

```text
Program
  └─ 명령 호출과 결과 출력
      ↓
Equipment Controller
  ├─ Equipment State 관리
  └─ Sequence 시작과 진행 조정
      ↓
Wafer Transfer Sequence
  └─ 현재 Sequence Step과 단계 변경 규칙 관리
```

Device가 추가되면 Sequence는 각 단계에서 Device에 Command를 전달하고 완료 Status를 확인한 뒤 다음 단계로 진행한다.

## Sequence 단계

```text
NotStarted
  ↓ Start()
Preparing
  ↓ MoveNext()
PickingWafer
  ↓ MoveNext()
MovingWafer
  ↓ MoveNext()
PlacingWafer
  ↓ MoveNext()
Completed
```

`SequenceStep` 열거형은 가능한 단계의 목록만 정의한다. 실제 변경 순서는 `WaferTransferSequence`의 `Start()`, `MoveNext()`와 `Reset()`이 관리한다.

## 명령 규칙

| 명령 | 허용 조건 | 결과 |
|---|---|---|
| `Start()` | 현재 단계가 `NotStarted` | `Preparing`으로 변경하고 `true` 반환 |
| `MoveNext()` | `Preparing`부터 `PlacingWafer`까지 | 정해진 다음 단계로 변경하고 `true` 반환 |
| `MoveNext()` | `NotStarted` 또는 `Completed` | `false`를 반환하고 현재 단계 유지 |
| `Reset()` | 현재 단계가 `Completed` | `NotStarted`로 변경하고 `true` 반환 |

`MoveNext()`는 `switch`로 현재 단계에 해당하는 `case`를 찾아 한 번 호출할 때 한 단계만 진행한다. 각 `case`의 `return`에서 메서드가 종료되므로 여러 단계가 한꺼번에 변경되지 않는다.

## Equipment Controller와 연결

Equipment Controller는 `WaferTransferSequence` 객체를 내부에 보관한다. 외부 코드는 `CurrentSequenceStep`으로 현재 단계만 읽을 수 있다.

- `EquipmentController.Start()`는 `Idle`에서 Sequence 시작에 성공한 경우 장비를 `Running`으로 변경한다.
- 이전 Sequence가 `Completed`라면 다음 작업 시작 전에 `NotStarted`로 초기화한다.
- `MoveNextSequenceStep()`은 장비가 `Running`일 때만 Sequence를 진행한다.
- Sequence가 `Completed`에 도달하면 `Complete()`가 장비를 `Idle`로 변경한다.
- `Complete()`는 장비가 `Running`이고 Sequence가 `Completed`인 경우에만 성공한다.

따라서 Program은 Equipment Controller와 Sequence를 각각 직접 맞춰서 호출하지 않는다. Equipment Controller에 명령을 요청하고 두 상태를 읽어 결과를 확인한다.

## 현재 실행 방식

현재는 Device가 없으므로 `while` 반복문에서 완료 신호를 기다리지 않고 `MoveNextSequenceStep()`을 연속 호출한다.

```text
Initial | Equipment: Uninitialized | Sequence: NotStarted
Initialize: True | Equipment: Idle | Sequence: NotStarted
Start: True | Equipment: Running | Sequence: Preparing
MoveNext: True | Equipment: Running | Sequence: PickingWafer
MoveNext: True | Equipment: Running | Sequence: MovingWafer
MoveNext: True | Equipment: Running | Sequence: PlacingWafer
MoveNext: True | Equipment: Idle | Sequence: Completed
```

마지막 단계에서 Sequence가 `Completed`가 되면서 Equipment State도 `Idle`로 변경된다. 이 구현은 단계 변경만 재현하며 Robot 이동이나 센서 확인은 수행하지 않는다.

## Device 단계에서 바뀔 부분

현재 `MoveNext()`는 호출 즉시 다음 단계로 변경한다. Device가 추가되면 각 단계에서 Command를 한 번 전달하고, 이후 반복 실행에서 Device Status를 확인한다.

```text
완료 Status 수신 → 다음 Sequence Step으로 변경
동작 중 Status → 현재 Sequence Step 유지
장치 오류 또는 시간 초과 → Equipment State를 Error로 변경
```

오류 중 Sequence 정리, 시간 초과와 Alarm 처리는 이후 Phase에서 구현한다.
