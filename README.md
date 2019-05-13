# QConsole
The control panel of accounts and tables with geodata in DBMS Postgis.

- View active sessions list;
- Manage roles and groups;
- Manage layers and dictionaries with the ability to enable/disable logging;
- Manage of access rights to layers and dictionaries;
- Getting statistics in graphs;
- View logger list with detailed information about changed attributes, operation, username, datetime, etc.


To work, it requires pre-installed tables, functions, and triggers for logging tables.
https://github.com/johnzet39/LoggingPostgreSQL

First of all, the application was created to administer the GIS database and manage its geospatial layers and its users.
This allows a specialist unfamiliar with the concept of dbms to manage the database here and now.
At the moment, the limitation of using this application is that it is to some extent tied to a specific database structure:
- name of schema logger: "logger";
- name of table logger: "logtable";
- dictionaries (non-spatial tables) can be placed in the schema named "schema_spr";