using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PhotoSharing.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        public int PhotoId { get; set; }

        [Required]
        [MaxLength(200, ErrorMessage = "Comment text must be at most 200 characters")]
        public string Text { get; set; }

    }
}