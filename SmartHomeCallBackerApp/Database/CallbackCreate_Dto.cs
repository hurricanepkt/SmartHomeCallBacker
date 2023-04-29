using System.ComponentModel.DataAnnotations;
using Infrastructure;
using Swashbuckle.AspNetCore.Annotations;

namespace Database;

public class CallbackCreate_Dto {
    
    [Required]
    [SwaggerSchema("The URL to call when time expires")]
    [SwaggerSchemaExample("http://10.10.1.7:8123/api/webhook/webhooktester")]
    public required string url {get; set;}  = "";


    [SwaggerSchema("How many seconds into the future should the service call the URL")]
    [SwaggerSchemaExample("55")]
    public int? secondsFromNow {get; set;} 

    [SwaggerSchema("When to call the URL.")]
    [SwaggerSchemaExample("2023-05-2T17:14:05.0743026+00:00")]
    [SwaggerSchemaDeprecated]
    public string timeof {get; set;} = "";



    [SwaggerSchema("Http Method to call the URL")]
    [SwaggerSchemaExample("POST")]
    public required string method {get; set;} = "POST";


    [SwaggerSchema("Json Data to post to URL (Base 64 Encoded)")]
    [SwaggerSchemaExample("eyJzZXJ2aWNlIjoibGlnaHQudG9nZ2xlIiwiZW50aXR5X2lkIjoibGlnaHQuZWRpc29uIn0=")]
    public string json {get;set; } = "";


    [SwaggerSchema("Form Data to post to URL (Base 64 Encoded) overriden by json")]
    [SwaggerSchemaExample("")]
    public string form {get; set;} = "";

    [SwaggerSchema("Should this remove all not completed with the same Json Data? a.k.a. cleanup Others")]
    [SwaggerSchemaExample("false")]
    public bool cleanupOthers {get; set;} = false;
    
    public static Callback ToDB(CallbackCreate_Dto input) {
        DateTime theTime;
        if (input.secondsFromNow != null) {
            theTime = DateTime.Now.AddSeconds(input.secondsFromNow ?? 0);
        } else {
            try {
            theTime = DateTime.Parse(input.timeof);
            } catch {
                theTime = DateTime.Now;
            }
        }
        return new Callback {
            url = input.url,
            timeof = theTime,
            json  = input.json,
            form = input.form,
            method = input.method
        };
    } 
}