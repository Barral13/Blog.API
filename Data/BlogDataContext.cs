using Blog.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog.API.Data;
public class BlogDataContext : DbContext
{
   protected override void OnConfiguring(DbContextOptionsBuilder options)
      => options.UseSqlServer(@"Server=localhost,1433;Database=Blog;
         User ID=sa;Password=Barral#13;TrustServerCertificate=true");
}