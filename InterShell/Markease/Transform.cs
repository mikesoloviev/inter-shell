using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Markease {

    public class Transform {

        #region Interface

        public string LoadApply(string path) {
            try {
                return Apply(File.ReadAllLines(path));
            }
            catch {
                return "";
            }
        }

        public string Apply(string[] lines) {
            return Template(Compile(Parse(lines)));
        }

        #endregion
        
        #region Main Methods

        Element Parse(string[] lines) {
            var document = new Element("body");
            var stack = new Stack<Element>();
            stack.Push(document);
            var element = new Element();
            document.Children.Add(element);
            var isPre = false;
            foreach (var rawLine in lines) {
                var trimLine = rawLine.Trim();
                var line = trimLine == "" ? "" : rawLine;
                // <pre> element
                if (line.StartsWith("```")) {
                    isPre = !isPre;
                    if (isPre) element.Tag = "pre";
                    continue;
                }
                if (isPre) {
                    element.Text += line + Environment.NewLine;
                    continue;
                }
                // <ul> element
                if (trimLine.StartsWith("* ")) {
                    var tab = line.IndexOf("*");
                    if (tab > stack.Peek().Tab) {
                        if (element.Tag == "") {
                            element.Tag = "ul";
                            element.Tab = tab;
                        }
                        else {
                            element = new Element("ul", "", tab);
                            stack.Peek().Children.Add(element);
                        }
                        stack.Push(element);
                    }
                    else if (tab < stack.Peek().Tab) {
                        while (tab < stack.Peek().Tab) stack.Pop();
                        element = stack.Peek();
                    }
                    element.Children.Add(new Element("li", TrimPrefix(line, '*')));
                    continue;
                }
                // other elements
                if (line == "") {
                    stack.Clear();
                    stack.Push(document);
                    element = new Element();
                    document.Children.Add(element);
                    continue;
                }
                if (element.Tag == "") {
                    Typify(element, line);
                }
                switch (element.Tag) {
                    case "hr": break;
                    case "p": element.Text += line + " "; break;
                    default: if (element.Tag.StartsWith("h")) element.Text = TrimPrefix(line, '#'); break;
                }
            }
            return document;
        }

        string Compile(Element document) {
            var content = new StringBuilder();
            var previous = new Element();
            foreach(var element in document.Children) {
                if (element.Tag == "") continue;
                switch (element.Tag) {
                    case "hr":
                        content.AppendLine(Single(element.Tag));
                        break;
                    case "p":
                        content.Append(Open(element.Tag));
                        content.Append(MarkSpans(element.Text.Trim()));
                        content.AppendLine(Close(element.Tag));
                        break;
                    case "pre": 
                        content.AppendLine(Open(element.Tag));
                        content.AppendLine(MarkEscape(element.Text.Trim()));
                        content.AppendLine(Close(element.Tag));
                        break;
                    case "ul": 
                        CompileUl(element, content);
                        break;
                    default: 
                        if (element.Tag.StartsWith("h")) {
                            content.Append(Open(element.Tag));
                            content.Append(MarkSpans(element.Text.Trim()));
                            content.AppendLine(Close(element.Tag));
                        }
                        break;
                }
                previous = element;
            }
            return content.ToString();
        }

        void CompileUl(Element element, StringBuilder content) {
            content.AppendLine(Open(element.Tag));
            foreach (var child in element.Children) {
                switch (child.Tag) {
                    case "li":
                        content.Append(Open(child.Tag));
                        content.Append(MarkSpans(child.Text.Trim()));
                        content.AppendLine(Close(child.Tag));
                        break;
                    case "ul":
                        CompileUl(child, content);
                        break;
                    default:
                        break;
                }
            }
            content.AppendLine(Close(element.Tag));
        }
        
        string Template(string content) {
            var text = new StringBuilder();
            text.AppendLine("<html>");
            text.AppendLine("<head>");
            text.Append("<style>");
            text.Append(Stylesheet.Style);
            text.AppendLine("</style>");
            text.AppendLine("</head>");
            text.AppendLine("<body>");
            text.Append(content);
            text.AppendLine("</body>");
            text.AppendLine("</html>");
            return text.ToString();
        }

        #endregion

        #region Mark Methods

        string Open(string tag, string style = "") {
            return style == "" ? $"<{tag}>" : $"<{tag} class='{style}'>";
        }

        string Close(string tag) {
            return $"</{tag}>";
        }

        string Single(string tag) {
            return $"<{tag}/>";
        }

        void Typify(Element element, string line) {
            if (line.StartsWith("***")) {
                element.Tag = "hr";
            }
            else if (line.StartsWith("# ")) {
                element.Tag = "h1";
            }
            else if (line.StartsWith("## ")) {
                element.Tag = "h2";
            }
            else if (line.StartsWith("### ")) {
                element.Tag = "h3";
            }
            else if (line.StartsWith("#### ")) {
                element.Tag = "h4";
            }
            else {
                element.Tag = "p";
            }
        }

        string MarkSpans(string line) {
            var result = new StringBuilder();
            var boldSpan = false;
            var italicSpan = false;
            var codeSpan = false;
            foreach (var c in MarkEscape(line).Replace("**", "~")) {
                switch (c) {
                    case '~': result.Append(PutTag("b", ref boldSpan)); break;
                    case '_': result.Append(PutTag("i", ref italicSpan)); break;
                    case '`': result.Append(PutTag("code", ref codeSpan)); break;
                    default: result.Append(c); break;  
                }
            }
            return result.ToString();
        }

        string PutTag(string tag, ref bool span) {
            span = !span;
            return span ? $"<{tag}>" : $"</{tag}>";
        }

        string TrimPrefix(string line, char prefix) {
            return line.TrimStart().TrimStart(prefix).TrimStart();
        }

        string MarkEscape(string line) {
            return line.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
        }

        #endregion
        
    }
}