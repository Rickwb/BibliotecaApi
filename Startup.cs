using BibliotecaApi.Repositories;
using BibliotecaApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft;
using Newtonsoft.Json;

namespace BibliotecaApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
              .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
              .AddNewtonsoftJson(c => c.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            services.AddSingleton<UserRepository>();
            services.AddTransient<UserService>();

            services.AddSingleton<CustomerRepository>();
            services.AddTransient<CustomerService>();

            services.AddSingleton<EmployeeRepository>();
            services.AddTransient<EmployeeService>();

            services.AddSingleton<BookRepository>();
            services.AddTransient<BookService>();

            services.AddSingleton<AuthorRepository>();
            services.AddTransient<AuthorService>();

            services.AddSingleton<ReservationRepository>();
            services.AddTransient<ReservationService>();

            services.AddSingleton<WithdrawRepository>();
            services.AddTransient<WithdrawService>();

            services.AddTransient<TokenService>();

            services.AddTransient<CepService>();

            var key = Encoding.ASCII.GetBytes(Configuration.GetValue<string>("ApiSecret"));

            services.AddAuthentication(options =>
            {
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                };
            });

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AutenticacaoAPI", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Description = @"JWT Authorization header using the Bearer scheme"
                });
                c.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                        {
                              new OpenApiSecurityScheme
                              {
                                  Reference = new OpenApiReference
                                  {
                                      Id = "Bearer",
                                      Type = ReferenceType.SecurityScheme
                                  }
                              },
                             new string[] {}
                        }
                    });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserRepository userRepository, CustomerRepository customerRepository, BookRepository bookRepository, AuthorRepository authorRepository,
            ReservationRepository reservationRepository, WithdrawRepository withdrawRepository)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BibliotecaApi v1"));
            }

            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            userRepository.Add(new Entities.User(
                username: "admin1",
                password: "string",
                role: "admin"
                ));
            var customer = customerRepository.Add(new Entities.Customer(
                id: Guid.NewGuid(),
                name: "Rick",
                document: "string",
                cep: "89110110"));
            var author = authorRepository.Add(new Entities.Authors(
                id: Guid.NewGuid(),
                name: "Machado",
                nacionality: "Brasileira",
                age: 21
                ));
            var book = bookRepository.Add(new Entities.Book(
                id: Guid.NewGuid(),
                author,
                title: "Meditações",
                description:"hahahahha",
                numCopies: 1,
                realeaseYear: 2020));
            var book2 = bookRepository.Add(new Entities.Book(
                id: Guid.NewGuid(),
                author,
                description:"fgsdfgsdfg",
                title: "C# programming",
                numCopies: 1,
                realeaseYear: 2020));
            var reservation = reservationRepository.Add(new Entities.Reservation(
                id: Guid.NewGuid(),
                client: customer,
                startDate: DateTime.Now,
                endDate: DateTime.Now.AddDays(5),
                books: new List<Entities.Book> { book, book2 }
                ));
            var reservation2 = reservationRepository.Add(new Entities.Reservation(
                id: Guid.NewGuid(),
                client: customer,
                startDate: DateTime.Now,
                endDate: DateTime.Now.AddDays(5),
                books: new List<Entities.Book> { book, book2 }
                ));

        }
    }
}
