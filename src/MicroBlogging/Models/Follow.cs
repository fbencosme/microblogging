using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MicroBlogging.Models
{
    public class Follow
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("source")]
        public int Source { get; set; }

        [ForeignKey("target")]
        public int Target { get; set; }

    }

    public class FollowRequest
    {
        public int Source { get; set; }

        public int Target { get; set; }

    }
}
