using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace lets.Models
{
    [MetadataType(typeof(userMetadata))]
    public partial class user
    {
    }
    public class userMetadata
    {
        [Key]
        [HiddenInput(DisplayValue = true)]
        public int user_id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
         public string Contact_Number { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Date_of_birth { get; set; }
         [Required]
         public string Username { get; set; }
          [Required]
         public int Password { get; set; }
          [Required]
          public string user_type { get; set; }

    }

}