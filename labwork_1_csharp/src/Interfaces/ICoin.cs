using labwork_1_csharp.Models;

namespace labwork_1_csharp.Interfaces
{
    public interface ICoin
    {
        decimal Balance { get; }
        decimal Income { get; }
        void InsertCoin(decimal amount);
        bool CanPurchase(decimal price);
        CommandResult Purchase(decimal price);
        decimal ReturnCoin();
        decimal TakeCoin();
        void Reset();
    }
}