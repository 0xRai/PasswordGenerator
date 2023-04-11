using System.Text.RegularExpressions;

namespace PasswordGenerator;

public static class Program {
    public static void Main(string[] args) {
        Console.Clear();
        if (args.Length == 0) {
            GeneratePassword();
        }
        else if (args[0].ToUpper() == "HELP") { 
            ShowHelp(); 
        }
        else {
            GenerateBuilder(args);
        }
        
    }

    private static void GenerateBuilder(string[] args) {
        bool isParse = Int32.TryParse(args[0], out var length);
        if (!isParse) {
            length = 8;
        }
        else {
            for (int i = 0; i < args.Length - 1; i++) {
                args[i] = args[i + 1];
            }
            Array.Resize(ref args,args.Length-1);
        }
        GeneratePassword(length,args);
    }

    private static void ShowHelp() {
        string[] helper = {
            "HELP:",
            "DEFAULT: 8 characters long, All special characters",
            "First Argument: Number of Characters",
            "Remaining Arguments: Options",
            "List of Options:",
            "\t-NONE, -N\t\tNo Special Characters",
            "\t-BASIC, -B\t\tBasic Special Characters:",
            "\t\t\t\t ['!','@','#','$','%','^','&','*']\n",
            "\t-UPPER, -U\t\tUpper Case Only [A-Z]",
            "\t-LOWER, -L\t\tLower Case Only [a-z]",
            "\t-NUM, -NO\t\tNumbers Only [0-9]",
            "\t-NONUM, -NN\t\tNo Numbers",
            "\t-X \t\t\tGenerate passwords 'X' amount of times",
            "\t\t\t\t (Ex: 8 -NN -X 10)",
            "\t\t\t\t 'Generate 10, no nums, 8-character passwords'",
        };
        Console.Clear();
        foreach (var help in helper) {
            Console.WriteLine(help);
        }
    }

    private static void GeneratePassword(int length = 8, string[]? options = null, int iterations = 1) {
        int maxCharCode = 127;
        int minCharCode = 33;
        List<string> opList = new List<string>();
        if (options != null) { 
    /*
        -NONE, -N		No Special Characters
        -BASIC, -B		Basic Special Characters: ['!','@','#','$','%','^','&','*']
        -UPPER, -U		Upper Case Only [A-Z]
        -LOWER, -L		Lower Case Only [a-z]
        -NUM, -NO		Numbers Only [0-9]
        -NONUM, -NN		No Numbers
        -X              Iterations
    */
            for (int i = 0; i < options.Length; i++) {
                switch (options[i].ToUpper()) {
                    case "-NONE":
                    case "-N":
                        opList.Add("N");
                        maxCharCode = 123;
                        minCharCode = 48;
                        break;
                    case "-BASIC":
                    case "-B":
                        opList.Add("B");
                        maxCharCode = 123;
                        break;
                    case "-UPPER":
                    case "-U":
                        opList.Add("U");
                        break;
                    case "-LOWER":
                    case "-L":
                        opList.Add("L");
                        break;
                    case "-NUM":
                    case "-NO":
                        opList.Add("NO");
                        break;
                    case "-NONUM":
                    case "-NN":
                        opList.Add("NN");
                        break;
                    case "-X":
                        if (i + 1 < options.Length) {
                            try {
                                iterations = Int32.Parse(options[i + 1]);
                                options[i + 1] = "PASS";
                            }
                            catch (FormatException) {
                                Console.WriteLine($"{options[i]} {options[i + 1]}: Bad Format. Specify a number of iterations.");
                                for (int j = 0; j <= options[i].Length; j++) {
                                    Console.Write(" ");
                                }
                                for (int j = 0; j <= options[i].Length; j++) {
                                    Console.Write("^");
                                }
                                Console.WriteLine("\n");
                                throw new ArgumentException("Parameter is not-integer");
                            }
                        }
                        break;
                    case "PASS":
                        break;
                    default:
                        Console.WriteLine($"'{options[i]}' is not a valid option! Type 'HELP' for available options\n");
                        throw new ArgumentException("Invalid option entered");
                }
            }
        }
        if (opList.Contains("NN") && opList.Contains("NO")) {
            Console.WriteLine("Cannot use 'No Numbers' and 'Numbers Only' options together\n");
            throw new ArgumentException("Contradicting Options");
        }
        if (opList.Contains("U") && opList.Contains("L")) {
            Console.WriteLine("Cannot use 'Lowercase Only' and 'Uppercase Only' options together\n");
            throw new ArgumentException("Contradicting Options");
        }
        string password = "";
        int x = 0;
        while (x < iterations) {
            while (password.Length != length) {
                Random rand = new Random();
                password += (char)rand.Next(minCharCode, maxCharCode);
                if (opList.Contains("N")) {
                    password = Regex.Replace(password, @"[^0-9a-zA-Z]+", "");
                }
                if (opList.Contains("B")) {
                    password = Regex.Replace(password, @"[^0-9a-zA-Z!@#$%^&*]+", "");
                }
                if (opList.Contains("NO")) {
                    password = Regex.Replace(password, @"[^0-9]+", "");
                }
                if (opList.Contains("NN")) {
                    password = Regex.Replace(password, @"[^a-zA-Z]+", "");
                }
            }
            if (opList.Contains("U")) {
                Console.WriteLine(password.ToUpper());
            }
            else if (opList.Contains("L")) {
                Console.WriteLine(password.ToLower());
            }
            else {
                Console.WriteLine(password);
            }
            password = "";
            x++;
        }
        
    }
}