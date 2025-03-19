using CompanyManagementSystem.Data;
using CompanyManagementSystem.IServices;
using CompanyManagementSystem.Services;

var builder = WebApplication.CreateBuilder(args);

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy.WithOrigins("http://localhost:3000")  
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Add services to the container.
builder.Services.AddScoped<DbService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable CORS middleware
app.UseCors("AllowLocalhost");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
