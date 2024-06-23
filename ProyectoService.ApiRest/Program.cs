using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProyectoService.AccesoDatos;
using ProyectoService.AccesoDatos.EntityFramework;
using ProyectoService.Aplicacion.CasosUso;
using ProyectoService.Aplicacion.ICasosUso;
using ProyectoService.LogicaNegocio.IRepositorios;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<ProyectoServiceContext>(option =>
        {
            option.UseSqlServer(builder.Configuration.GetConnectionString("ProyectoServiceContext"));
        });

        //repositorios 
        builder.Services.AddScoped<IClienteRepositorio, ClienteEFRepositorio>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<ITecnicoRepositorio, TecnicoEFRepositorio>();
        builder.Services.AddScoped<IAdministradorRepositorio, AdministradorEFRepositorio>();
        builder.Services.AddScoped<IReparacionRepositorio, ReparacionEFRepositorio>();
        builder.Services.AddScoped<IEnviarEmail,EnviarEmail>();
        // casos de uso
        builder.Services.AddScoped<IAgregarClienteUC, AgregarClienteUC>();
        builder.Services.AddScoped<IObtenerTodosLosClientesUC, ObtenerTodosLosClientesUC>();
        builder.Services.AddScoped<IObtenerClientePorCI,ObtenerClientePorCi>();
        builder.Services.AddScoped<IObtenerUsuario,ObtenerUsuario>();
        builder.Services.AddScoped<IAgregarTecnico,AgregarTecnico>();
        builder.Services.AddScoped<IObtenerTecnicoPorEmail,ObtenerTecnicoPorEmail>();
        builder.Services.AddScoped<IObtenerTodosLosTecnicos,ObtenerTodosLosTecnicos>();
        builder.Services.AddScoped<IObtenerTecnicoPorId,ObtenerTecnicoPorId>();
        builder.Services.AddScoped<IValidarPassword, ValidarPassword>();
        builder.Services.AddScoped<IAgregarAdministrador,AgregarAdministrador>();
        builder.Services.AddScoped<IObtenerAdministradores,ObtenerAdministradores>();
        builder.Services.AddScoped<IAgregarReparacion, AgregarReparacion>();
        builder.Services.AddScoped<IObtenerTodasLasReparaciones,ObtenerTodasLasReparaciones>();
        builder.Services.AddScoped<IObtenerReparacionesPorCliente,ObtenerReparacionesPorCliente>();
        builder.Services.AddScoped<IObtenerReparacionesPorTecnico, ObtenerReparacionesPorTecnico>();
        builder.Services.AddScoped<IPresupuestarReparacion, PresupuestarReparacion>();
        builder.Services.AddScoped<IObtenerReparacionesPresupuestadas,ObtenerReparacionesPresupuestadas>();
        builder.Services.AddScoped<IObtenerReparacionesPresupuestadasPorCliente,ObtenerReparacionesPresupuestadasPorCliente>();
        builder.Services.AddScoped<IObtenerReparacionesPresupuestadasPorTecnico, ObtenerReparacionesPresupuestadasPorTecnico>();
        builder.Services.AddScoped<IObtenerReparacionesEnTaller,ObtenerReparacionesEnTaller>();
        builder.Services.AddScoped<IObtenerReparacionesEnTallerPorCliente, ObtenerReparacionesEnTallerPorCliente>();
        builder.Services.AddScoped<IObtenerReparacionesEnTallerPorTecnico, ObtenerReparacionesEnTallerPorTecnico>();
        builder.Services.AddScoped<IAvisoNuevaReparacion,AvisoNuevaReparacion>();
        builder.Services.AddScoped<IAvisoNuevoPresupuesto,AvisoNuevoPresupuesto>();
        builder.Services.AddScoped<IAceptarPresupuesto, AceptarPresupuesto>();
        builder.Services.AddScoped<ITerminarReparacion, TerminarReparacion>();
        builder.Services.AddScoped<IEntregarReparacion,EntregarReparacion>();
        builder.Services.AddScoped<INoAceptarPresupuesto, NoAceptarPresupuesto>();
        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        //builder.Services.AddSwaggerGen();
        builder.Services.AddSwaggerGen(opciones =>
        {
            opciones.SwaggerDoc("v1", new OpenApiInfo { Title = "ProyectoService API", Version = "v1" });
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            opciones.IncludeXmlComments(xmlPath);

            opciones.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme()
            {
                Description = "autorizacion std mediante esquema bearer",
                In = ParameterLocation.Header,
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });

            opciones.OperationFilter<SecurityRequirementsOperationFilter>();

        });
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opciones =>
        {
            opciones.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:SecretTokenKey").Value!)),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("nuevaPolitica", app =>
            {
                app.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();

            });
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseCors("nuevaPolitica");
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
