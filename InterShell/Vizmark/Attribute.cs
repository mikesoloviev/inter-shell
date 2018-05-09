using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vizmark {

    public class Attribute {

        public string Name = "";
        public string Value = "";

        public Attribute() {
        }

        public Attribute(string name, string value) {
            Name = name;
            Value = value;
        }
    }

}
