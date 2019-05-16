using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace alintaApi.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(30)]
        public string firstName { get; set; }
        [Required]
        [StringLength(30)]
        public string lastName { get; set; }
        [Required]
        public DateTime dateOfBirth { get; set; }
    }
}
