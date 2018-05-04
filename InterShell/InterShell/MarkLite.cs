using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace InterShell {

    public class MarkLite {

        public string Load(string path) {
            if (!File.Exists(path)) return "";
            return Transform(File.RealAllLines(path).ToList());
        }

        public string Transform(List<string> lines) {
            var text = new StringBuilder();
            // TODO: head
            foreach (var line in inLines) {
                text.AppendLine(MarkLine(line));
            }
            // TODO: tail
            return text.ToString();
        }

        string MarkLine(string line) {
            line = line.Replace("***", "<hr/>").Replace("**", "~");
            line = MarkHeader(line);
            line = MarkList(line);
            line = MarkSpan(line);
            return line;
        }

        string MarkSpan(string inLine) {
            var outLine = StringBuilder();
            var boldSpan = false;
            var italicSpan = false;
            var codeSpan = false;
            foreach (var c in inLine) {
                switch (c) {
                    case '~': outLine.Append(PutTag("b", ref boldSpan)); break;
                    case '_': outLine.Append(PutTag("i", ref italicSpan)); break;
                    case '`': outLine.Append(PutTag("code", ref codeSpan)); break;
                    default: outLine.Append(c); break;  
                }
            }
            return outLine.ToString();
        }

        string PutTag(string tag, ref bool span) {
            span = !span;
            return span ? $"<{tag}>" : $"</{tag}>";
        }

        string MarkHeader(string line) {
            if (line.StartsWith("# ")) {
                return Enclose(line, '#', "h1");
            }
            else if (line.StartsWith("## ")) {
                return Enclose(line, '#', "h2");
            }
            else if (line.StartsWith("### ")) {
                return Enclose(line, '#', "h3");
            }
            else if (line.StartsWith("#### ")) {
                return Enclose(line, '#', "h4");
            }
            else {
                return line;
            }
        }
        
        string MarkList(string line) {
            if (line.StartsWith("* ")) {
                return Enclose(line, '*', "li");
            }
            else {
                return line;
            }
        }

        string Enclose(string line, char prefix, string tag) {
            return $"<{tag}>{Trim(line, prefix)}</{tag}>";
        }

        void Trim(string line, char symbol) {
            return line.Trim(symbol).Trim();
        }
    }
}