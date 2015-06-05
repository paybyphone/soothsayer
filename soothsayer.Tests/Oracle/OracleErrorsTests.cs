using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Oracle.ManagedDataAccess.Client;
using soothsayer.Oracle;

namespace soothsayer.Tests.Oracle
{
    [TestFixture]
    public class OracleErrorsTests
    {
        [Test]
        public void can_determine_when_an_oracle_error_occurs_for_a_given_error_message()
        {
            var exception = MakeMeAnOracleExceptionWithMessage("some error");

            Assert.That(exception.IsFor("some error"));
        }

        [Test]
        public void can_determine_when_an_oracle_error_is_not_for_a_given_error_message()
        {
            var exception = MakeMeAnOracleExceptionWithMessage("some error");

            Assert.That(exception.IsFor("some other error"), Is.False);
        }

        private OracleException MakeMeAnOracleExceptionWithMessage(string message)
        {
            var oracleExceptionType = typeof(OracleException);
            var oracleExceptionConstructors = oracleExceptionType.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);

            var constructFromComponents = oracleExceptionConstructors.Single(c => c.GetParameters().First().ParameterType == typeof(int));

            var exception = constructFromComponents.Invoke(new object[] { 1, string.Empty, string.Empty, message });
            return exception as OracleException;
        }
    }
}