using Moq;
using NUnit.Framework;
using soothsayer.Infrastructure.IO;

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
