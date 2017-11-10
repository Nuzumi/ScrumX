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
        public List<string> TypeList { get; set; }

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
               // ToDoJobs = repo.JobsRepo.GetJobsInTable()
            }
        }

        private typeBacklog selectedType;
        public string SelectedType
        {
            get { return selectedType.ToString(); }
            set
            {
                switch (value)
                {
                    case "None":
                        SetProperty(ref selectedType, typeBacklog.None);
                        break;

                    case "New":
                        SetProperty(ref selectedType, typeBacklog.New);
                        break;

                    case "Ready":
                        SetProperty(ref selectedType, typeBacklog.Ready);
                        break;

                    case "Scheduled":
                        SetProperty(ref selectedType, typeBacklog.Scheduled);
                        break;

                    case "Completed":
                        SetProperty(ref selectedType, typeBacklog.Completed);
                        break;
                }
            }
        }

        public ICommand GoToBacklogCommand { get; set; }

        #endregion


        public TableVM(User user) :base(user)
        {
            GoToBacklogCommand = new DelegateCommand<Window>(GoToBacklogCommandExecute, GoToBacklogCommandCanExecute);
            TypeList = new List<string> { "None","ToDo","Doing","Done" };
            logedUser = user;
            UserName = user.Name;
            Projects = new ObservableCollection<Project>(repo.ProjectsRepo.Projects);
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
