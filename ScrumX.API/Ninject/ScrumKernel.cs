using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumX.API.Ninject
{
    public class ScrumKernel : StandardKernel
    {
        public ScrumKernel () : base (new RepoModule()){}
    }
}
