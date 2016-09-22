using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MBODM.Common.DialogServiceExample
{
    public sealed class DialogWindowViewModel : INotifyPropertyChanged
    {
        private string input;
        public string Input
        {
            get
            {
                return input;
            }
            set
            {
                if (input != value)
                {
                    input = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Input)));
                }
            }
        }

        public bool Result
        {
            get;
            private set;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler CloseRequested;

        public ICommand OK
        {
            get
            {
                return new RelayCommand(p =>
                {
                    Input = $"OK command executed.\nInput: {Input}";
                    Result = true;
                    CloseRequested?.Invoke(this, EventArgs.Empty);
                },
                null);
            }
        }

        public ICommand Cancel
        {
            get
            {
                return new RelayCommand(p =>
                {
                    Input = $"Cancel command executed.\nInput: {Input}";
                    Result = false;
                    CloseRequested?.Invoke(this, EventArgs.Empty);
                },
                null);
            }
        }
    }
}
