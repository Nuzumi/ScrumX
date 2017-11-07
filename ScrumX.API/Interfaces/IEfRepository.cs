using ScrumX.API.Logic;
using ScrumX.API.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumX.API.Interfaces
{
    public interface IEfRepository
    {
         HistoryJobRepo HistoryJobsRepo { get; }
         JobRepo JobsRepo { get; }
         UserRepo UsersRepo { get; }
         ProjectRepo ProjectsRepo { get; }
         SprintRepo SprintsRepo { get; }
    }
}
