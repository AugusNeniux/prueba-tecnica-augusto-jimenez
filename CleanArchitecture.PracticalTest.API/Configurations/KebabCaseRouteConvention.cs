using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace CleanArchitecture.PracticalTest.API.Configurations;

public class KebabCaseRouteConvention : IApplicationModelConvention
{
    public void Apply(ApplicationModel application)
    {
        foreach (var controller in application.Controllers)
        {
            var controllerName = ToKebabCase(controller.ControllerName);

            foreach (var selector in controller.Selectors)
            {

                if (selector.AttributeRouteModel == null)
                {
                    selector.AttributeRouteModel = new AttributeRouteModel
                    {
                        Template = $"/api/v{{version:apiVersion}}/{controllerName}"
                    };
                }
                else
                {
                    selector.AttributeRouteModel.Template = selector.AttributeRouteModel.Template!
                        .Replace("[controller]", controllerName);
                }
            }
        }
    }

    private static string ToKebabCase(string input)
    {
        return Regex.Replace(input, "(?<!^)([A-Z])", "-$1").ToLowerInvariant();
    }
}
