using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBODM.WPF
{
    public sealed class DialogService : IDialogService
    {
        private const string ArgumentNullOrEmptyMessage = "Argument is null or empty.";
        private const string DialogNotRegisteredMessage = "The dialog is not registered.";
        private const string DialogAlreadyRegisteredMessage = "The dialog is already registered.";

        private readonly object syncRoot = new object();
        private readonly Dictionary<Type, Func<IDialog>> dialogsByType = new Dictionary<Type, Func<IDialog>>();
        private readonly Dictionary<string, Func<IDialog>> dialogsByName = new Dictionary<string, Func<IDialog>>();

        public void RegisterDialog<T>(Func<IDialog> factory)
        {
            lock (syncRoot)
            {
                if (factory == null)
                {
                    throw new ArgumentNullException(nameof(factory));
                }

                if (IsRegistered<T>())
                {
                    throw new DialogServiceException(DialogAlreadyRegisteredMessage);
                }

                dialogsByType.Add(typeof(T), factory);
            }
        }

        public void RegisterDialog(string name, Func<IDialog> factory)
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

                dialogsByName.Add(name, factory);
            }
        }

        public void UnregisterDialog<T>()
        {
            lock (syncRoot)
            {
                if (!IsRegistered<T>())
                {
                    throw new DialogServiceException(DialogNotRegisteredMessage);
                }

                dialogsByType.Remove(typeof(T));
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

                dialogsByName.Remove(name);
            }
        }

        public IDialog CreateDialog<T>()
        {
            lock (syncRoot)
            {
                if (!IsRegistered<T>())
                {
                    throw new DialogServiceException(DialogNotRegisteredMessage);
                }

                return dialogsByType[typeof(T)]();
            }
        }

        public IDialog CreateDialog(string name)
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

                return dialogsByName[name]();
            }
        }

        private bool IsRegistered<T>()
        {
            return dialogsByType.Any(kvp => kvp.Key == typeof(T));
        }

        private bool IsRegistered(string name)
        {
            return dialogsByName.Any(kvp => kvp.Key == name);
        }
    }
}
