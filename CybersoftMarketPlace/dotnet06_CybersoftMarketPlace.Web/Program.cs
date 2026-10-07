using Microsoft.AspNetCore.SignalR.Client;
var builder = WebApplication.CreateBuilder(args);

//DI blazor page service
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
//Di blazor local storage service
builder.Services.AddLocalStorageServices();





//DI http client service`
builder.Services.AddHttpClient("CybersoftMarketplaceApi", client =>
{
    client.BaseAddress = new Uri("http://localhost:1234"); // Thay đổi URL của API nếu cần
});

//DI Service State management
builder.Services.AddScoped<ProductStateService>();
builder.Services.AddScoped<UserStateService>();
builder.Services.AddScoped<CartStateService>();
builder.Services.AddScoped<OrderStateService>();

//DI hubconnection builder
builder.Services.AddSingleton<HubConnectionBuilder>();

var app = builder.Build();




//Cấu hình routing
app.UseRouting();


//Cấu hình blazor hub
app.MapBlazorHub();

//Cấu hình trang chủ mặc định
app.MapFallbackToPage("/_Host");

app.UseStaticFiles();

app.Run();




