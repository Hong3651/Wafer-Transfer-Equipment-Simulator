using System;

namespace WaferTransferEquipmentSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            EquipmentController controller = new EquipmentController();
            Console.WriteLine(controller.State);

            bool result = controller.Initialize();

            Console.WriteLine(result);
            Console.WriteLine(controller.State);

            // 첫 시작 요청: Idle에서 Running으로 변경합니다.
            result = controller.Start();
            Console.WriteLine(result);
            Console.WriteLine(controller.State);

            // 중복 시작 요청: 거부하고 Running 상태를 유지합니다.
            result = controller.Start();
            Console.WriteLine(result);
            Console.WriteLine(controller.State);

            // 작업 완료를 반영하고 Idle 상태로 돌아갑니다.
            result = controller.Complete();
            Console.WriteLine(result);
            Console.WriteLine(controller.State);

            // 오류 상태로 변경합니다. SetError()는 반환값이 없습니다.
            controller.SetError();
            Console.WriteLine(controller.State);

            // 오류 상태에서는 시작 요청을 거부합니다.
            result = controller.Start();
            Console.WriteLine(result);
            Console.WriteLine(controller.State);

            // 오류 상태를 초기화 전 상태로 되돌립니다.
            result = controller.Reset();
            Console.WriteLine(result);
            Console.WriteLine(controller.State);

            // 리셋 직후에는 초기화가 필요하므로 시작을 거부합니다.
            result = controller.Start();
            Console.WriteLine(result);
            Console.WriteLine(controller.State);

            // 다시 초기화한 뒤 시작합니다.
            result = controller.Initialize();
            Console.WriteLine(result);
            Console.WriteLine(controller.State);

            result = controller.Start();
            Console.WriteLine(result);
            Console.WriteLine(controller.State);
        }
    }
}
