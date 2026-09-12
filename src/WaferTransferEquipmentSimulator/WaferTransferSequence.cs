namespace WaferTransferEquipmentSimulator
{
    public class WaferTransferSequence
    {
        // 현재 작업 단계는 외부에서 읽을 수 있지만, 변경은 이 클래스 내부에서만 가능합니다.
        public SequenceStep CurrentStep { get; private set; }
            = SequenceStep.NotStarted;

        public bool Start()
        {
            if (CurrentStep != SequenceStep.NotStarted)
            {
                return false;
            }

            CurrentStep = SequenceStep.Preparing;
            return true;
        }

        public bool MoveNext()
        {
            switch (CurrentStep)
            {
                case SequenceStep.Preparing:
                    CurrentStep = SequenceStep.PickingWafer;
                    return true;

                case SequenceStep.PickingWafer:
                    CurrentStep = SequenceStep.MovingWafer;
                    return true;

                case SequenceStep.MovingWafer:
                    CurrentStep = SequenceStep.PlacingWafer;
                    return true;

                case SequenceStep.PlacingWafer:
                    CurrentStep = SequenceStep.Completed;
                    return true;

                default:
                    return false;
            }
        }

        public bool Reset()
        {
            if (CurrentStep != SequenceStep.Completed)
            {
                return false;
            }

            CurrentStep = SequenceStep.NotStarted;
            return true;
        }
    }
}
