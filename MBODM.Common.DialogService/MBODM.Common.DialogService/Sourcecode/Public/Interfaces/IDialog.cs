using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBODM.WPF
{
    public interface IDialog
    {
        object DataContext
        {
            get;
            set;
        }

        object Parameter
        {
            get;
            set;
        }

        object Result
        {
            get;
            set;
        }

        event CancelEventHandler Closing;
        event EventHandler Closed;

        void Show(bool modal);
        object Show(bool modal, object parameter);
        void Close();
    }
}
