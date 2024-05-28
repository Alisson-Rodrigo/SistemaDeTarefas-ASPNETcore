using Microsoft.EntityFrameworkCore;
using Refit;
using SistemaDeTarefas.Integracacao.Interfaces;
using SistemaDeTarefas.Integracacao.Refit;
using SistemaDeTarefas.Integracacao;
using SistemaDeTarefas.Repositorios;
using SistemaDeTarefas.Repositorios.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using SistemaDeTarefas.Data;
using Microsoft.Extensions.Configuration;
using SistemaDeTarefas.Helper.Interfaces;
using SistemaDeTarefas.Helper;

namespace SistemaDeTarefas
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.AddUserSecrets<Program>();
            string keySecret = builder.Configuration["JWT_SECRET"];

            //Configurações de SMPT para envio de email
            string host = builder.Configuration["SMTP:Host"];
            string nome = builder.Configuration["SMTP:Nome"];
            string username = builder.Configuration["SMTP:UserName"];
            string senha = builder.Configuration["SMTP:Senha"];
            int porta = builder.Configuration.GetValue<int>("SMTP:Porta");

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();



            //configurando o swagger e o header de autenticação
            builder.Services.AddSwaggerGen(v =>
            { 
                v.SwaggerDoc("v1", new OpenApiInfo { Title = "SistemaDeTarefas", Version = "v1" });
                var securitySchema = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "JWT Authorization header using the Bearer scheme.",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme,

                    }
                };
                v.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securitySchema);
                v.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { securitySchema, new[] { JwtBearerDefaults.AuthenticationScheme } }
                });
            });

            var provider = builder.Services.BuildServiceProvider();
            var configuration = provider.GetRequiredService<IConfiguration>();


            // Add DbContext Sql server
            //builder.Services.AddDbContext<SistemaDeTarefasDbContext>(item =>
            //item.UseSqlServer(configuration.GetConnectionString("Database")));

            // Add DBContext MySql
            // Add services to the container ---------------------------------------------------------------------
            string mySqlConnection = builder.Configuration.GetConnectionString("Database");
            builder.Services.AddDbContextPool<SistemaDeTarefasDbContext>(options =>
            options.UseMySql(mySqlConnection,
                      ServerVersion.AutoDetect(mySqlConnection)));
            //---------------------------------------------------------------------------------------------------

            builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
            builder.Services.AddScoped<ITarefasRepositorio, TarefaRepositorio>();
            builder.Services.AddScoped<IViaCepIntegracao, ViaCepIntegracao>();
            builder.Services.AddScoped<IDadosMoedasIntegracao, DadosMoedasIntegracao>();
            builder.Services.AddScoped<IEnviarEmailRecuperacao, EnviarEmailRecuperacao>();

            builder.Services.AddRefitClient<IViaCepIntegracaoRefit>().ConfigureHttpClient
                (c => c.BaseAddress = new Uri("https://viacep.com.br"));

            builder.Services.AddHttpClient<IDadosMoedasIntegracao, DadosMoedasIntegracao>(client =>
            {
                client.BaseAddress = new Uri("https://economia.awesomeapi.com.br");
            });

            builder.Services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                }).AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "SistemaDeTarefas",
                        ValidAudience = "SistemaDeTarefas",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keySecret))
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

            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
