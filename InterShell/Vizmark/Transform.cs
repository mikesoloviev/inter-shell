using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Vizmark {

    public class Transform {

        #region Interface

        public string Apply(string[] lines) {
            return Compile(Parse(lines));
        }

        #endregion
        
        #region Main Methods

        Dictionary<string, string> Synonyms = {
            {"figure", "svg"},
            {"group", "g"},
            {"rectangle", "rect"},
            {"oval", "ellipse"}
        };

        Element Parse(string[] lines) {
            var document = new Element("svg", -1);
            var element = document;
            var stack = new Stack<Element>();
            stack.Push(document);
            var lastName = "";
            foreach (var rawLine in lines) {
                var trimLine = rawLine.Trim();
                var line = trimLine == "" ? "" : rawLine;
                if (trimLine == "") {
                    // skip
                }
                else if (trimLine.StartsWith("*")) {
                    var tab = line.IndexOf("*");
                    if (tab <= stack.Peek().Tab) {
                        while (tab <= stack.Peek().Tab) stack.Pop();
                        element = stack.Peek();
                    }
                    element = new Element("", tab);
                    stack.Peek().Children.Add(element);
                    stack.Push(element);
                    lastName = ParseElement(trimLine, element);
                }
                else if (trimLine.StartsWith("-")) {
                    lastName = ParseAttributes(line, element);
                }
                else if (trimLine.StartsWith(">")) {
                    element.Content += trimLine.Substring(1).Trim() + " ";
                }
                else {
                    if (element.Attributes.ContainsKey(lastName)) {
                        element.Attributes[lastName] += " " + trimLine;
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
                else {
                    lastName = ParseAttribute(field, element);
                }
            }
            return lastName;
        }

        string CanonicTag(string tag) {
            return Synonyms.ContainsKey(tag) ? Synonyms[tag] : tag;
        }

        string ParseAttributes(string line, Element element) {
            var lastName = "";
            foreach (var field in line.TrimStart('-').Split(';')) {
                lastName = ParseAttribute(field, element);
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
            if (element.Childern.Any()) {
                content.AppendLine(">");
                foreach (var child in element.Childern) {
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