# Templates

### Modules

The templates are grouped into modules, which are subfolders of the template folder.
The templates belonging to the currently active module are executed.
The currently active module is defined in [settings](./07-Settings) like this:
```
module = basicmysql 
```

### Templates from _basicmysql_ module

**[upgrade](https://github.com/mikesoloviev/2man-tools/blob/master/2mantools/2mantools/basicmysql/upgrade.sut)**

Adds the Angular Material and MySQL libraries to the project.

**[seed](https://github.com/mikesoloviev/2man-tools/blob/master/2mantools/2mantools/basicmysql/seed.txt)**

Creates the database service and an example of seed SQL script file.

**[migrate](https://github.com/mikesoloviev/2man-tools/blob/master/2mantools/2mantools/basicmysql/migrate.txt)**

Runs the SQL migration script on the development database and maps the database tables to the Entity Framework classes.

**[welcome](https://github.com/mikesoloviev/2man-tools/blob/master/2mantools/2mantools/basicmysql/welcome.txt)**

Creates an example of minimalist web app: the home page, the web site navigation component, and few other elements to simplify the client-server interaction.

**[deploy](https://github.com/mikesoloviev/2man-tools/blob/master/2mantools/2mantools/basicmysql/deploy.txt)**

Runs the SQL migration script on the production database.

**[complete](https://github.com/mikesoloviev/2man-tools/blob/master/2mantools/2mantools/basicmysql/complete.txt)**

Runs the following templates in sequence: _upgrade_, _seed_, _migrate_, _example_.