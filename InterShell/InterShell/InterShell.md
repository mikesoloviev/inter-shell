# InterShell

A GUI tool for organizing and executing console (or command-line) applications.

It operates a library of commands to run the applications.
The library is a plain text file with minimalistic syntax,
which is easy to read and edit.

These are the main elements of the library.

#### Command

A basic unit of the library. It includes:

* Command name: prefixed with `*`
* Description: prefixed with `-`
* Instructions: no prefix

Normally an instruction is one line of code including the console applications path and 
a list of arguments, but it also can be any line of code allowed in the Windows `.bat` file 
as during execution the instructions are written in a temporary `.bat` file, which then is 
executed by the Windows shell.

#### Group

A collection of commands. It includes:

* Group name: prefixed with `#`
* Description: prefixed with `-`
* Settings: no prefix
* Commands: syntax described above

#### Settings

A collection of parameters common for all commands of the group
(like the path to your data folder), represented as key-value
pairs separated by `=`.

To include the parameter in the command's instructions prefix
it with `$`.

There are two predefined settings:

* `$OUT` - the temporary output file
* `$ERR` - an option to add error output to `$OUT`

The information from the `$OUT` file will be displayed in 
the _Details_ tab (see below) after the command execution.

Usage example:

```
echo test >> $OUT $ERR
```

#### Library

A collection of groups. The library is a text file located in the **InterShell** 
home directory and named _InterShell.dat_.

This is an example of the library file.

```
# mysql-ef-core
- Commands to install and update MySQL EF Core

mysqlsh = "C:/Program Files/MySQL/MySQL Shell 1.0/bin/mysqlsh"
connection-sh = --host=localhost --user=root --password=xxxxxxxx --sql
connection-ef = "server=localhost;user=root;password=xxxxxxxxx;database=my_database;"
data-folder = C:\Users\MyName\MyData
project-folder = C:\Users\MyName\MyProjects\MyWebSite
sql-script = migrate.sql

* install
- Installs EF Core libraries

cd $project-folder
dotnet add package Microsoft.EntityFrameworkCore.Tools >> $OUT $ERR
dotnet add package Pomelo.EntityFrameworkCore.MySql >> $OUT $ERR

* scaffold
- Autogenerates the EF classes from the database tables

cd $project-folder
dotnet ef dbcontext scaffold $connection-ef Pomelo.EntityFrameworkCore.MySql

* migrate
- Runs the data migration SQL script

$mysqlsh $connection-sh --file=$data-folder\$sql-script
```

***

## User Interface

It consists of the following tabs:
**Commands**, **Settings**, **Groups**, **Details**, **Library**, **Guide**.

#### Commands

The list of commands of the current group.

'Execute' button runs the selected command and displays the result the _Details_ tab.

'Details' button displays the full command description in the _Details_ tab.

#### Settings

The list of settings of the current group.
The selected setting can be edited at the panel below the list.

`Update` button saves the changes to the setting value.

#### Groups

The list all groups of the library.

`Select` makes the selected group the current group.

`Details` button displays the full group description in the _Details_ tab.

#### Details

The complete description of the group, or the command, or the result
of the command execution (depends on the user's action).

#### Library

The complete description of the **InterShell** _Library_.
Edit the _Library_ text to add and modify _Commands_, _Groups_, and _Settings_.

`Update` button saves the _Library_ and updates all its elements.

`Export` button save the _Library_ to the file of your choice.

`Import` button load _Library_ from the file of your choice.

#### Guide

A quick start guide to **InterShell**.
