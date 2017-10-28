using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumX.API.Model
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdUser { get; set; }
        [MaxLength(20), MinLength(3)]
        public string Name { get; set; }
        public string Password { get; set; }
        
        public virtual IList<Job> UsersJob { get; set; }
        
       }
}
