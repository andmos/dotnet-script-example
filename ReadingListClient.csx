#! "netcoreapp2.0"
#r "nuget:NetStandard.Library,2.0.0"
#r "nuget: Newtonsoft.Json, 10.0.3"
#r "nuget: System.Net.Http, 4.3.3"

using System.Net.Http; 
using Newtonsoft.Json; 
using System.Linq; 
using System.Threading.Tasks;

private string readingListUrl = @"https://app.amosti.net/reading/api/"; 

await PrintReadingListAsync(readingListUrl, await GetReadingListTypeFromArgsAsync(Args, readingListUrl)); 

private async Task<string> GetReadingListTypeFromArgsAsync(IList<string> args, string url)
{
    if(string.IsNullOrEmpty(args.FirstOrDefault())) 
    {
        throw new ArgumentNullException("Arguments can't be empty, choose a readinglist type.");
    }
    if(await ValidateReadingListTypeAsync(args.First(), url))
    {
        return args.First(); 
    } 
    else
    {
        return string.Empty; 
    }
}

private async Task<bool> ValidateReadingListTypeAsync(string listType, string url) 
{
    var availableEndpoints = await GetAllApiEndpointsAsync(url); 
    return availableEndpoints.Any(t => t.Key.ToLower().Contains(listType.ToLower()) && t.Value.Equals("GET")); 
}
private async Task PrintReadingListAsync(string url, string listType)
{
    Console.WriteLine(await GetReadingListAsync(url, listType)); 
}

private async Task<string> GetReadingListAsync(string url, string listType)
{
    using (var client = new HttpClient())
    {
        var respons = await client.GetAsync($"{url}/{listType}"); 
        return await respons.Content.ReadAsStringAsync(); 
    }
}

private async Task<IEnumerable<ApiEndpoint>> GetAllApiEndpointsAsync(string url)
{
    List<ApiEndpoint> apiEndpoints; 
    using(var client = new HttpClient())
    {
        var respons = await client.GetAsync(url); 
        var responseString = await respons.Content.ReadAsStringAsync();
        apiEndpoints = new List<ApiEndpoint>(JsonConvert.DeserializeObject<IEnumerable<ApiEndpoint>>(responseString));
    }
    return apiEndpoints; 
}

private class ApiEndpoint
{
    public string Key {get; private set; }
    public string Value {get; private set;}
    public ApiEndpoint(string key, string value)
    {
        Key = key; 
        Value = value; 
    }
}