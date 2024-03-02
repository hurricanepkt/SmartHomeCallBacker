using Database;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace Handlers;

public class CallbackHandlers
{
    internal static void Setup(RouteGroupBuilder callbacksGroup)
    {
    
        callbacksGroup.MapGet("/", CallbackHandlers.GetAllCallbacks)
            .WithDescription("For debugging purposes only")
            .WithSummary("Dump the Database to JSON");
        callbacksGroup.MapGet("/homeassistant", CallbackHandlers.GetHAformatted)
            .WithDescription("Gives Count and then array of callbacks")
            .WithSummary("Home Assistant Friendly count and info on active callbacks");
        callbacksGroup.MapPost("/", CallbackHandlers.CreateCallback)
            .WithDescription("Post a CallbackCreate_Dto and the service will call it.")
            .WithSummary("Create a Callback");
        callbacksGroup.MapDelete("/{id}", CallbackHandlers.DeleteCallback)
            .WithDescription("Primarily for preventing a callback from occurring.")
            .WithSummary("Delete a callback by id");
    }


    internal static async Task<IResult> CreateCallback(CallbackCreate_Dto callback, Context db)
    {
        var blah = CallbackCreate_Dto.ToDB(callback);
        if (callback.cleanupOthers) {
            var itemsToDelete = db.Callbacks.Where(f=> f.json == callback.json && f.form == callback.form && !f.IsComplete);
            db.Callbacks.RemoveRange(itemsToDelete);
            await db.SaveChangesAsync();
        }
        db.Callbacks.Add(blah);
        await db.SaveChangesAsync();

        return TypedResults.Created($"/todoitems/{blah.id}", blah);
    }

    internal static async Task<IResult> DeleteCallback(Guid id, Context db)
    {
        if (await db.Callbacks.FindAsync(id) is Callback callback)
        {
            db.Callbacks.Remove(callback);
            await db.SaveChangesAsync();
            return TypedResults.Ok(callback);
        }

        return TypedResults.NotFound();
    }

    internal static async Task<IResult> GetAllCallbacks(Context db)
    {
        return TypedResults.Ok(await db.Callbacks.ToArrayAsync());
    }

    internal static async Task<IResult> GetHAformatted(Context db)
    {
        var thelist = await db.Callbacks.Where(t => !t.IsComplete).ToListAsync();

        return TypedResults.Ok(
                new { 
                    count = thelist.Count,
                    incompleteCallbacks = thelist,
                    CustomString = TheConfiguration.CustomString                   
                });
    }
}