using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Viscont.Data {

    public class Library {

        public string Home;

        public string DataFile = "viscont.dat";
        public string PrefFile = "viscont.ini";

        public List<Group> Groups = new List<Group>();

        public Dictionary<string, string> Preferences = new Dictionary<string, string>();

        public string TheGroupName {
            get { return Preferences.ContainsKey("group") ? Preferences["group"] : ""; }
            set { Preferences["group"] = value; }
        }

        public Library(string home) {
            Home = home;
        }

        public void Load() {
            LoadPrefs();
            LoadData();
        }

        public void Save() {
            SavePrefs();
            SaveData();
        }

        public void LoadPrefs() {
            var path = Path.Combine(Home, PrefFile);
            if (!File.Exists(path)) return;
            foreach (var line in File.ReadAllLines(path)) {
                if (line.Contains("=")) {
                    var fields = line.Split('=');
                    if (fields.Length == 2) {
                        Preferences[fields[0].Trim()] = fields[1].Trim();
                    }
                }
            }
        }

        public void SavePrefs() {
            var code = new List<string>();
            foreach (var name in Preferences.Keys) {
                code.Add($"{name} = {Preferences[name]}");
            }
            File.WriteAllLines(Path.Combine(Home, PrefFile), code);
        }

        public void LoadData() {
            var path = Path.Combine(Home, DataFile);
            if (!File.Exists(path)) return;
            var group = new Group();
            var command = new Command();
            var setting = new Setting();
            var context = "";
            foreach (var rawLine in File.ReadAllLines(path)) {
                var line = rawLine.Trim();
                if (line.StartsWith("#")) {
                    context = "group";
                    group = new Group();
                    group.Name = line.TrimStart('#').Trim();
                    Groups.Add(group);
                }
                else if (line.StartsWith("*")) {
                    context = "command";
                    command = new Command();
                    command.Name = line.TrimStart('*').Trim();
                    group.Commands.Add(command);
                }
                else if (line.StartsWith("-")) {
                    switch (context) {
                        case "group": group.Note = line.TrimStart('-').Trim(); break;
                        case "command": command.Note = line.TrimStart('-').Trim(); break;
                        default: break;
                    }
                }
                else if (line.StartsWith(":")) {
                    setting.AddOptions(line.TrimStart(':').Split(','));
                }
                else {
                    switch (context) {
                        case "group":
                            setting = new Setting(line.Split('='));
                            group.Settings.Add(setting);
                            break;
                        case "command":
                            command.Instructions.Add(line);
                            break;
                        default: break;
                    }
                }
            }
        }

        public void SaveData() {
            var code = new List<string>();
            foreach (var preference in Preferences.Values) {
                code.Add(preference.Encode());
            }
            foreach (var group in Groups) {
                code.AddRange(group.Encode());
            }
            File.WriteAllLines(Path.Combine(Home, DataFile), code);
        }

    }
}
