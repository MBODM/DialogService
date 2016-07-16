using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MBODM.WPF
{
    [Serializable]
    public class DialogServiceException : Exception
    {
        public DialogServiceException() { }
        public DialogServiceException(string message) : base(message) { }
        public DialogServiceException(string message, Exception inner) : base(message, inner) { }
        protected DialogServiceException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
