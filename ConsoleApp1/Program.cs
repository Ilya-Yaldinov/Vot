class ConsoleApp1
{
    static void Main()
    {
        bool isAlive = true;
        string name = Environment.UserName;
        string machine = Environment.MachineName;
        string path = Environment.CurrentDirectory;
        UserInterface userInterface = new UserInterface(name, machine, path);
        userInterface.Init();
        while (isAlive)
        {
            userInterface.Invite();
        }
    } 
}

public class UserInterface
{

    private static string Username { get; set; }
    private static string Machine { get; set; }

    private static string Currentpath { get; set; }
    
    private const int Command = 0;

    public UserInterface(string username, string machine, string currentpath)
    {
        Username = username;
        Machine = machine;
        Currentpath = currentpath;
    }
    List<Command> commands = new List<Command>();
    Envirement envirement = new Envirement();
    public void Init()
    {
        commands.Add(new Cat(envirement));
        commands.Add(new Echo(envirement));
        commands.Add(new Mkdir(envirement));
        commands.Add(new Pwd(envirement));
        commands.Add(new Cd(envirement));
        commands.Add(new Ls(envirement));
        commands.Add(new Touch(envirement));
    }

    public void Invite()
    {
        string path = $"{Username} {Machine} {envirement.CurrentPath}";
        string[] nameOfPath = path.Split(' ');
        if (nameOfPath[2] == "" || envirement.CurrentPath == @"C:\Users\ilyay\RiderProjects\ConsoleApp1\ConsoleApp1\bin\Debug\net6.0")
        {
            envirement.CurrentPath = @"C:\Users\ilyay\RiderProjects\ConsoleApp1\ConsoleApp1\bin\Debug\net6.0";
            Console.WriteLine($"{Username}@{Machine}: ~");
            Console.Write("$");
        }
        else 
        {
            Console.WriteLine($"{Username}@{Machine}: {envirement.CurrentPath}");
            Console.Write("$");
        }
        string word = Console.ReadLine();
        string[] input = word.Split(' ');
        foreach (var e in commands)
        {
            if (e.GetName() == input[Command])
            {
                e.Execute(word);
            }
        }
    }
}

abstract class Command
{
    public Envirement envirement;

    public Command(Envirement envirement)
    {
        this.envirement = envirement;
    }
    public abstract string Execute(string word);
    public abstract string GetName();
}

class Echo : Command
{
    public Echo(Envirement envirement) : base(envirement) { }

    public override string Execute(string word)
    {
        string[] input = word.Split(' ');
        string fileName = input[input.Length - 1];
        if (input[input.Length - 2] == ">")
        {
            File.WriteAllText($"{envirement.CurrentPath}\\{fileName}.txt",null);
            for (int i = 1; i < input.Length-2; i++)
            {
                File.AppendAllText($"{envirement.CurrentPath}\\{fileName}.txt", $"{input[i]}");
            }
        }
        else if (input[input.Length - 2] == ">>")
        {
            for (int i = 1; i < input.Length-2; i++)
            {
                File.AppendAllText($"{envirement.CurrentPath}\\{fileName}.txt", $"{input[i]}");
            }
        }
        else
        {
            for (int i = 1; i < input.Length; i++)
            {
                Console.WriteLine(input[i]);
            } 
        }
        return null;
    }

    public override string GetName()
    {
        return "echo";
    }
}

class Cat : Command
{
    public Cat(Envirement envirement) : base(envirement) { }

    public override string Execute(string word)
    {
        string[] input = word.Split(' ');
        if (File.Exists($"{envirement.CurrentPath}\\{input[1]}.txt"))
        {
            string[] file = File.ReadAllLines($"{envirement.CurrentPath}\\{input[1]}.txt");
            for (int i = 0; i < file.Length; i++)
            {
                Console.WriteLine(file[i]);
            }
        }
        else
        {
            Console.WriteLine("Файл не существует. Нажмите любую клавишу.");
            Console.ReadKey();
        }

        return null;
    }

    public override string GetName()
    {
        return "cat";
    }
}

class Touch : Command
{
    public Touch(Envirement envirement) : base(envirement) { }

    public override string Execute(string word)
    {
        string[] input = word.Split(' ');
        for (int i = 1; i < input.Length; i++)
        {
            File.Create($"{envirement.CurrentPath}\\{input[i]}.txt");
        }

        return null;
    }

    public override string GetName()
    {
        return "touch";
    }
}

class Pwd : Command
{
    public Pwd(Envirement envirement) : base(envirement) { }

    public override string Execute(string word)
    {
        Console.WriteLine($"{envirement.CurrentPath}");
        return null;
    }

    public override string GetName()
    {
        return "pwd";
    }
}

class Ls : Command
{
    public Ls(Envirement envirement) : base(envirement) { }
    
    public override string Execute(string word)
    {
        string[] input = word.Split(' ');
        if (Directory.Exists(envirement.CurrentPath))
        {
            string[] ls = Directory.GetFileSystemEntries(envirement.CurrentPath);
            for (int i = 0; i < ls.Length; i++)
            {
                Console.WriteLine(ls[i]);
            }
        }
        else
        {
            Console.WriteLine("Директория не существует. Нажмите любую клавишу.");
            Console.ReadKey();
        }
        
        return null;
    }

    public override string GetName()
    {
        return "ls";
    }
}

class Mkdir : Command 
{
    public Mkdir(Envirement envirement) : base(envirement){}
    
    public override string Execute(string word)
    {
        string[] input = word.Split(' ');
        for (int i = 1; i < input.Length; i++)
        {
            Directory.CreateDirectory($"{envirement.CurrentPath}\\{input[i]}");
        }

        return null;
    }
    
    public override string GetName()
    {
        return "mkdir";
    }
}

class Cd : Command
{
    public Cd(Envirement envirement) : base(envirement) { }
    
    public override string Execute(string word)
    {
        string[] input = word.Split(' ');
        if (Directory.Exists(input[1]))
        {
           envirement.CurrentPath = input[1];
        }
        else
        {
            Console.WriteLine("Путь не существует. Нажмите любую клавишу.");
            Console.ReadKey();
        }

        return null;
    }
    
    public override string GetName()
    {
        return "cd";
    }
}

class Envirement
{
    public string CurrentPath { get; set; }
}