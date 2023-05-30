using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Repositories.AccountModRepo;
using Capstone_VotingSystem.Repositories.AuthenRepo;
using Capstone_VotingSystem.Repositories.CampaignRepo;
using Capstone_VotingSystem.Repositories.CampaignStageRepo;
using Capstone_VotingSystem.Repositories.QuestionRepo;
using Capstone_VotingSystem.Repositories.TeacherRepo;
using Capstone_VotingSystem.Repositories.VoteRepo;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

builder.Services.AddScoped<ICampaignRepositories, CampaignRepositories>();
builder.Services.AddScoped<ITeacherRepositories, TeacherRepositories>();
builder.Services.AddScoped<IQuestionRepositories, QuestionRepositories>();
builder.Services.AddScoped<IVoteRepositories, VoteRepositories>();
builder.Services.AddScoped<ICampaignStageRepositories, CampaignStageRepositories>();
builder.Services.AddScoped<IAuthenRepositories, AuthenRepositories>();
builder.Services.AddScoped<IAccountModRepositories, AccountModRepositories>();

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


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
