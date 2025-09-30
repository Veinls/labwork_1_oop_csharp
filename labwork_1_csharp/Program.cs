using labwork_1_csharp.Interfaces;
using labwork_1_csharp.Services;

namespace labwork_1_csharp
{ 
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            IVendingMachine vendingMachine = new VendingMachine();
            vendingMachine.Run();
        }
    }
}


