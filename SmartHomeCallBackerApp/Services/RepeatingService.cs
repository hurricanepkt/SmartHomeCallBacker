using System;
using System.Text;
using Database;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Services;

public class RepeatingService : BackgroundService
{
    public RepeatingService(ILogger<RepeatingService> logger, HttpClient httpClient,  IServiceProvider service)
    {
        logger.LogWarning("Repeating Service CTOR");
        _service = service;
        _logger = logger;
        _httpClient = httpClient;
    }
    private readonly PeriodicTimer _timer = new(TimeSpan.FromSeconds(TheConfiguration.ServiceFrequency));
    private IServiceProvider _service;
    private ILogger<RepeatingService> _logger;
    private HttpClient _httpClient;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        _logger.LogWarning("Repeating Service ExecuteAsync");
        while(await _timer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested) {
            try {
                await RunTick();
            } catch (Exception ex) {        
                _logger.LogError(ex, "error in service");
            }
        }
    }

    protected async Task RunTick() {
        _logger.LogWarning("Repeating Service RunTick");
        using var scope = _service.CreateScope();
        var db = scope.ServiceProvider.GetService<Context>();
        ArgumentNullException.ThrowIfNull(db);
        foreach(var todo in await db.Callbacks.Where(t => !t.IsComplete && t.timeof <= DateTime.Now).ToListAsync()) {
            switch(todo.method) {
            case "POST": 
                await Post(todo);
                break;
            case "PUT": 
                await Put(todo);
                break;
            case "GET": 
                await GetUrl(todo);
                break;
            default:
                throw new NotImplementedException("unknown method");
            }
        }            
        Cleanup(db);
        await db.SaveChangesAsync();
    }   

    protected async Task Post(Callback todo) {
        if (string.IsNullOrWhiteSpace(todo.json)) {
            await PostForm(todo);
        } else {
            await PostJson(todo);
        }
    }

    protected async Task Put(Callback todo) {
        if (string.IsNullOrWhiteSpace(todo.json)) {
            await PutForm(todo);
        } else {
            await PutJson(todo);
        }
    }

    protected async Task PostJson(Callback todo) {
        await DoIt(todo, (a,b) => _httpClient.PostAsync(a,b), todo.json, "json", TheConfiguration.JsonType);
    }

    protected async Task PutJson(Callback todo) {
        await DoIt(todo, (a,b) => _httpClient.PutAsync(a,b), todo.json, "json", TheConfiguration.JsonType);
    }


    protected async Task PostForm(Callback todo) {
        await DoIt(todo, (a,b) => _httpClient.PostAsync(a,b), todo.form, "form", TheConfiguration.FormType);
    }

    protected async Task PutForm(Callback todo) {
        await DoIt(todo, (a,b) => _httpClient.PutAsync(a,b), todo.form, "form", TheConfiguration.FormType);
    }
    protected async Task GetUrl(Callback todo) {
        await DoIt(todo, (a,b) => _httpClient.GetAsync(a), String.Empty, String.Empty, String.Empty);
    }

    
    protected async Task DoIt(Callback todo, Func<string?, HttpContent?, Task<HttpResponseMessage>> thething, string toSend, string friendlyType, string contentType) {
        try {
            StringContent stringContent = new StringContent(String.Empty);
            string rawContent = String.Empty;
            if (!String.IsNullOrWhiteSpace(toSend)) {
                rawContent = toSend.DecodeBase64();
                stringContent = new StringContent(rawContent,Encoding.UTF8,contentType);
            } 
            _logger.LogInformation($"Calling {todo.url} with method --> {todo.method} {(!String.IsNullOrEmpty(friendlyType) ? "and " + friendlyType + " --> " + rawContent : "")}");
            using HttpResponseMessage response = await thething.Invoke(todo.url,stringContent);
            var ret = response.EnsureSuccessStatusCode();
            var rawResponse = await response.Content.ReadAsStringAsync();
            _logger.LogInformation($"{rawResponse}\n");
            todo.IsComplete = true;
            todo.response = rawResponse;
        }
        catch (Exception ex) {
            todo.IsError = true;
            todo.failureMessage = ex.Message;
            todo.failures+= 1;
            todo.IsComplete = (todo.failures >= TheConfiguration.MaxFailures);
        }
    }

    private void Cleanup(Context db)
    {   
        switch(TheConfiguration.CleanupAggressiveness) {
            case TheConfiguration.AgressivenessLevel.AllComplete:
                CleanupInner(f=> f.IsComplete, db);
                break;
            case TheConfiguration.AgressivenessLevel.SuccessOnly:
                CleanupInner(f=> f.IsComplete && !f.IsError, db);
                break;
            case TheConfiguration.AgressivenessLevel.None:
                return;
            default: 
                return;
        }
    }
    private void CleanupInner(Func<Callback, bool> pred, Context db) { 
        var rng = db.Callbacks.Where(pred);
        if (rng.Count() > 0) {
            _logger.LogInformation($"cleaning up {rng.Count()} records");
            db.Callbacks.RemoveRange(rng);
        }        
    } 
}