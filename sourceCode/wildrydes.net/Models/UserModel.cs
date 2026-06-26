using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace wildrydes.net.Models
{
    [Table("Users")]
    public class UserModel
    {
        public Guid Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        //// navigation property. the idea is to be able to see
        //// all rides for a User
        //public virtual ICollection<RideModel> Rides { get; set; }

    }
}