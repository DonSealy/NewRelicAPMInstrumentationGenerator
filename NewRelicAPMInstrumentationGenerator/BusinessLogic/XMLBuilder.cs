using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace NewRelicAPMInstrumentationGenerator
{
    public class XMLBuilder
    {
        public string FileName { get; }

        public XMLBuilder(string filename)
        {
            FileName = filename;
        }

        private XDocument BuildXMLFromDataModelEnumerable(IEnumerable<ClassDataModel> data)
        {
            List<XElement> traces = new List<XElement>();

            foreach (var model in data)
            {
                foreach (var method in model.Methods)
                {
                    XElement tracerFactory =
                        new XElement("tracerFactory",
                            new XElement("match",
                                new XAttribute("assemblyName", model.Assembly),
                                new XAttribute("className", model.ClassName),
                                new XElement("exactMethodMatcher",
                                    new XAttribute("methodName", method.Name)
                                )
                            )
                        );

                    if (method.IsTransaction)
                    {
                        tracerFactory.Add(new XAttribute("name", "NewRelic.Agent.Core.Tracer.Factories.BackgroundThreadTracerFactory"));
                    }

                    // Keep this out of the tracerFactory initialization so that it appears after the name attribute if it exists.
                    tracerFactory.Add(new XAttribute("metricName", $"Custom/{method.MetricLabel}"));

                    traces.Add(tracerFactory);
                }
            }

            XNamespace xNamespace = "urn:newrelic-extension";

            XDocument xmlTree = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement(xNamespace + "extension",
                    new XElement("instrumentation",
                        from element in traces
                        select element
                    )
                )
            );

            return xmlTree;
        }

        public void BuildXMLToFile(IEnumerable<ClassDataModel> data)
        {
            BuildXMLFromDataModelEnumerable(data).Save(FileName);
        }
    }
}

// NewRelic Output XML Sample
//
//<? xml version="1.0" encoding="utf-8"?>
//<extension xmlns = "urn:newrelic-extension" >
//  < instrumentation >
//    < !--Define the method which triggers the creation of a transaction. -->
//    <tracerFactory name = "NewRelic.Agent.Core.Tracer.Factories.BackgroundThreadTracerFactory" metricName="Custom/PackRun">
//      <match assemblyName = "MyAssembly" className="MyAssembly.Namespace.ClassName">
//        <exactMethodMatcher methodName = "DoStuff" />
//      </ match >
//    </ tracerFactory >
//    < !--Instrument 0 or more methods called by the trigger method.These methods appear in the transaction breakdown table and in transaction traces. -->
//    <tracerFactory metricName = "Custom/OpenForm" >
//      < match assemblyName= "MyAssembly" className= "MyAssembly.Namespace.ClassName" >
//        < exactMethodMatcher methodName= "OpenForm" />
//      </ match >
//    </ tracerFactory >
//  </ instrumentation >
//</ extension > 