// See https://aka.ms/new-console-template for more information
Console.WriteLine("Console application started");

var endTime = DateTime.Now.AddSeconds(10);


using CancellationTokenSource cts = new();

HttpClient httpClient = new HttpClient();
using var httpResponse = await httpClient.GetAsync(
    requestUri: "https://api.twitter.com/2/tweets/sample/stream",
    completionOption: HttpCompletionOption.ResponseHeadersRead,
    cancellationToken: cts.Token)
    .ConfigureAwait(false);


using var stream = await httpResponse.Content.ReadAsStreamAsync(cts.Token);
using var reader = new StreamReader(stream);
while(!reader.EndOfStream && DateTime.Now < endTime)
{
    var line = reader.ReadLine();
    Console.WriteLine(line);
}

Console.WriteLine("Press 'Enter' to end application");
Console.ReadLine();

