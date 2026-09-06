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
- [ ] Phase 2: Equipment State, State Machine, Equipment Controller
- [ ] Phase 3: Sequence
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
├── docs/
├── blog/
├── images/
└── src/
```

- `docs`: 공부한 장비 SW 설계 내용을 정리합니다.
- `blog`: 개발 과정과 학습 기록을 날짜별로 정리합니다.
- `images`: 문서와 Portfolio에 사용하는 이미지를 관리합니다.
- `src`: 실제 코드를 관리합니다.

## 현재 진행 상황

Repository 문서와 기본 폴더를 구성했습니다. C# 코드와 Solution은 아직 생성하지 않았습니다.

## 기술 스택

- C#
- C++
- .NET
- WPF
- TCP/IP
- Git / GitHub

## 향후 계획

YAGNI 원칙에 따라 현재 필요한 기능만 구현하면서 확장 가능한 구조를 유지합니다. 다음 단계에서 Solution과 Console Project를 생성합니다.
