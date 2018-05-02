using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace InterShell.DataSource {

    public class Library {

        public string Home;
        public string DataFile = "InterShell.dat";
        public string PrefFile = "InterShell.ini";

        public List<Group> Groups { get; set; } = new List<Group>();

        public Dictionary<string, string> Preferences = new Dictionary<string, string>();

        public Library(string home) {
            Home = home;
        }

        #region Convenience

        public string TheGroupName {
            get { return Preferences.ContainsKey("group") ? Preferences["group"] : ""; }
            set { Preferences["group"] = value; }
        }

        public Group TheGroup {
            get { return Groups.Where(x => x.Name == TheGroupName).FirstOrDefault() ?? new Group(); }
        }

        public List<Setting> TheSettings {
            get { return TheGroup.Settings; }
        }

        public List<Command> TheCommands {
            get { return TheGroup.Commands; }
        }

        public string Code {
            get { return string.Join(Environment.NewLine, Encode()); }
            set { Decode(value); }
        }

        public void SelectGroup(int index) {
            TheGroupName = Groups[index].Name;
        }

        public void SetSetting(string name, string value) {
            var setting = TheSettings.Where(x => x.Name == name).FirstOrDefault();
            if (setting != null) setting.Value = value;
        }

        #endregion

        #region Serialization 

        string[] Encode() {
            var code = new List<string>();
            foreach (var group in Groups) {
                code.AddRange(group.Encode());
            }
            return code.ToArray();
        }

        void Decode(string text) {
            Decode(text.Replace('\r', '\n').Split('\n'));
        }

        void Decode(string[] lines) {
            Groups.Clear();
            var group = new Group();
            var command = new Command();
            var setting = new Setting();
            var context = "";
            foreach (var rawLine in lines) {
                var line = rawLine.Trim();
                if (line.Length == 0) {
                    // skip
                }
                else if (line.StartsWith("#")) {
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

        #endregion

        #region Persistence

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
            Decode(File.ReadAllLines(path));
        }

        public void SaveData() {
            File.WriteAllLines(Path.Combine(Home, DataFile), Encode());
        }

        #endregion

    }
}
