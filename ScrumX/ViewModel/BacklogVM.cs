using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScrumX.API.Model;
using System.Windows.Input;
using Prism.Commands;
using System.Windows;
using ScrumX.ViewModel;
using ScrumX.View;
using ScrumX.HelperClasses;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using ScrumX.API.Repository;
using ScrumX.API.Content;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.Controls;

namespace ScrumX.ViewModel
{
    class BacklogVM : DialogDisplay
    {
        private EfRepository repo;
        #region Properties

        public List<double> PriorityValues { get; set; }
        public List<double> StoryPointValues { get; set; }
        public string UserName { get; set; }
        public List<string> TypeList { get; set; }
        IDialogCoordinator _dialogCoordinator;

        private ObservableCollection<Project> projects;
        public ObservableCollection<Project> Projects
        {
            get { return projects; }
            set { SetProperty(ref projects, value); }
        }

        private Sprint actualSprint;
        public Sprint ActualSprint
        {
            get { return actualSprint; }
            set
            {
                SetProperty(ref actualSprint, value);
               
            }
        }

        private Project selectedProject;
        public Project SelectedProject
        {
            get { return selectedProject; }
            set
            {
                SetProperty(ref selectedProject, value);
                if(SelectedProject != null)
                {
                    Jobs = new ObservableCollection<Job>(repo.JobsRepo.GetJobsInBacklog(SelectedProject, (int)selectedType));
                    IsDescVisible = false;
                    ActualSprint = repo.SprintsRepo.GetLastSprintForProject(SelectedProject.IdProject);
                }
            }
        }

        private string tag;
        public string Tag
        {
            get { return tag; }
            set
            {
                SetProperty(ref tag, value);
                SearchJobCommandExecute();
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
                    case "All":
                        SetProperty(ref selectedType, typeBacklog.All);
                        Jobs = new ObservableCollection<Job>(repo.JobsRepo.GetJobsInBacklog(SelectedProject,(int)typeBacklog.All));
                        break;

                    case "New":
                        SetProperty(ref selectedType, typeBacklog.New);
                        Jobs = new ObservableCollection<Job>(repo.JobsRepo.GetJobsInBacklog(SelectedProject, (int)typeBacklog.New));
                        break;

                    case "Ready":
                        SetProperty(ref selectedType, typeBacklog.Ready);
                        Jobs = new ObservableCollection<Job>(repo.JobsRepo.GetJobsInBacklog(SelectedProject, (int)typeBacklog.Ready));
                        break;

                    case "Scheduled":
                        SetProperty(ref selectedType, typeBacklog.Scheduled);
                        Jobs = new ObservableCollection<Job>(repo.JobsRepo.GetJobsInBacklog(SelectedProject, (int)typeBacklog.Scheduled));
                        break;

                    case "Completed":
                        SetProperty(ref selectedType, typeBacklog.Completed);
                        Jobs = new ObservableCollection<Job>(repo.JobsRepo.GetJobsInBacklog(SelectedProject, (int)typeBacklog.Completed));
                        break;
                }
                IsDescVisible = false;
            }
        }

        private ObservableCollection<Job> jobs;
        public ObservableCollection<Job> Jobs
        {
            get { return jobs; }
            set { SetProperty(ref jobs, value); }
        }

        private Job selectedJob;
        public Job SelectedJob
        {
            get { return selectedJob; }
            set
            {
                SetProperty(ref selectedJob, value);
                IsDescVisible = true;
                VisibilityFlyout = true;
            }
        }

        private double selectedSP;
        public double SelectedSP
        {
            get { return selectedSP; }
            set
            {
                SetProperty(ref selectedSP, value);
                if (SelectedJob != null)
                {
                    
                    repo.JobsRepo.ChangeJobSP(SelectedJob, (int)value, logedUser);
                    Job tmp = SelectedJob;
                    SelectedJob = null;
                    SelectedJob = tmp;

                }
            }
        }




        private double selectedPriority;
        public double SelectedPriority
        {
            get { return selectedPriority; }
            set
            {
                SetProperty(ref selectedPriority, value);
                if(SelectedJob != null)
                {
                    repo.JobsRepo.ChangeJobPriority(SelectedJob,(int) value, logedUser);
                    Job tmp = SelectedJob;
                    SelectedJob = null;
                    SelectedJob = tmp;
                    Jobs = new ObservableCollection<Job>(repo.JobsRepo.GetJobsInBacklog(SelectedProject, (int)selectedType));
                }
            }
        }

        private bool visibilityFlyout;
        public bool VisibilityFlyout
        {
            get { return visibilityFlyout; }
            set { SetProperty(ref visibilityFlyout, value); }
        }

        private bool isDescVisible;
        public bool IsDescVisible
        {
            get { return isDescVisible; }
            set { SetProperty(ref isDescVisible, value); }
        }

        public ICommand DeleteProjectCommand { get; set; }
        public ICommand DeleteJobCommand { get; set; }
        public ICommand EndJobCommand { get; set; }

        private ICommand showMessageDialogCommand;

        public ICommand SearchCommand { get; set; }
        public ICommand GoToTableCommand { get; set; }
        public ICommand ClickDataGridJob { get; set; }

        public ICommand LaunchGT { get; set; }

        public ICommand LaunchNuzumiGT { get; set; }

        public ICommand LaunchRoennaGT { get; set; }
        #endregion

        public BacklogVM(User user, IDialogCoordinator dialogCoordinator) :base(user)
        {
            GoToTableCommand = new DelegateCommand<Window>(GoToTableCommandExecute,GoToTableCommandCanExecute);
            DeleteProjectCommand = new DelegateCommand(DeleteProjectCommandExecute);
            DeleteJobCommand = new DelegateCommand(DeleteJobCommandExecute);
            EndJobCommand = new DelegateCommand(EndJobCommandExecute);
            SearchCommand = new DelegateCommand(SearchJobCommandExecute);
            ClickDataGridJob = new DelegateCommand(ClickDataGridJobExecute);
            LaunchRoennaGT = new DelegateCommand(LaunchRoennaGTExecute);
            LaunchNuzumiGT = new DelegateCommand(LaunchNuzumiGTExecute);
            LaunchGT = new DelegateCommand(LaunchGTExecute);
            TypeList = new List<string> { "All", "New", "Ready", "Scheduled", "Completed" };
            StoryPointValues = new List<double> { 1.0, 2.0, 3.0, 5.0, 8.0, 13.0, 20.0, 40.0, 100.0 };
            PriorityValues = new List<double> { 1.0, 2.0, 3.0, 4.0, 5.0 };
            repo = new EfRepository();
            logedUser = user;
            UserName = user.Name;
            _dialogCoordinator = dialogCoordinator;
            SetProperties();
        }

        public void SetProperties()
        {
            Projects = new ObservableCollection<Project>(repo.ProjectsRepo.Projects);
            if (Projects != null && Projects.Count() > 0)
            {
                SelectedProject = Projects[0];
                Jobs = new ObservableCollection<Job>(repo.JobsRepo.GetJobsForProject(SelectedProject));
            }
            else
            {
                Jobs = new ObservableCollection<Job>();
            }
            IsDescVisible = false;
        }

        protected override void riseGoToCommands()
        {
            if(GoToTableCommand != null)
            {
                (GoToTableCommand as DelegateCommand<Window>).RaiseCanExecuteChanged();
                Jobs = new ObservableCollection<Job>(repo.JobsRepo.GetJobsInBacklog(SelectedProject,(int)selectedType));
                Projects = new ObservableCollection<Project>(repo.ProjectsRepo.Projects);
            }
        }


        public ICommand ShowMessageDialogCommand
        {
            get
            {
                return this.showMessageDialogCommand ?? (this.showMessageDialogCommand = new SimpleCommand
                {
                    CanExecuteDelegate = x => true,
                    ExecuteDelegate = x => PerformDialogCoordinatorAction(this.ShowMessage((string)x), (string)x == "DISPATCHER_THREAD")
                });
            }
        }

        private Action ShowMessage(string startingThread)
        {
            return () =>
            {
                var message = "Keke";
                this._dialogCoordinator.ShowMessageAsync(this, $"Message from VM created by ", message).ContinueWith(t => Console.WriteLine(t.Result));
            };
        }

        private static void PerformDialogCoordinatorAction(Action action, bool runInMainThread)
        {
            if (!runInMainThread)
            {
                Task.Factory.StartNew(action);
            }
            else
            {
                action();
            }
        }

        #region CommandFunctions

        private void GoToTableCommandExecute(Window window)
        {
            TableVM dataContext = new TableVM(logedUser, _dialogCoordinator);
            Table table = new Table();
            table.DataContext = dataContext;
            table.Show();
            window.Close();
        }

        private void DeleteProjectCommandExecute()
        {
            Console.WriteLine("Usuwam projekt");
            repo.ProjectsRepo.DeleteProject(SelectedProject);
            SetProperties();
        }

        private void LaunchGTExecute()
        {
            System.Diagnostics.Process.Start("https://github.com/Nuzumi/ScrumX");
        }
        
        private void EndJobCommandExecute()
        {
            Console.WriteLine("Kończę zadanie");
            repo.JobsRepo.EndJob(SelectedJob, logedUser);
            SetProperties();
        }

        public void ClickDataGridJobExecute()
        {
            
        }

        private void DeleteJobCommandExecute()
        {
            Console.WriteLine("Usuwam zadanie");
            repo.JobsRepo.DeleteJob(SelectedJob);
            SetProperties();
        }

        private void SearchJobCommandExecute()
        {
            if (Tag == null || Tag.Equals(""))
                SetProperties();
            else 
                Jobs = new ObservableCollection<Job>(repo.JobsRepo.SearchJob(SelectedProject, Tag));
        }

        private bool GoToTableCommandCanExecute(Window dummy)
        {
            return CanAddTask && CanAddProject && CanAddSprint;
        }

        private void LaunchRoennaGTExecute()
        {
            System.Diagnostics.Process.Start("https://github.com/Roenna");
        }

        private void LaunchNuzumiGTExecute()
        {
            System.Diagnostics.Process.Start("https://github.com/Nuzumi");
        }
        #endregion

    }
}
