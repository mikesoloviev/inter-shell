using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace InterShell {

    public class ShellEngine {

        public CommandResult Run(string home, string filename, Command command, Group group) {
            var script = List<string>();
            var settingSet = group.GetSettingSet();
            var result = new CommandResult();
            foreach (var line in command.Instructions) {
                script.Add(Eval(line, settings));
            }
            return RunShell home, filename, script, result);
        }

        void RunShell(string home, string filename, List<string> script, CommandResult result) {
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
