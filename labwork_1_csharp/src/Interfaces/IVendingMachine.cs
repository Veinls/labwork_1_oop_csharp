using labwork_1_csharp.Enums;
using labwork_1_csharp.Models;
using System.Collections.Generic;

namespace labwork_1_csharp.Interfaces
{
    public interface IVendingMachine
    {
        void Run();
        void Display();
        WorkMode? ParseCommand(string input);
        string GetCommandDisplay(WorkMode command);
        
        CommandResult DisplayProducts();
        CommandResult InsertCoin();
        CommandResult SelectProduct();
        CommandResult ReturnCoin();
        CommandResult AddProduct();
        CommandResult TakeCoin();
        
        decimal GetIncome();
        decimal GetBalance();
        IReadOnlyList<Product> GetProducts();
    }
}