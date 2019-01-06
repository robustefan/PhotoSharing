using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PhotoSharing.Models
{
    public class Album
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "The Album Name is Required!")]
        [MaxLength(30, ErrorMessage = "The Album Name must be at most 30 characters longs!")]

        public string Name { get; set; }
        public string UserId { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }

    }
}