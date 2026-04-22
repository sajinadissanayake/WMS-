using Microsoft.EntityFrameworkCore;
using WorkforceShiftPortal.Data; // Make sure this matches your AppDbContext namespace

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Add DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Enable session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

// Enable static files (CSS, JS)
app.UseStaticFiles();

// Enable routing
app.UseRouting();

// Enable session before authorization
app.UseSession();

// Authorization middleware (optional, since we’re using custom session-based login)
app.UseAuthorization();

// Map Razor Pages
app.MapRazorPages();

app.Run();
