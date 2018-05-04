using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InterShell {

    public class StatusResult {

        public string Name = "";
        public string Code = "";
        public string Error = "";
        public string Output = "";

        public string ToText() {
            var text = new StringBuilder();

            return text.ToString();
        }
    }

}
