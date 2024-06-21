namespace Blog.API;
public static class Configuration
{
   public static string JwtKey = "d7emA7Jwtj8gxdv29zAn&813kj5E4Yz2vJ6pMpnjpiy=";
   public static string ApiKeyName = "api_key";
   public static string ApiKey = "blog_api_Dh4N6l3R5On=b4r8aL";
   public static SmtpConfiguration Smtp = new();

   public class SmtpConfiguration
   {
      public string Host { get; set; }
      public int Port { get; set; } = 25;
      public string UserName { get; set; }
      public string Password { get; set; }
   }
}