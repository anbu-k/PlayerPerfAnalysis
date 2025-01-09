using PlayerPerfAnalysis.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddHttpClient<SportsDataService>();
builder.Services.AddEndpointsApiExplorer();  // Required for Swagger UI
builder.Services.AddSwaggerGen();            // Registers Swagger generator
builder.Services.AddMemoryCache();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.UseSwagger();
app.UseSwaggerUI();


app.Run();
