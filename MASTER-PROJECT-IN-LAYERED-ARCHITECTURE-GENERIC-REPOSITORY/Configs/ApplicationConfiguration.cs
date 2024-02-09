﻿using Swashbuckle.AspNetCore.SwaggerUI;

namespace MASTER_PROJECT_IN_LAYERED_ARCHITECTURE_GENERIC_REPOSITORY.Configs
{
    public static class ApplicationConfiguration
    {
        public static void Configure(WebApplication app)
        {

            //Remove from Response Headers
            app.Use((context, next) =>
            {   
                
                context.Response.Headers.Remove("X-Powered-By");
                context.Response.Headers.Remove("Server");
                context.Response.Headers.Add("X-Frame-Options", "DENY");
                return next();
               

            });

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Simple Api");
                    c.DocExpansion(DocExpansion.None);
                });
            }


            app.UseHttpsRedirection();

            app.UseStaticFiles();



            //app.UseRouting
            //UseCors must be placed  after UseRouting and before UseAuthorization.
            //This is to ensure that CORS headers are  included in the response for both authorized and unauthorized calls.

            app.UseCors("AllowAllOrigins");

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();


        }
    }
}
