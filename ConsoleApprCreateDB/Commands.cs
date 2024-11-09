using DBCreater.Generators;
using DBCreater.Models;
using DBCreater.SQL;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApprCreateDB
{
    public static class Commands
    {
        public static void ShowDatabaseInfo(string connectionString, DatabaseType dbType)
        {
            Console.WriteLine($"\nИнформация о базе данных {dbType}:");
            using var dbContext = TestDBContext.Create(connectionString, dbType);

            bool databaseExists = dbContext.Database.CanConnect();
            if (databaseExists)
            {
                Console.WriteLine("База данных доступна.");
                Console.WriteLine($"Название базы данных: {dbContext.Database.GetDbConnection().Database}");
                Console.WriteLine("\nСписок таблиц и количество записей:");

                try
                {
                    var articlesCount = dbContext.Set<Article>().Count();
                    var rolesCount = dbContext.Set<Role>().Count();
                    var usersCountInTable = dbContext.Set<User>().Count();
                    var userProfilesCount = dbContext.Set<UserProfile>().Count();

                    Console.WriteLine($"Таблица: Articles, Количество записей: {articlesCount}");
                    Console.WriteLine($"Таблица: Roles, Количество записей: {rolesCount}");
                    Console.WriteLine($"Таблица: Users, Количество записей: {usersCountInTable}");
                    Console.WriteLine($"Таблица: UserProfiles, Количество записей: {userProfilesCount}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при получении информации о таблицах: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("База данных не существует или не доступна.");
            }
            Console.WriteLine("\nДополнительная информация о базе данных завершена.");
        }

        public static void InitializeAndSeedDatabase(string connectionString, DatabaseType dbType)
        {
            Console.WriteLine(); 
            Console.Write("Введите количество пользователей для создания: ");
            if (!int.TryParse(Console.ReadLine(), out int userCount) || userCount < 1)
            {
                Console.WriteLine("\nНекорректное количество пользователей.");
                return;
            }

            Console.WriteLine();
            Console.Write("Введите верхнюю границу диапазона для количества статей на пользователя: ");
            if (!int.TryParse(Console.ReadLine(), out int maxArticles) || maxArticles < 1)
            {
                Console.WriteLine("\nНекорректное количество статей.");
                return;
            }

            using var dbContext = TestDBContext.Create(connectionString, dbType);
            dbContext.InitializeDatabase(10, 3);

            TestDataGenerator.SeedUserWithArticlesDatabase(dbContext, userCount, maxArticles);

            Console.WriteLine($"\nСоздано пользователей: {userCount}");
        }

        public static void ShowDatabaseCommands(string connectionString, DatabaseType dbType)
        {
            bool backToMainMenu = false;
            while (!backToMainMenu)
            {
                Console.Clear();
                Console.WriteLine($"Работа с базой данных {dbType}:");
                Console.WriteLine("1. Инициализация и заполнение базы данных");
                Console.WriteLine("2. Показать информацию о базе данных");
                Console.WriteLine("3. Назад в главное меню");
                Console.WriteLine();
                Console.Write("Введите номер команды: ");
                var commandChoice = Console.ReadLine();

                switch (commandChoice)
                {
                    case "1":
                        InitializeAndSeedDatabase(connectionString, dbType);
                        break;

                    case "2":
                        ShowDatabaseInfo(connectionString, dbType);
                        break;

                    case "3":
                        backToMainMenu = true;
                        break;

                    default:
                        Console.WriteLine("\nНеверная команда, попробуйте еще раз.");
                        break;
                }

                if (!backToMainMenu)
                {
                    Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                }
            }
        }

        public static void ReadInfoFile()
        {
            string filePath = Path.Combine(AppContext.BaseDirectory, "Info.txt");

            if (File.Exists(filePath))
            {
                Console.WriteLine("\nИнформация: ");
                Console.WriteLine(File.ReadAllText(filePath));
            }
            else
            {
                Console.WriteLine("\nФайл Info.txt не найден.");
            }

            Console.WriteLine("\nНажмите любую клавишу для возвращения...");
            Console.ReadKey();
        }
    }
}
