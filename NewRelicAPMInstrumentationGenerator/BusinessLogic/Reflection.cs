using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;

namespace NewRelicAPMInstrumentationGenerator
{
    public class Reflection
    {
        private readonly ConfigurationModel Config;

        public Reflection(ConfigurationModel config)
        {
            DataList = new List<ClassDataModel>();
            ProcessedAssemblies = new List<string>();

            Config = config;
        }

        public string NamespacePrefix => Config.namespacePrefix;
        public ImmutableList<string> MethodFilterMatchList => ImmutableList.Create(Config.methodFilterMatchList.ToArray());
        public ImmutableList<string> MethodFilterStartsWithList => ImmutableList.Create(Config.methodFilterStartsWithList.ToArray());
        public ImmutableList<string> MethodFilterEndsWithList => ImmutableList.Create(Config.methodFilterEndsWithList.ToArray());
        public ImmutableList<string> ClassFilterMatchList => ImmutableList.Create(Config.classFilterMatchList.ToArray());
        public ImmutableList<string> ClassFilterStartsWithList => ImmutableList.Create(Config.classFilterStartsWithList.ToArray());
        public ImmutableList<string> ClassFilterEndsWithList => ImmutableList.Create(Config.classFilterEndsWithList.ToArray());
        public ImmutableList<string> TransactionConstructorList => ImmutableList.Create(Config.transactionConstructorList.ToArray());
        public ImmutableList<string> TransactionParameterTypes => ImmutableList.Create(Config.transactionParameterTypes.ToArray());
        public bool ExcludeTraceMethods => Config.excludeTraces;
        public List<string> ProcessedAssemblies { get; private set; }
        public List<ClassDataModel> DataList { get; private set; }

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
                    ProcessAssemblyFile($"{source.Location}\\{assembly.Name}.dll");
                }
            }
        }

        private void AddClassConstructorMethods(Type type, ClassDataModel data)
        {
            var ctor = new MethodDataModel
            {
                Name = $".ctor",
                IsTransaction = TransactionConstructorList?.Contains(type.Name) ?? false,
                MetricLabel = $"{type.Name}_{type.Name}"
            };

            if (ExcludeTraceMethods == false || ctor.IsTransaction)
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

            if (ExcludeTraceMethods == false || newMethod.IsTransaction)
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
                if (TransactionParameterTypes.Contains(parameterType) || TransactionParameterTypes.Contains(parameterType.TrimEnd('&')))
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

        public bool ExcludeNamespace(string classNamespace)
        {
            return !classNamespace?.ToUpper().StartsWith(NamespacePrefix.ToUpper()) ?? true;
        }

        public bool ExcludeClass(string className)
        {
            if (string.IsNullOrWhiteSpace(className))
                return true;

            int count = ClassFilterMatchList.Count(s => className.ToUpper() == s.ToUpper());
            count += ClassFilterStartsWithList.Count(s => className.ToUpper().StartsWith(s.ToUpper()));
            count += ClassFilterEndsWithList.Count(s => className.ToUpper().EndsWith(s.ToUpper()));

            return count > 0;
        }

        public bool ExcludeMethod(string methodName, List<MethodDataModel> dataMethodsList)
        {
            if (string.IsNullOrWhiteSpace(methodName))
                return true;

            int count = MethodFilterMatchList.Count(s => methodName.ToUpper() == s.ToUpper());
            count += MethodFilterStartsWithList.Count(s => methodName.ToUpper().StartsWith(s));
            count += MethodFilterEndsWithList.Count(s => methodName.ToUpper().EndsWith(s));

            // Exclude overloaded methods.
            if (dataMethodsList != null && dataMethodsList.Count() > 0)
                count += dataMethodsList.Count(s => methodName.ToUpper() == s.Name.ToUpper());

            return count > 0;
        }
    }
}