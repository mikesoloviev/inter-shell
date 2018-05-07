using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markease {

    public class Element {

        public string Tag = "";
        public string Text = "";
        public int Tab = -1;
        public List<Element> Children = new List<Element>();

        public Element() {
        }

        public Element(string tag, string text = "", int tab = -1) {
            Tag = tag;
            Text = text;
            Tab = tab;
        }
    }

}
