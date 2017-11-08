using Prism.Commands;
using Prism.Mvvm;
using ScrumX.API.Model;
using ScrumX.API.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ScrumX.ViewModel
{
    class AddSprintVM : BindableBase
    {
        private Action changeCanAddSprintToTrue;
        private EfRepository repo;

        #region Properties

        private Project selectedProject;
        public Project SelectedProject
        {
            get { return selectedProject; }
            set
            {
                SetProperty(ref selectedProject, value);
                (AddSprintCommand as DelegateCommand<Window>).RaiseCanExecuteChanged();
            }
        }

        private DateTime endDate;
        public DateTime EndDate
        {
            get { return endDate; }
            set
            {
                SetProperty(ref endDate, value);
                (AddSprintCommand as DelegateCommand<Window>).RaiseCanExecuteChanged();
            }
        }

        public ICommand AddSprintCommand { get; set; }
        public ICommand CancleCommand { get; set; }

        #endregion

        public AddSprintVM(Action changeCanAddSprintToTrue)
        {
            AddSprintCommand = new DelegateCommand<Window>(AddSprintCommandExecute);
            this.changeCanAddSprintToTrue = changeCanAddSprintToTrue;
            EndDate = DateTime.Today;

        }

        #region Command Functions

        private void AddSprintCommandExecute(Window window)
        {
            //repo.SprintsRepo.AddSprint()
            //repo.SaveChanges(); 
            changeCanAddSprintToTrue.DynamicInvoke();
            window.Close();
        }

        private bool AddSprintCommandCanExecute(Window window)
        {
            return SelectedProject != null && EndDate != null;
        }

        private void CancleCommandEcevute(Window window)
        {
            changeCanAddSprintToTrue.DynamicInvoke();
            window.Close();
        }

        #endregion
    }
}
