#! "netcoreapp2.0"
#r "nuget:NetStandard.Library,2.0.0"
// NuGet packages are referenced directly in code, no need for script_packages.config.
#r "nuget: Newtonsoft.Json, 10.0.3"

using Newtonsoft.Json; 

var fooBar = new Foo{ Bar = "Hello World!"}; 

Console.WriteLine(JsonConvert.SerializeObject(fooBar)); 

private class Foo 
{
    public string Bar {get; set;}
}