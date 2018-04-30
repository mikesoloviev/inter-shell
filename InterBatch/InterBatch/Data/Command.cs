using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterBatch.Data {

    public class Command {

        public string Name = "";

        public string Note = "";

        public List<string> Instructions = new List<string>();

        public List<string> Encode() {
            var code = new List<string> { $"* {Name}", $"- {Note}" };
            code.AddRange(Instructions);
            return code;
        }

    }
}
