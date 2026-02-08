using CleanArchitecture.PracticalTest.API.Configurations;
using CleanArchitecture.PracticalTest.API.ErrorHandling;
using CleanArchitecture.PracticalTest.API.Middlewares;
using CleanArchitecture.PracticalTest.Application;
using CleanArchitecture.PracticalTest.Application.Abstractions.Persistence;
using CleanArchitecture.PracticalTest.Application.Contracts.ContextApplication;
using CleanArchitecture.PracticalTest.Infrastructure;
using CleanArchitecture.PracticalTest.Infrastructure.Data;
using CleanArchitecture.PracticalTest.Infrastructure.Localization;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Sobreescribe la configuración del archivo appsettings.json con las variables de entorno
    // Esto permite que las configuraciones se puedan modificar sin necesidad de recompilar la aplicación.
    // builder.Configuration
    //     .SetBasePath(Directory.GetCurrentDirectory())
    //     .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    //     .AddEnvironmentVariables();

    // Inicialización y configuración de Serilog
    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .CreateLogger();

    #region Common Setup API
    // Configura el servicio de health check para verificar el estado de la aplicación
    builder.Services.AddHealthChecks()
        .AddNpgSql(
            connectionString: builder.Configuration.GetConnectionString("ContextDb")!,
            name: "postgresql",
            timeout: TimeSpan.FromSeconds(5),
            tags: ["db", "postgresql"]
        );

    // Configura el host para usar Serilog como el proveedor de registro de eventos
    builder.Host.UseSerilog();

    // Configura los controladores personalizados para la aplicación
    builder.Services.AddCustomControllers();

    // Habilita y configura el versionado de la API
    builder.Services.AddCustomApiVersioning();

    // Agrega y configura Swagger para la documentación de la API,
    // incluyendo esquemas de seguridad y soporte para múltiples versiones.
    builder.Services.AddCustomSwagger()
        .ConfigureOptions<ConfigureSwaggerOptions>();

    // Configura las políticas de CORS personalizadas para controlar 
    // el acceso desde orígenes externos.
    builder.Services.AddCustomCors();

    // Agrega un servicio para acceder al contexto HTTP actual en cualquier parte de la aplicación
    builder.Services.AddHttpContextAccessor();

    // Registra el middleware global para manejar excepciones no controladas en la aplicación
    builder.Services.AddTransient<IExceptionMapper, DefaultExceptionMapper>();
    // builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();
    #endregion

    #region Services Registration
    // Registra los servicios de infraestructura, como acceso a datos, repositorios y otros servicios relacionados
    builder.Services.AddInfrastructureServices(builder.Configuration);

    // Registra los servicios de la capa de aplicación, como casos de uso, validaciones y lógica de negocio
    builder.Services.AddApplicationServices();

    builder.Services.AddScoped<IContextDb, ContextDbAdapter>();

    builder.Services.AddScoped<ILocalizer, Localizer>();

    #endregion

    #region Application Pipeline

    var app = builder.Build();

    // Configura el ambiente actual de la aplicación
    Console.WriteLine($"Ambiente actual: {builder.Environment.EnvironmentName}");

    // Obtiene el proveedor de descripciones de versiones de la API para configurar Swagger
    var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

    // Habilita el registro de solicitudes HTTP con Serilog
    app.UseSerilogRequestLogging();

    // Configura Swagger y la interfaz de usuario de Swagger en entornos que no sean de producción
    if (!app.Environment.IsProduction())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            // Ocultar la seccion de Shcema en la UI de Swagger
            options.HeadContent = "<style>.swagger-ui .models { display: none !important; }</style>";

            // Agrega un endpoint de Swagger para cada versión de la API disponible
            foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions.Reverse())
            {
                options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                    description.GroupName.ToUpperInvariant());
            }
        });
    }

    // Configura HSTS (HTTP Strict Transport Security) en entornos que no sean de desarrollo
    if (!app.Environment.IsDevelopment())
    {
        app.UseHsts();
    }
    else
    {
        // Habilita la página de excepciones del desarrollador en entornos de desarrollo
        app.UseDeveloperExceptionPage();
    }

    // Configura las políticas de CORS para permitir solicitudes desde orígenes específicos
    app.UseCors("CorsPolicy");

    // Redirige automáticamente las solicitudes HTTP a HTTPS para garantizar una conexión segura
    app.UseHttpsRedirection();

    // Habilita la autenticación en la aplicación
    app.UseAuthentication();

    // Habilita la autorización en la aplicación
    app.UseAuthorization();

    // Agrega un middleware global para manejar excepciones no controladas
    app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

    // Configura la localización de solicitudes en la aplicación
    app.UseRequestLocalization();

    // Mapea los endpoints de health checks para verificar el estado de la aplicación
    app.MapHealthChecks("/health", new HealthCheckOptions
    {
        Predicate = _ => true,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

    // Mapea las rutas HTTP a los controladores definidos en la aplicación.
    app.MapControllers();

    // Crea un alcance de servicio para realizar tareas iniciales
    using var scope = app.Services.CreateScope();

    // Obtiene los servicios necesarios para realizar migraciones y sembrar datos iniciales
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ContextDb>();

    // Aplica migraciones pendientes a la base de datos
    //await context.Database.MigrateAsync();

    // Inicia la aplicación
    app.Run();

    #endregion
}
catch (Exception ex)
{
    // if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
    // {
    Log.Fatal(ex, "Host terminated unexpectedly");
    // }
}
finally
{
    Log.CloseAndFlush();
}