namespace Markease {

    public class Stylesheet {

        public static string Style =
@"
body {
  font-family: 'Segoe UI', 'Helvetica Neue', Helvetica, Arial, sans-serif;
  font-size: 14px;
  line-height: 1.5;
  background-color: white;
  color: #202020;
  margin-top: 10px;
  margin-left: 10px;
}
h1, h2, h3, h4 {
  margin: 20px 0px 10px 0px;
  padding: 0;
}
h1, h2, h3 {
  margin-bottom: 15px;
  border-bottom: 1px solid #C0C0C0;
}
h1 {
  font-size: 24px;
}
h2 {
  font-size: 20px;
}
h3 {
  font-size: 18px;
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
p {
  margin-top: 10px;
  margin-bottom: 10px;
}
p + p {
  margin-top: 0px;
}
ul {
  margin-left: 15px;
  margin-top: 10px;
  margin-bottom: 10px;
}
ul ul {
  margin-top: 0px;
  margin-bottom: 0px;
}
pre {
  color: #000020;
  background-color: #F0F0F0;
  line-height: 1.2;
  padding: 10px;
  border: 1px solid #E0E0E0;
  border-radius: 4px;
  overflow: auto;
}
code {
  color: #000020;
  background-color: #F0F0F0;
  padding-left: 5px;
  padding-right: 5px;
}
b {
  color: #586068;
}
i {
  color: #004080;
}
a {
  color: #4080C0;
  text-decoration: none;
}
";
        
    }
}