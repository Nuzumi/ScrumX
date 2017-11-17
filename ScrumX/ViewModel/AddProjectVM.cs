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
    class AddProjectVM : BindableBase
    {
        private Action changeCanAddProjectToTrue;
        private EfRepository repo;
        private User logedUser;

        #region Properties

        private string projectName;
        public string ProjectName
        {
            get { return projectName; }
            set
            {
                SetProperty(ref projectName, value);
                (AddProjectCommand as DelegateCommand<Window>).RaiseCanExecuteChanged();
            }
        }

        public ICommand AddProjectCommand { get; set; }
        public ICommand CancleCommand { get; set; }

        #endregion

        public AddProjectVM(Action changeCanAddProjectToTrue, User user)
        {
            logedUser = user;
            AddProjectCommand = new DelegateCommand<Window>(AddProjectCommandExecute,AddProjectCommandCanExecute);
            CancleCommand = new DelegateCommand<Window>(CancleCommandExecute);
            this.changeCanAddProjectToTrue = changeCanAddProjectToTrue;
            repo = new EfRepository();
        }
        

        #region Command Functions

        private void AddProjectCommandExecute(Window window)
        {
            repo.ProjectsRepo.AddProject(ProjectName);
            repo.SaveChanges();
            changeCanAddProjectToTrue.DynamicInvoke();
            window.Close();
        }

        private bool AddProjectCommandCanExecute(Window dummy)
        {
            return ProjectName != null && ProjectName != "";
        }

        private void CancleCommandExecute(Window window)
        {
            changeCanAddProjectToTrue.DynamicInvoke();
            window.Close();
        }

        #endregion
    }
}
