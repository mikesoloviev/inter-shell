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
                prevLine = line;
            }
            text.Append(Tail);
            return text.ToString();
        }

        #endregion

        #region Mark Methods

        bool listBlock = false;
        bool codeBlock = false;
        bool paraBlock = false;
        string prevLine = "";

        void Reset() {
            listBlock = false;
            codeBlock = false;
            paraBlock = false;
            prevLine = "";
        }

        string MarkLine(string line) {
            return MarkSpan(MarkBlock(line));
        }

        string MarkBlock(string line) {
            var result = new StringBuilder();
            if (line.StartsWith("* ")) {
                paraBlock = false;
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
                    paraBlock = false;
                    result.AppendLine("<hr/>");
                }
                else if (line.StartsWith("```")) {
                    paraBlock = false;
                    if (codeBlock) {
                        codeBlock = false;
                        result.AppendLine("</pre>");
                    }
                    else {
                        codeBlock = true;
                        result.AppendLine("<pre>");
                    }
                }
                else if (line.StartsWith("# ")) {
                    paraBlock = false;
                    result.AppendLine(Enclose(line, '#', "h1"));
                }
                else if (line.StartsWith("## ")) {
                    paraBlock = false;
                    result.AppendLine(Enclose(line, '#', "h2"));
                }
                else if (line.StartsWith("### ")) {
                    paraBlock = false;
                    result.AppendLine(Enclose(line, '#', "h3"));
                }
                else if (line.StartsWith("#### ")) {
                    paraBlock = false;
                    result.AppendLine(Enclose(line, '#', "h4"));
                }
                else if (codeBlock) {
                    result.AppendLine(line);
                }
                else if (line.Trim() == "") {
                    // skip
                }
                else if (prevLine.Trim() == "") {
                    if (paraBlock) {
                        result.AppendLine("<p/>");
                    }
                    else {
                        paraBlock = true;
                    }
                    result.AppendLine(line);
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