using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroBlogging.Models
{
    public class Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(200, ErrorMessage = "Max")]
        public string Text { get; set; }

        [Required(ErrorMessage = "Required")]
        public bool Public { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public DateTime Date { get; set; }

        public string Picture { get; internal set; }
    }
}