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
    class AddTaskVM : BindableBase
    {
        private Action changeCanAddTaskToTrue;
        private User logedUser;
        private EfRepository repo;

        #region Properties

        private string taskTitle;
        public string TaskTitle
        {
            get { return taskTitle; }
            set
            {
                SetProperty(ref taskTitle, value);
                (AddTaskCommand as DelegateCommand<Window>).RaiseCanExecuteChanged();
            }
        }

        private ObservableCollection<Project> projects;
        public ObservableCollection<Project> Projects
        {
            get { return projects; }
            set { SetProperty(ref projects, value); }
        }

        private Project taskProject;
        public Project TaskProject
        {
            get { return taskProject; }
            set
            {
                SetProperty(ref taskProject, value);
                //(AddTaskCommand as DelegateCommand<Window>).RaiseCanExecuteChanged();
                Sprints = new ObservableCollection<Sprint>(repo.SprintsRepo.GetSprintsForProject(value.IdProject));
            }
        }

        private ObservableCollection<Sprint> sprints;
        public ObservableCollection<Sprint> Sprints
        {
            get { return sprints; }
            set { SetProperty(ref sprints, value); }
        }

        private Sprint taskSprint;
        public Sprint TaskSprint
        {
            get { return taskSprint; }
            set
            {
                SetProperty(ref taskSprint, value);
                (AddTaskCommand as DelegateCommand<Window>).RaiseCanExecuteChanged();
            }
        }

        private string taskDescription;
        public string TaskDescription
        {
            get { return taskDescription; }
            set{ SetProperty(ref taskDescription, value); }
        }

        public ICommand AddTaskCommand { get; set; }
        public ICommand CancleCommand { get; set; }

        #endregion

        public AddTaskVM(Action changeCanAddTaskToTrue, User user)
        {
            logedUser = user;
            AddTaskCommand = new DelegateCommand<Window>(AddTaskCommandExecute,AddTAskCommandCanExecute);
            CancleCommand = new DelegateCommand<Window>(CancleCommandExecute);
            this.changeCanAddTaskToTrue = changeCanAddTaskToTrue;
            repo = new EfRepository();
            Projects = new ObservableCollection<Project>(repo.ProjectsRepo.Projects);
            TaskDescription = "";
        }



        #region Command Functions

        private void AddTaskCommandExecute(Window window)
        {
            Job task = new Job { IdUser = logedUser.IdUser, IdSprint = TaskSprint.IdSprint, Title = TaskTitle, Desc = TaskDescription };
            repo.JobsRepo.AddJob(task);
            repo.SaveChanges();
            changeCanAddTaskToTrue.DynamicInvoke();
            window.Close();
        }

        private bool AddTAskCommandCanExecute(Window dummy)
        {
            return TaskTitle != null && TaskTitle != "" && TaskSprint != null;
        }

        private void CancleCommandExecute(Window window)
        {
            changeCanAddTaskToTrue.DynamicInvoke();
            window.Close();
        }

        #endregion
    }
}
