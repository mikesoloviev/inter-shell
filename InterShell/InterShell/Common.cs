namespace InterShell {

    public class Config { 

        public static string AppName = "InterShell";
        public static string Home = ".";

        public static string PrefFile = $"{AppName}.ini";
        public static string DataFile = $"{AppName}.dat";
        public static string HelpFile = $"{AppName}.md";
        public static string ShellFile = $"{AppName}.bat";
        public static string OutFile = $"{AppName}.out";

        public static int WindowWidth = 800;
        public static int WindowHeight = 600;
    }

    public class Selection { 
        public const int None = -1;
    }

    public enum Division { 
        Library,
        Group, 
        Command,
        Setting
    }
}
