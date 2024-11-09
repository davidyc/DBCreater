using Microsoft.Extensions.Configuration;
using DBCreater.SQL;
using Microsoft.EntityFrameworkCore;
using ConsoleApprCreateDB;
using System;
using System.Diagnostics;

var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var postgresConnectionString = configuration.GetConnectionString("PostgreSql");
var sqlServerConnectionString = configuration.GetConnectionString("SqlServer");
var outputDirectory = configuration.GetSection("NuGet:OutputDirectory")?.Value;

bool exit = false;
while (!exit)
{
    Console.Clear();
    Console.WriteLine("Выберите базу данных для работы:");
    Console.WriteLine("0. Чтение информации из файла Info.txt");
    Console.WriteLine("1. PostgreSQL");
    Console.WriteLine("2. MS SQL");
    Console.WriteLine("3. Упаковка библиотеки DBCreater в NuGet");
    Console.WriteLine("4. Выход");
    Console.WriteLine();
    Console.Write("Введите номер команды: ");
    var dbChoice = Console.ReadLine();

    string selectedConnectionString = null;
    DatabaseType dbType;

    switch (dbChoice)
    {
        case "0":
            Commands.ReadInfoFile();
            continue;

        case "1":
            selectedConnectionString = postgresConnectionString;
            dbType = DatabaseType.PostgreSql;
            break;

        case "2":
            selectedConnectionString = sqlServerConnectionString;
            dbType = DatabaseType.SqlServer;
            break;

        case "3":
            Console.WriteLine($"Путь к выходной директории для NuGet: {outputDirectory}");
            NuGetPackager.BuildAndPackProject(outputDirectory ?? AppContext.BaseDirectory);
            continue;

        case "4":
            exit = true;
            continue;

        default:
            Console.WriteLine("\nНеверная команда, попробуйте еще раз.");
            Console.ReadKey();
            continue;
    }

    if (!exit && (dbChoice == "1" || dbChoice == "2"))
    {
        Commands.ShowDatabaseCommands(selectedConnectionString, dbType);
    }
}

