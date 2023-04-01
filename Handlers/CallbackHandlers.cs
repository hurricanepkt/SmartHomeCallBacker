using Database;
using Microsoft.EntityFrameworkCore;

namespace Handlers
{
    public class CallbackHandlers
    {
        internal static void Setup(RouteGroupBuilder callbacksGroup)
        {
            callbacksGroup.MapGet("/", CallbackHandlers.GetAllCallbacks);
            callbacksGroup.MapGet("/complete", CallbackHandlers.GetCompleteCallbacks);
            callbacksGroup.MapGet("/incomplete", CallbackHandlers.GetIncompleteCallbacks);
            callbacksGroup.MapGet("/{id}", CallbackHandlers.GetCallback);
            callbacksGroup.MapPost("/", CallbackHandlers.CreateCallback);
            callbacksGroup.MapPut("/{id}", CallbackHandlers.UpdateCallback);
            callbacksGroup.MapDelete("/{id}", CallbackHandlers.DeleteCallback);
        }


        internal static async Task<IResult> CreateCallback(CallbackCreate_Dto callback, FileContext db)
        {
            var blah = CallbackCreate_Dto.ToDB(callback);
            db.Callbacks.Add(blah);
            await db.SaveChangesAsync();

            return TypedResults.Created($"/todoitems/{blah.id}", blah);
        }

        internal static async Task<IResult> DeleteCallback(Guid id, FileContext db)
        {
            if (await db.Callbacks.FindAsync(id) is Callback callback)
            {
                db.Callbacks.Remove(callback);
                await db.SaveChangesAsync();
                return TypedResults.Ok(callback);
            }

            return TypedResults.NotFound();
        }

        internal static async Task<IResult> GetAllCallbacks(FileContext db)
        {
            return TypedResults.Ok(await db.Callbacks.ToArrayAsync());
        }

        internal static async Task<IResult> GetCallback(Guid id, FileContext db)
        {
            return await db.Callbacks.FindAsync(id)
                        is Callback callback
                            ? TypedResults.Ok(callback)
                            : TypedResults.NotFound();
        }

        internal static async Task<IResult> GetCompleteCallbacks(FileContext db)
        {
            return TypedResults.Ok(await db.Callbacks.Where(t => t.IsComplete).ToListAsync());
        }

        internal static async Task<IResult> GetIncompleteCallbacks(FileContext db)
        {
            return TypedResults.Ok(await db.Callbacks.Where(t => !t.IsComplete).ToListAsync());
        }



        internal static async Task<IResult> UpdateCallback(Guid id, Callback inputCallback, FileContext db)
        {
            var todo = await db.Callbacks.FindAsync(id);

            if (todo is null) return TypedResults.NotFound();

            todo.url = inputCallback.url;
            await db.SaveChangesAsync();

            return TypedResults.Ok(todo);
        }
    }
}