<h1>New Relic APM Instrumentation File Generator</h1>

This is a simple command-line utility that will generate an XML instrumentation configuration file for New Relic's .Net APM Agent. It does so by using reflection libraries to collect metafile detatils about a .Net Executable or Assembly. It will also instrument any dependant assemblies found and include those in the instrumentation output.

It is configurable via a configuration.json file that allow you to configure output in a number of ways.  These are relatively specific to the project this was designed to instrument, but are flexible enough to be useful in a variety of scenarios.

<h2>configuration.json reference (example available in repo)</h2>

<ul>
  <li>xmlFile - filename for the output XML file.</li>
  <li>stdOutFile - filename to store output messages such as success, failure, or progress output for troubleshooting. This is optional, but if it is missing the output will redirect to std out (usually the Console window).</li>
  <li>transactionParameterTypes - This is a json string array of object types that will differentiate a method signature as a transaction entry-point rather than a tracing data-point.</li>
  <li>transactionConstructorList - This is a json string array of class names where the constructor for those classes will be used as a transaction entry-point.</li>
  <li>excludeTraces - Boolean flag. Can be used to exclude tracing data-points from the instrumentation output file if desired. true = exclude.</li>
  <li>namespacePrefix - Specify the namespace of the application to instrument.  This prevents .Net and third-party dependency output in the XML file.</li>
</ul>

<h4>Filters (json string arrays)</h4>

Filters arrays provide a simple mechanism for filtering out methods and classes from the instrumentation xml based on specified patterns.  They come in three standardized flavors:  exact string matches, string prefix, and string post filters.

<ul>
  <li>methodFilterMatchList</li>
  <li>methodFilterStartsWithList</li>
  <li>methodFilterEndsWithList</li>
  <li>classFilterMatchList</li>
  <li>classFilterStartsWithList</li>
  <li>classFilterEndsWithList</li>
</ul>
