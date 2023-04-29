using System.ComponentModel.DataAnnotations;
using Infrastructure;
using Swashbuckle.AspNetCore.Annotations;

namespace Database;

public class Callback {
    [Key]
    [SwaggerSchema("Unique id of the callback",ReadOnly = true)]
    [SwaggerSchemaExample("f455050d-5d44-41a3-9705-51ec44f4632c")]
    public Guid id {get; set;}
    
    [Required]
    [SwaggerSchema("The URL to call when time expires",ReadOnly = true)]
    [SwaggerSchemaExample("http://10.10.1.7:8123/api/webhook/webhooktester")]
    public required string url {get; set;}

    [SwaggerSchema("When to call the URL.",ReadOnly = true)]
    [SwaggerSchemaExample("2023-05-2T17:14:05.0743026+00:00")]
    public DateTime timeof {get; set;} 

    [SwaggerSchema("Has the callback been done",ReadOnly = true)]
    public bool IsComplete {get; set;}  = false;

    [SwaggerSchema("Did the callback occur successfully",ReadOnly = true)]
    public bool IsError {get; set;}  = false;

    [SwaggerSchema("Json Data to post to URL (Base 64 Encoded)",ReadOnly = true)]
    [SwaggerSchemaExample("eyJzZXJ2aWNlIjoibGlnaHQudG9nZ2xlIiwiZW50aXR5X2lkIjoibGlnaHQuZWRpc29uIn0=")]
    public string json {get; set;} = "";

    [SwaggerSchema("Form Data to post to URL (Base 64 Encoded)",ReadOnly = true)]
    [SwaggerSchemaExample("c2VydmljZT1saWdodC50b2dnbGUmZW50aXR5X2lkPWxpZ2h0LmVkaXNvbg==")]
    public string form {get; set;} = "";

    [SwaggerSchema("Http Method to call the URL",ReadOnly = true)]
    [SwaggerSchemaExample("POST")]
    public string method {get; set;} = "";

    [SwaggerSchema("Http response body to request",ReadOnly = true)]
    public string response {get; set;} = "";

    [SwaggerSchema("How many failures (should be less than Environment Variable MaxFailures",ReadOnly = true)]
    public int failures {get; set;} = 0;

    [SwaggerSchema("Response message for debugging",ReadOnly = true)]
    public string failureMessage {get; set;} = "";

    [SwaggerSchema("approximate current time (for easy reference)",ReadOnly = true)]
    public DateTime approx => DateTime.Now;

    [SwaggerSchema("uses approx to calculate how long until callback is called (in seconds)",ReadOnly = true)]
    public int how_long => (int) (timeof - approx).TotalSeconds;
}
