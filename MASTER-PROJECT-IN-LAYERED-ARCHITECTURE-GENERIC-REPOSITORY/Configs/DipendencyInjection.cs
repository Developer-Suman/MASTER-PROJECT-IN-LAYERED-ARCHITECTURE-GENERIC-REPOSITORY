using AutoMapper;
using Microsoft.OpenApi.Models;

namespace MASTER_PROJECT_IN_LAYERED_ARCHITECTURE_GENERIC_REPOSITORY.Configs
{
    public static class DipendencyInjection
    {
        public static void Inject(WebApplicationBuilder builder)
        {
            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(
                option =>
                {
                    option.SwaggerDoc("v1", new OpenApiInfo { Title = "MASTER PROJECT API", Version = "V1" });
                    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Please enter a valid token",
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        BearerFormat = "JWT",
                        Scheme = "Bearer"
                    });
                    option.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type=ReferenceType.SecurityScheme,
                                    Id="Bearer"
                                }
                            },
                            Array.Empty<string>()
                        }
                    });
                }
            );


            //CORS Enable
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                    });
            });


            builder.Services.AddAuthorization();


            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            builder.Services.AddSingleton(mapper);
        }
    }
}
