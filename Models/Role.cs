namespace Blog.API.Models;
public class Role
{
   public int id { get; set; }
   public string Name { get; set; } = string.Empty;
   public string Slug { get; set; } = string.Empty;

   public IList<User>? Users { get; set; }
}