using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumX.API.Model
{
    public class Sprint
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdSprint { get; set; }

        [ForeignKey("IdProject")]
        public virtual Project Project { get; set; }
        public int IdProject { get; set; }

        public virtual IList<Job> SprintJob { get; set; }

        public int NoSprint { get; set; }
        public DateTime StartData { get; set; }
        public DateTime? EndData { get; set; }
    }
}
