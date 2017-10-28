using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragAndDrop
{
    class DragAndDropEventArgs : EventArgs
    {
        public Object DataContext;
        public DragAndDropEventArgs () { }
        public DragAndDropEventArgs (object dataContext)
        {
            DataContext = dataContext;
        }
    }
}
