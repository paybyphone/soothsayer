using System;
using System.Data;
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

        public IScript GetAppliedScript(DatabaseVersion version, string schema)
        {
            throw new NotImplementedException();

            //try
            //{
            //    var appliedScripts = _connection.Query<Script>(@"select version, script_name as scriptname from (select version, script_name from {0}.versions order by version desc) where rownum <= 1".FormatWith(schema));

            //    return appliedScripts.SingleOrDefault();
            //}
            //catch (OracleException oracleException)
            //{
            //    if (oracleException.IsFor(OracleErrors.TableOrViewDoesNotExist))
            //    {
            //        Output.Warn("Applied scripts table in schema '{0}' could not be found.".FormatWith(schema));
            //        return null;
            //    }

            //    throw;
            //}
        }

        public IScript GetRollbackScript(DatabaseVersion version, string schema)
        {
            throw new NotImplementedException();
        }

        public void InsertAppliedScript(DatabaseVersion version, string schema, IScript script, IScript rollbackScript = null)
        {
            var reader = new ScriptReader();

            try
            {
                _connection.Execute(@"INSERT INTO {0}.appliedscripts (id, version_id, forward_script, backward_script)
                                        VALUES ({0}.appliedscripts_seq.nextval, (SELECT version_id FROM {0}.versions WHERE version = :version), :forwardScript, :rollbackScript)".FormatWith(schema),
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