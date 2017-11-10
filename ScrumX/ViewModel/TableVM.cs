using ScrumX.API.Model;
using System.Windows.Input;
using System.Windows;
using Prism.Commands;
using ScrumX.HelperClasses;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using ScrumX.API.Repository;
using ScrumX.API.Content;

namespace ScrumX.ViewModel
{
    class TableVM : DialogDisplay
    {
        private EfRepository repo;

        #region Properties

        public string UserName { get; set; }

        private ObservableCollection<Job> toDoJobs;
        public ObservableCollection<Job> ToDoJobs
        {
            get { return toDoJobs; }
            set { SetProperty(ref toDoJobs, value); }
        }

        private ObservableCollection<Job> doingJobs;
        public ObservableCollection<Job> DoingJobs
        {
            get { return doingJobs; }
            set { SetProperty(ref doingJobs, value); }
        }

        private ObservableCollection<Project> projects;
        public ObservableCollection<Project> Projects
        {
            get { return projects; }
            set
            {
                SetProperty(ref projects, value);
            }
        }

        private Project selectedProject;
        public Project SelectedProject
        {
            get { return selectedProject; }
            set
            {
                SetProperty(ref selectedProject, value);
                Sprints = new ObservableCollection<Sprint>(repo.SprintsRepo.GetSprintsForProject(value.IdProject));
                if(Sprints.Count != 0)
                {
                    SelectedSprint = repo.SprintsRepo.GetLastSprintForProject(SelectedProject);
                }
            }
        }

        private ObservableCollection<Sprint> sprints;
        public ObservableCollection<Sprint> Sprints
        {
            get { return sprints; }
            set { SetProperty(ref sprints, value); }
        }

        private Sprint selectedSprint;
        public Sprint SelectedSprint
        {
            get { return selectedSprint; }
            set
            {
                SetProperty(ref selectedSprint, value);
                if (SelectedSprint != null)
                {
                    ToDoJobs = new ObservableCollection<Job>(repo.JobsRepo.GetJobsInTable(value, (int)typeTable.ToDo));
                    DoingJobs = new ObservableCollection<Job>(repo.JobsRepo.GetJobsInTable(value, (int)typeTable.Doing));
                }
                else
                {
                    ToDoJobs.Clear();
                    DoingJobs.Clear();
                }
            }
        }

        public ICommand GoToBacklogCommand { get; set; }

        #endregion


        public TableVM(User user) :base(user)
        {
            GoToBacklogCommand = new DelegateCommand<Window>(GoToBacklogCommandExecute, GoToBacklogCommandCanExecute);
            logedUser = user;
            UserName = user.Name;
            repo = new EfRepository();
            Projects = new ObservableCollection<Project>(repo.ProjectsRepo.Projects);
            if(Projects[0] != null)
            {
                SelectedProject = Projects[0];
                if(Sprints[0] != null)
                {
                    SelectedSprint = Sprints[0];
                }
            }
        }

        protected override void riseGoToCommands()
        {
            if(GoToBacklogCommand != null)
            {
                (GoToBacklogCommand as DelegateCommand<Window>).RaiseCanExecuteChanged();
            }
        }

        #region CommandFunctions

        private void GoToBacklogCommandExecute(Window window)
        {
            BacklogVM dataContext = new BacklogVM(logedUser); // mozna by przekazywac ta wczesniej uzywana klase by parametry wyszukiwania zostawaly
            Backlog backlog = new Backlog();
            backlog.DataContext = dataContext;
            backlog.Show();
            window.Close();
        }

        private bool GoToBacklogCommandCanExecute(Window dummy)
        {
            return CanAddTask;
        }
        #endregion
    }
}
