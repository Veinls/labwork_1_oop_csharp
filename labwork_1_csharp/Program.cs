using System;
using System.Linq;

namespace labwork_1_csharp
{
    public enum WorkMode
    {
        Exit = -1, 
        CustomMode = 0,
        CViewProduct = 1,
        CInsertCoin = 2,
        CSelectionProduct = 3,
        CReturnCoin = 4,
        
        AdminMode = 5,
        AAddProduct = 6,
        ATakeCoin = 7
    }
    
    public class CommandResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class SelectionMode
    {
        private WorkMode _currentMode = WorkMode.CustomMode;

        public WorkMode CurrentMode => _currentMode;

        public CommandResult SetCurrentMode(WorkMode new_mode)
        {
            if (new_mode == WorkMode.CustomMode || new_mode == WorkMode.AdminMode)
            {
                _currentMode = new_mode;
                return new CommandResult { Success = true, Message = $"Режим изменен на : {GetModeDisplay(new_mode)}" };
            }

            return new CommandResult { Success = false, Message = "Ошибка выбора режима!" };
        }

        public bool VerifyPermission(WorkMode mode)
        {
            return _currentMode switch
            {
                WorkMode.CustomMode => VerifyPermissionCustomMode(mode),
                WorkMode.AdminMode => VerifyPermissionAdminMode(mode),
                _ => false
            };
        }

        private bool VerifyPermissionCustomMode(WorkMode command)
        {
            return command switch
            {
                WorkMode.CViewProduct => true,
                WorkMode.CInsertCoin => true,   
                WorkMode.CSelectionProduct => true,
                WorkMode.CReturnCoin => true,
                WorkMode.AdminMode => true,
                _ => false
            };
        }

        private bool VerifyPermissionAdminMode(WorkMode command)
        {
            return command switch
            {
                WorkMode.AAddProduct=> true,
                WorkMode.ATakeCoin => true,
                WorkMode.CustomMode => true,
                _ => false
            };
        }

        public string GetModeDisplay(WorkMode mode)
        {
            return mode switch
            {
                WorkMode.CustomMode => "Пользоваательский режим",
                WorkMode.AdminMode => "Режим администратора",
                _ => mode.ToString()
            };
        }
    }

    public class CommandProcessor
    {
        private readonly SelectionMode _modeManager;
        private readonly VendingMachine _vendingMachine;

        public CommandProcessor(SelectionMode modeManager, VendingMachine vendingMachine)
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

        private CommandResult SwitchToCustomMode()
        {
            return _modeManager.SetCurrentMode(WorkMode.CustomMode);
        }

        private CommandResult SwitchToAdminMode()
        {
            return _modeManager.SetCurrentMode(WorkMode.AdminMode);
        }

        private CommandResult ExecuteCViewProduct()
        {
            Console.WriteLine("Просмотр товаров");
            return new CommandResult { Success = true };
        }

        private CommandResult ExecuteCInsertCoin()
        {
            Console.WriteLine("Внесите оплату");
            return new CommandResult { Success = true, Message = "Оплата принята" };
        }

        private CommandResult ExecuteCSelectionProduct()
        {
            Console.WriteLine("Выберите товар");
            return new CommandResult { Success = true, Message = $"Выбран {4}" };
        }

        private CommandResult ExecuteCReturnCoin()
        {
            Console.WriteLine("Возврат средства");
            return new CommandResult { Success = true, Message = "Можете забрать сдачу" };
        }

        private CommandResult ExecuteAAddProduct()
        {
            Console.WriteLine("Добавьте тавар");
            return new CommandResult { Success = true, Message = "Товар добавлен" };
        }

        private CommandResult ExecuteATakeCoin()
        {
            Console.WriteLine("Заберите средсва");
            return new CommandResult { Success = true, Message = "Средства изъяты" };
        }
        
    }

    public class VendingMachine
    {
        private readonly SelectionMode _modeManager;
        private readonly CommandProcessor _commandProcessor;

        public VendingMachine()
        {
            _modeManager = new SelectionMode();
            _commandProcessor = new CommandProcessor(_modeManager, this);
        }
        
        public void Run()
        {
            Console.WriteLine("Вендинговый аппарат запущен");
            Console.WriteLine($"Текущий режим: {_modeManager.GetModeDisplay(_modeManager.CurrentMode)}");

            while (true)
            {
                Console.WriteLine("\nКоманды:");
                Display();
                
                Console.WriteLine("Введите команду: ");
                var input = Console.ReadLine();

                if (input?.ToLower() == "exit")
                {
                    Console.WriteLine("Завершение работы");
                    break;
                }

                WorkMode? command = ParseCommand(input);
                if (command.HasValue)
                {
                    var result = _commandProcessor.ExecuteCommand(command.Value);
                    Console.WriteLine(result.Message);

                    if (_modeManager.CurrentMode == WorkMode.AdminMode)
                    {
                        Console.WriteLine($"Текущий режим:  {_modeManager.GetModeDisplay(_modeManager.CurrentMode)}");
                    }
                }
                else
                {
                    Console.WriteLine("Неверный ввод команды");
                }
            }
        }
        public void Display()
        {
            var allCommands = Enum.GetNames(typeof(WorkMode));

            foreach (var commandName in allCommands)
            {
                if (Enum.TryParse<WorkMode>(commandName, out var command))
                {
                    if (_modeManager.VerifyPermission(command))
                    {
                        string displayName = GetCommandDisplay(command);
                        Console.WriteLine($"{displayName} - {(int)command}");
                    }
                }
            }
            Console.WriteLine("exit - Выход из программы");
        }

        private WorkMode? ParseCommand(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return null;
            
            if (int.TryParse(input, out int numCommand))
            {
                if (Enum.IsDefined(typeof(WorkMode),numCommand))
                {
                    return (WorkMode)numCommand;
                }
            }

            if (Enum.TryParse<WorkMode>(input, out var enumCommand))
            {
                return enumCommand;
            }

            return input.ToLower() switch
            {
                "пользовательский режим" or "0" => WorkMode.CustomMode,
                "просмотр товаров" or "1" => WorkMode.CViewProduct,
                "внести монету" or "2" => WorkMode.CInsertCoin,
                "выбрать товар" or "3" => WorkMode.CSelectionProduct,
                "вернуть монеты" or "4" => WorkMode.CReturnCoin,
                "режим администратора" or "5" => WorkMode.AdminMode,
                "добавить товар" or "6" => WorkMode.AAddProduct,
                "забрать монеты" or "7" => WorkMode.ATakeCoin,
                _ => null
            };
        }
        
        
        
        public string GetCommandDisplay(WorkMode command)
        {
            return command switch
            {
                WorkMode.CustomMode => "Пользовательский режим",
                WorkMode.CViewProduct => "Просмотр товаров",
                WorkMode.CInsertCoin => "Внести монету",
                WorkMode.CSelectionProduct => "Выбрать товар",
                WorkMode.CReturnCoin => "Вернуть монеты",
                WorkMode.AdminMode => "Режим администратора",
                WorkMode.AAddProduct => "Добавить товар",
                WorkMode.ATakeCoin => "Забрать монеты",
                _ => command.ToString()
            };
        }
    }

    class Program
    {
        static void Main()
        {
            var vendingMachine = new VendingMachine();
            vendingMachine.Run();
        }
    }
}


