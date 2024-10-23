using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;

namespace RPSSL.Api.Models;

internal sealed class ErrorObjectResult : ObjectResult
{
    private static readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
    {
        ContractResolver = new CamelCasePropertyNamesContractResolver()
    };

    public ErrorObjectResult(object? value) : base(value)
    { }

    public ErrorObjectResult(object? value, int? statusCode) : base(value)
    {
        StatusCode = statusCode;
    }

    public override async Task ExecuteResultAsync(ActionContext context)
    {
        var response = context.HttpContext.Response;
        
        response.ContentType = "application/json";
        response.StatusCode = StatusCode ?? StatusCodes.Status500InternalServerError;

        var serializedObject = JsonConvert.SerializeObject(Value, _serializerSettings);

        await response.WriteAsync(serializedObject);
    }
}
