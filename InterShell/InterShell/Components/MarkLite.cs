using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace InterShell.Components {

    public class MarkLite {

        #region Interface

        public string Load(string path) {
            if (!File.Exists(path)) return "";
            return Transform(File.ReadAllLines(path));
        }

        public string Transform(string[] lines) {
            Reset();
            var text = new StringBuilder();
            text.Append(Head);
            foreach (var line in lines) {
                text.AppendLine(MarkLine(line));
            }
            text.Append(Tail);
            return text.ToString();
        }

        #endregion

        #region Mark Methods

        bool listBlock = false;
        bool codeBlock = false;

        void Reset() {
            listBlock = false;
            codeBlock = false;
        }

        string MarkLine(string line) {
            return MarkSpan(MarkBlock(line));
        }

        string MarkBlock(string line) {
            var result = new StringBuilder();
            if (line.StartsWith("* ")) {
                if (!listBlock) {
                    listBlock = true;
                    result.AppendLine("<ul>");
                }
                result.AppendLine(Enclose(line, '*', "li"));
            }
            else {
                if (listBlock) {
                    listBlock = false;
                    result.AppendLine("</ul>");
                }
                if (line.StartsWith("***")) {
                    result.AppendLine("<hr/>");
                }
                else if (line.StartsWith("```")) {
                    if (codeBlock) {
                        codeBlock = false;
                        result.AppendLine("</code>");
                    }
                    else {
                        codeBlock = true;
                        result.AppendLine("<code>");
                    }
                }
                else if (line.StartsWith("# ")) {
                    result.AppendLine(Enclose(line, '#', "h1"));
                }
                else if (line.StartsWith("## ")) {
                    result.AppendLine(Enclose(line, '#', "h2"));
                }
                else if (line.StartsWith("### ")) {
                    result.AppendLine(Enclose(line, '#', "h3"));
                }
                else if (line.StartsWith("#### ")) {
                    result.AppendLine(Enclose(line, '#', "h4"));
                }
                else {
                    result.AppendLine(line);
                }
            }
            return result.ToString();
        }
        
        string MarkSpan(string line) {
            var result = new StringBuilder();
            var boldSpan = false;
            var italicSpan = false;
            var codeSpan = false;
            foreach (var c in line.Replace("**", "~")) {
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

        string Enclose(string line, char prefix, string tag) {
            return $"<{tag}>{line.TrimStart(prefix).Trim()}</{tag}>";
        }

        #endregion

        #region Static Content

        string Head = 
@"
<html>
<head>
<style>
</style>
</head>
<body>
";
        string Tail =
@"
</body>
</html>
";

        #endregion
        
    }
}