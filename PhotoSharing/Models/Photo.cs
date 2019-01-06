using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PhotoSharing.Models
{
    public class Photo
    {
        public Photo()
        {
            this.Categories = new HashSet<Category>();
        }
        [Key]
        public int Id { get; set; }

        [MaxLength(30, ErrorMessage = "Photo Name must be at most 30 characters long!")]
        [Required(ErrorMessage = "Photo Name field is required!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Photo Description is required!")]
        [MaxLength(200, ErrorMessage ="Photo Description must be at most 200 characters long!")]
        public string Description { get; set; }

        public byte[] Image { get; set; }

        public string UserId { get; set; }

        public Category category;
    }

    public class PhotoDBContext : DbContext
    {
        public PhotoDBContext() : base("DefaultConnection") { }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}