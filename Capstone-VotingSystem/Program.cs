using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Repositories.CampaignRepo;
using Capstone_VotingSystem.Repositories.TeacherRepo;
using Capstone_VotingSystem.Repositories.VoteRepo;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<VotingSystemContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection")));
 
builder.Services.AddScoped<ICampaignRepositories, CampaignRepositories>();
builder.Services.AddScoped<ITeacherRepositories, TeacherRepositories>();
builder.Services.AddScoped<IVoteRepositories, VoteRepositories>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
