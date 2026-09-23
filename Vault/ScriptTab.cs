using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Vault
{
    public class ScriptTab : INotifyPropertyChanged
    {
        private string _name = "";
        private string _content = "";
        private bool _isActive;

        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }

        public string Content
        {
            get { return _content; }
            set { _content = value; OnPropertyChanged(); }
        }

        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChangedEventHandler? handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
