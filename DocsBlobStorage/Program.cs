using DocsBlobStorage.Configuration;
using DocsBlobStorage.Services.Abstractions;
using DocsBlobStorage.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.Configure<BlobStorageOptions>(options =>
{
    var configuration = builder.Configuration;
    options.ConnectionString = configuration["CONNECTION_STRING"];
    options.ContainerName = configuration["CONTAINER_NAME"];

    if (string.IsNullOrEmpty(options.ConnectionString))
    {
        throw new ArgumentException("Connection string cannot be null or empty.", nameof(options.ConnectionString));
    }
    if (string.IsNullOrEmpty(options.ContainerName))
    {
        throw new ArgumentException("Container name cannot be null or empty.", nameof(options.ContainerName));
    }
});

builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{    
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
