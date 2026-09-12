using System;

namespace WaferTransferEquipmentSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            EquipmentController controller = new EquipmentController();

            Console.WriteLine(
                "Initial | Equipment: " + controller.State +
                " | Sequence: " + controller.CurrentSequenceStep);

            bool result = controller.Initialize();
            Console.WriteLine(
                "Initialize: " + result +
                " | Equipment: " + controller.State +
                " | Sequence: " + controller.CurrentSequenceStep);

            result = controller.Start();
            Console.WriteLine(
                "Start: " + result +
                " | Equipment: " + controller.State +
                " | Sequence: " + controller.CurrentSequenceStep);

            // Device가 아직 없으므로 완료 신호를 기다리지 않고 즉시 다음 단계로 진행합니다.
            while (controller.State == EquipmentState.Running)
            {
                result = controller.MoveNextSequenceStep();
                Console.WriteLine(
                    "MoveNext: " + result +
                    " | Equipment: " + controller.State +
                    " | Sequence: " + controller.CurrentSequenceStep);

                if (!result)
                {
                    break;
                }
            }
        }
    }
}
