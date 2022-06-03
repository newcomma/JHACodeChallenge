// See https://aka.ms/new-console-template for more information
using System.Net.Http.Headers;

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


using var stream = await httpResponse.Content.ReadAsStreamAsync(cts.Token);
using var reader = new StreamReader(stream);
while (!reader.EndOfStream && DateTime.Now < endTime)
{
    var line = reader.ReadLine();
    Console.WriteLine(line);
}

Console.WriteLine("Press 'Enter' to end application");
Console.ReadLine();

