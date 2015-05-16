# omt - Overview #

omt is a command-line tool for applying a set of organised scripts to a target Oracle database. It can be used as a mechanism for incremental migrations and database version tracking.

## Script Structure ##
Oracle scripts should be organised into one of four folders within the input folder:
* `init` - Initialisation scripts, for creating schemas, tablespaces etc. Generally all the steps that are required to provide a space in which to create everything else (including the versioning tables used by _omt_);
* `up` - Roll-forward scripts;
* `down` - Roll-back scripts — while not strictly required, it is generally recommended that there should be a roll-back script for every roll-forward script to allow for migrating back and forth to any database version;
* `term` - Termination scripts, for cleaning up and destroying the initial tablespaces and schemas created in the Initialisation scripts.

## init/up ##
Scripts contained in the `init` folder will only be executed if the schema (as given in the `-s/--schema` argument) cannot be detected. They are run before any of the up scripts, and are executed in alphabetical order.

Roll-forward scripts contained in the `up` folder will be executed after any init scripts (if no matching schema was detected) or will be executed if the script version number is higher than the current version of the database.

Scripts which are used for `init` or `up` should be named using the following convention:

```
<numerical version>_<description>[.<environment>].sql
```

To minimise numbering conflicts when working within a team, it is recommended to use the current date and 24-hour time as your version number in the format of YYYYMMddHHmm, e.g. 201502242211.

## down/term ##
Roll-back scripts contained in the `down` folder are used when running omt in the down migration mode (using the `-d/--down` switch). They are run before any of the `term` scripts, and are executed in _reverse_ alphabetical order.

It is a general recommendation that a roll-back script be created for any corresponding roll-forward script. This makes it much easier to undo any unwanted changes. _omt_ will display a warning for any roll-forward scripts which do not appear to have a corresponding roll-back script.

Scripts contained in the `term` folder will only be executed after the down scripts have finished executing. They are executed in _reverse_ alphabetical order, and should perform any clean-up duties (deleting schema users, tablespaces etc.) which need to be performed to remove the database entirely from Oracle.

Scripts which are used for down or term should be named using the following convention:
```
RB_<numerical version>_<description>[.<environment>].sql
```

While not strictly necessary, it is usually a good idea to use the same script name as the roll-forward script, and append the `RB_` prefix to the front.

__Note__: If a target version is specified using the `-v/--version` argument, then the termination scripts will not be run.

## Versioning ##
_omt_ relies on a versions table to keep track of which scripts have been or should be executed to migrate a database. The versions table is intended to be created within the same schema/tablespace as the database being migrated (for ease of tracking), and has the following structure:

```
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

Version numbers are parsed from the beginning of the script file name (before the first underscore `_`), so it is important to maintain the proper script structure.

## Per-Environment Scripts ##
Scripts which should only be executed against particular environments are supported by added the environment name to the end of the script name, e.g. `script.prod.sql`, `script.qa.sql`.
When executing _omt_, the environment can be specified through the `-e/--environment` argument. Only scripts which either do not specify an environment, or match the specified environment will be executed as part of the migration.

By default, the environment is set to `qa`.

## Usage ##

###Supported commands:###
	 
	list                  Display a list of existing versions.
	migrate               Run a database migration.
  
###list###

	-c, --connection    Required. The data source connection string for
						connecting to the target Oracle instance.
	-s, --schema        Required. The oracle schema in which the version tables
						reside. Most likely the same schema as the tables being
						migrated.
	-u, --username      Required. The username to use to connect to target Oracle
						instance.
	-p, --password      The password for connecting to the target Oracle
						instance. If not provided in the commandline then you
						will be prompted to enter it in.
	--help              Display this help screen.

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
	-e, --environment    (Default: qa) The environment of the target Oracle
						 instance. This enables running of environment specific
						 scripts.
	-v, --version        The target database version to migrate up (or down) to.
						 Migration will stop if the next script will bring the
						 database to a higher version than specified here (or
						 lower in the case of roll-backs).
	-y, --noprompt       (Default: False) The target database version to migrate
						 up (or down) to. Migration will stop if the next script
						 will bring the database to a higher version than
						 specified here (or lower in the case of roll-backs).
	--force              (Default: False) Tells omt to ignore any errors from
						 executing scripts within SQL*Plus and continue execution
						 of all the scripts.
	--help               Display this help screen.
