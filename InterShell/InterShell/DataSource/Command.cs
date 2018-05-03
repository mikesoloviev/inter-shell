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

        public bool IsEmpty { get { return Name.Length == 0; } }

        public List<string> Encode() {
            if (IsEmpty) return new List<string>();
            var code = new List<string> { $"* {Name}", $"- {Note}", "" };
            code.AddRange(Instructions);
            return code;
        }

    }
}
