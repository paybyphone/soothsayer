using NUnit.Framework;
using soothsayer.Infrastructure;

namespace soothsayer.Tests.Infrastructure
{
    [TestFixture]
    public class NameTests
    {
        [Test]
        public void name_of_variable_can_be_read()
        {
            var someValue = Some.Value<int>();
            var propertyName = Name.For(() => someValue);

            Assert.That(propertyName, Is.EqualTo("someValue"));
        }

        [Test]
        public void name_of_member_can_be_read()
        {
            var someObject = new {
                someValue = Some.Value<int>()
            };

            var propertyName = Name.For(() => someObject.someValue);

            Assert.That(propertyName, Is.EqualTo("someValue"));
        }
    }
}

