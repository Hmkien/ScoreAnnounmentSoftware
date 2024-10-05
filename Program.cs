using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ScoreAnnouncementSoftware.Data;
using ScoreAnnouncementSoftware.Models.Entities;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("ApplicationDbContext") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContext' not found.")));
builder.Services.AddSession();
// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    var examTypes = builder.Configuration.GetSection("Examtype").Get<List<ExamType>>();

    if (!context.ExamType.Any())
    {
        context.ExamType.AddRange(examTypes);
        context.SaveChanges();
    }
    else
    {
        var existingExamTypes = await context.ExamType.ToListAsync();

        context.ExamType.RemoveRange(existingExamTypes);
        context.ExamType.AddRange(examTypes);
        context.SaveChanges();
    }
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=WhoareYou}/{action=Index}/{id?}");

app.Run();
