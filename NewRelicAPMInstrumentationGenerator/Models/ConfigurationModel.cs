using System.Collections.Generic;

namespace NewRelicAPMInstrumentationGenerator
{
    public class ConfigurationModel
    {
        public string xmlFile { get; set; }
        public string stdOutFile { get; set; }
        public string namespacePrefix { get; set; }
        public List<string> methodFilterMatchList { get; set; }
        public List<string> methodFilterStartsWithList { get; set; }
        public List<string> methodFilterEndsWithList { get; set; }
        public List<string> classFilterMatchList { get; set; }
        public List<string> classFilterStartsWithList { get; set; }
        public List<string> classFilterEndsWithList { get; set; }
        public List<string> transactionParameterTypes { get; set; }
        public List<string> transactionConstructorList { get; set; }
        public bool excludeTraces { get; set; }
    }
}