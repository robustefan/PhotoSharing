using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PhotoSharing.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(30, ErrorMessage = "Category Name must be at most 30 characters long!")]
        [Required(ErrorMessage = "Category Name is required!")]
        public string Name { get; set; }

        public Photo photo;

    }
}