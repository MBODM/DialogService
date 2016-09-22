using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBODM.Common
{
    public interface IDialog<TParam, TResult>
    {
        TResult ShowModal(TParam param);
        void ShowNonModal(TParam param, Action<TResult> closed);
        void CloseNonModal();
    }
}
