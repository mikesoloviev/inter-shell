using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace InterBatch.Data {

    public class Library {

        public string Home;

        public string DataFile = "InterBatch.dat";

        public List<Group> Groups = new List<Group>();

        public Dictionary<string, Preference> Preferences = new Dictionary<string, Preference>();

        public string TheGroupName {
            get { return Preferences.ContainsKey("group") ? Preferences["group"].Value : ""; }
            set { Preferences["group"].Value = value; }
        }

        public Library(string home) {
            Home = home;
        }

        public void Load() {
            var path = Path.Combine(Home, DataFile);
            if (!File.Exists(path)) return;
            var group = new Group();
            var command = new Command();
            var preference = new Preference();
            var setting = new Setting();
            var context = "preference";
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
                        case "preference":
                            preference = new Preference(line.Split('='));
                            Preferences[preference.Name] = preference;
                            break;
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

        public void Save() {
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
