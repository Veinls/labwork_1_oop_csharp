using labwork_1_csharp.Interfaces;
using labwork_1_csharp.Models;

namespace labwork_1_csharp.Models
{
    public class Coin : ICoin
    {
        public decimal Balance { get; private set; }
        public decimal  Income { get; private set; }
        
        public void InsertCoin(decimal amount) => Balance += amount;
        
        public bool CanPurchase(decimal price) => Balance  >= price;

        public CommandResult Purchase(decimal price)
        {
            if(!CanPurchase(price))
               return new CommandResult{Success = false, Message = "Недостаточно средств"};
            Balance -= price;
            Income += price;
            return new CommandResult{Success = true, Message = $"Покупка совершена. Сдача: {Balance:C}"};
        }

        public decimal ReturnCoin()
        {
            var change = Balance;
            Balance = 0;
            return change;
        }
        
        public decimal TakeCoin()
        {
            var income = Income;
            Income = 0;
            return income;
        }

        public void Reset()
        {
            Balance = 0;
            Income = 0;
        }
    }
}