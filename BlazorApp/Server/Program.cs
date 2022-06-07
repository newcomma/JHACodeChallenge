using Statistics;
using TweetProcessing.Abstractions;
using TweetProcessing.ApiV2;
using TwitterWorkerService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddTweetStatistics();
builder.Services.AddTweetStreaming();
builder.Services.AddHostedService<Worker>();

builder.Services.Configure<ConcurrencyOptions>(
    builder.Configuration.GetSection(ConcurrencyOptions.Concurrency));
builder.Services.Configure<TwitterOptions>(
    builder.Configuration.GetSection(TwitterOptions.Twitter));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
