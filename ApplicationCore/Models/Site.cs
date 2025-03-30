using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    public class Site
    {
        [Key]
        public int Id { get; set; }



        [Required]
        [Display(Name = "Site Name")]
        public string Name { get; set; } = "";

        [Required]
        [Range(1, float.MaxValue, ErrorMessage = "length must be greater than 1.")]
        public float MaxLength { get; set; }
        public int SiteTypeId { get; set; }

        //Connect Objects or Tables
        [ForeignKey("SiteTypeId")]
        public virtual SiteType? SiteType { get; set; }
    }
}
