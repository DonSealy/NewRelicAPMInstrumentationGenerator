**** New Relic APM Instrumentation File Generator ****

This is a simple command-line utility that will generate an XML instrumentation configuration file for New Relic's .Net APM Agent. It does so by using reflection libraries to collect metafile detatils about a .Net Executable or Assembly. It will also instrument any dependant assemblies found and include those in the instrumentation output.

It is configurable via a configuration.json file that allow you to configure output in a number of ways.  These are relatively specific to the project this was designed to instrument, but are flexible enough to be useful in a variety of scenarios.

** configuration.json reference (example available in repo) **

*xmlFile - filename for the output XML file.
*stdOutFile - filename to store output messages such as success, failure, or progress output for troubleshooting. This is optional, but if it is missing the output will redirect to std out (usually the Console window).

*transactionParameterTypes - This is a json string array of object types that will differentiate a method signature as a transaction entry-point rather than a tracing data-point.

*transactionConstructorList - This is a json string array of class names where the constructor for those classes will be used as a transaction entry-point.

*excludeTraces - Boolean flag. Can be used to exclude tracing data-points from the instrumentation output file if desired. true = exclude.

*namespacePrefix - Specify the namespace of the application to instrument.  This prevents .Net and third-party dependency output in the XML file.

-- Filters (json string arrays) --

Filters arrays provide a simple mechanism for filtering out methods and classes from the instrumentation xml based on specified patterns.  They come in three standardized flavors:  exact string matches, string prefix, and string post filters.

*methodFilterMatchList
*methodFilterStartsWithList
*methodFilterEndsWithList
*classFilterMatchList
*classFilterStartsWithList
*classFilterEndsWithList
