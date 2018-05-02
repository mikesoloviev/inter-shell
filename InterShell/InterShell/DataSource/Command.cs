using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterShell.DataSource {

    public class Command {

        public string Name { get; set; } = "";

        public string Note { get; set; } = "";

        public List<string> Instructions { get; set; } = new List<string>();

        public bool IsEmpty { get { return !Instructions.Any(); } }

        public List<string> Encode() {
            var code = new List<string> { $"* {Name}", $"- {Note}", "" };
            code.AddRange(Instructions);
            return code;
        }

    }
}
