using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MBODM.Common.DialogServiceExample
{
    public partial class DialogWindow : Window, IDialog<MyParam, MyResult>
    {
        // Normally you do this stuff via DI container, but lets keep it simple.

        private readonly DialogWindowViewModel viewModel = new DialogWindowViewModel();

        private bool isModal;

        public DialogWindow()
        {
            InitializeComponent();

            viewModel.CloseRequested += (s, e) =>
            {
                if (isModal) DialogResult = viewModel.Result; else Close();
            };

            DataContext = viewModel;
        }

        #region IDialog Implementation

        public MyResult ShowModal(MyParam param)
        {
            isModal = true;

            if (param != null) SetDataFromParameter(param);

            var dialogResult = ShowDialog();

            if (!dialogResult.HasValue) return null;

            return new MyResult() { SomeState = dialogResult.Value, SomeText = viewModel.Input };
        }

        public void ShowNonModal(MyParam param, Action<MyResult> closed)
        {
            isModal = false;

            if (param != null) SetDataFromParameter(param);

            Closed += (s, e) =>
            {
                closed?.Invoke(new MyResult() { SomeState = viewModel.Result, SomeText = viewModel.Input });
            };

            Show();
        }

        public void CloseNonModal()
        {
            if (!isModal)
            {
                // There are two windows existent. First has no width and no height.
                // At the moment i will not invest time to figure out how WPF handles
                // window mechanisms here. Dispatcher says we are calling in the right
                // thread, but if the window loose his focus, Close() will not close it.
                // Whatever, this small hack works. It is just an example. More advanced
                // WPF developers will know what to do here, so i accept it for the moment.

                var window = Application.Current.Windows.OfType<DialogWindow>().
                    Where(w => w.ActualWidth != 0).
                    FirstOrDefault();

                if (window != null) window.Close();
            }
        }

        #endregion

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            if (isModal) DialogResult = true; else Close();
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            if (isModal) DialogResult = false; else Close();
        }

        private void SetDataFromParameter(MyParam param)
        {
            // CodeBehind

            Title = param.SomeText;
            textBlock.Text = $"Over {param.SomeValue} !";

            // MVVM

            viewModel.Input = "Enter some text here";
        }
    }
}
