using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumX.API.Model
{
    public class HistoryJob
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdHistoryJob { get; set; }

        [ForeignKey("IdJob")]
        public virtual Job Job { get; set; }
        [Index]
        public int IdJob { get; set; }

        [ForeignKey("IdUser")]
        public virtual User User { get; set; }
        [Index]
        public int IdUser { get; set; }

        public string Comment { get; set; }
        
        public int FromBacklog { get; set; }
        public int ToBacklog { get; set; }
        public int FromTable { get; set; }
        public int ToTable { get; set; }
    }
}
