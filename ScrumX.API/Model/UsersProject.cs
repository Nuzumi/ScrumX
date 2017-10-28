using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumX.API.Model
{
    public class UsersProject
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdUsersProject { get; set; }

        [Key, Column(Order = 1)]
        [ForeignKey("IdUser")]
        public virtual User User { get; set; }
        public int IdUser { get; set; }

        [Key, Column(Order = 2)]
        [ForeignKey("IdProject")]
        public virtual Project Project { get; set; }
        public int IdProject { get; set; }
    }
}
