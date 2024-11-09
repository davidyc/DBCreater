using System.Diagnostics;

public class NuGetPackager
{
    public static void BuildAndPackProject(string outputDirectory)
    {
        var baseDir = AppContext.BaseDirectory;          
        var projectPath = Directory.GetParent(baseDir).Parent.Parent.Parent.Parent.FullName + "\\DBCreater\\DBCreater.csproj";
        Console.WriteLine("Путь к корню проекта: " + projectPath);
        if (!File.Exists(projectPath))
        {
            Console.WriteLine($"Проект по пути {projectPath} не найден.");
            return;
        }


        Directory.CreateDirectory(outputDirectory);

        var packCommand = $"dotnet pack \"{projectPath}\" --configuration Release --output \"{outputDirectory}\"";

        Console.WriteLine("Запускаем упаковку проекта... \n");
        RunCommand(packCommand);

        Console.WriteLine("\nПроект успешно упакован!\n");
        Console.WriteLine("\nNuget был сохранен по пути " + outputDirectory+"\n");
        Console.WriteLine("\nНажмите любую клавишу для возвращения...");
        Console.ReadKey();
    }

 

    private static void RunCommand(string command)
    {
        using (var process = new Process())
        {
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = $"/C {command}";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            process.Start();

            while (!process.StandardOutput.EndOfStream)
            {
                Console.WriteLine(process.StandardOutput.ReadLine());
            }

            string errors = process.StandardError.ReadToEnd();
            if (!string.IsNullOrEmpty(errors))
            {
                Console.WriteLine($"Errors: {errors}");
            }

            process.WaitForExit();
        }
    }
}
