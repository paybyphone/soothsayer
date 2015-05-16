using Moq;
using NUnit.Framework;
using omt.Infrastructure;

namespace omt.Tests.Commands
{
    public abstract class BaseDatabaseCommandTestFixture
    {
        protected Mock<ISecureConsole> MockSecureConsole;

        [SetUp]
        public void BaseSetUp()
        {
            MockSecureConsole = new Mock<ISecureConsole>();
        }
    }
}