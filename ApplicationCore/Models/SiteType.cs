using System.ComponentModel.DataAnnotations;

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