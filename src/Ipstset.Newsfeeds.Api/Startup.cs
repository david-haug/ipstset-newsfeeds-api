using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Ipstset.Newsfeeds.Api.Auth;
using Ipstset.Newsfeeds.Application.Behaviors;
using Ipstset.Newsfeeds.Application.EventHandling;
using Ipstset.Newsfeeds.Application.EventHandling.Handlers;
using Ipstset.Newsfeeds.Application.Feeds;
using Ipstset.Newsfeeds.Application.Feeds.GetFeed;
using Ipstset.Newsfeeds.Application.Posts;
using Ipstset.Newsfeeds.Application.Posts.CreatePost;
using Ipstset.Newsfeeds.Application.Posts.DeletePostsByFeed;
using Ipstset.Newsfeeds.Application.Users;
using Ipstset.Newsfeeds.Domain.Feeds;
using Ipstset.Newsfeeds.Domain.Posts;
using Ipstset.Newsfeeds.Infrastructure.SqlData;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Ipstset.Newsfeeds.Api
{
    public class Startup
    {
        private string _contentRoot;
        public Startup(IHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
            _contentRoot = env.ContentRootPath;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreatePostValidator>());

            var connString = Configuration["ConnectionStrings:NewsfeedsConnection"];

            #region IdentityServer
            //--IDENTITY SERVER-->
            //todo: real cert
            //var cert = new X509Certificate2("cert_file", "your_cert_password");
            services.AddIdentityServer(options =>
            {
                options.Authentication.CookieLifetime = new TimeSpan(0, 60, 0);
                options.Authentication.CookieSlidingExpiration = true;
            })
                .AddDeveloperSigningCredential()
                .AddInMemoryIdentityResources(AuthConfig.IdentityResources)
                .AddInMemoryClients(AuthConfig.Clients)
                .AddInMemoryApiResources(AuthConfig.Apis)
                .AddProfileService<ProfileService>();

            //IdentityServer dependency injection
            services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>()
                .AddTransient<IProfileService, ProfileService>()
                .AddTransient<IUserStore, UserStore>();

            //configure authentication for API
            //prevents mapping of standard claim types to Microsoft proprietary ones
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication()
                .AddJwtBearer(jwt =>
                {
                    jwt.Authority = Configuration[$"Auth:Authority"];
                    jwt.RequireHttpsMetadata = Convert.ToBoolean(Configuration[$"Auth:RequireHttpsMetadata"]);
                    jwt.Audience = Configuration[$"Auth:ApiName"];
                });

            //services.AddLocalApiAuthentication();
            //--IDENTITY SERVER END-->
            #endregion IdentityServer

            #region Repository injection
            services.AddTransient<IEventDispatcher, EventDispatcher>();
            services.AddTransient<IEventRepository, EventRepository>((ctx) => new EventRepository(connString));

            services.AddTransient<IFeedRepository, FeedRepository>((ctx) =>
            {
                var dispatcher = ctx.GetService<IEventDispatcher>();
                return new FeedRepository(connString, dispatcher);
            });
            services.AddTransient<IFeedReadOnlyRepository, FeedReadOnlyRepository>((ctx) => new FeedReadOnlyRepository(connString));

            services.AddTransient<IPostRepository,PostRepository>((ctx) =>
            {
                var dispatcher = ctx.GetService<IEventDispatcher>();
                return new PostRepository(connString, dispatcher);
            });
            services.AddTransient<IPostReadOnlyRepository, PostReadOnlyRepository>((ctx) => new PostReadOnlyRepository(connString));

            services.AddTransient<IUserReadOnlyRepository, UserReadOnlyRepository>((ctx) => new UserReadOnlyRepository(connString));
            #endregion

            #region Mediatr
            services.AddMediatR(typeof(GetFeedHandler).GetTypeInfo().Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ExceptionHandlingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            #endregion

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient(u => ClaimsService.CreateAppUser(new HttpContextAccessor().HttpContext?.User?.Claims));

            #region EventHandlers
            //event handlers
            //todo: read from assembly?
            services.AddTransient<IEventHandler<FeedDeleted>, FeedDeletedHandler>();
            services.AddTransient<DeletePostsByFeedHandler>();

            #endregion

            //CORS
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

            //RESPONSE JSON FORMATTING
            services.AddMvc(option => option.EnableEndpointRouting = false)
                .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.ContractResolver =
                            new CamelCasePropertyNamesContractResolver();
                        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    }
                );

            #region swagger

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Newsfeeds API", Version = "v1"});
                c.AddSecurityDefinition("Bearer", //Name the security scheme
                    new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme.",
                        Type = SecuritySchemeType.Http, //We set the scheme type to http since we're using bearer authentication
                        Scheme = "bearer" //The name of the HTTP Authorization scheme to be used in the Authorization header. In this case "bearer".
                    });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement{
                    {
                        new OpenApiSecurityScheme{
                            Reference = new OpenApiReference{
                                Id = "Bearer", //The name of the previously defined security scheme.
                                Type = ReferenceType.SecurityScheme
                            }
                        },new List<string>()
                    }
                });

                var filePath = Path.Combine(AppContext.BaseDirectory, "Ipstset.Newsfeeds.Api.xml");
                c.IncludeXmlComments(filePath);
            });

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //needed for Heroku
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedProto
            });

            app.UseSwagger();
            app.UseSwaggerUI(c => 
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Newsfeeds API V1");
                }
            );

            app.UseRouting();
            app.UseCors();
            //app.UseIdentityServer();
            app.UseAuthentication();
            app.UseAuthorization();

            //app.UseHangfireDashboard();
            //app.UseHangfireServer();

            app.Map("/auth", builder =>
            {
                builder.UseIdentityServer();
                builder.UseMvcWithDefaultRoute();
            });


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
