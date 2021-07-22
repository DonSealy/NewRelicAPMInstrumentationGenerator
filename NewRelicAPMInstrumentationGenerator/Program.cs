using System;
using System.IO;

namespace NewRelicAPMInstrumentationGenerator
{
    internal class Program
    {
        private static Reflection reflectionHelper;
        private static XMLBuilder xmlBuilder;

        private static void Main(string[] args)
        {
            if (args[0] == null)
            {
                Console.WriteLine($"No target file given.");
                return;
            }

            var config = ConfigurationReader.ReadConfigurationFile();
            xmlBuilder = new XMLBuilder($"./{config.xmlFile}");

            reflectionHelper = new Reflection(config);
            reflectionHelper.ProcessAssemblyFile(args[0]);

            BuildXMLOutputToFile();
            OuputDataListToStream(config.stdOutFile);
        }

        private static void OuputDataListToStream(string stdOutFile)
        {
            bool writeToFile = !string.IsNullOrEmpty(stdOutFile);

            TextWriter stdOut = Console.Out;
            FileStream fstream = null;
            StreamWriter writer = null;

            if (writeToFile)
            {
                try
                {
                    fstream = new FileStream($"./{stdOutFile}", FileMode.CreateNew, FileAccess.Write);
                    writer = new StreamWriter(fstream);
                    Console.SetOut(writer);
                }
                catch (IOException)
                {
                    fstream = new FileStream($"./{stdOutFile}", FileMode.Truncate, FileAccess.Write);
                    writer = new StreamWriter(fstream);
                    Console.SetOut(writer);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return;
                }
            }

            foreach (var data in reflectionHelper.DataList)
            {
                foreach (var method in data.Methods)
                {
                    Console.WriteLine($"Assembly: {data.Assembly}; ClassName: {data.ClassName}; IsTransaction: {method.IsTransaction}; Method: {method.Name};");
                }
            }

            if (writeToFile)
            {
                writer?.Close();
                fstream?.Close();
            }

            Console.SetOut(stdOut);
            Console.Write("Finished. Press Any Key.");

            if (!Console.IsInputRedirected)
            {
                Console.ReadKey();
            }
        }

        private static void BuildXMLOutputToFile()
        {
            xmlBuilder.BuildXMLToFile(reflectionHelper.DataList);
        }
    }
}