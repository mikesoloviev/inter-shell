using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterShell.DataSource {

    public class Group {

        public string Name { get; set; } = "";
        public List<string> Notes { get; set; } = new List<string>();
        public List<Setting> Settings { get; set; } = new List<Setting>();
        public List<Command> Commands { get; set; } = new List<Command>();

        public string Note { get { return Notes.First() ?? ""; } }
        public bool IsEmpty { get { return Name.Length == 0; } }

        public Dictionary<string, string> GetSettingSet() {
            var settingSet = new Dictionary<string, string>();
            foreach (var setting in Settings) {
                settingSet[setting.Name] = setting.Value;
            }
            return settingSet;
        }

        public List<string> Encode() {
            if (IsEmpty) return new List<string>();
            var code = new List<string> { $"# {Name}" };
            code.AddRange(Notes.Select(x => $"- {x}"));
            code.Add("");
            foreach (var setting in Settings) {
                code.AddRange(setting.Encode());
            }
            code.Add("");
            foreach (var command in Commands) {
                code.AddRange(command.Encode());
                code.Add("");
            }
            return code;
        }

    }
}
