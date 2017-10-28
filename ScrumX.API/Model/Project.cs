using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumX.API.Model
{
    public class Project
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdProject { get; set; }
        public string Name { get; set; }

        public DateTime DayCreated { get; set; }
        
        public virtual IList<Sprint> ProjectSprint { get; set; }
        
    }
}
