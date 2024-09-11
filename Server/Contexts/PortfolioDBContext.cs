using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Server.Entities;

namespace Server.Contexts;

public partial class PortfolioDBContext : DbContext
{
    public PortfolioDBContext()
    {
    }

    public PortfolioDBContext(DbContextOptions<PortfolioDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Blog> Blogs { get; set; }

    public virtual DbSet<Blogimage> Blogimages { get; set; }

    public virtual DbSet<Contacttable> Contacttables { get; set; }

    public virtual DbSet<ProjectCard> ProjectCards { get; set; }

    public virtual DbSet<Skill> Skills { get; set; }

    public virtual DbSet<SkillDescription> SkillDescriptions { get; set; }

    public virtual DbSet<VideoUrl> VideoUrls { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("blog_pkey");

            entity.ToTable("blog");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Date)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date");
            entity.Property(e => e.FullDesc).HasColumnName("full_desc");
            entity.Property(e => e.PreviewDesc).HasColumnName("preview_desc");
            entity.Property(e => e.RouteName)
                .HasMaxLength(255)
                .HasColumnName("route_name");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Blogimage>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("blogimages_pkey");

            entity.ToTable("blogimages");

            entity.Property(e => e.ImageId).HasColumnName("image_id");
            entity.Property(e => e.BlogId).HasColumnName("blog_id");
            entity.Property(e => e.ContentType)
                .HasMaxLength(50)
                .HasColumnName("content_type");
            entity.Property(e => e.ImageData).HasColumnName("image_data");
            entity.Property(e => e.UploadDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("upload_date");

            entity.HasOne(d => d.Blog).WithMany(p => p.Blogimages)
                .HasForeignKey(d => d.BlogId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("blogimages_blog_id_fkey");
        });

        modelBuilder.Entity<Contacttable>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("contacttable_pkey");

            entity.ToTable("contacttable");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.Message).HasColumnName("message");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Subject).HasColumnName("subject");
        });

        modelBuilder.Entity<ProjectCard>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("project_card_pkey");

            entity.ToTable("project_card");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Imgurl).HasColumnName("imgurl");
            entity.Property(e => e.Projectlink).HasColumnName("projectlink");
            entity.Property(e => e.Sourcecodelink).HasColumnName("sourcecodelink");
            entity.Property(e => e.Title).HasColumnName("title");
        });

        modelBuilder.Entity<Skill>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("skill_pkey");

            entity.ToTable("skill");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Title).HasColumnName("title");
        });

        modelBuilder.Entity<SkillDescription>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("skill_description_pkey");

            entity.ToTable("skill_description");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.SkillId).HasColumnName("skill_id");

            entity.HasOne(d => d.Skill).WithMany(p => p.SkillDescriptions)
                .HasForeignKey(d => d.SkillId)
                .HasConstraintName("skill_description_skill_id_fkey");
        });

        modelBuilder.Entity<VideoUrl>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("video_url_pkey");

            entity.ToTable("video_url");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BlogId).HasColumnName("blog_id");
            entity.Property(e => e.Url).HasColumnName("url");

            entity.HasOne(d => d.Blog).WithMany(p => p.VideoUrls)
                .HasForeignKey(d => d.BlogId)
                .HasConstraintName("video_url_blog_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
