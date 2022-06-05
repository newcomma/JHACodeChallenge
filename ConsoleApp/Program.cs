// See https://aka.ms/new-console-template for more information
using System.Net.Http.Headers;
using System.Text;

Console.WriteLine("Console application started");

var endTime = DateTime.Now.AddSeconds(10);


using CancellationTokenSource cts = new();

HttpClient httpClient = new HttpClient();
httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
    scheme: "Bearer",
    parameter: "AAAAAAAAAAAAAAAAAAAAAKkEdQEAAAAABy0zBygPRP5JbigYnzH7L%2BW71R8%3DoxJimqeYNvNZbBJG0A7XN1W11VlafisS6VSDIScxrYT6hKimIa");
using var httpResponse = await httpClient.GetAsync(
    requestUri: "https://api.twitter.com/2/tweets/sample/stream",
    completionOption: HttpCompletionOption.ResponseHeadersRead,
    cancellationToken: cts.Token)
    .ConfigureAwait(false);


List<string> lines = new();

using var stream = await httpResponse.Content.ReadAsStreamAsync(cts.Token);
using var reader = new StreamReader(stream);
int lineNum = 1;
//while (!reader.EndOfStream && DateTime.Now < endTime)
//{
//    var line = reader.ReadLine();
//    if (line is not null)
//    {
//        Console.WriteLine($"({lineNum}) - {line}");
//        lines.Add(line);
//        lineNum++;
//    }
//}

StringBuilder sampleData = new StringBuilder(4000);

var buffer = new char[4000];
while (!reader.EndOfStream && DateTime.Now < endTime)
{

    int received = reader.Read(buffer,0,4000);
    if (received > 0)
    {
        sampleData.Append(buffer, 0, received);
    }
}

string final = sampleData.ToString();
string total = lines.Aggregate((prev,cur)=>prev + cur);

Console.WriteLine("Press 'Enter' to end application");
Console.ReadLine();

