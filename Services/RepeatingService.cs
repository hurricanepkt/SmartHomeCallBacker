using System;
using System.Text;
using Database;
using Microsoft.EntityFrameworkCore;

namespace Services;

public class RepeatingService : BackgroundService
{
    public RepeatingService(FileContext db, ILogger<RepeatingService> logger, HttpClient httpClient)
    {
        _db = db;
        _logger = logger;
        _httpClient = httpClient;
    }
    private readonly PeriodicTimer _timer = new(TimeSpan.FromSeconds(1));
    private FileContext _db;
    private ILogger<RepeatingService> _logger;
    private HttpClient _httpClient;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while(await _timer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested) {
        try {
            var todos = await _db.Callbacks.Where(t => !t.IsComplete && t.timeof <= DateTime.Now).ToListAsync();
            foreach(var todo in todos) {
                switch(todo.method) {
                    case "POST": 
                        if (string.IsNullOrWhiteSpace(todo.json)) {
                            await PostForm(todo);
                        } else {
                            await PostJson(todo);
                        }
                        break;
                    case "GET": 
                        await GetUrl(todo);
                        break;
                    default:
                        throw new NotImplementedException("unknown method");
                }
            }
            await _db.SaveChangesAsync();
        } catch (Exception ex) {
            _logger.LogError(ex, "error in service");
        }
        }
    }


    const string FormType = "application/x-www-form-urlencoded";
    const string JsonType = "application/json";
    protected async Task PostJson(Callback todo) {
        await DoIt(todo, (a,b) => _httpClient.PostAsync(a,b), todo.json, "json", JsonType);
    }

    protected async Task PutJson(Callback todo) {
        await DoIt(todo, (a,b) => _httpClient.PutAsync(a,b), todo.json, "json", JsonType);
    }


    protected async Task PostForm(Callback todo) {
        await DoIt(todo, (a,b) => _httpClient.PostAsync(a,b), todo.form, "form", FormType);
    }

    protected async Task PutForm(Callback todo) {
        await DoIt(todo, (a,b) => _httpClient.PutAsync(a,b), todo.form, "form", FormType);
    }
    protected async Task GetUrl(Callback todo) {
        await DoIt(todo, (a,b) => _httpClient.GetAsync(a), String.Empty, String.Empty, String.Empty);
    }
    protected async Task DoIt(Callback todo, Func<string?, HttpContent?, Task<HttpResponseMessage>> thething, string toSend, string friendlyType, string contentType) {
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
}