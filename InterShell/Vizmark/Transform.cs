using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

// EXAMPLE:
// ^^^svg:vizmark
// $size = width: 100; height: 100 | $small-black-box = width: 10; height: 10; fill: black; stroke: black
// - $size
// * rect; x: 0; y: 0; $size
//   - fill: green; stroke: red
// * rect; x: 10; y:10; $small-black-box
// ^^^

// DOCS -- MACROS
// $origin = x: 100; y: 200 | $size = width: 100; height: 50
// * rect; $origin; $size

// TODO
/*
(1) Defs

@ blue-gradient-1; ...
  @ stop; ...
  @ stop; ...

(2) Plot

* plot
  - view: graph
  - fill: red, blue, green
  - stroke: red, blue, green
  - mark: plus, cross, dot 
  - origin-x: 10; origin-y: 200
  - scale-x: 10; scale-y: 1
  - axis-x: 0, 100, 5; axis-y: 100, 200, 10
  - axis-color: black
  - data: data-table-1

* plot
  - view: graph
  - skin: black-white-graph
  - origin: 10, 200
  - margin: 10, 50
  - scale: 10, 1
  - axes: 0, 100, 5, 100, 200, 10
  - data: data-table-1
  - legend: bottom

# data-table-1
: x, Red Growth, Blue Growth, Green Growth
1, 1, 2, 3
2, 2, 2, 2
3, 4, 7, 5


== NOTES ==

VIEW SET: graph, histogram, piechart
MARK SET: plus, cross, dot, square, rhomb, triangle, star
SKIN SET: predefined plot attributes gouped into 'skin'
LEGEND: table column names will go to 'legend', like this:

<red line> Red Growth
<blue line> Blue Growth
<green line> Green Growth
*/

namespace Vizmark {

    public class Transform {

        #region Interface

        public string Apply(string[] lines) {
            return Compile(Parse(lines));
        }

        #endregion

        #region Macros

        // NOTE: Depending on implementation Macros can be global for the source .markdown file

        Dictionary<string, string> Macros = new Dictionary<string, string>();

        string ApplyMacros(string line) {
            if (line.Contains("$")) {
                foreach (var key in Macros.Keys.OrderByDescending(x => x.Length)) {
                    if (line.Contains(key)) {
                        line = line.Replace(key, Macros[key]);
                    }
                }
            }
            return line;
        }

        void DefineMacros(string line) {
            foreach (var field in line.Split('|')) {
                DefineMacro(field.Trim());
            }
        }

        void DefineMacro(string line) {
            var fields = line.Split('=');
            if (fields.Length < 2) return;
            Macros[fields[0].Trim()] = ApplyMacros(fields[1].Trim());
        }

        #endregion

        #region Parse Methods

        Dictionary<string, string> Synonyms = new Dictionary<string, string> {
            {"figure", "svg"},
            {"group", "g"},
            {"rectangle", "rect"},
            {"oval", "ellipse"}
        };

        string CanonicTag(string tag) {
            return Synonyms.ContainsKey(tag) ? Synonyms[tag] : tag;
        }

        Element Parse(string[] lines) {
            var document = new Element("svg", -1);
            var parents = new Stack<Element>();
            parents.Push(document);
            var element = document;
            var lastName = "";
            foreach (var rawLine in lines) {
                var defLine = rawLine.Trim();
                if (defLine == "") {
                    continue;
                }
                // macros
                if (defLine.StartsWith("$") && defLine.Contains("=")) {
                    DefineMacros(defLine);
                    continue;
                }
                defLine = ApplyMacros(defLine);
                // elements
                if (defLine.StartsWith("* ")) {
                    var tab = rawLine.IndexOf("*");
                    while (tab <= parents.Peek().Tab) parents.Pop();
                    element = new Element("", tab);
                    parents.Peek().Children.Add(element);
                    parents.Push(element);
                    lastName = ParseElement(defLine, element);
                }
                else if (defLine.StartsWith("- ")) {
                    lastName = ParseAttributes(defLine, element);
                }
                else if (defLine.StartsWith("> ")) {
                    element.Content += defLine.Substring(1).Trim() + " ";
                }
                else {
                    if (element.Attributes.ContainsKey(lastName)) {
                        element.Attributes[lastName] += " " + defLine;
                    }
                }
            }
            return document;
        }

        string ParseElement(string line, Element element) {
            var lastName = "";
            foreach (var field in line.Split(';')) {
                if (field.StartsWith("*")) {
                    element.Tag = CanonicTag(field.TrimStart('*').Trim());
                }
                else if (field.Contains(":")) {
                    lastName = ParseAttribute(field, element);
                }
            }
            return lastName;
        }

        string ParseAttributes(string line, Element element) {
            var lastName = "";
            foreach (var field in line.TrimStart('-').Split(';')) {
                if (field.Contains(":")) {
                    lastName = ParseAttribute(field, element);
                }
            }
            return lastName;
        }

        string ParseAttribute(string field, Element element) {
            var tokens = field.Split(':');
            var name = tokens[0].Trim();
            var value = tokens[1].Trim();
            if (tokens.Length == 2) {
                element.Attributes[name] = value;
            }
            return name;
        }

        #endregion

        #region Compile Methods

        string Compile(Element document) {
            var content = new StringBuilder();
            CompileElement(document, content);
            return content.ToString();
        }

        void CompileElement(Element element, StringBuilder content) {
            content.Append($"<{element.Tag}");
            foreach (var name in element.Attributes.Keys) {
                content.Append($" {name}='{element.Attributes[name]}'");
            }
            if (element.Children.Any()) {
                content.AppendLine(">");
                foreach (var child in element.Children) {
                    CompileElement(child, content);
                }
                content.AppendLine($"</{element.Tag}>");
            }
            else if (element.Content.Length > 0) {
                content.Append(">");
                content.Append(MarkEscape(element.Content));
                content.AppendLine($"</{element.Tag}>");
            }
            else {
                content.AppendLine("/>");
            }
        }

        string MarkEscape(string line) {
            return line.Replace("<", "&lt;").Replace(">", "&gt;").Replace("& ", "&amp; ").Trim();
        }

        #endregion

    }
}