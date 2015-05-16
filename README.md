# omt - Overview #

omt is a command-line tool for applying a set of organised scripts to a target Oracle database. It can be used as a mechanism for incremental migrations and database version tracking.

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
