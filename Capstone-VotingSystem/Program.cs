using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Repositories.AuthenRepo;
using Capstone_VotingSystem.Repositories.CampaignRepo;
using Capstone_VotingSystem.Repositories.CampaignStageRepo;
using Capstone_VotingSystem.Repositories.CandidateRepo;
using Capstone_VotingSystem.Repositories.VoteRepo;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<VotingSystemContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection")));

FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile("Config/fvssystemswp409-firebase-adminsdk-x9pg7-687b1c4ddd.json")
});

builder.Services.AddScoped<ICampaignService, CampaignService>();
builder.Services.AddScoped<IVoteService, VoteService>();
builder.Services.AddScoped<ICampaignStageService, CampaignStageService>();
builder.Services.AddScoped<ICandidateService, CandidateService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidateIssuer = false,
        ValidateAudience = false,
        RequireExpirationTime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtConfig:Key"]))
    };
});

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("AllowOrigin",
        builder =>
        {
            builder
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()
            .WithExposedHeaders(new string[] { "Authorization", "authorization" });
        });
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "VotingSystem", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme.",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
                },
                new string[] {}
                }
                });
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
