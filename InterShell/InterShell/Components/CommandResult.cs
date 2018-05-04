using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InterShell.Components {

    public class CommandResult {

        public string Name = "";
        public string Code = "";
        public string Error = "";
        public string Output = "";

        public string ToText() {
            var text = new StringBuilder();
            text.AppendLine("EXECUTED:");
            text.AppendLine(Name);
            text.AppendLine();
            text.AppendLine("SCRIPT:");
            text.AppendLine(Code);
            text.AppendLine();
            if (Error.Length > 0) {
                text.AppendLine("ERROR:");
                text.AppendLine(Error);
                text.AppendLine();
            }
            if (Output.Length > 0) {
                text.AppendLine("OUTPUT:");
                text.AppendLine(Output);
                text.AppendLine();
            }
            return text.ToString();
        }
    }

}
