using System;

namespace InterShell {

    public class Config { 
        public static string AppName = "InterShell";
        public static string Home = ".";

        public static string PrefFile = "InterShell.ini";
        public static string DataFile = "InterShell.dat";
        public static string HelpFile = "InterShell.md";
        public static string ExecFile = "InterShell.bat";
        
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
