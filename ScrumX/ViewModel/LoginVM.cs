using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ScrumX.ViewModel
{
    class LoginVM : BindableBase
    {
        private bool loginMode = true;

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
        public string Login { get; set; }
        public string Password { get; set; }

        public ICommand OkComamnd { get; set; }
        public ICommand RegisterCommand { get; set; }
        public ICommand LoginCommand { get; set; }
        
        #endregion

        public LoginVM()
        {
            OkComamnd = new DelegateCommand(OkCommandExecute,OKCommandCanExecute);
            RegisterCommand = new DelegateCommand(RegisterCommandExecute, RegisterCommandCanExecute);
            LoginCommand = new DelegateCommand(LoginCommandExecute, LoginCommandCanExecute);
            LabeleContent = "Logowanie";
        }

        #region commanadFunction

        private void OkCommandExecute()
        {
            if (loginMode)
            {

            }
            else
            {

            }
        }

        private bool OKCommandCanExecute()
        {
            return true;
        }

        private void RegisterCommandExecute()
        {
            LabeleContent = "Rejestracja";
            loginMode = false;
            IsVisibleLogin = true;
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
        }

        private bool LoginCommandCanExecute()
        {
            return !loginMode;
        }
        #endregion
    }
}
