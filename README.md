# Wafer Transfer Equipment Simulator

C#과 C++를 활용하여 실제 제조·반도체 장비 SW 구조를 학습하고 구현하는 프로젝트입니다.

단순한 문법 학습이 아니라 Wafer Transfer Equipment Simulator를 점진적으로 완성하여 제조·반도체 장비 SW 취업용 GitHub Portfolio로 활용하는 것을 목표로 합니다.

## 학습 목표

- 실제 장비 SW의 계층 구조 이해
- Equipment State와 State Machine 구현
- Sequence와 Device의 역할 분리
- Interlock, Alarm, Logging 구현
- WPF HMI 및 Virtual Device 구현
- TCP/IP Communication 구현
- C#과 C++ Device Module 연동

## 기본 구조

```text
HMI
Equipment Control
Sequence
Device
IO / Communication
Hardware
```

Command는 위에서 아래로 전달하고 Status는 아래에서 위로 전달합니다.

## Roadmap

- [x] Phase 1: Repository 기본 구조
- [x] Phase 2: Equipment State, State Machine, Equipment Controller
- [x] Phase 3: Sequence
- [ ] Phase 4: Device Layer - Robot, Door, Sensor, Motor, Cylinder
- [ ] Phase 5: Console Equipment Simulator
- [ ] Phase 6: Interlock, Alarm, Logging
- [ ] Phase 7: WPF HMI
- [ ] Phase 8: Virtual Device, Virtual Robot, Virtual Sensor, Virtual Motor
- [ ] Phase 9: TCP/IP Communication
- [ ] Phase 10: C++ Device Module
- [ ] Phase 11: C#과 C++ 연동
- [ ] Phase 12: Wafer Transfer Equipment Simulator v1

## 프로젝트 구조

```text
Wafer-Transfer-Equipment-Simulator/
├── README.md
├── TERMINOLOGY.md
├── LICENSE
├── .gitignore
├── WaferTransferEquipmentSimulator.sln
├── docs/
├── blog/
├── images/
└── src/
    └── WaferTransferEquipmentSimulator/
        ├── WaferTransferEquipmentSimulator.csproj
        ├── EquipmentState.cs
        ├── EquipmentController.cs
        ├── SequenceStep.cs
        ├── WaferTransferSequence.cs
        └── Program.cs
```

- `docs`: 공부한 장비 SW 설계 내용을 정리합니다.
- `blog`: 그날 연구·공부한 내용과 작업 결과를 `YYYY-MM-DD_주제.md` 형식의 로그로 남깁니다.
- `images`: 문서와 Portfolio에 사용하는 이미지를 관리합니다.
- `src`: 실제 코드를 관리합니다.

## 실행 방법

.NET 10 SDK가 필요합니다. 저장소 루트 폴더의 cmd 또는 PowerShell 터미널에서 실행합니다. `dotnet --version`으로 `10.0.x` SDK가 선택되는지 확인합니다.

```powershell
dotnet --version
dotnet run --project src/WaferTransferEquipmentSimulator
```

기존 SDK가 선택되는 경우, 사용자 폴더에 설치한 .NET 10 SDK의 경로를 직접 지정할 수 있습니다.

cmd:

```cmd
"%LOCALAPPDATA%\Microsoft\dotnet\dotnet.exe" run --project src/WaferTransferEquipmentSimulator
```

PowerShell:

```powershell
& "$env:LOCALAPPDATA\Microsoft\dotnet\dotnet.exe" run --project src/WaferTransferEquipmentSimulator
```

로컬 `.vscode/settings.json`에서 터미널의 SDK 경로를 설정했다면 기존 터미널을 종료하고 새 터미널을 엽니다. `.vscode/`는 Git 관리 대상에서 제외되므로 다른 PC에는 해당 설정이 전달되지 않습니다.

현재 프로그램은 실제 장비 동작 없이 Equipment State와 Wafer Transfer Sequence의 단계 변경을 확인하는 학습용 구현입니다. Sequence가 완료되면 Equipment Controller가 장비 상태를 `Running`에서 `Idle`로 변경합니다. 설계 내용은 [Phase 2 설계](docs/phase2_equipment_state.md)와 [Phase 3 설계](docs/phase3_sequence.md)를 참고합니다.

## 파일 역할

| 파일 / 폴더 | 역할 |
|---|---|
| `.sln` | 여러 Project를 묶어 관리하는 Solution 파일 |
| `.csproj` | 대상 .NET 버전과 실행 프로그램 여부 등 Project의 빌드 설정 |
| `Program.cs` | 프로그램 시작 지점인 `Main()`이 있는 소스 파일 |
| `EquipmentState.cs` | 장비 상태 다섯 개의 정의 |
| `EquipmentController.cs` | 현재 상태와 명령 허용 조건, 상태 변경 처리 |
| `SequenceStep.cs` | Wafer Transfer Sequence의 작업 단계 정의 |
| `WaferTransferSequence.cs` | Sequence 시작, 단계 진행과 초기화 처리 |
| `bin/` | 빌드의 최종 결과물이 생성되는 폴더 |
| `obj/` | 빌드에 사용하는 중간 파일 등이 생성되는 폴더 |

`bin/`과 `obj/`는 Project 폴더 안에 자동 생성됩니다. 직접 수정하지 않으며, 삭제해도 다음 빌드 때 다시 생성됩니다. `.gitignore`에 등록되어 Git 관리 대상에서 제외됩니다.

## 문서 관리

- README는 프로젝트 소개, 구조, 실행 방법과 Roadmap을 관리합니다.
- [TERMINOLOGY.md](TERMINOLOGY.md)는 장비와 프로젝트 설계 용어의 기준을 관리합니다.
- 날짜별 연구·학습 내용, 작업 결과와 다음 할 일은 [blog](blog/)에 기록합니다. 언어 문법과 개발 환경에 관한 학습도 해당 날짜의 로그에 남깁니다.
- 학습 로그는 매 작업마다 수정하지 않고 해당 학습일을 마무리할 때 최종 정리합니다.

## 기술 스택

- C#
- C++
- .NET
- WPF
- TCP/IP
- Git / GitHub

## 향후 계획

YAGNI 원칙에 따라 현재 필요한 기능만 구현하면서 확장 가능한 구조를 유지합니다. 단계별 목표는 Roadmap에서, 구체적인 다음 할 일은 날짜별 학습 로그에서 관리합니다.
