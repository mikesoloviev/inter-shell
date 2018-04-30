using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterBatch.Data {

    public class Preference {

        public string Name = "";

        public string Value = "";

        public Preference() {
        }

        public Preference(string[] fields) {
            Name = fields.Length > 0 ? fields[0].Trim() : "";
            Value = fields.Length > 1 ? fields[1].Trim() : "";
        }

        public string Encode() {
            return $"{Name} = {Value}";
        }
    }
}
