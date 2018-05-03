using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace InterShell {

    public class ShellEngine {

        public string Run(string home, string filename, List<string> instructions, Dictionary<string, string> settings) {
            var script = List<string>();
            foreach (var line in instructions) {
                script.Add(Eval(line, settings));
            }
            return RunShell home, filename, script);
        }

        string RunShell(string home, string filename, List<string> script) {
            var command = Path.Combine(home, filename);
            File.WriteAllLines(command, script);
            var startInfo = new ProcessStartInfo();
            startInfo.UseShellExecute = true;
            startInfo.FileName = command;
            startInfo.WorkingDirectory = home;
            try {
                using (var process = Process.Start(startInfo)) {
                    process.WaitForExit();
                }
                return "OK";
            }
            catch (Exception e) {
                return $"ERROR: {e.ToString()}";
            }
        }

        string Eval(string line, Dictionary<string, string> settings) {
            if (line.Contains("$")) {
                foreach (var key in settings.Keys) {
                    var target = "$" + key;
                    if (line.Contains(target)) {
                        line = line.Replace(target, settings[key]);
                    }
                }
            }
            return line;
        }
        

    }
}
