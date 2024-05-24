using System.ComponentModel;
using System.Text.Json.Serialization;

namespace PTMobile.Models
{
    public class User : INotifyPropertyChanged
    {
        private string _userName;
        private string _email;
        private string _password;
        private string _status;
        private string? _tvCode;
        private List<string> _roles;
        private DateTime? _joined;
        private DateTime? _updatedAt;
        private string? _twoFactorSecret;
        private bool _isAdmin;
        private bool _isGod;

        public string? Id { get; set; }

        public string UserName 
        {
            get => _userName;
            set
            {
                if (_userName != value)
                {
                    _userName = value;
                    OnPropertyChanged(nameof(UserName));
                }
            }
        }

        public string Email 
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }
        }

        public string Password 
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged(nameof(Password));
                }
            }
        }

        public string Status 
        {
            get => _status;
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged(nameof(Status));
                }
            }
        }

        public string? TVCode 
        {
            get => _tvCode;
            set
            {
                if (_tvCode != value)
                {
                    _tvCode = value;
                    OnPropertyChanged(nameof(TVCode));
                }
            }
        }


        public List<string> Roles 
        {
            get => _roles;
            set
            {
                if (_roles != value)
                {
                    _roles = value;
                    OnPropertyChanged(nameof(Roles));
                }
            }
        }

        public DateTime? Joined 
        {
            get => _joined;
            set
            {
                if (_joined != value)
                {
                    _joined = value;
                    OnPropertyChanged(nameof(Joined));
                }
            }
        }

        public DateTime? UpdatedAt 
        {
            get => _updatedAt;
            set
            {
                if (_updatedAt != value)
                {
                    _updatedAt = value;
                    OnPropertyChanged(nameof(UpdatedAt));
                }
            }
        }

        public string? TwoFactorSecret 
        {
            get => _twoFactorSecret;
            set
            {
                if (_twoFactorSecret != value)
                {
                    _twoFactorSecret = value;
                    OnPropertyChanged(nameof(TwoFactorSecret));
                }
            }
        }

        public bool IsAdmin
        {
            get => _isAdmin;
            set
            {
                if (_isAdmin != value)
                {
                    _isAdmin = value;
                    OnPropertyChanged(nameof(IsAdmin));
                }
            }
        }

        public bool IsGod
        {
            get => _isGod;
            set
            {
                if (_isGod != value)
                {
                    _isGod = value;
                    OnPropertyChanged(nameof(IsGod));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}