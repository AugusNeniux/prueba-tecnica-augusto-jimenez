using CleanArchitecture.PracticalTest.Application.Contracts.ContextApplication;

namespace CleanArchitecture.PracticalTest.Infrastructure.Localization;

public class Localizer : ILocalizer
{
    private static readonly Dictionary<string, string> Responses = new()
    {
        ["Package.Created"] = "Paquete creado correctamente",
        ["Package.Retrieved"] = "Paquete obtenido correctamente",
        ["Package.StatusUpdated"] = "Estado del paquete actualizado correctamente",
        ["Package.RouteAssigned"] = "Ruta asignada correctamente"
    };

    private static readonly Dictionary<string, string> Validations = new();
    private static readonly Dictionary<string, string> Loggers = new();
    private static readonly Dictionary<string, string> Exceptions = new();
    private static readonly Dictionary<string, string> Concepts = new();
    private static readonly Dictionary<string, string> Enums = new();

    public string GetValidationMessage(string key, params object[] args)
        => Format(Get(Validations, key), args);

    public string GetLoggerMessage(string key, params object[] args)
        => Format(Get(Loggers, key), args);

    public string GetExceptionMessage(string key, params object[] args)
        => Format(Get(Exceptions, key), args);

    public string GetResponseMessage(string key, params object[] args)
        => Format(Get(Responses, key), args);

    public string GetDomainConcept(string key, params object[] args)
        => Format(Get(Concepts, key), args);

    public string GetEnumValue(string key, params object[] args)
        => Format(Get(Enums, key), args);

    private static string Get(Dictionary<string, string> dict, string key)
        => dict.TryGetValue(key, out var value) ? value : key;

    private static string Format(string templateOrKey, object[] args)
    {
        if (args is null || args.Length == 0)
            return templateOrKey;

        try
        {
            return string.Format(templateOrKey, args);
        }
        catch
        {
            return templateOrKey;
        }
    }
}
