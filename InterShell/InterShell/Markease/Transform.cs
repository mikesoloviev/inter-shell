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
                Apply(File.ReadAllLines(path));
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

        Document Parse(string[] lines) {
            var document = new Element("body");
            var stack = new Stack<Element>();
            stack.push(document);
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
                    if (tab > stack.Peek().tab) {
                        if (element.tag == "") {
                            element.Tag = "ul";
                            element.Tab = tab;
                        }
                        else {
                            element = new Element("ul", "", tab);
                            stack.Peek().Children.Add(element);
                        }
                        stack.push(element);
                    }
                    else if (tab < stack.Peek().tab) {
                        element = stack.Pop();
                    }
                    element.Children.Add(new Element("li", TrimPrefix(line, '*')));
                    continue;
                }
                // other elements
                if (line == "") {
                    stack.Clear();
                    stack.push(document);
                    element = new Element();
                    document.Children.Add(element);
                    continue;
                }
                if (element.tag == "") {
                    Typify(element, line);
                }
                switch (element.tag) {
                    case "hr": break;
                    case "p": element.Text += line + " "; break;
                    default: if (element.tag.StartsWith("h")) element.Text = TrimPrefix(line, '#'); break;
                }
            }
            return document;
        }

        string Compile(Element document) {
            var content = new StringBuilder();
            var previous = new Element();
            foreach(var element in document.Children) {
                if (element.tag == "") continue;
                switch (element.tag) {
                    case "hr":
                        content.AppendLine(Single(element.tag));
                        break;
                    case "p":
                        content.Append(Open(element.tag), previous.tag == "p" ? "" : "lead");
                        content.Append(MarkSpans(element.Text.Trim()));
                        content.AppendLine(Close(element.tag));
                        break;
                    case "pre": 
                        content.AppendLine(Open(element.tag));
                        content.AppendLine(MarkEscape(element.Text.Trim());
                        content.AppendLine(Close(element.tag));
                        break;
                    case "ul": 
                        CompileUl(child, element);
                        break;
                    default: 
                        if (element.tag.StartsWith("h")) {
                            content.Append(Open(element.tag));
                            content.Append(MarkSpans(element.Text.Trim()));
                            content.AppendLine(Close(element.tag));
                        }
                        break;
                }
                previous = element;
            }
            return content.ToString();
        }

        void CompileUl(Element element, StringBuilder content) {
            content.AppendLine(Open(element.tag));
            foreach(var child in element.children) {
                switch (child.Tag) {
                    case "li":
                        content.Append(Open(child.tag));
                        content.Append(MarkSpans(child.Text.Trim()));
                        content.AppendLine(Close(child.tag));
                    case "ul":
                        CompileUl(child, content);
                        break;
                    default:
                        break;
                }
            }
            content.AppendLine(Close(element.tag));
        }
        
        string Template(string content) {
            var text = new StringBuilder();
            text.AppendLine("<html>")
            text.AppendLine("<head>")
            text.AppendLine("<style>")
            text.Append(Style)
            text.AppendLine("</style>");
            text.AppendLine("</head>");
            text.AppendLine("<body>")
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

        string Typify(Element element, string line) {
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

        #region Static Content

        string Style =
@"
body {
  font-family: 'Segoe UI', Helvetica, Arial, sans-serif;
  font-size: 14px;
  line-height: 1.5;
  background-color: white;
  color: #202020;
  margin-top: 10px;
  margin-left: 10px;
}
h1, h2, h3, h4, h5, h6 {
  margin: 20px 0 10px;
  padding: 0;
  font-weight: bold;
  cursor: text;
  position: relative;
}
h1 {
  font-size: 24px;
  border-bottom: 1px solid #C0C0C0;
}
h2 {
  font-size: 20px;
  border-bottom: 1px solid #C0C0C0;
}
h3 {
  font-size: 18px;
  border-bottom: 1px solid #C0C0C0;
}
h4 {
  font-size: 16px;
}
hr {
  border: 0 none;
  color: #C0C0C0;
  height: 1px;
  padding: 0;
}
ul {
  margin-left: 15px;
  margin-top: 10px;
  margin-bottom: 10px;
}
b {
  color: #202040;
}
i {
  color: #202060;
}
code {
  color: #000020;
  background-color: #F0F0F0;
  padding-left: 5px;
  padding-right: 5px;
}
pre {
  color: #000020;
  background-color: #F0F0F0;
  line-height: 0.75;
  padding: 0px 10px;
  margin-top: 10px;
  margin-bottom: 10px;
  border: 1px solid #E0E0E0;
  border-radius: 4px;
  overflow: auto;
}
p {
  margin-top: 10px;
}
p.lead {
  margin-top: 0px;
}
";
        #endregion
        
    }
}