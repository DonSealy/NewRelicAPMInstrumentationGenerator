using System.Collections.Generic;

namespace NewRelicAPMInstrumentationGenerator
{
    public class ClassDataModel
    {
        public string Assembly { get; set; }
        public string ClassName { get; set; }
        public List<MethodDataModel> Methods { get; set; }
    }

    public class MethodDataModel
    {
        public string Name { get; set; }
        public bool IsTransaction { get; set; }
        public string MetricLabel { get; set; }
    }
}