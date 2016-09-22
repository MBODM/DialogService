using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBODM.Common
{
    public interface IDialogService
    {
        void RegisterDialog<TParam, TResult>(string name, Func<IDialog<TParam, TResult>> factory);
        void UnregisterDialog(string name);
        void ShowModalDialog<TParam, TResult>(string name, TParam param, out TResult result);
        void ShowNonModalDialog<TParam, TResult>(string name, TParam param, Action<TResult> closed);
        void CloseNonModalDialog<TParam, TResult>(string name);
    }
}
