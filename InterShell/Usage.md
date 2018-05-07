# Usage

#### A. By _Visual Studio_

* Select _Tools > 2mantools_ to apply the current template
* To change the template you can modify the _Arguments_ field in the _External Tools_ dialog
* Alternatively you can add several _2mantools_ items to the _Tools_ menu -- each containing its own template -- for example:
  * 2mantools - upgrade
  * 2mantools - migrate

#### B. By _InterShell_

* Install [InterShell](https://github.com/mikesoloviev/intershell/wiki)
* In the _Groups_ tab select _2mantools_ and click _Select_
* In the _Settings_ tab enter the correct paths to _2mantools.exe_ and your _project folder_
* In the _Commands_ tab select the template and click _Execute_

#### C. By itself

* Run _2mantools_ as a command line with arguments indicating: `-a` the template,
and `-p` the path to your project folder (the latter is not needed if you run
_2mantools_ from you project folder) -- for example:

```
C:\Apps\2mantools\2mantools.exe -a upgrade -p C:\Users\Public\Projects\MyWebApp
```