using Blog.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.API.Data.Mappings;

public class PostMap : IEntityTypeConfiguration<Post>
{
   public void Configure(EntityTypeBuilder<Post> builder)
   {
      builder.ToTable("Post");

      builder.HasKey(x => x.Id);
      builder.Property(x => x.Id)
         .ValueGeneratedOnAdd()
         .UseIdentityColumn();

      builder.Property(x => x.Title)
         .IsRequired()
         .HasColumnName("Title")
         .HasColumnType("NVARCHAR")
         .HasMaxLength(120);

      builder.Property(x => x.Summary)
         .HasColumnName("Summary")
         .HasColumnType("TEXT")
         .HasMaxLength(500);

      builder.Property(x => x.Body)
         .IsRequired()
         .HasColumnName("Body")
         .HasColumnType("TEXT");

      builder
         .HasIndex(x => x.Slug, "IX_Post_Slug")
         .IsUnique();

      builder.Property(x => x.CreateDate)
         .IsRequired()
         .HasColumnName("CreateDate")
         .HasColumnType("SMALLDATETIME")
         .HasMaxLength(60)
         .HasDefaultValueSql("GETDATE()");

      builder.Property(x => x.LastUpdateDate)
         .IsRequired()
         .HasColumnName("LastUpdateDate")
         .HasColumnType("SMALLDATETIME")
         .HasMaxLength(60)
         .HasDefaultValueSql("GETDATE()");

      builder
         .HasOne(x => x.Category)
         .WithMany(x => x.Posts)
         .HasConstraintName("FK_Post_Category")
         .OnDelete(DeleteBehavior.Cascade);

      builder
         .HasOne(x => x.Author)
         .WithMany(x => x.Posts)
         .HasConstraintName("FK_Post_Author")
         .OnDelete(DeleteBehavior.Cascade);

      builder
         .HasMany(x => x.Tags)
         .WithMany(x => x.Posts)
         .UsingEntity<Dictionary<string, object>>(
            "PostTag",
            post => post
               .HasOne<Tag>()
               .WithMany()
               .HasForeignKey("PostId")
               .HasConstraintName("FK_PostRole_PostId")
               .OnDelete(DeleteBehavior.Cascade),
            tag => tag
               .HasOne<Post>()
               .WithMany()
               .HasForeignKey("TagId")
               .HasConstraintName("FK_PostTag_TagId")
               .OnDelete(DeleteBehavior.Cascade));
   }
}