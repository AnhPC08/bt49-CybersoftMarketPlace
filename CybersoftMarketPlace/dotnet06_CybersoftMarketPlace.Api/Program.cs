using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();
builder.Services.AddControllers();
//DI Swagger
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    //Viết doc cho swagger api 
    // Nạp file XML chứa chú thích (summary, response...) để hiển thị trên Swagger UI
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (System.IO.File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }


    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API",
        Version = "v1",
        Description = "API documentation for .NET 10"
    });
    // Khai báo scheme Bearer -> tạo nút "Authorize" + ô nhập token trong Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Nhập token JWT vào ô dưới đây"
    });

    // Áp scheme cho toàn bộ endpoint -> hiện icon ổ khóa và tự gắn header Authorization khi gọi API
    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference("Bearer", document),
            new List<string>()
        }
    });
}
);

//Add userid token vào header Authorization khi gọi api
// builder.Services.AddAuthentication(options =>
// {
//     options.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
//     options.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
// }).AddJwtBearer("Bearer", options =>
// {
//     // Cấu hình JWT Bearer ở đây nếu cần
//     options.TokenValidationParameters = new()
//     {
//         ValidateIssuer = true,
//         ValidateAudience = true,
//         ValidateLifetime = true,
//         ValidateIssuerSigningKey = true
//     };
// });





//DI authentication - authorization = jwt
var key = builder.Configuration["Jwt:Key"];           // Khóa bí mật để ký token
var issuer = builder.Configuration["Jwt:Issuer"];     // Issuer (bên phát hành token)
var audience = builder.Configuration["Jwt:Audience"]; // Audience (người nhận token)
// 2. Cấu hình Authentication sử dụng JWT Bearer
builder.Services.AddAuthentication("Bearer").AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {

        ValidateIssuerSigningKey = true, // Xác thực key bí mật của token
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
        ValidateIssuer = true,// Xác thực Issuer 
        ValidIssuer = issuer, // Phải khớp với Issuer trong token
        ValidateAudience = true,    // Xác thực Audience
        ValidAudience = audience, // Phải khớp với Audience trong token
        ValidateLifetime = true, // Xác thực thời gian hết hạn của token
        ClockSkew = TimeSpan.Zero, // Bỏ qua độ trễ thời gian giữa server và client (ngăn lỗi thời gian)
        RoleClaimType = ClaimTypes.Role, // Ánh xạ claim role
        NameClaimType = JwtRegisteredClaimNames.Name, // Ánh xạ claim name qua httpContext.User.Identity.Name
    };

    //Cho phép SignalR gửi token qua query string ?access_token=...
    //(client chạy trong trình duyệt không set được header Authorization trên WebSocket)
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;

            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/cart-hub"))
            {
                context.Token = accessToken;
            }

            return Task.CompletedTask;
        }
    };
});



//DI ef 
//bật proxies

string connectionString = builder.Configuration.GetConnectionString("DBConnectionstring");
builder.Services.AddDbContext<CybersoftMarketPlaceContext>();


//DI Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartItemRepository, CartItemRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IConversationRepository, ConversationRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomer1Repository, Customer1Repository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
builder.Services.AddScoped<IProductImageRepository, ProductImageRepository>();
builder.Services.AddScoped<IProductVariantRepository, ProductVariantRepository>();
builder.Services.AddScoped<IRatingRepository, RatingRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IShopRepository, ShopRepository>();
builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();
//di jwt service
builder.Services.AddScoped<JwtAuthService>();

//DI UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


//DI Service
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();



//Khai cors cho fe : http://localhost:5290
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
    {
        builder.WithOrigins("http://localhost:5290") // Thay đổi URL của FE nếu cần
               .AllowAnyHeader()
               .AllowCredentials()
               .AllowAnyMethod();
    });
});

//DI hub
builder.Services.AddSignalR();


var app = builder.Build();

app.MapControllers();

app.UseCors("AllowSpecificOrigin");



app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    options.RoutePrefix = string.Empty; // Đặt Swagger UI tại root (http://localhost:<port>/)

});

app.UseAuthentication(); //Xác thực (đăng nhập)
app.UseAuthorization(); //Phân quyền
app.UseStaticFiles();
app.UseHttpsRedirection();

//gọi hàm load ws server khi app starts
app.MapHub<CartHub>("/cart-hub");




app.Run();

