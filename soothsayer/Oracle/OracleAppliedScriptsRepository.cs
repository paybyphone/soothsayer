using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Oracle.ManagedDataAccess.Client;
using soothsayer.Infrastructure;
using soothsayer.Infrastructure.IO;
using soothsayer.Scripts;

namespace soothsayer.Oracle
{
    public class OracleAppliedScriptsRepository : IAppliedScriptsRepository
    {
        private readonly IDbConnection _connection;

        public OracleAppliedScriptsRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public IEnumerable<IStep> GetAppliedScripts(string schema)
        {
            try
            {
                var appliedScripts = _connection.Query<AppliedScriptDto>(@"select version, v.script_name as scriptname, forward_script as forwardscript, backward_script as backwardscript from {0}.versions v join {0}.appliedscripts ap on v.id = ap.version_id order by version desc".FormatWith(schema));

                return appliedScripts.Select(s => new DatabaseStep(new StoredScript(s.Version, s.ScriptName, s.ForwardScript), new StoredScript(s.Version, "RB_" + s.ScriptName, s.BackwardScript)));
            }
            catch (OracleException oracleException)
            {
                if (oracleException.IsFor(OracleErrors.TableOrViewDoesNotExist))
                {
                    Output.Warn("Applied scripts table in schema '{0}' could not be found.".FormatWith(schema));
                    return Enumerable.Empty<IStep>();
                }

                throw;
            }
        }
        
        public void InsertAppliedScript(DatabaseVersion version, string schema, IScript script, IScript rollbackScript = null)
        {
            var reader = new ScriptReader();

            try
            {
                _connection.Execute(@"INSERT INTO {0}.appliedscripts (id, version_id, forward_script, backward_script)
                                        VALUES ({0}.appliedscripts_seq.nextval, (SELECT id as version_id FROM {0}.versions WHERE version = :version), :forwardScript, :rollbackScript)".FormatWith(schema),
                    new
                    {
                        version = version.Version,
                        forwardScript = string.Join(Environment.NewLine, reader.GetContents(script.Path)),
                        rollbackScript = rollbackScript.IsNull() ? null : string.Join(Environment.NewLine, reader.GetContents(rollbackScript.Path))
                    });
            }
            catch (OracleException oracleException)
            {
                if (oracleException.IsFor(OracleErrors.TableOrViewDoesNotExist))
                {
                    Output.Warn("Applied scripts table in schema '{0}' could not be found, applied script could not be recorded.".FormatWith(schema));
                    return;
                }

                throw;
            }
        }

        public void RemoveAppliedScript(DatabaseVersion version, string schema)
        {
            try
            {
                _connection.Execute(@"DELETE FROM {0}.appliedscripts WHERE version_id = (SELECT id FROM {0}.versions WHERE version = :version)".FormatWith(schema),
                    new
                    {
                        version = version.Version
                    });
            }
            catch (OracleException oracleException)
            {
                if (oracleException.IsFor(OracleErrors.TableOrViewDoesNotExist))
                {
                    Output.Warn("Applied scripts table in schema '{0}' could not be found, applied script could not be recorded.".FormatWith(schema));
                    return;
                }

                throw;
            }
        }

        public bool AppliedScriptsTableExists(string schema)
        {
            string sql = @"select count(*) from all_tables where owner='{0}' and table_name='APPLIEDSCRIPTS'".FormatWith(schema.ToUpper());

            var count = _connection.ExecuteScalar<int>(sql);

            return count > 0;
        }

        public void InitialiseAppliedScriptsTable(string schema, string tablespace = null)
        {
            const string appliedScriptsTableSql = @"create table {0}.appliedscripts
                                                    (
                                                      id NUMBER not null,
                                                      version_id NUMBER not null,
                                                      forward_script CLOB not null,
                                                      backward_script CLOB null,
                                                      CONSTRAINT appliedscripts_pk PRIMARY KEY (id),
                                                      CONSTRAINT fk_version_id FOREIGN KEY (version_id) REFERENCES {0}.versions(id)
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
            const string appliedScriptsTableSequenceSql = @"create sequence {0}.appliedscripts_seq
	                                                    start with 1
	                                                    increment by 1
	                                                    nomaxvalue";

            _connection.Execute(appliedScriptsTableSql.FormatWith(schema, tablespace ?? schema));
            _connection.Execute(appliedScriptsTableSequenceSql.FormatWith(schema));
        }
    }
}