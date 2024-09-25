using Api.Features.Dotenv;

namespace Api;

public class ConfigApp
{
    private readonly Dictionary<string, string> _defaults;

    public ConfigApp()
    {
        _defaults = new Dictionary<string, string>
        {
            ["app.name"] = GetterEnv.Get("APP_NAME") ?? String.Empty,
            ["app.version"] = GetterEnv.Get("APP_VERSION") ?? String.Empty,
            ["app.env"] = GetterEnv.Get("APP_ENV") ?? String.Empty,
            ["app.url"] =  GetterEnv.Get("APP_URL") ?? String.Empty,
            ["app.key"] = GetterEnv.Get("APP_KEY") ?? String.Empty,
            ["app.expiration.token"] = GetterEnv.Get("EXPIRATION_TOKEN") ?? String.Empty,
            ["db.connection"] = GetConnectionString()
        };
    }
    
    public string Get(string key)
    {
        return _defaults[key];
    }

    private string GetConnectionString()
    {
        string? host = GetHostByEnv();
        string? user = GetterEnv.Get("DB_USER");
        string? password = GetterEnv.Get("DB_PASSWORD");
        string? database = GetterEnv.Get("DB_DATABASE");

        return $"Data Source={host},1433;User Id={user};"
               + $"Password={password};Database={database};"
               + "Encrypt=True;TrustServerCertificate=True;";
    }

    private string? GetHostByEnv()
    {
        return GetterEnv.Get("APP_ENV") == "local"
            ? GetterEnv.Get("DB_HOST")
            : GetterEnv.Get("DB_CONTAINER");
    }
}