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
        [Index]
        public int IdSprint { get; set; }

        [ForeignKey("IdUser")]
        public virtual User User { get; set; }
        public int IdUser { get; set; }

        public string Title { get; set; }
        public string Desc { get; set; }
        public int? Priority { get; set; }
        public int? SP { get; set; }

        [Index]
        public int BacklogStatus { get; set; } = 1;

        [Index]
        public int TableStatus { get; set; } = 0;

        public virtual IList<HistoryJob> JobsHistoryJob { get; set; }
    }

}
