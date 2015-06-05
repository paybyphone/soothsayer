using NUnit.Framework;
using soothsayer.Infrastructure;
using soothsayer.Scanners;

namespace soothsayer.Tests.Scanners
{
    [TestFixture]
    public class FilePatternTests
    {
        [Test]
        public void environment_pattern_matches_environment_targetted_script()
        {
            var pattern = FilePattern.ForEnvironment("yaks");

            Assert.That("somescript.yaks.sql".Matches(pattern));
            Assert.That("somescript.geese.sql".Matches(pattern), Is.False);
        }

        [Test]
        public void environment_pattern_does_not_match_no_environment_script()
        {
            var pattern = FilePattern.ForEnvironment("cheese");

            Assert.That("somescript.sql".Matches(pattern), Is.False);
        }

        [Test]
        public void no_environment_pattern_matches_script_files_without_environment()
        {
            var pattern = FilePattern.NoEnvironment;

            Assert.That("somescript.sql".Matches(pattern));
        }

        [Test]
        public void no_environment_pattern_does_not_match_script_files_with_environment()
        {
            var pattern = FilePattern.NoEnvironment;

            Assert.That("somescript.cactus.sql".Matches(pattern), Is.False);
        }
    }
}