# Setup

#### Common step

* Download and build the [2mantools](https://github.com/mikesoloviev/2man-tools) console application

#### A. Setup steps to run _2mantools_ by _Visual Studio_

* Ensure that the paths to the needed database shells are up to date by checking [settings](./07-Settings)
* Add **2mantools** to the _Tools_ menu of _Visual Studio_
  * Open _Tools_ > _External Tools_
  * Click _Add_
  * Fill the form fields
    * Title: _2mantools_
    * Command: (Path to _2mantools.exe_)
    * Arguments: _-a_ `template`
    * Initial directory: _$(ProjectDir)_
  * Check _Use Output window_
  * Click _Apply_ and _OK_

Note: `template` is the name of any available template.

#### B. Setup steps to run _2mantools_ by _InterShell_

* Install [InterShell](https://github.com/mikesoloviev/intershell/wiki)
* In the _Groups_ tab select _2mantools_ and click _Select_
* In the _Settings_ tab enter the correct paths to _2mantools.exe_ and your _project folder_
* In the _Commands_ tab select the template and click _Execute_
