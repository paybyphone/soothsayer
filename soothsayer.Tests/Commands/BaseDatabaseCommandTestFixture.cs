using Moq;
using NUnit.Framework;
using soothsayer.Infrastructure;

namespace soothsayer.Tests.Commands
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
