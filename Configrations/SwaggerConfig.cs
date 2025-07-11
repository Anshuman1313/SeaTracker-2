//namespace Assiginment.Configrations
//{
//    using Microsoft.OpenApi.Models;
//    public static class SwaggerConfig
//    {
//        public static void AddSwaggerWithJwt(IServiceCollection services) 
//        {
//            services.AddSwaggerGen(c =>
//            {
//                c.SwaggerDoc("v1", new OpenApiInfo
//                {
//                    Version = "v1",
//                    Title = "Attendence Api"
//                });

//                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//                {
//                    Name = "Authorization",
//                    Type = SecuritySchemeType.Http,
//                    Scheme = "Bearer",
//                    BearerFormat = "JWT",
//                    In = ParameterLocation.Header,
//                    Description = "Please Enter 'Bearer' followed by your JWT Token\n  Example: Bearer 12345456JHIHIH......"
//                });

//                //c.AddSecurityRequirement(new OpenApiSecurityRequirement
//                //{

//                //});
//            }
//        }
//    }
//}
