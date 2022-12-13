using System.Net;
using DeliveryBackend.DTO;
using Newtonsoft.Json;

namespace DeliveryBackend.Services.ExceptionHandler;

public class ExceptionsService
{
    
    private readonly RequestDelegate _next;  
    
    public ExceptionsService(RequestDelegate next)    
    {    
        _next = next;    
    }    
    
    public async Task Invoke(HttpContext context)    
    {    
        try  
        {  
            await _next.Invoke(context);  
        }  
        catch (Exception ex)  
        {  
            await HandleExceptionMessageAsync(context, ex).ConfigureAwait(false);  
        }    
    }    
    private static Task HandleExceptionMessageAsync(HttpContext context, Exception exception)  
    {  
        context.Response.ContentType = "application/json";  
        const int statusCode = (int)HttpStatusCode.InternalServerError;  
        var result = JsonConvert.SerializeObject(new Response 
        {  
            Status = statusCode.ToString(),  
            Message = exception.Message  
        });  
        context.Response.ContentType = "application/json";  
        context.Response.StatusCode = statusCode;  
        return context.Response.WriteAsync(result);  
    } 
}