using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }


        public int SiteId { get; set; }
        public string UserId { get; set; }

        // Connect Objects or Tables
        [ForeignKey("SiteId")]
        public virtual Site Site { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
