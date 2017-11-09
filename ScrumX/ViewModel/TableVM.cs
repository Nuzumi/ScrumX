using ScrumX.API.Model;
using System.Windows.Input;
using System.Windows;
using Prism.Commands;
using ScrumX.HelperClasses;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using ScrumX.API.Repository;
using ScrumX.API.Content;
using System;
using GongSolutions.Wpf.DragDrop;

namespace ScrumX.ViewModel
{
    class TableVM : DialogDisplay , IDropTarget
    {
        private EfRepository repo;

        #region Properties

        public string UserName { get; set; }

        private ObservableCollection<Job> toDoJobs;
        public ObservableCollection<Job> ToDoJobs
        {
            get { return toDoJobs; }
            set
            {
                SetProperty(ref toDoJobs, value);
            }
        }
        public bool getJobForUser = false;
        private string jobsForUser;
        public string JobsForUser
        {
            get { return jobsForUser; }
            set
            {
                SetProperty(ref jobsForUser, value);
            }
        }

        private ObservableCollection<Job> doingJobs;
        public ObservableCollection<Job> DoingJobs
        {
            get { return doingJobs; }
            set
            {
                SetProperty(ref doingJobs, value);
            }
        }

        private ObservableCollection<Job> reviewJobs;
        public ObservableCollection<Job> ReviewJobs
        {
            get { return reviewJobs; }
            set { SetProperty(ref reviewJobs, value); }
        }

        private ObservableCollection<Job> doneJobs;
        public ObservableCollection<Job> DoneJobs
        {
            get { return doneJobs; }
            set { SetProperty(ref doneJobs, value); }
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
                SetJobs();
            }
        }

        public void SetJobs()
        {
            if (SelectedSprint != null)
                {
                    ToDoJobs = new ObservableCollection<Job>(repo.JobsRepo.GetJobsInTable(SelectedSprint, (int)typeTable.ToDo));
                    DoingJobs = new ObservableCollection<Job>(repo.JobsRepo.GetJobsInTable(SelectedSprint, (int)typeTable.Doing));
                    ReviewJobs = new ObservableCollection<Job>(repo.JobsRepo.GetJobsInTable(SelectedSprint, (int)typeTable.Review));
                    DoneJobs = new ObservableCollection<Job>(repo.JobsRepo.GetJobsInTable(SelectedSprint, (int)typeTable.Done));
                }
                else
                {
                    ToDoJobs = new ObservableCollection<Job>();
                    DoingJobs = new ObservableCollection<Job>();
                    ReviewJobs = new ObservableCollection<Job>();
                    DoneJobs = new ObservableCollection<Job>();
                }
        }

        public ICommand GoToBacklogCommand { get; set; }
        public ICommand GetJobsForUserCommand { get; set; }
        #endregion


        public TableVM(User user) :base(user)
        {
            GoToBacklogCommand = new DelegateCommand<Window>(GoToBacklogCommandExecute, GoToBacklogCommandCanExecute);
            GetJobsForUserCommand = new DelegateCommand(GetJobsForUserCommandExecute);
            logedUser = user;
            UserName = user.Name;
            repo = new EfRepository();
            jobsForUser = "Tylko moje zadania";
            Projects = new ObservableCollection<Project>(repo.ProjectsRepo.Projects);
            if(Projects != null && Projects.Count > 0)
            {
                SelectedProject = Projects[0];
                if(Sprints != null && Sprints.Count > 0)
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

        public void DragOver(IDropInfo dropInfo)
        {
            dropInfo.Effects = DragDropEffects.All;
        }

        public void Drop(IDropInfo dropInfo)
        {
            if (repo.SprintsRepo.IsSprintOpen(SelectedSprint)) {
                if (ToDoJobs.Contains(dropInfo.Data as Job))
                {
                    if ((dropInfo.TargetCollection as ObservableCollection<Job>).Equals(DoingJobs))
                    {
                        ToDoJobs.Remove((dropInfo.Data as Job));
                        DoingJobs.Add((dropInfo.Data as Job));
                        repo.JobsRepo.ChangeJobTable((dropInfo.Data as Job), logedUser, (int)API.Content.typeTable.Doing);
                    }
                    else
                    {
                        MessageBox.Show("nie mozna");
                    }
                }
                else
                {
                    if (DoingJobs.Contains(dropInfo.Data as Job))
                    {
                        if ((dropInfo.TargetCollection as ObservableCollection<Job>).Equals(ToDoJobs))
                        {
                            DoingJobs.Remove(dropInfo.Data as Job);
                            ToDoJobs.Add(dropInfo.Data as Job);
                            repo.JobsRepo.ChangeJobTable((dropInfo.Data as Job), logedUser, (int)typeTable.ToDo);
                        }
                        else
                        {
                            if ((dropInfo.TargetCollection as ObservableCollection<Job>).Equals(ReviewJobs))
                            {
                                DoingJobs.Remove(dropInfo.Data as Job);
                                ReviewJobs.Add(dropInfo.Data as Job);
                                bool i = repo.JobsRepo.ChangeJobTable((dropInfo.Data as Job), logedUser, (int)typeTable.Review);
                                Console.WriteLine(i);
                            }
                            else
                            {
                                if ((dropInfo.TargetCollection as ObservableCollection<Job>).Equals(DoneJobs))
                                {
                                    DoingJobs.Remove(dropInfo.Data as Job);
                                    DoneJobs.Add(dropInfo.Data as Job);
                                    repo.JobsRepo.ChangeJobTable((dropInfo.Data as Job), logedUser, (int)typeTable.Done);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (ReviewJobs.Contains(dropInfo.Data as Job))
                        {
                            if ((dropInfo.TargetCollection as ObservableCollection<Job>).Equals(ToDoJobs))
                            {
                                ReviewJobs.Remove(dropInfo.Data as Job);
                                ToDoJobs.Add(dropInfo.Data as Job);
                                repo.JobsRepo.ChangeJobTable((dropInfo.Data as Job), logedUser, (int)typeTable.ToDo);
                            }
                            else
                            {
                                if ((dropInfo.TargetCollection as ObservableCollection<Job>).Equals(DoingJobs))
                                {
                                    ReviewJobs.Remove(dropInfo.Data as Job);
                                    DoingJobs.Add(dropInfo.Data as Job);
                                    repo.JobsRepo.ChangeJobTable((dropInfo.Data as Job), logedUser, (int)typeTable.Doing);
                                }
                                else
                                {
                                    if ((dropInfo.TargetCollection as ObservableCollection<Job>).Equals(DoneJobs))
                                    {
                                        ReviewJobs.Remove(dropInfo.Data as Job);
                                        DoneJobs.Add(dropInfo.Data as Job);
                                        repo.JobsRepo.ChangeJobTable((dropInfo.Data as Job), logedUser, (int)typeTable.Done);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
                MessageBox.Show("Sprint jest zamknięty!");
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

        private void GetJobsForUserCommandExecute()
        {
            if(getJobForUser == false)
            {
                getJobForUser = true;
                JobsForUser = "Wszystkie zadania";
                ToDoJobs = new ObservableCollection<Job>(repo.JobsRepo.GetJobsForUser(SelectedSprint, logedUser, (int)typeTable.ToDo));
                DoingJobs = new ObservableCollection<Job>(repo.JobsRepo.GetJobsForUser(SelectedSprint, logedUser, (int)typeTable.Doing));
                ReviewJobs = new ObservableCollection<Job>(repo.JobsRepo.GetJobsForUser(SelectedSprint, logedUser, (int)typeTable.Review));
                DoneJobs = new ObservableCollection<Job>(repo.JobsRepo.GetJobsForUser(SelectedSprint, logedUser, (int)typeTable.Done));

            }
            else
            {
                getJobForUser = false;
                JobsForUser = "Tylko moje zadania";
                SetJobs();
            }
        }
        
        private bool GoToBacklogCommandCanExecute(Window dummy)
        {
            return CanAddTask;
        }

        #endregion
    }
}
