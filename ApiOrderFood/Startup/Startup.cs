using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ApiOrderFood.Startup
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "https://localhost:7161",   
                        ValidAudience = "https://localhost:7161", 
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Th1sIsASup3rS3cretKey")) // Replace with your secret key
                    };
                });

        }

    }
}
