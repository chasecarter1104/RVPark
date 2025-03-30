using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    public class Fee
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Fee Name")]
        public string Name { get; set; }

        [Range(0, float.MaxValue)]
        public float FeeAmount { get; set; }

    }
}
