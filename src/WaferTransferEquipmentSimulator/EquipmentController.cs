namespace WaferTransferEquipmentSimulator
{
    public class EquipmentController
    {
        // 현재 상태는 외부에서 읽을 수 있지만, 변경은 이 클래스 내부에서만 가능합니다.
        public EquipmentState State { get; private set; }
            = EquipmentState.Uninitialized;

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

            State = EquipmentState.Running;
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
            if (State != EquipmentState.Running)
            {
                return false;
            }

            State = EquipmentState.Idle;
            return true;
        }
    }
}
