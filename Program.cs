using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Controllers;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<IBookService, BookService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// AccountController routes
app.MapControllerRoute(
    name: "account-login",
    pattern: "Account/StudentLogin",
    defaults: new { controller = "Account", action = "StudentLogin" }
);

app.MapControllerRoute(
    name: "account-register",
    pattern: "Account/Register",
    defaults: new { controller = "Account", action = "Register" }
);

app.MapControllerRoute(
    name: "account-admin-login",
    pattern: "Account/AdminLogin",
    defaults: new { controller = "Account", action = "AdminLogin" }
);

// AdminController routes
app.MapControllerRoute(
    name: "admin-portal",
    pattern: "Admin/Portal",
    defaults: new { controller = "Admin", action = "Portal" }
);

// BookController routes
app.MapControllerRoute(
    name: "book-add",
    pattern: "Book/Add/{id?}",
    defaults: new { controller = "Book", action = "Add" }
);

app.MapControllerRoute(
    name: "book-delete",
    pattern: "Book/Delete/{isbn?}",
    defaults: new { controller = "Book", action = "Delete" }
);

app.MapControllerRoute(
    name: "book-rented",
    pattern: "Book/Rented",
    defaults: new { controller = "Book", action = "Rented" }
);

app.MapControllerRoute(
    name: "book-update",
    pattern: "Book/Update/{isbn?}",
    defaults: new { controller = "Book", action = "Update" }
);

// StudentController routes
app.MapControllerRoute(
    name: "student-page",
    pattern: "Student/Page",
    defaults: new { controller = "Student", action = "Page" }
);

app.MapControllerRoute(
    name: "student-returnbook",
    pattern: "Student/ReturnBook",
    defaults: new { controller = "Student", action = "ReturnBook" }
);

// HomeController routes
app.MapControllerRoute(
    name: "home-index",
    pattern: "",
    defaults: new { controller = "Home", action = "Index" }
);

app.MapControllerRoute(
    name: "home-privacy",
    pattern: "Home/Privacy",
    defaults: new { controller = "Home", action = "Privacy" }
);


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate(); // Applies any pending migrations
        SeedAdminUser(context); // Seeds the admin user
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error seeding admin user: {ex.Message}");
    }
}



app.Run();

 static void SeedAdminUser(ApplicationDbContext context)
{
    // Check if an admin user already exists
    if (!context.Users.Any(u => u.Role == "Admin"))
    {
        // Create a new admin user
        var adminUser = new User
        {
            Username = "admin",
            Email = "admin@admin.com",
            Password = AccountController.HashPassword("admin"), // Use your desired admin password
            Role = "Admin"
        };

        context.Users.Add(adminUser);
        context.SaveChanges();
        Console.WriteLine("Admin user created successfully!");
    }
    else
    {
        Console.WriteLine("Admin user already exists.");
    }
}
