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
                // <table> element
                if (line.StartsWith("|")) {
                    if (line.Contains("---")) {
                        ParseColumns(line, element)
                    }
                    else {
                        if (element.Tag == "") {
                            element.Tag = "table";
                        }
                        ParseRow(line, element)
                    }
                    continue;
                }
                if (line.StartsWith("|-") && element.Tag == "table") {
                    // TODO
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

        void ParseRow(string line, Element table) {
            var row = new Element("tr");
            table.Children.Add(row);
            foreach (var cell in ParseCells(line)) {
                row.Children.Add(new Element("td", cell.Trim()));
            }
        }

        void ParseColumns(string line, Element table) {
            if (!table.Chilrden.Any()) return;
            var columns = table.Chilrden.First();
            var aligns = ParseCells(line);
            for (var i = 0; i < Math.Min(aligns.Count, columns.Count); i++) {
                columns[i].Tag = "th";
                if (aligns[i].EndsWith(":")) {
                    columns[i].Align = aligns[i].StartsWith(":") ? "center" : "right";
                }
            }
        }

        List<string> ParseCells(string line) {
            var cells = new List<string>();
            foreach (var cell in line.Trim().Trim('|').Trim().Split('|')) {
                cells.Add(cell.Trim());
            }
            return cells;
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
                        content.Append(MarkInner(element));
                        content.AppendLine(Close(element.Tag));
                        break;
                    case "pre": 
                        content.AppendLine(Open(element.Tag));
                        content.AppendLine(MarkInner(element));
                        content.AppendLine(Close(element.Tag));
                        break;
                    case "ul": 
                        CompileUl(element, content);
                        break;
                    case "table":
                        if (!element.Chilrden.Any()) break;
                        var columns = element.Chilrden.First();
                        content.AppendLine(Open(element.Tag));
                        foreach (var row in table.Childen) {
                            content.Append(Open(row.Tag));
                            for (var i = 0; i < Math.Min(row.Children.Count, columns.Count); i++) {
                                var cell = row.Children[i];
                                content.Append(Open(cell.Tag, columns[i].Align));
                                content.Append(MarkInner(cell));
                                content.Append(Close(cell.Tag));
                            }
                            content.AppendLine(Close(row.Tag));
                        }
                        content.AppendLine(Close(element.Tag));
                        break;
                    default: 
                        if (element.Tag.StartsWith("h")) {
                            content.Append(Open(element.Tag));
                            content.Append(MarkInner(element));
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
                        content.Append(MarkInner(child));
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

        string MarkInner(Element element) {
            return element.Tag == "pre" ? MarkEscape(element.Text) : MarkLinks(MarkSpans(MarkEscape(element.Text)));
        }

        string MarkLinks(string line) {
            while (true) {
                var i = line.IndexOf("](");
                if (i < 0) break;
                var j = line.IndexOf("(", i);
                if (j < 0) break;
                var h = line.LastIndexOf("[", i);
                if (h < 0) break;
                var k = line.IndexOf(")", j);
                if (k < 0) break;
                var text = line.Substring(h + 1, i - h - 1).Trim();
                var link = line.Substring(j + 1, k - j - 1).Trim();
                line = CutPaste(line, h, k, $"<a href='{link}'>{text}</a>");
            }
            return line;
        }

        string CutPaste(string line, int i, int j, string clip) {
            return line.Substring(0, i) + clip + (j == line.Length - 1 ? "" : line.Substring(j + 1));

        }

        string MarkSpans(string line) {
            var result = new StringBuilder();
            var boldSpan = false;
            var italicSpan = false;
            var codeSpan = false;
            var linkSpan = false;
            var before = ' ';
            foreach (var c in line.Replace("**", "~")) {
                switch (c) {
                    case '(': linkSpan = before == ']'; result.Append(c); break;
                    case ')': linkSpan = false; result.Append(c); break;
                    case '~': if (linkSpan) result.Append(c); else result.Append(PutTag("b", ref boldSpan)); break;
                    case '_': if (linkSpan) result.Append(c); else result.Append(PutTag("i", ref italicSpan)); break;
                    case '`': result.Append(PutTag("code", ref codeSpan)); break;
                    default: result.Append(c); break;  
                }
                before = c;
            }
            return result.ToString();
        }

        string MarkEscape(string line) {
            var result = new StringBuilder();
            var linkSpan = false;
            var before = ' ';
            foreach (var c in line) {
                switch (c) {
                    case '(': linkSpan = before == ']'; result.Append(c); break;
                    case ')': linkSpan = false; result.Append(c); break;
                    case '<': result.Append("&lt;"); break;
                    case '>': result.Append("&gt;"); break;
                    case '&': if (linkSpan) result.Append(c); else result.Append("&amp;"); break;
                    default: result.Append(c); break;
                }
                before = c;
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

        #endregion
        
    }
}