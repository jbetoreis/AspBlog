namespace AspBlog;

public static class Configuration
{
    public static string JwtKey = "Jdv35NBaOb87hfgJrBA76KAmfA138KAd9KA";
    public static string ApiKeyName = "api_key";
    public static string ApiKey = "PaddsaAJKSDhjdkA385AJKSDbnjaAd412AS";
    public static SmtpConfiguration Smtp = new();
    
    public class SmtpConfiguration
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}