using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using soothsayer.Infrastructure;

namespace soothsayer.Tests.Infrastructure
{
    [TestFixture]
    public class CollectionExtensionsTests
    {
        [Test]
        public void when_enumerable_does_not_contain_any_elements_then_it_is_empty()
        {
            var emptyCollection = Enumerable.Empty<string>();

            Assert.That(emptyCollection.IsNullOrEmpty());
        }

        [Test]
        public void when_enumerable_is_null_then_it_is_empty()
        {
            IEnumerable<string> emptyCollection = null;

            Assert.That(emptyCollection.IsNullOrEmpty());
        }

        [Test]
        public void when_enumerable_contains_an_element_then_it_is_not_empty()
        {
            var emptyCollection = new[] { Some.String() };

            Assert.That(emptyCollection.IsNullOrEmpty(), Is.False);
        }
    }
}