using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Services.ActionHistoryService;
using Capstone_VotingSystem.Services.RatioService;
using Capstone_VotingSystem.Services.AuthenticationService;
using Capstone_VotingSystem.Services.CampaignService;
using Capstone_VotingSystem.Services.StageService;
using Capstone_VotingSystem.Services.CandidateService;
using Capstone_VotingSystem.Services.VoteService;
using Capstone_VotingSystem.Services.CategoryService;
using Capstone_VotingSystem.Services.FormService;
using Capstone_VotingSystem.Services.QuestionService;
using Capstone_VotingSystem.Services.TypeService;
using Capstone_VotingSystem.Services.GroupService;
using Capstone_VotingSystem.Services.FeedbackService;
using Capstone_VotingSystem.Services.SearchService;
using Capstone_VotingSystem.Services.ActivityService;
using Capstone_VotingSystem.Services.ScoreService;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Capstone_VotingSystem.Helpers;
using Capstone_VotingSystem.Services.CloudinaryService;
using Capstone_VotingSystem.Services.AccountService;
using Capstone_VotingSystem.Services.NotificationService;
using Capstone_VotingSystem.Services.ActionTypeService;
using Capstone_VotingSystem.Services.UserService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<VotingSystemContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection")));

//Cloudinary connect
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));

//FireBase
FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile("Config/fvssystemswp409-firebase-adminsdk-x9pg7-687b1c4ddd.json")
});

builder.Services.AddScoped<ICampaignService, CampaignService>();
builder.Services.AddScoped<IVoteService, VoteService>();
builder.Services.AddScoped<IStageService, StageService>();
builder.Services.AddScoped<ICandidateService, CandidateService>();
builder.Services.AddScoped<IActionHistoryService, ActionHistoryService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IRatioService, RatioService>();
builder.Services.AddScoped<IFormService, FormService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IActionTypeService, ActiontypeService>();
builder.Services.AddScoped<ITypeService, TypeService>();
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<ISearchService, SearchService>();
builder.Services.AddScoped<IActivityService, ActivityService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IScoreService, ScoreService>();



// Authen
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

//CORS
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("MyCors",
        build =>
        {
            build
            .WithOrigins("*")
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});

//Mapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "VotingSystem", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
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
                        Id ="Bearer"
                }
                },
                new string[] {}
                }
                });
});

builder.Services.AddResponseCompression();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "VotingSystem v1"));

}
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "VotingSystem v1");
    c.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();

app.UseCors("MyCors");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
