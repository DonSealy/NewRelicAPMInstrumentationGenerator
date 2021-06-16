using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace NewRelicAPMInstrumentationGenerator
{
    public class Reflection
    {
        private readonly string Path;
        private readonly string NamespacePrefix;

        private readonly List<string> methodFilterMatchList;
        private readonly List<string> methodFilterStartsWithList;
        private readonly List<string> methodFilterEndsWithList;
        private readonly List<string> classFilterMatchList;
        private readonly List<string> classFilterStartsWithList;
        private readonly List<string> classFilterEndsWithList;
        private readonly List<string> transactionParameterTypes;
        private readonly List<string> transactionConstructorList;
        private readonly bool excludeTraceMethods;

        public List<string> ProcessedAssemblies { get; private set; }
        public List<ClassDataModel> DataList { get; private set; }

        public Reflection(string path)
        {
            Path = path;
            DataList = new List<ClassDataModel>();
            ProcessedAssemblies = new List<string>();

            var config = ConfigurationReader.ReadConfigurationFile();
            NamespacePrefix = config.namespacePrefix;

            methodFilterMatchList = config.methodFilterMatchList;
            methodFilterStartsWithList = config.methodFilterStartsWithList;
            methodFilterEndsWithList = config.methodFilterEndsWithList;

            classFilterMatchList = config.classFilterMatchList;
            classFilterStartsWithList = config.classFilterStartsWithList;
            classFilterEndsWithList = config.classFilterEndsWithList;

            transactionConstructorList = config.transactionConstructorList;
            transactionParameterTypes = config.transactionParameterTypes;

            excludeTraceMethods = config.excludeTraces;
        }

        public void ProcessAssemblyFile(string filePath)
        {
            var source = Assembly.LoadFile(filePath);

            BuildAssemblyReferencesPathList(source);

            var types = BuildLoadableTypesList(source);
            foreach (var type in types)
            {
                if (ExcludeClass(type.Name) || ExcludeNamespace(type.Namespace))
                {
                    continue;
                }

                ClassDataModel data = new ClassDataModel
                {
                    Methods = new List<MethodDataModel>(),
                    Assembly = $"{type.Assembly.GetName().Name}",
                    ClassName = $"{type.Namespace}.{type.Name}",
                };

                AddClassConstructorMethods(type, data);

                foreach (var methodInfo in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
                {
                    if (ExcludeMethod(methodInfo.Name, data.Methods))
                    {
                        continue;
                    }

                    AddClassMethod(type, data, methodInfo);
                }

                if (data.Methods.Count > 0)
                {
                    DataList.Add(data);
                }
            }
        }

        private void BuildAssemblyReferencesPathList(Assembly source)
        {
            var assemblies = source.GetReferencedAssemblies();

            foreach (var assembly in assemblies)
            {
                if (assembly.Name.ToUpper().StartsWith(NamespacePrefix) && ProcessedAssemblies.Contains(assembly.Name) == false)
                {
                    ProcessedAssemblies.Add(assembly.Name);
                    ProcessAssemblyFile($"{Path}\\{assembly.Name}.dll");
                }
            }
        }

        private void AddClassConstructorMethods(Type type, ClassDataModel data)
        {
            var ctor = new MethodDataModel
            {
                Name = $".ctor",
                IsTransaction = transactionConstructorList?.Contains(type.Name) ?? false,
                MetricLabel = $"{type.Name}_{type.Name}"
            };

            if (excludeTraceMethods == false || ctor.IsTransaction)
            {
                data.Methods.Add(ctor);
            }
        }

        private void AddClassMethod(Type type, ClassDataModel data, MethodInfo methodInfo)
        {
            var newMethod = new MethodDataModel
            {
                Name = methodInfo.Name,
                IsTransaction = IsMethodTransactionByReflection(methodInfo),
                MetricLabel = $"{type.Name}_{methodInfo.Name}",
            };

            if (excludeTraceMethods == false || newMethod.IsTransaction)
            {
                data.Methods.Add(newMethod);
            }
        }

        private bool IsMethodTransactionByReflection(MethodInfo method)
        {
            var parameters = BuildLoadableParametersList(method);

            if (parameters == null)
            {
                return false;
            }

            foreach (var parameter in parameters)
            {
                var parameterType = parameter.ParameterType.ToString();

                // second condition catches parameter passed by reference.
                if (transactionParameterTypes.Contains(parameterType) || transactionParameterTypes.Contains(parameterType.TrimEnd('&')))
                {
                    return true;
                }
            }

            return false;
        }

        // This is an ugly workaround for a nasty exception that is thrown when the type assembly can't be loaded.
        //   https://stackoverflow.com/questions/7889228/how-to-prevent-reflectiontypeloadexception-when-calling-assembly-gettypes
        private IEnumerable<Type> BuildLoadableTypesList(Assembly source)
        {
            try
            {
                return source.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null);
            }
        }

        private IEnumerable<ParameterInfo> BuildLoadableParametersList(MethodInfo method)
        {
            try
            {
                return method.GetParameters();
            }
            catch (FileNotFoundException)
            {
                return null;
            }
        }

        private bool ExcludeNamespace(string classNamespace)
        {
            if (classNamespace == null)
            {
                return true;
            }

            return !classNamespace.ToUpper().StartsWith(NamespacePrefix);
        }

        private bool ExcludeClass(string className)
        {
            int count = classFilterMatchList.Count(s => className == s);
            count += classFilterStartsWithList.Count(s => className.ToUpper().StartsWith(s));
            count += classFilterEndsWithList.Count(s => className.ToUpper().EndsWith(s));

            return count > 0;
        }

        private bool ExcludeMethod(string methodName, List<MethodDataModel> dataMethodsList)
        {
            int count = methodFilterMatchList.Count(s => methodName == s);
            count += methodFilterStartsWithList.Count(s => methodName.ToUpper().StartsWith(s));
            count += methodFilterEndsWithList.Count(s => methodName.ToUpper().EndsWith(s));
            count += dataMethodsList.Count(s => methodName == s.Name);

            return count > 0;
        }
    }
}