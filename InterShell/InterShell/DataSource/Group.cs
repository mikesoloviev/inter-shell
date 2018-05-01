using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterShell.DataSource {

    public class Group {

        public string Name { get; set; } = "";

        public string Note { get; set; } = "";

        public List<Setting> Settings { get; set; } = new List<Setting>();

        public List<Command> Commands { get; set; } = new List<Command>();

        public List<string> Encode() {
            var code = new List<string> { $"# {Name}", $"- {Note}" };
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
