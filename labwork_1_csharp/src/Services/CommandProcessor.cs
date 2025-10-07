using labwork_1_csharp.Models;
using labwork_1_csharp.Enums;
using labwork_1_csharp.Interfaces;

namespace labwork_1_csharp.Services
{
    public class CommandProcessor : ICommandProcessor
    {
        private readonly ISelectionMode _modeManager;
        private readonly IVendingMachine _vendingMachine;

        public CommandProcessor(ISelectionMode modeManager, IVendingMachine vendingMachine)
        {
            _modeManager = modeManager;
            _vendingMachine = vendingMachine;
        }

        public CommandResult ExecuteCommand(WorkMode command)
        {
            if (!_modeManager.VerifyPermission(command))
            {
                return new CommandResult
                    { Success = false, Message = $"Команда {_vendingMachine.GetCommandDisplay(command)} не доступна в {_modeManager.GetModeDisplay(_modeManager.CurrentMode)}" };
            }

            return command switch
            {
                WorkMode.CustomMode => SwitchToCustomMode(),
                WorkMode.CViewProduct => ExecuteCViewProduct(),
                WorkMode.CInsertCoin => ExecuteCInsertCoin(),
                WorkMode.CSelectionProduct => ExecuteCSelectionProduct(),
                WorkMode.CReturnCoin => ExecuteCReturnCoin(),

                WorkMode.AdminMode => SwitchToAdminMode(),
                WorkMode.AAddProduct => ExecuteAAddProduct(),
                WorkMode.ATakeCoin => ExecuteATakeCoin(),
                _ => new CommandResult { Success = false, Message = "Неизвестная команда" }
            };
        }

        private CommandResult SwitchToCustomMode() => _modeManager.SetCurrentMode(WorkMode.CustomMode);

        private CommandResult SwitchToAdminMode() => _modeManager.SetCurrentMode(WorkMode.AdminMode);

        private CommandResult ExecuteCViewProduct() => _vendingMachine.DisplayProducts();

        private CommandResult ExecuteCInsertCoin() => _vendingMachine.InsertCoin();

        private CommandResult ExecuteCSelectionProduct() => _vendingMachine.SelectProduct();

        private CommandResult ExecuteCReturnCoin() => _vendingMachine.ReturnCoin();

        private CommandResult ExecuteAAddProduct() => _vendingMachine.AddProduct();

        private CommandResult ExecuteATakeCoin() => _vendingMachine.TakeCoin();
    }
}