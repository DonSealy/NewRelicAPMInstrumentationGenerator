using Newtonsoft.Json;
using System.IO;

namespace NewRelicAPMInstrumentationGenerator
{
    public static class ConfigurationReader
    {
        public static ConfigurationModel ReadConfigurationFile()
        {
            return JsonConvert.DeserializeObject<ConfigurationModel>(File.ReadAllText(@"./configuration.json"));
        }
    }
}