using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using omt.Infrastructure;
using Oracle.ManagedDataAccess.Client;

namespace omt.Oracle
{
    public class OracleVersionRespository : IVersionRespository
    {
        private readonly IDbConnection _connection;

        public OracleVersionRespository(IDbConnection connection)
        {
            _connection = connection;
        }

        public IEnumerable<DatabaseVersion> GetAllVersions(string schema)
        {
            try
            {
                var versions = _connection.Query<DatabaseVersion>(@"select version, script_name as scriptname from {0}.versions order by version asc".FormatWith(schema));

                return versions;
            }
            catch (OracleException oracleException)
            {
                if (oracleException.IsFor(OracleErrors.TableOrViewDoesNotExist))
                {
                    Output.Warn("Version table in schema '{0}' could not be found.".FormatWith(schema));
                    return Enumerable.Empty<DatabaseVersion>();
                }

                throw;
            }
        }

        public DatabaseVersion GetCurrentVersion(string schema)
        {
            try
            {
                var versions = _connection.Query<DatabaseVersion>(@"select version, script_name as scriptname from (select version, script_name from {0}.versions order by version desc) where rownum <= 1".FormatWith(schema));

                return versions.SingleOrDefault();
            }
            catch (OracleException oracleException)
            {
                if (oracleException.IsFor(OracleErrors.TableOrViewDoesNotExist))
                {
                    Output.Warn("Version table in schema '{0}' could not be found.".FormatWith(schema));
                    return null;
                }

                throw;
            }
        }

        public bool ContainsVersion(DatabaseVersion version, string schema)
        {
            try
            {
                var versionNumber = _connection.Query<long>(@"select version from {0}.versions where version = :version".FormatWith(schema), new { version.Version });

                return versionNumber.Any();
            }
            catch (OracleException oracleException)
            {
                if (oracleException.IsFor(OracleErrors.TableOrViewDoesNotExist))
                {
                    Output.Warn("Version table in schema '{0}' could not be found.".FormatWith(schema));
                    return false;
                }

                throw;
            }
        }

        public void InsertVersion(DatabaseVersion version, string schema)
        {
            try
            {
                _connection.Execute(@"INSERT INTO {0}.versions (id, version, script_name, applied_date) VALUES ({0}.versions_seq.nextval, :versionNumber, :scriptName, sysdate)".FormatWith(schema),
                    new
                    {
                        versionNumber = version.Version,
                        scriptName = version.ScriptName
                    });
            }
            catch (OracleException oracleException)
            {
                if (oracleException.IsFor(OracleErrors.TableOrViewDoesNotExist))
                {
                    Output.Warn("Version table in schema '{0}' could not be found, version not recorded.".FormatWith(schema));
                    return;
                }

                throw;
            }
        }

        public void RemoveVersion(DatabaseVersion version, string schema)
        {
            try
            {
                _connection.Execute(@"DELETE FROM {0}.versions WHERE version = :versionNumber".FormatWith(schema),
                    new
                    {
                        versionNumber = version.Version
                    });
            }
            catch (OracleException oracleException)
            {
                if (oracleException.IsFor(OracleErrors.TableOrViewDoesNotExist))
                {
                    Output.Warn("Version table in schema '{0}' could not be found, version not removed.".FormatWith(schema));
                    return;
                }

                throw;
            }
        }

        public void InitialiseVersioningTable(string schema, string tablespace = null)
        {
            const string versionsTableSql = @"create table {0}.versions 
                                                    (
                                                      id NUMBER not null,
                                                      version NUMBER not null,
                                                      script_name VARCHAR2(255) not null,
                                                      applied_date TIMESTAMP(6) not null,
                                                      CONSTRAINT versions_pk PRIMARY KEY (id),
                                                      CONSTRAINT unique_version UNIQUE (version)
                                                    )
                                                    tablespace {1}
                                                      pctfree 10
                                                      initrans 1
                                                      maxtrans 255
                                                      storage
                                                      (
                                                        initial 10M
                                                        next 10M
                                                        minextents 1
                                                        maxextents unlimited
                                                      )";
            const string versionsTableSequenceSql = @"create sequence {0}.versions_seq 
	                                                    start with 1 
	                                                    increment by 1 
	                                                    nomaxvalue";

            _connection.Execute(versionsTableSql.FormatWith(schema, tablespace ?? schema));
            _connection.Execute(versionsTableSequenceSql.FormatWith(schema));
        }
    }
}