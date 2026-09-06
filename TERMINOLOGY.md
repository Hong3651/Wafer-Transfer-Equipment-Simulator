# Terminology

프로젝트의 설계, 코드, 문서에서 같은 개념을 같은 용어로 표현하기 위한 기준입니다.

장비와 프로젝트 설계 용어를 중심으로 관리합니다. 언어 문법과 개발 환경에 관한 학습 내용은 `blog/`의 날짜별 로그에 기록합니다.

## 사용 규칙

1. 아래에 정의된 용어를 일관되게 사용합니다.
2. 새로운 프로젝트 용어가 필요하면 먼저 의미와 필요한 이유를 설명합니다.
3. 사용자의 허락을 받은 뒤 이 문서에 추가하고 사용합니다.
4. 같은 개념을 임의의 다른 이름으로 바꾸어 사용하지 않습니다.

## 등록 용어

| 용어 | 의미 |
|---|---|
| HMI | 사용자가 장비를 조작하고 Status를 확인하는 계층 |
| Equipment Control | 장비 전체의 동작과 상태를 관리하는 계층 |
| Equipment State | 장비의 현재 상태 |
| State Machine | Equipment State와 상태 변경을 관리하는 구조 |
| Equipment Controller | Equipment Control을 담당하는 구성 요소 |
| Sequence | 정해진 순서에 따라 Device에 Command를 전달하는 계층 |
| Device | Robot, Door, Sensor, Motor, Cylinder를 캡슐화하는 계층 |
| IO / Communication | Device와 Hardware 사이의 입출력 및 통신 계층 |
| Hardware | 실제 장비 또는 이를 대신하는 대상 |
| Command | 위 계층에서 아래 계층으로 전달되는 동작 요청 |
| Status | 아래 계층에서 위 계층으로 전달되는 상태 정보 |
| Robot | Wafer를 이동하는 Device |
| Door | 장비의 출입 영역을 열고 닫는 Device |
| Sensor | 장비의 상태를 감지하는 Device |
| Motor | 회전 또는 직선 운동을 만드는 Device |
| Cylinder | 공압을 이용하여 직선 운동을 만드는 Device |
| Interlock | 안전하지 않은 동작을 막는 조건 또는 기능 |
| Alarm | 비정상 상태를 알리는 정보 또는 기능 |
| Logging | 장비의 동작과 상태를 기록하는 기능 |
| Virtual Device | Hardware 없이 Device 동작을 재현하는 구성 요소 |
| TCP/IP Communication | TCP/IP를 이용한 통신 기능 |
| Device Module | Device 기능을 구현한 SW 구성 요소 |
| Solution | 여러 Project를 관리하는 단위 |
| Project | 관련 코드를 구성하는 단위 |
| Repository | 프로젝트 파일과 변경 이력을 관리하는 공간 |
| GitHub Portfolio | 개발 결과와 과정을 보여주는 GitHub 자료 |
| YAGNI | 현재 필요하지 않은 기능을 미리 구현하지 않는 원칙 |

## 장비 상태 용어

코드와 설계 문서에서 사용하는 Equipment State의 이름과 의미입니다.

| 용어 | 의미 |
|---|---|
| `Uninitialized` | 초기화 전 상태 |
| `Initializing` | 초기화 작업을 수행 중인 상태 |
| `Idle` | 초기화 완료 후 작업 명령을 기다리는 상태 |
| `Running` | 작업을 실행 중인 상태 |
| `Error` | 오류가 발생해 정상 작업을 진행할 수 없는 상태 |
