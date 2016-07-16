using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBODM.WPF
{
    public interface IDialogService
    {
        void RegisterDialog<T>(Func<IDialog> factory);
        void RegisterDialog(string name, Func<IDialog> factory);

        void UnregisterDialog<T>();
        void UnregisterDialog(string name);

        IDialog CreateDialog<T>();
        IDialog CreateDialog(string name);
    }
}
