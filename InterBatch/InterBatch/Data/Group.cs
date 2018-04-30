using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterBatch.Data {

    public class Group {

        public string Name = "";

        public string Note = "";

        public List<Setting> Settings = new List<Setting>();

        public List<Command> Commands = new List<Command>();

        public List<string> Encode() {
            var code = new List<string> { "", $"# {Name}", $"- {Note}" };
            foreach(var setting in Settings) {
                code.AddRange(setting.Encode());
            }
            foreach (var command in Commands) {
                code.Add("");
                code.AddRange(command.Encode());
            }
            return code;
        }

    }
}
