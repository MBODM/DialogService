using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBODM.Common
{
    public sealed class DialogService : IDialogService
    {
        private const string ArgumentNullOrEmptyMessage =
            "Argument is null or empty.";
        private const string DialogNotRegisteredMessage =
            "The dialog is not registered.";
        private const string DialogAlreadyRegisteredMessage =
            "The dialog is already registered.";
        private const string DialogDoNotMatchWithGivenTypes =
            "Registered dialog with given name do not match given parameter/result type(s).";

        private readonly object syncRoot =
            new object();
        private readonly Dictionary<string, Func<object>> registeredDialogs =
            new Dictionary<string, Func<object>>();

        public void RegisterDialog<TParam, TResult>(string name, Func<IDialog<TParam, TResult>> factory)
        {
            lock (syncRoot)
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new ArgumentException(ArgumentNullOrEmptyMessage, nameof(name));
                }

                if (factory == null)
                {
                    throw new ArgumentNullException(nameof(factory));
                }

                if (IsRegistered(name))
                {
                    throw new DialogServiceException(DialogAlreadyRegisteredMessage);
                }

                registeredDialogs.Add(name, factory);
            }
        }

        public void UnregisterDialog(string name)
        {
            lock (syncRoot)
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new ArgumentException(ArgumentNullOrEmptyMessage, nameof(name));
                }

                if (!IsRegistered(name))
                {
                    throw new DialogServiceException(DialogNotRegisteredMessage);
                }

                registeredDialogs.Remove(name);
            }
        }

        public void ShowModalDialog<TParam, TResult>(string name, TParam param, out TResult result)
        {
            result = FindDialog<TParam, TResult>(name).ShowModal(param);
        }

        public void ShowNonModalDialog<TParam, TResult>(string name, TParam param, Action<TResult> closed)
        {
            FindDialog<TParam, TResult>(name).ShowNonModal(param, closed);
        }

        public void CloseNonModalDialog<TParam, TResult>(string name)
        {
            FindDialog<TParam, TResult>(name).CloseNonModal();
        }

        private IDialog<TParam, TResult> FindDialog<TParam, TResult>(string name)
        {
            lock (syncRoot)
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new ArgumentException(ArgumentNullOrEmptyMessage, nameof(name));
                }

                if (!IsRegistered(name))
                {
                    throw new DialogServiceException(DialogNotRegisteredMessage);
                }

                var factory = registeredDialogs[name];

                var dialog = factory.Invoke() as IDialog<TParam, TResult>;

                if (dialog == null)
                {
                    throw new DialogServiceException(DialogDoNotMatchWithGivenTypes);
                }

                return dialog;
            }
        }

        private bool IsRegistered(string name)
        {
            return registeredDialogs.Any(kvp => kvp.Key == name);
        }
    }
}
