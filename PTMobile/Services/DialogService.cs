using PTMobile.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTMobile.Services
{
    public class DialogService : IDialogService
    {
        public async Task<bool> DisplayAlert(string title, string message, string option1, string option2)
        {
            return await Application.Current.MainPage.DisplayAlert(title, message, option1, option2);
        }
    }
}
