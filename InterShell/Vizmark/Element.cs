using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vizmark {

    public class Element {

        public string Tag = "";
        public int Tab = -1;
        public string Content = "";
        public Dictionary<string, string> Attributes = new Dictionary<string, string>();
        public List<Element> Children = new List<Element>();

        public Element() {
        }

        public Element(string tag, int tab = -1) {
            Tag = tag;
            Tab = tab;
        }
    }

}
