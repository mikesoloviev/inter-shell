using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterShell.DataSource {

    public class Setting {

        public string Name { get; set; } = "";

        public string Value { get; set; } = "";

        public List<string> Options { get; set; } = new List<string>();

        public bool IsEmpty { get { return Name.Length == 0; } }

        public Setting() {
        }

        public Setting(string[] fields) {
            Name = fields.Length > 0 ? fields[0].Trim() : "";
            Value = fields.Length > 1 ? fields[1].Trim() : "";
        }

        public Setting Clone() {
            var cc = new Setting();
            cc.Name = Name;
            cc.Value = Value;
            cc.Options = Options;
            return cc;
        }

        public void AddOptions(string[] options) {
            foreach (var option in options) {
                Options.Add(option.Trim());
            }
        }

        public List<string> Encode() {
            if (IsEmpty) return new List<string>();
            var code = new List<string> { $"{Name} = {Value}" };
            if (Options.Any()) code.Add($": {string.Join(", ", Options)}");
            return code;
        }

    }
}
