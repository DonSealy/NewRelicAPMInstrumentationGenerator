using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NewRelicAPMInstrumentationGenerator.UnitTests
{
    [TestClass]
    public class ReflectionNamespaceTests
    {
        ConfigurationModel Config = ConfigurationReader.ReadConfigurationFile();

        [TestMethod]
        public void ExcludeNamespace_MatchTest()
        {
            Reflection reflection = new Reflection(Config);

            string configuredNamespace = Config.namespacePrefix;

            Assert.IsFalse(reflection.ExcludeNamespace(configuredNamespace));
        }

        [TestMethod]
        public void ExcludeNamespace_MatchCaseInsensitveTest()
        {
            Reflection reflection = new Reflection(Config);

            string configuredNamespace = Config.namespacePrefix;

            Assert.IsFalse(reflection.ExcludeNamespace(configuredNamespace.ToUpper()));
        }

        [TestMethod]
        public void ExcludeNamespace_NonMatchTest()
        {
            Reflection reflection = new Reflection(Config);

            string configuredNamespace = Config.namespacePrefix;

            Assert.IsTrue(reflection.ExcludeNamespace($"Not{configuredNamespace}"));
        }

        [TestMethod]
        public void ExcludeNamespace_NullTest()
        {
            Reflection reflection = new Reflection(Config);

            string configuredNamespace = Config.namespacePrefix;

            Assert.IsTrue(reflection.ExcludeNamespace(null));
        }

    }
}
