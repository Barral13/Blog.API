namespace Blog.API.Models;
public class Category
{
   public int Id { get; set;}
   public string Name { get; set;} = string.Empty;
   public string Slug { get; set; } = string.Empty;

   public IList<Category> Categories { get; set;}
}

