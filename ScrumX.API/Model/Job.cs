using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumX.API.Model
{
    public class Job
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdJob { get; set; }

        [ForeignKey("IdSprint")]
        public virtual Sprint Sprint { get; set; }
        public int IdSprint { get; set; }

        [ForeignKey("IdUser")]
        public virtual User User { get; set; }
        public int IdUser { get; set; }


        public string Title { get; set; }
        public string Desc { get; set; }
        public int? Priority { get; set; }
        public int? SP { get; set; }
        
        public int BacklogStatus { get; set; }
       
        public int TableStatus { get; set; }

        public virtual IList<HistoryJob> JobsHistoryJob { get; set; }
    }

}
