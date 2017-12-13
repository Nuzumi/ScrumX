using Prism.Commands;
using Prism.Mvvm;
using ScrumX.API.Model;
using ScrumX.API.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private User logedUser;
        private Sprint sprintToEdit;

        #region Properties

        public ObservableCollection<Project> Projects { get; set; }
        public bool EnableProjectSelection { get; set; }
        public string ButtonText { get; set; }
        public int? SelectedProjectIndex { get; set; }

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

        public AddSprintVM(Action changeCanAddSprintToTrue, User user)
        {
            baseConstructor(changeCanAddSprintToTrue, user);
            ButtonText = "Dodaj Sprint";
            EnableProjectSelection = true;
            SelectedProjectIndex = null;
        }

        public AddSprintVM(Action changeCanAddSprintToTrue,User user,Project project)
        {
            baseConstructor(changeCanAddSprintToTrue, user);
            ButtonText = "zapisz";
            EnableProjectSelection = false;
            sprintToEdit = repo.SprintsRepo.GetLastSprintForProject(project);
            EndDate = sprintToEdit.EndData.Value;
            for(int i = 0; i < Projects.Count; i++)
            {
                if(Projects[i].Name == project.Name)
                {
                    SelectedProjectIndex = i;
                }
            }
        }

        private void baseConstructor(Action changeCanAddSprintToTrue,User user)
        {
            logedUser = user;
            AddSprintCommand = new DelegateCommand<Window>(AddSprintCommandExecute, AddSprintCommandCanExecute);
            CancleCommand = new DelegateCommand<Window>(CancleCommandExecute);
            this.changeCanAddSprintToTrue = changeCanAddSprintToTrue;
            EndDate = DateTime.Today;
            repo = new EfRepository();
            Projects = new ObservableCollection<Project>(repo.ProjectsRepo.Projects);
        }

        #region Command Functions

        private void AddSprintCommandExecute(Window window)
        {
            if(sprintToEdit == null)
            {
                repo.SprintsRepo.AddSprint(SelectedProject, EndDate);
            }
            else
            {
                repo.SprintsRepo.ChangeEndDate(sprintToEdit, EndDate);
            }
            changeCanAddSprintToTrue.DynamicInvoke();
            window.Close();
        }

        private bool AddSprintCommandCanExecute(Window window)
        {
            return SelectedProject != null && EndDate != null && EndDate > DateTime.Today;
        }

        private void CancleCommandExecute(Window window)
        {
            changeCanAddSprintToTrue.DynamicInvoke();
            window.Close();
        }

        #endregion
    }
}
