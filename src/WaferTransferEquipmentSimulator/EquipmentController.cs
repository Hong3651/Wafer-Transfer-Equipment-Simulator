namespace WaferTransferEquipmentSimulator
{
    public class EquipmentController
    {
        private readonly WaferTransferSequence _sequence
            = new WaferTransferSequence();

        // 현재 상태는 외부에서 읽을 수 있지만, 변경은 이 클래스 내부에서만 가능합니다.
        public EquipmentState State { get; private set; }
            = EquipmentState.Uninitialized;

        public SequenceStep CurrentSequenceStep
        {
            get { return _sequence.CurrentStep; }
        }

        public bool Initialize()
        {
            if (State != EquipmentState.Uninitialized)
            {
                return false;
            }

            State = EquipmentState.Initializing;

            // 학습용: 실제 장비 동작 없이 초기화가 즉시 성공했다고 가정
            State = EquipmentState.Idle;
            return true;
        }

        public bool Start()
        {
            if (State != EquipmentState.Idle)
            {
                return false;
            }

            // 이전 작업이 끝난 상태라면 다음 작업을 위해 Sequence를 초기화합니다.
            if (_sequence.CurrentStep == SequenceStep.Completed)
            {
                _sequence.Reset();
            }

            if (!_sequence.Start())
            {
                return false;
            }

            State = EquipmentState.Running;
            return true;
        }

        public bool MoveNextSequenceStep()
        {
            if (State != EquipmentState.Running)
            {
                return false;
            }

            if (!_sequence.MoveNext())
            {
                return false;
            }

            if (_sequence.CurrentStep == SequenceStep.Completed)
            {
                return Complete();
            }

            return true;
        }

        // 오류 상태만 반영합니다. 실제 장비 정지 기능은 아직 없습니다.
        public void SetError()
        {
            State = EquipmentState.Error;
        }

        public bool Reset()
        {
            if (State != EquipmentState.Error)
            {
                return false;
            }

            // 학습용: 상태만 변경하며, 오류 원인 해결 여부 검사는 아직 없습니다.
            State = EquipmentState.Uninitialized;
            return true;
        }

        public bool Complete()
        {
            if (State != EquipmentState.Running ||
                _sequence.CurrentStep != SequenceStep.Completed)
            {
                return false;
            }

            State = EquipmentState.Idle;
            return true;
        }
    }
}
