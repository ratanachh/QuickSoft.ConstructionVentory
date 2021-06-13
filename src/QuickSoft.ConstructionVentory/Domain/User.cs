using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace QuickSoft.ConstructionVentory.Domain
{
    public class User
    {
        [Key]
        [JsonIgnore]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is obligatory")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Name has to have at least 3 characters")]
        public string Username { get; set; }

        [JsonIgnore]
        [Required(ErrorMessage = "Password is obligatory")]
        [StringLength(120, MinimumLength = 3, ErrorMessage = "Name has to have at least 3 characters")]
        public string Password { get; set; }
        
        [StringLength(15)]
        public string Phone { get; set; }
        
        public string ProfileUrl { get; set; }


        [Required(ErrorMessage = "UserType is obligatory")]
        public int UserType { get; set; }


        public DateTime CreatedDate { get; set; }

        [JsonIgnore]
        public ICollection<Audit> Audits { get; set; }
    }
}