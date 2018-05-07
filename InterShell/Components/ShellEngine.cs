using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using InterShell.DataSource;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterShell.Components {

    public class ShellEngine {

        // INFO: Usage of $OUT and $ERR
        // echo test >> $OUT $ERR

        public CommandResult Run(string home, string filename, Command command, Group group) {
            var settings = group.GetSettingSet();
            MakeOut(home, settings);
            var script = new List<string>();
            foreach (var line in command.Instructions) {
                script.Add(Eval(line, settings));
            }
            var result = new CommandResult();
            result.Name = command.Name;
            result.Code = string.Join(Environment.NewLine, script);
            result.Error = RunShell(home, filename, script);
            result.Output = ReadOut(home);
            return result;
        }

        void MakeOut(string home, Dictionary<string, string> settings) {
            var path = Path.Combine(home, Config.OutFile);
            File.WriteAllText(path, "");
            settings["OUT"] = $"\"{path}\"";
            settings["ERR"] = "2>&1";
        }

        string ReadOut(string home) {
            return File.ReadAllText(Path.Combine(home, Config.OutFile));
        }

        string RunShell(string home, string filename, List<string> script) {
            File.WriteAllLines(Path.Combine(home, filename), script);
            var startInfo = new ProcessStartInfo();
            startInfo.UseShellExecute = true;
            startInfo.WorkingDirectory = home;
            startInfo.FileName = filename;
            try {
                using (var process = Process.Start(startInfo)) {
                    process.WaitForExit();
                }
                return "";
            }
            catch (Exception e) {
                return e.ToString();
            }
        }

        string Eval(string line, Dictionary<string, string> settings) {
            if (line.Contains("$")) {
                foreach (var key in settings.Keys.OrderByDescending(x => x.Length)) {
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
