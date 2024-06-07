using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTMobile.Interfaces
{
    public interface IDialogService
    {
        Task<bool> DisplayAlert(string title, string message, string option1, string option2);
    }
}
