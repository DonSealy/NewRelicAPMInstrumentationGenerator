using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace NewRelicAPMInstrumentationGenerator.UnitTests
{
    [TestClass]
    public class ReflectionMethodTests
    {
        ConfigurationModel Config = ConfigurationReader.ReadConfigurationFile();

        #region Match Tests

        [TestMethod]
        public void ExcludeMethod_MatchTest()
        {
            Reflection reflection = new Reflection(Config);

            string testMethod = Config.methodFilterMatchList[0];
            var classMethodList = new List<MethodDataModel> {
                new MethodDataModel() { Name = "dataModelMethod", IsTransaction = true, MetricLabel = null }
            };

            Assert.IsTrue(reflection.ExcludeMethod(testMethod, classMethodList));
        }

        [TestMethod]
        public void ExcludeMethod_MatchCaseInsensitveTest()
        {
            Reflection reflection = new Reflection(Config);

            string testMethod = Config.methodFilterMatchList[0];
            var classMethodList = new List<MethodDataModel> {
                new MethodDataModel() { Name = "dataModelMethod", IsTransaction = true, MetricLabel = null }
            };

            Assert.IsTrue(reflection.ExcludeMethod(testMethod.ToUpper(), classMethodList));
        }

        [TestMethod]
        public void ExcludeMethod_NonMatchTest()
        {
            Reflection reflection = new Reflection(Config);

            string testMethod = Config.methodFilterMatchList[0];
            var classMethodList = new List<MethodDataModel> {
                new MethodDataModel() { Name = "dataModelMethod", IsTransaction = true, MetricLabel = null }
            };

            Assert.IsFalse(reflection.ExcludeMethod($"Not{testMethod}", classMethodList));
        }

        [TestMethod]
        public void ExcludeMethod_MatchNullTest()
        {
            Reflection reflection = new Reflection(Config);

            string testMethod = null;
            var classMethodList = new List<MethodDataModel> {
                new MethodDataModel() { Name = "dataModelMethod", IsTransaction = true, MetricLabel = null }
            };

            Assert.IsTrue(reflection.ExcludeMethod(testMethod, classMethodList));
        }

        [TestMethod]
        public void ExcludeMethod_MatchEmptyTest()
        {
            Reflection reflection = new Reflection(Config);

            string testMethod = string.Empty;
            var classMethodList = new List<MethodDataModel> {
                new MethodDataModel() { Name = "dataModelMethod", IsTransaction = true, MetricLabel = null }
            };

            Assert.IsTrue(reflection.ExcludeMethod(testMethod, classMethodList));
        }

        [TestMethod]
        public void ExcludeMethod_MatchWhitespaceTest()
        {
            Reflection reflection = new Reflection(Config);

            string testMethod = "   ";
            var classMethodList = new List<MethodDataModel> {
                new MethodDataModel() { Name = "dataModelMethod", IsTransaction = true, MetricLabel = null }
            };

            Assert.IsTrue(reflection.ExcludeMethod(testMethod, classMethodList));
        }

        #endregion Match Tests

        #region StartsWith Tests

        [TestMethod]
        public void ExcludeMethod_StartsWithTest()
        {
            Reflection reflection = new Reflection(Config);

            string testMethod = $"{Config.methodFilterStartsWithList[0]}TestMethod";
            var classMethodList = new List<MethodDataModel> {
                new MethodDataModel() { Name = "dataModelMethod", IsTransaction = true, MetricLabel = null }
            };

            Assert.IsTrue(reflection.ExcludeMethod(testMethod, classMethodList));
        }

        [TestMethod]
        public void ExcludeMethod_StartsWithCaseInsensitveTest()
        {
            Reflection reflection = new Reflection(Config);

            string testMethod = $"{Config.methodFilterStartsWithList[0]}TestMethod";
            var classMethodList = new List<MethodDataModel> {
                new MethodDataModel() { Name = "dataModelMethod", IsTransaction = true, MetricLabel = null }
            };

            Assert.IsTrue(reflection.ExcludeMethod(testMethod.ToUpper(), classMethodList));
        }

        [TestMethod]
        public void ExcludeMethod_NonStartsWithTest()
        {
            Reflection reflection = new Reflection(Config);

            string testMethod = "ValidMethodName";
            var classMethodList = new List<MethodDataModel> {
                new MethodDataModel() { Name = "dataModelMethod", IsTransaction = true, MetricLabel = null }
            };

            Assert.IsFalse(reflection.ExcludeMethod(testMethod, classMethodList));
        }

        [TestMethod]
        public void ExcludeMethod_StartsWithNullTest()
        {
            Reflection reflection = new Reflection(Config);

            string testMethod = null;
            var classMethodList = new List<MethodDataModel> {
                new MethodDataModel() { Name = "dataModelMethod", IsTransaction = true, MetricLabel = null }
            };

            Assert.IsTrue(reflection.ExcludeMethod(testMethod, classMethodList));
        }

        [TestMethod]
        public void ExcludeMethod_StartsWithEmptyTest()
        {
            Reflection reflection = new Reflection(Config);

            string testMethod = string.Empty;
            var classMethodList = new List<MethodDataModel> {
                new MethodDataModel() { Name = "dataModelMethod", IsTransaction = true, MetricLabel = null }
            };

            Assert.IsTrue(reflection.ExcludeMethod(testMethod, classMethodList));
        }

        [TestMethod]
        public void ExcludeMethod_StartsWithWhitespaceOnlyTest()
        {
            Reflection reflection = new Reflection(Config);

            string testMethod = "   ";
            var classMethodList = new List<MethodDataModel> {
                new MethodDataModel() { Name = "dataModelMethod", IsTransaction = true, MetricLabel = null }
            };

            Assert.IsTrue(reflection.ExcludeMethod(testMethod, classMethodList));
        }

        #endregion StartsWith Tests

        #region EndsWith Tests

        [TestMethod]
        public void ExcludeMethod_EndsWithTest()
        {
            Reflection reflection = new Reflection(Config);

            string testMethod = $"TestMethod{Config.methodFilterEndsWithList[0]}";
            var classMethodList = new List<MethodDataModel> {
                new MethodDataModel() { Name = "dataModelMethod", IsTransaction = true, MetricLabel = null }
            };

            Assert.IsTrue(reflection.ExcludeMethod(testMethod, classMethodList));
        }

        [TestMethod]
        public void ExcludeMethod_EndsWithCaseInsensitveTest()
        {
            Reflection reflection = new Reflection(Config);

            string testMethod = $"TestMethod{Config.methodFilterEndsWithList[0]}";
            var classMethodList = new List<MethodDataModel> {
                new MethodDataModel() { Name = "dataModelMethod", IsTransaction = true, MetricLabel = null }
            };

            Assert.IsTrue(reflection.ExcludeMethod(testMethod.ToUpper(), classMethodList));
        }

        [TestMethod]
        public void ExcludeMethod_NonEndsWithTest()
        {
            Reflection reflection = new Reflection(Config);

            string testMethod = "ValidMethodName";
            var classMethodList = new List<MethodDataModel> {
                new MethodDataModel() { Name = "dataModelMethod", IsTransaction = true, MetricLabel = null }
            };

            Assert.IsFalse(reflection.ExcludeMethod(testMethod, classMethodList));
        }

        [TestMethod]
        public void ExcludeMethod_EndsWithNullTest()
        {
            Reflection reflection = new Reflection(Config);

            string testMethod = null;
            var classMethodList = new List<MethodDataModel> {
                new MethodDataModel() { Name = "dataModelMethod", IsTransaction = true, MetricLabel = null }
            };

            Assert.IsTrue(reflection.ExcludeMethod(testMethod, classMethodList));
        }

        [TestMethod]
        public void ExcludeMethod_EndsWithEmptyTest()
        {
            Reflection reflection = new Reflection(Config);

            string testMethod = string.Empty;
            var classMethodList = new List<MethodDataModel> {
                new MethodDataModel() { Name = "dataModelMethod", IsTransaction = true, MetricLabel = null }
            };

            Assert.IsTrue(reflection.ExcludeMethod(testMethod, classMethodList));
        }

        [TestMethod]
        public void ExcludeMethod_EndsWithWhitespaceOnlyTest()
        {
            Reflection reflection = new Reflection(Config);

            string testMethod = "   ";
            var classMethodList = new List<MethodDataModel> {
                new MethodDataModel() { Name = "dataModelMethod", IsTransaction = true, MetricLabel = null }
            };

            Assert.IsTrue(reflection.ExcludeMethod(testMethod, classMethodList));
        }

        #endregion EndsWith Tests

        #region DataModel Name Tests

        [TestMethod]
        public void ExcludeMethod_MatchClassMethodNameTest()
        {
            Reflection reflection = new Reflection(Config);

            string testMethod = "AMethodName1";
            var classMethodList = new List<MethodDataModel> {
                new MethodDataModel() { Name = "AMethodName1", IsTransaction = true, MetricLabel = null },
                new MethodDataModel() { Name = "AMethodName1", IsTransaction = true, MetricLabel = null },
                new MethodDataModel() { Name = "AMethodName2", IsTransaction = true, MetricLabel = null }
            };

            Assert.IsTrue(reflection.ExcludeMethod(testMethod, classMethodList));
        }

        [TestMethod]
        public void ExcludeMethod_MatchClassMethodNameCaseInsensitiveTest()
        {
            Reflection reflection = new Reflection(Config);

            string testMethod = "AMETHODNAME1";
            var classMethodList = new List<MethodDataModel> {
                new MethodDataModel() { Name = "AMethodName1", IsTransaction = true, MetricLabel = null },
                new MethodDataModel() { Name = "AMethodName1", IsTransaction = true, MetricLabel = null },
                new MethodDataModel() { Name = "AMethodName2", IsTransaction = true, MetricLabel = null }
            };

            Assert.IsTrue(reflection.ExcludeMethod(testMethod, classMethodList));
        }

        [TestMethod]
        public void ExcludeMethod_NonMatchClassMethodNameTest()
        {
            Reflection reflection = new Reflection(Config);

            string testMethod = "AMethodName3";
            var classMethodList = new List<MethodDataModel> {
                new MethodDataModel() { Name = "AMethodName1", IsTransaction = true, MetricLabel = null },
                new MethodDataModel() { Name = "AMethodName1", IsTransaction = true, MetricLabel = null },
                new MethodDataModel() { Name = "AMethodName2", IsTransaction = true, MetricLabel = null }
            };

            Assert.IsFalse(reflection.ExcludeMethod(testMethod, classMethodList));
        }

        [TestMethod]
        public void ExcludeMethod_EmptyMatchClassMethodNameTest()
        {
            Reflection reflection = new Reflection(Config);

            string testMethod = "AMethodName3";
            var classMethodList = new List<MethodDataModel>();

            Assert.IsFalse(reflection.ExcludeMethod(testMethod, classMethodList));
        }

        [TestMethod]
        public void ExcludeMethod_NullMatchClassMethodNameTest()
        {
            Reflection reflection = new Reflection(Config);

            string testMethod = "AMethodName3";
            List<MethodDataModel> classMethodList = null;

            Assert.IsFalse(reflection.ExcludeMethod(testMethod, classMethodList));
        }

        #endregion DataModel Name Tests
    }
}
