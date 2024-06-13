using Blog.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.API.Data.Mappings;

public class UserMap : IEntityTypeConfiguration<User>
{
   public void Configure(EntityTypeBuilder<User> builder)
   {
      builder.ToTable("User");

      builder.HasKey(x => x.Id);
      builder.Property(x => x.Id)
         .ValueGeneratedOnAdd()
         .UseIdentityColumn();

      builder.Property(x => x.Name)
         .IsRequired()
         .HasColumnName("Name")
         .HasColumnType("NVARCHAR")
         .HasMaxLength(80);

      builder.Property(x => x.Email)
         .IsRequired()
         .HasColumnName("Email")
         .HasColumnType("NVARCHAR")
         .HasMaxLength(255);

      builder.Property(x => x.PasswordHash)
         .IsRequired()
         .HasColumnName("PassworHash")
         .HasColumnType("TEXT");

      builder.Property(x => x.Image)
         .HasColumnName("Image")
         .HasColumnType("VARCHAR")
         .HasMaxLength(120);

      builder
         .HasIndex(x => x.Slug, "Ix_User_Slug")
         .IsUnique();

      builder.Property(x => x.Bio)
         .HasColumnName("Bio")
         .HasColumnType("TEXT")
         .HasMaxLength(500);

      builder
         .HasMany(x => x.Roles)
         .WithMany(x => x.Users)
         .UsingEntity<Dictionary<string, object>>(
            "UserRole",
            role => role
               .HasOne<Role>()
               .WithMany()
               .HasForeignKey("RoleId")
               .HasConstraintName("FK_UserRole_RoleId")
               .OnDelete(DeleteBehavior.Cascade),
            user => user
               .HasOne<User>()
               .WithMany()
               .HasForeignKey("UserId")
               .HasConstraintName("FK_UserRole_UserId")
               .OnDelete(DeleteBehavior.Cascade));
   }
}