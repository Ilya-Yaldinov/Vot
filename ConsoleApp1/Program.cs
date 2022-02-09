class ConsoleApp1
{
    static void Main()
    {
        string name = Environment.UserName;
        string machine = Environment.MachineName;
        string path = Environment.CurrentDirectory;
        UserInterface userInterface = new UserInterface(name, machine, path);
        userInterface.Invite();
    } 
}

public class UserInterface
{

    private static string Username { get; set; }
    private static string Machine { get; set; }

    private static string Currentpath { get; set; }
    
    private const int Command = 0;
    private const int Text = 1;
    private const int ToFile = 2;
    private const int FileName = 3;

    public UserInterface(string username, string machine, string currentpath)
    {
        Username = username;
        Machine = machine;
        Currentpath = currentpath;
    }

    public void Invite()
    {
        if (Currentpath == Environment.CurrentDirectory)
        {
            Console.WriteLine($"{Username}@{Machine} ~");
            Console.Write("$");
        }
        else
        {
            Console.WriteLine($"{Username}@{Machine}@{Currentpath}");
            Console.Write("$");
        }
        string word = Console.ReadLine();
        string[] input = word.Split(' ');
        if (input[Command] == "cd")
        {
            Cd.CdCommand(word);
        }
        if (input[Command] == "pwd")
        {
            Console.WriteLine($"{Currentpath}");
        }
        if (input[Command] == "clear")
        {
            Console.Clear();
        }
        if (input[Command] == "ls")
        {
            Ls.LsCommand(word);
        }
        if (input[Command] == "mkdir")
        {
            Mkdir.MkdirCommand(word);
        }
        if (input[Command] == "touch")
        {
            Touch.TouchCommand(word);
        }
        if (input[Command] == "cat")
        {
            Cat.CatCommand(word);
        }
        if (input[Command] == "echo")
        {
            Echo.EchoCommand(word);
            Invite();
        }

        if (input[Command] == "exit")
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
        else
        {
            Invite();
        }
    }
    
    static class Echo
    {
        public static void EchoCommand(string word)
        {
            string[] input = word.Split(' ');
            string fileName = input[input.Length - 1];
            if (input[input.Length - 2] == ">>")
            {
                for (int i = 0; i < input.Length-1; i++)
                {
                    File.AppendAllText($"{Currentpath}\\{fileName}.txt", $"{input[i]}\n");
                }
            }
            if (input[input.Length - 2] == ">")
            {
                for (int i = 0; i < input.Length-1; i++)
                {
                    File.WriteAllText($"{Currentpath}\\{fileName}.txt", $"{input[i]}\n");
                }
            }
            else
            {
                for (int i = 1; i < input.Length; i++)
                {
                    Console.WriteLine(input[i]);
                }
            }
        }
    }

    static class Cat
    {
        public static void CatCommand(string word)
        {
            string[] input = word.Split(' ');
            if (File.Exists($"{Currentpath}\\{input[1]}.txt"))
            {
                string[] file = File.ReadAllLines($"{Currentpath}\\{input[1]}.txt");
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
        }
    }

    static class Touch
    {
        public static void TouchCommand(string word)
        {
            string[] input = word.Split(' ');
            for (int i = 1; i < input.Length; i++)
            {
                File.Create($"{Currentpath}\\{input[i]}.txt");
            }
        }
    }

    static class Mkdir
    {
        public static void MkdirCommand(string word)
        {
            string[] input = word.Split(' ');
            for (int i = 1; i < input.Length; i++)
            {
                Directory.CreateDirectory(input[i]);
            }
        }
    }

    static class Ls
    {
        public static void LsCommand(string word)
        {
            string[] input = word.Split(' ');
            if (Directory.Exists(Currentpath))
            {
                string[] ls = Directory.GetFileSystemEntries(Currentpath);
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
        }
    }

    static class Cd
    {
        public static void CdCommand(string word)
        {
            string[] input = word.Split(' ');
            if (Directory.Exists(input[Text]))
            {
                Currentpath = input[Text];
            }
            else
            {
                Console.WriteLine("Путь не существует. Нажмите любую клавишу.");
                Console.ReadKey();
            }
        }
    }
}
