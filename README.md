# soothsayer - Overview #

> __soothsayer__ (noun | so͞oth′sā′ər): one who practises divination — may include fortune-telling, haruspicy, and __providing Oracle wisdom__.

_soothsayer_ is a command-line tool for applying organised _SQL*Plus_ scripts to an Oracle database. It can be used to manage database migrations as well as keep track of database versions.

## Script Structure ##
Scripts should be organised into one of four folders within the input folder:
* `init` - Initialisation scripts — for creating schemas, tablespaces etc. Generally all the steps that are required to provide a space in which to create everything else (including the database versioning tables used by _soothsayer_);
* `up` - Roll-forward scripts;
* `down` - Roll-back scripts — while not strictly required, there should be a roll-back script for every roll-forward script to allow for migrating back and forth to any database version;
* `term` - Termination scripts, for dropping and cleaning up the initial tablespaces and schemas created in the Initialisation scripts.

## init/up ##
Scripts contained in the `init` folder will only be executed if the schema (as given in the `-s/--schema` argument) cannot be detected. They are run before any of the `up` scripts, and are executed in _alphabetical_ order.

Roll-forward scripts contained in the `up` folder will be executed after any `init` scripts (if required), or will be executed if the script version number is higher than the current version of the database.

Scripts which are used for `init` or `up` should use the following naming convention:

```
<numerical version>_<description>[.<environment>].sql
```

To minimise numbering conflicts when working within a team, it is recommended to use the current date and 24-hour time as your version number in the format of `YYYYMMddHHmm`, e.g. `201502242211`.

## down/term ##
Roll-back scripts contained in the `down` folder are used when running soothsayer in the down migration mode (using the `-d/--down` switch). They are run before any of the `term` scripts, and are executed in _reverse alphabetical_ order.

It is a general recommendation that a roll-back script be created for any corresponding roll-forward script. _soothsayer_ will display a warning for any roll-forward scripts it detects which do not appear to have a corresponding roll-back script.

Scripts contained in the `term` folder will only be executed after the `down` scripts have finished executing. They are also executed in _reverse alphabetical_ order, and should perform any clean-up duties (dropping schema users, tablespaces etc.) which need to be performed to remove the database schema entirely from the Oracle instance.

Scripts which are used for `down` or `term` should use the following naming convention:
```
RB_<numerical version>_<description>[.<environment>].sql
```

While not strictly necessary, it is usually a good idea to simply use the same script name as the roll-forward script, and append the `RB_` prefix to the front.

__Note__: If a target version is specified using the `-v/--version` argument, then the termination scripts will not be run.

## Versioning ##
_soothsayer_ relies on a versions table to keep track of which scripts have been or should be executed to migrate a database. The versions table is intended to be created within the same schema/tablespace as the database being migrated (for ease of tracking), and has the following definition:

```PLSQL
create table <schema>.versions
       (
           id NUMBER not null,
           version NUMBER not null,
           script_name VARCHAR2(255) not null,
           applied_date TIMESTAMP(6) not null,
           CONSTRAINT versions_pk PRIMARY KEY (id),
           CONSTRAINT unique_version UNIQUE (version)
       )
```

Version numbers are parsed from the beginning of the script file name (before the first underscore `_`).

## Per-Environment Scripts ##
Scripts which should only be executed against particular environments are supported by added the environment name to the end of the script name, e.g. `script.prod.sql`, `script.dev.sql`.
When executing _soothsayer_, the environment can be specified through the `-e/--environment` argument.

Only scripts which either do not specify an environment, or match the specified environment will be executed as part of the migration. This makes it easy to mark particular scripts (e.g. test data scripts) as only needing to be run in certain environments.

## Stored Applied Scripts ##
As scripts are executed against the database, they are also stored in the target schema. Both the roll forward and roll back scripts for the version migrated are stored (if there is a roll back script).

As well as enabling a trail of exactly what commands have been executed against the database, this also allows for `down` migrations to be performed using the roll back scripts stored in the database (using the `--usestored` switch).

```PLSQL
create table <schema>.appliedscripts
       (
           id NUMBER not null,
           version_id NUMBER not null,
           forward_script CLOB not null,
           backward_script CLOB null,
           CONSTRAINT appliedscripts_pk PRIMARY KEY (id),
           CONSTRAINT fk_version_id FOREIGN KEY (version_id) REFERENCES <schema>.versions(id)
       )
```

## Configuration ##
_soothsayer_ requires a single configuration setting — `RunnerPath` — which should be set to the path of the _SQL*Plus_ executable. The installer for _SQL*Plus_ is made available by Oracle as part of their _Instant Client_ tools.

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <configSections>
    <section name="sqlPlus" type="System.Configuration.NameValueSectionHandler" />
  </configSections>
  <sqlPlus>
    <add key="RunnerPath" value="C:\oracle\client\product\11.2.0\client_1\sqlplus.exe" />
  </sqlPlus>
</configuration>
```
## Usage ##

###Supported commands:###

	list                 Display a list of existing versions.
	migrate              Run a database migration.

###list###

	-c, --connection     Required. The data source connection string for
						 connecting to the target Oracle instance.
	-s, --schema         Required. The oracle schema in which the version tables
						 reside. Most likely the same schema as the tables being
						 migrated.
	-u, --username       Required. The username to use to connect to target Oracle
						 instance.
	-p, --password       The password for connecting to the target Oracle
						 instance. If not provided in the commandline then you
						 will be prompted to enter it in.
	--help               Display this help screen.

###migrate###

    -d, --down           (Default: False) Executes the rollback scripts instead
                         of the forward scripts. By default, migrations are run
                         in roll-forward mode.

	-c, --connection     Required. The data source connection string for
						 connecting to the target Oracle instance.

	-s, --schema         Required. The oracle schema in which the version tables
						 reside. Most likely the same schema as the tables being
						 migrated.

    --tablespace         The oracle tablespace in which the version tables should
						 reside. By default the schema name will be used if this
						 is not specified.

    -u, --username       Required. The username to use to connect to target
						 Oracle instance.

    -p, --password       The password for connecting to the target Oracle
						 instance. If not provided in the commandline then you
						 will be prompted to enter it in.

    -i, --input          Required. The input folder containing both the
						 roll-forward (up) and roll-back (down) sql scripts.

	-e, --environment    The environment of the target Oracle instance. This
						 enables running of environment specific scripts. More
						 than one environment can be specified, separated by a
						 comma.

	-v, --version        The target database version to migrate up (or down) to.
						 Migration will stop if the next script will bring the
						 database to a higher version than specified here (or
						 lower in the case of roll-backs).

	-y, --noprompt       (Default: False) The target database version to migrate
						 up (or down) to. Migration will stop if the next script
						 will bring the database to a higher version than
						 specified here (or lower in the case of roll-backs).

	--concise            Suppresses verbose information (such as SqlPlus output)

    --usestored          (Default: False) Tells soothsayer to ignore the down
                         migration script files and use the stored scripts in the
                         target database schema.

	--force              (Default: False) Tells soothsayer to ignore any errors
						 from executing scripts within SQL*Plus and continue
						 execution of all the scripts.

	--help               Display this help screen.

## Examples ##
### Migrating up to the latest version ###
```
say.exe migrate -c (DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=database.host)(PORT=49161))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=oracle))) -s sample -i ..\..\..\sample\ -e qa -u system -p password -y
```
### Migrating down to a specific version ###
```
say.exe migrate -c (DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=database.host)(PORT=49161))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=oracle))) -s sample -i ..\..\..\sample\ -e qa -u system -p password -d --version 201502251210 -y
```

## Troubleshooting ##
### One of my scripts failed and now everything is ruined and down/term won't even run ###
Try running _soothsayer_ `down` migration using the `--force` switch. This will cause _soothsayer_ to ignore any errors emitted from _SQL*Plus_ and attempt to execute all the scripts. If your termination scripts properly destroy all tablespaces and schemas, then this should bring you back to a blank state.

### soothsayer appears to hang when running a particular script ###
Does your script contain any begin/end block statements? E.g.,

```SQLPlus
declare
  -- some declares
begin
  -- do some things
end;
```

_SQL*Plus_ requires that you terminate the execution of a block with a forward-slash (`/`). E.g.,

``````SQLPlus
declare
  -- some declares
begin
  -- do some things
end;
/
```

If the slash is omitted, then _SQL*Plus_ sits there waiting for the statement to be terminated.

## Legal ##

soothsayer is made available as-is under the _Apache License, Version 2.0_. See the LICENSE file for full license details.

Oracle sqlplus.exe, which is bundled with soothsayer, is made available by Oracle under the _Oracle Technology Network Development and Distribution License Terms for Instant Client_ (http://www.oracle.com/technetwork/licenses/instant-client-lic-152016.html).
