using Oracle.ManagedDataAccess.Client;

namespace omt.Oracle
{
    public static class OracleErrors
    {
        public static readonly string TableOrViewDoesNotExist = @"ORA-00942: table or view does not exist";

        public static bool IsFor(this OracleException oracleException, string oracleError)
        {
            return oracleException.Message.Equals(oracleError);
        }
    }
}

