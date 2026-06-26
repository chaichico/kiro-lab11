using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace wildrydes.net.Models
{
    [Table("Unicorns")]
    public class UnicornModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Color { get; set; }

        [Required]
        public string Description { get; set; }

        public int Rating { get; set; }

        //// navigation property. the idea is to be able to see
        //// all rides for a Unicorn
        //public virtual ICollection<RideModel> Rides { get; set; }


    }
}