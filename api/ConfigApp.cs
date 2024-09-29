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
            ["app.frontend"] =  GetterEnv.Get("APP_FRONTEND") ?? String.Empty,
            ["app.key"] = GetterEnv.Get("APP_KEY") ?? String.Empty,
            ["app.expiration.token"] = GetterEnv.Get("EXPIRATION_TOKEN") ?? String.Empty,
            ["db.connection"] = GetConnectionString(),
            ["smtp.host"] = GetSmtpHost() ?? String.Empty,
            ["smtp.username"] = GetterEnv.Get("SMTP_USER") ?? String.Empty,
            ["smtp.email"] = GetterEnv.Get("SMTP_EMAIL") ?? String.Empty,
            ["smtp.password"] = GetterEnv.Get("SMTP_PASSWORD") ?? String.Empty,
            ["smtp.port"] = GetterEnv.Get("SMTP_PORT") ?? String.Empty,
            ["rabbitmq.host"] = GetRabbitMQHost()?? String.Empty,
            ["rabbitmq.user"] = GetterEnv.Get("RABBITMQ_USER") ?? String.Empty,
            ["rabbitmq.password"] = GetterEnv.Get("RABBITMQ_PASSWORD") ?? String.Empty
        };
    }
    
    public string Get(string key)
    {
        return _defaults[key];
    }

    private string? GetSmtpHost()
    {
        return GetterEnv.Get("APP_ENV") == "local"
            ? GetterEnv.Get("SMTP_HOST")
            : GetterEnv.Get("SMTP_CONTAINER");
    }

    private string? GetRabbitMQHost()
    {
        return GetterEnv.Get("APP_ENV") == "local"
            ? GetterEnv.Get("RABBITMQ_HOSt")
            : GetterEnv.Get("RABBITMQ_CONTAINER");
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