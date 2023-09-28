namespace MF.JwtStore.Core;

public static class Configuration
{
    public static SecretsConfiguration Secrets { get; set; } = new();
    public static DatabaseConfiguration Database { get; set; } = new();
    public static EmailConfiguration Email { get; set; } = new();
    public static SendGridConfiguration SendGrid { get; set; } = new();

    public class SecretsConfiguration
    {
        public string ApiKey { get; set; } = string.Empty;
        public string JwtPrivateKey { get; set; } = string.Empty;
        public string PasswordSaltKey { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string SendGrid { get; set; } = string.Empty;
    }

    public class DatabaseConfiguration
    {
        public string ConnectionString { get; set; } = string.Empty;
    }

    public class EmailConfiguration
    {
        public string DefaultFromEmail { get; set; } = "mf.mai@hotmail.com";
        public string DefaultFromName { get; set; } = "mf.mai";
    }

    public class SendGridConfiguration
    {
        public string ApiKey { get; set; } = string.Empty;
    }
}