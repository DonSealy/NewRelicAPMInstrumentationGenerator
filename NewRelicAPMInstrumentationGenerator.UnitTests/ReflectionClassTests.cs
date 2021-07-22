using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NewRelicAPMInstrumentationGenerator.UnitTests
{
    [TestClass]
    public class ReflectionClassTests
    {
        ConfigurationModel Config = ConfigurationReader.ReadConfigurationFile();

        #region Match Tests

        [TestMethod]
        public void ExcludeClass_MatchTest()
        {
            Reflection reflection = new Reflection(Config);

            string testClass = Config.classFilterMatchList[0];

            Assert.IsTrue(reflection.ExcludeClass(testClass));
        }

        [TestMethod]
        public void ExcludeClass_MatchCaseInsensitveTest()
        {
            Reflection reflection = new Reflection(Config);

            string testClass = Config.classFilterMatchList[0];

            Assert.IsTrue(reflection.ExcludeClass(testClass.ToUpper()));
        }

        [TestMethod]
        public void ExcludeClass_NonMatchTest()
        {
            Reflection reflection = new Reflection(Config);

            string testClass = Config.classFilterMatchList[0];

            Assert.IsFalse(reflection.ExcludeClass($"Not{testClass}"));
        }

        [TestMethod]
        public void ExcludeClass_MatchNullTest()
        {
            Reflection reflection = new Reflection(Config);

            string testClass = null;

            Assert.IsTrue(reflection.ExcludeClass(testClass));
        }

        [TestMethod]
        public void ExcludeClass_MatchEmptyTest()
        {
            Reflection reflection = new Reflection(Config);

            string testClass = string.Empty;

            Assert.IsTrue(reflection.ExcludeClass(testClass));
        }

        [TestMethod]
        public void ExcludeClass_MatchWhitespaceTest()
        {
            Reflection reflection = new Reflection(Config);

            string testClass = "   ";

            Assert.IsTrue(reflection.ExcludeClass(testClass));
        }

        #endregion Match Tests

        #region StartsWith Tests

        [TestMethod]
        public void ExcludeClass_StartsWithTest()
        {
            Reflection reflection = new Reflection(Config);

            string testClass = $"{Config.classFilterStartsWithList[0]}TestClass";

            Assert.IsTrue(reflection.ExcludeClass(testClass));
        }

        [TestMethod]
        public void ExcludeClass_StartsWithCaseInsensitveTest()
        {
            Reflection reflection = new Reflection(Config);

            string testClass = $"{Config.classFilterStartsWithList[0]}TestClass";

            Assert.IsTrue(reflection.ExcludeClass(testClass.ToUpper()));
        }

        [TestMethod]
        public void ExcludeClass_NonStartsWithTest()
        {
            Reflection reflection = new Reflection(Config);

            string testClass = "JustAClassName";

            Assert.IsFalse(reflection.ExcludeClass(testClass));
        }

        [TestMethod]
        public void ExcludeClass_StartsWithNullTest()
        {
            Reflection reflection = new Reflection(Config);

            string testClass = null;

            Assert.IsTrue(reflection.ExcludeClass(testClass));
        }

        [TestMethod]
        public void ExcludeClass_StartsWithEmptyTest()
        {
            Reflection reflection = new Reflection(Config);

            string testClass = string.Empty;

            Assert.IsTrue(reflection.ExcludeClass(testClass));
        }

        [TestMethod]
        public void ExcludeClass_StartsWithWhitespaceOnlyTest()
        {
            Reflection reflection = new Reflection(Config);

            string testClass = "   ";

            Assert.IsTrue(reflection.ExcludeClass(testClass));
        }

        #endregion StartsWith Tests

        #region EndsWith Tests

        [TestMethod]
        public void ExcludeClass_EndsWithTest()
        {
            Reflection reflection = new Reflection(Config);

            string testClass = $"TestClass{Config.classFilterEndsWithList[0]}";

            Assert.IsTrue(reflection.ExcludeClass(testClass));
        }

        [TestMethod]
        public void ExcludeClass_EndsWithCaseInsensitveTest()
        {
            Reflection reflection = new Reflection(Config);

            string testClass = $"TestClass{Config.classFilterEndsWithList[0]}";

            Assert.IsTrue(reflection.ExcludeClass(testClass.ToUpper()));
        }

        [TestMethod]
        public void ExcludeClass_NonEndsWithTest()
        {
            Reflection reflection = new Reflection(Config);

            string testClass = "JustAClassName";

            Assert.IsFalse(reflection.ExcludeClass(testClass));
        }

        [TestMethod]
        public void ExcludeClass_EndsWithNullTest()
        {
            Reflection reflection = new Reflection(Config);

            string testClass = null;

            Assert.IsTrue(reflection.ExcludeClass(testClass));
        }

        [TestMethod]
        public void ExcludeClass_EndsWithEmptyTest()
        {
            Reflection reflection = new Reflection(Config);

            string testClass = string.Empty;

            Assert.IsTrue(reflection.ExcludeClass(testClass));
        }

        [TestMethod]
        public void ExcludeClass_EndsWithWhitespaceOnlyTest()
        {
            Reflection reflection = new Reflection(Config);

            string testClass = "   ";

            Assert.IsTrue(reflection.ExcludeClass(testClass));
        }

        #endregion EndsWith Tests
    }
}
