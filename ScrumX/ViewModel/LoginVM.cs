using Prism.Commands;
using Prism.Mvvm;
using ScrumX.API.Context;
using ScrumX.API.Model;
using ScrumX.API.Repository;
using ScrumX.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ScrumX.ViewModel
{
    class LoginVM : BindableBase
    {
        private User user;
        private bool loginMode = true;
        private EfRepository repo;

        #region Properties

        private string labelContent;
        public string LabeleContent
        {
            get { return labelContent; }
            set { SetProperty(ref labelContent, value); }
        }

        private bool isVisibleLogin;
        public bool IsVisibleLogin
        {
            get { return isVisibleLogin; }
            set
            {
                SetProperty(ref isVisibleLogin, value);
                (RegisterCommand as DelegateCommand).RaiseCanExecuteChanged();
                (LoginCommand as DelegateCommand).RaiseCanExecuteChanged();
            }
        }

        private bool passwordCorrect;
        public bool PasswordCorrect
        {
            get { return passwordCorrect; }
            set { SetProperty(ref passwordCorrect, value); }
        }

        private bool loginCorrect;
        public bool LoginCorrect
        {
            get { return loginCorrect; }
            set { SetProperty(ref loginCorrect, value); }
        }

        private string login;
        public string Login
        {
            get { return login; }
            set
            {
                SetProperty(ref login, value);
                (OkComamnd as DelegateCommand<Window>).RaiseCanExecuteChanged();
            }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                SetProperty(ref password, value);
                (OkComamnd as DelegateCommand<Window>).RaiseCanExecuteChanged();
            }
        }

        private string passwordAgain;
        public string PasswordAgain
        {
            get { return passwordAgain; }
            set
            {
                SetProperty(ref passwordAgain, value);
                (OkComamnd as DelegateCommand<Window>).RaiseCanExecuteChanged();
            }
        }

        public ICommand OkComamnd { get; set; }
        public ICommand RegisterCommand { get; set; }
        public ICommand LoginCommand { get; set; }
        
        #endregion

        public LoginVM()
        {
            OkComamnd = new DelegateCommand<Window>(OkCommandExecute,OKCommandCanExecute);
            RegisterCommand = new DelegateCommand(RegisterCommandExecute, RegisterCommandCanExecute);
            LoginCommand = new DelegateCommand(LoginCommandExecute, LoginCommandCanExecute);
            LabeleContent = "Logowanie";
            repo = new EfRepository();
            LoginCorrect = true;
            PasswordCorrect = true;
        }

        #region commanadFunction

        private void OkCommandExecute(Window window)
        {
            if (loginMode)
            {
                BacklogVM dataContext = new BacklogVM(repo.UsersRepo.GetUserById(1));
                Backlog backlog = new Backlog();
                backlog.DataContext = dataContext;
                backlog.Show();
                window.Close(); 
            }
            else
            {
                Console.WriteLine("OK");
            }
        }

        private bool OKCommandCanExecute(Window dummyWindow)
        {
            if (loginMode)
            {
                return true;
            }
            else
            {
                //PasswordCorrect = (Password != string.Empty && Password != null);
                LoginCorrect = !repo.UsersRepo.UserExists(Login);

                return (!repo.UsersRepo.UserExists(Login) && Login != string.Empty && Login !=null && Password != string.Empty && Password != null);
            }
        }

        private void RegisterCommandExecute()
        {
            LabeleContent = "Rejestracja";
            loginMode = false;
            IsVisibleLogin = true;
            (OkComamnd as DelegateCommand<Window>).RaiseCanExecuteChanged();
        }

        private bool RegisterCommandCanExecute()
        {
            return loginMode;
        }

        private void LoginCommandExecute()
        {
            LabeleContent = "Logowanie";
            loginMode = true;
            IsVisibleLogin = false;
            (OkComamnd as DelegateCommand<Window>).RaiseCanExecuteChanged();
        }

        private bool LoginCommandCanExecute()
        {
            return !loginMode;
        }
        #endregion
    }
}
