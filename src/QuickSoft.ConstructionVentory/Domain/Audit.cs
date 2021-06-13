using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace QuickSoft.ConstructionVentory.Domain
{
    public class Audit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [StringLength(255)]
        public string Descriptions { get; set; }
        
        public DateTime CreatedDate { get; set; }
        
        [JsonIgnore]
        public int UserId { get; set; }
        
        public User User { get; set; }
    }
}