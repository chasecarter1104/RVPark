using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    public class SiteType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Site Type Name")]
        public string Name { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }

        public bool IsLocked { get; set; }
    }
}
