using System.Text;
using System.Text.Json;
using Database;
using Microsoft.EntityFrameworkCore;

namespace Services;

public class TheCallerService {
    private readonly ILogger<TheCallerService> _logger;
    private readonly FileContext _db;
    private readonly HttpClient _httpClient;

    public TheCallerService(ILogger<TheCallerService> logger, FileContext db, HttpClient httpClient)
    {
        _logger = logger;
        _db = db;
        _httpClient = httpClient;
    }

    public async Task SendTheRequests() {
        var todos = await _db.Callbacks.Where(t => !t.IsComplete && t.timeof <= DateTime.Now).ToListAsync();
        foreach(var todo in todos) {
            try {
                _logger.LogInformation("Calling " + todo.url);
                using StringContent jsonContent = new(todo.json,Encoding.UTF8,"application/json");
                using HttpResponseMessage response = await _httpClient.PostAsync(todo.url,jsonContent);
                
                var ret = response.EnsureSuccessStatusCode();
                var jsonResponse = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"{jsonResponse}\n");
                todo.IsComplete = true;
            } catch (Exception ex) {
                _logger.LogError(ex, "Could not call " + todo.url);
            }

        }

        try {
            await _db.SaveChangesAsync();
        }catch(Exception ex) {
            _logger.LogError(ex, "Could Save Changes");
        }
    }


}