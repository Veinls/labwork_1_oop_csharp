using System;
using System.Collections.Generic;
using System.Linq;
using labwork_1_csharp.Enums;
using labwork_1_csharp.Interfaces;
using labwork_1_csharp.Models;


namespace labwork_1_csharp.Services
{ 
    public class VendingMachine : IVendingMachine
    {
        private readonly ISelectionMode _modeManager;
        private readonly ICommandProcessor _commandProcessor;
        private readonly ICoin _coin;
        private readonly List<Product> _products;

        public VendingMachine()
        {
            _modeManager = new SelectionMode();
            _commandProcessor = new CommandProcessor(_modeManager, this);
            _coin = new Coin();
            _products = new List<Product>
            {
                new Product(1, "Лабубу", 13, 4),
                new Product(2, "Арбуз", 30, 8)
            };
        }
        
        public void Run()
        {
            Console.WriteLine("---------------------------");
            Console.WriteLine("Вендинговый аппарат запущен");
            Console.WriteLine("---------------------------");

            while (true)
            {
                DisplayStatus();
                Display();
                
                Console.Write("\nВведите команду: ");
                var input = Console.ReadLine();

                var command = ParseCommand(input) ;
                
                if(command == WorkMode.Exit)
                {
                    Console.WriteLine("Завершение работы");
                    break;
                }

                if (command.HasValue)
                {
                    var result = _commandProcessor.ExecuteCommand(command.Value);
                    Console.WriteLine(result.Message);
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("Неверный ввод команды");
                    Console.ReadLine();
                }
                Console.Clear();
            }
        }

        private void DisplayStatus()
        {
            if (_modeManager.CurrentMode == WorkMode.AdminMode)
            {
                Console.WriteLine($"\nТекущий режим: {_modeManager.GetModeDisplay(_modeManager.CurrentMode)}");
                Console.WriteLine($"Доход: {_coin.Income:C}");
            }
            else
            {
                Console.WriteLine($"Баланс: {_coin.Balance:C}");
            }
            
        }
        public void Display()
        {
            Console.WriteLine("\nКоманды:");
            var allCommands = Enum.GetValues(typeof(WorkMode))
                .Cast<WorkMode>()
                .Where(c => c != WorkMode.Exit)
                .OrderBy(c => (int)c);
            
            Console.WriteLine("-1 - Выход из программы");

            foreach (var command in allCommands)
            {
                if (_modeManager.VerifyPermission(command))
                {
                    Console.WriteLine($"{(int)command} - {GetCommandDisplay(command)}");
                }
            }
            
        }

        public WorkMode? ParseCommand(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return null;
            
            if (int.TryParse(input, out int numCommand) && Enum.IsDefined(typeof(WorkMode),numCommand))
            {
                return (WorkMode)numCommand;
            }

            return input.ToLower() switch
            {
                "exit" or "выход"  => WorkMode.Exit,
                _ => null
            };
        }


        public string GetCommandDisplay(WorkMode command)
        {
            return command switch
            {
                WorkMode.CustomMode => "Пользовательский режим",
                WorkMode.CViewProduct => "Просмотр товаров",
                WorkMode.CInsertCoin => "Внести деньги",
                WorkMode.CSelectionProduct => "Выбрать товар",
                WorkMode.CReturnCoin => "Вернуть деньги",
                WorkMode.AdminMode => "Режим администратора",
                WorkMode.AAddProduct => "Добавить товар",
                WorkMode.ATakeCoin => "Забрать деньги",
                WorkMode.Exit => "Выход",
                _ => command.ToString()
            };
        }

        public CommandResult DisplayProducts()
        {
            Console.WriteLine("\n----------------");
            Console.WriteLine("Каталог товаров");
            Console.WriteLine("----------------\n");

            if (!_products.Any(p => p.Quantity > 0))
            {
                Console.WriteLine("Нет в наличии");
                return new CommandResult{Success = true};
            }

            foreach (var product in _products.Where(p => p.Quantity > 0))
            {
                Console.WriteLine(product);
                Console.WriteLine();
            }
            return new CommandResult{Success = true};
        }
        
        public CommandResult InsertCoin()
        {
            Console.Write("Внесите оплату: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
            {
                _coin.InsertCoin(amount);
                return new CommandResult{Success = true, Message = $"Внесено: {amount:C} \nБаланс: {_coin.Balance:C}"};
            }
            return new CommandResult{Success = false, Message = "Неверная сумма"};
        }

        public CommandResult SelectProduct()
        {
            var availableProducts = _products.Where(p => p.Quantity > 0).ToList();
            if (!availableProducts.Any())
                return new CommandResult { Success = false, Message = "Товары отсутствуют" };
            
            Console.WriteLine("\n----------------");
            Console.WriteLine("Каталог товаров");
            Console.WriteLine("----------------\n");
            foreach (var product in availableProducts)
            {
                Console.WriteLine(product);
                Console.WriteLine();
            }
            
            Console.Write("\nВведите ID товара: ");
            if (int.TryParse(Console.ReadLine(), out int productId))
            {
                var product = _products.FirstOrDefault(p => p.Id == productId && p.Quantity > 0);
                
                if (product == null)
                {
                    return new CommandResult { Success = false, Message = "Товар не найден" };
                }

                if (!_coin.CanPurchase(product.Price))
                {
                    return new CommandResult
                    { Success = false, Message = $"Недостаточно средств! \nНужно:{product.Price:C} \nВаш баланс:{_coin.Balance:C}" };
                }
                var result = _coin.Purchase(product.Price);
                if (result.Success)
                {
                    product.Quantity--;
                    return new CommandResult{Success = true, Message = $"Выдан товар: {product.Name}. \n{result.Message}"};
                }
                return result;
            }

            return new CommandResult { Success = false, Message = "Неверный ID" };
        }

        public CommandResult ReturnCoin()
        {
            var change = _coin.ReturnCoin();
            if (change > 0)
            {
                return new CommandResult { Success = true, Message = $"Возвращено: {change:C}" };
            }
            else
            {
                return new CommandResult { Success = false, Message = "Нет средств для возврата" };
            }
            
        }

        public CommandResult AddProduct()
        {
            Console.Write("Введите название товара: ");
            var name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                return new CommandResult { Success = false, Message = "Название товара не может быть пустым" };
            }
            Console.Write("Введите цену: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal price) || price <= 0)
            {
                return new CommandResult { Success = false, Message = "Неверная цена" };
            }
            Console.Write("Введите количество: ");
            if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
            {
                return new CommandResult { Success = false, Message = "Неверное количество" };
            }
            
            var newId = _products.Any()  ? _products.Max(p => p.Id) + 1 : 1;
            _products.Add(new Product(newId, name.Trim(), price, quantity));

            return new CommandResult
                { Success = true, Message = $"Добавлен товар: {name} \nЦена: {price:C} \nКолическтво: {quantity}" };
        }

        public CommandResult TakeCoin()
        {
            var income = _coin.TakeCoin();
            if (income > 0)
            {
                return new CommandResult { Success = true, Message = $"Изъята сумма: {income:C}" };
            }
            else
            {
                return new CommandResult { Success = false, Message = "Нет денег для изъятия" };
            }
        }
        
        public decimal GetBalance() => _coin.Balance;
        public decimal GetIncome() => _coin.Income;
        public IReadOnlyList<Product> GetProducts() => _products.AsReadOnly();
    }
}

