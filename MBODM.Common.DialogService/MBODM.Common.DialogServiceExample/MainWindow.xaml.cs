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
    public partial class MainWindow : Window
    {
        private readonly IDialogService dialogService = new DialogService();

        public MainWindow()
        {
            InitializeComponent();

            // Normally you do this stuff via DI container, but lets keep it simple.

            dialogService.RegisterDialog("funkydialog", () => new DialogWindow());
        }

        private void Button_Click_Modal_NoParamNoResult(object sender, RoutedEventArgs eventArgs)
        {
            // Either you can do this and make use of compiler inference,
            // or you have to specify both types in generic method below.

            MyParam param = null;

            // This dialog service is designed that way, because compiler
            // inference (the part that makes this version more fluid) do
            // not work with return values. So you get the result via out.

            MyResult result = null;

            // So now we can call generic method without specifying types.
            // This is what sexy compiler inference is doing for you here.

            dialogService.ShowModalDialog("funkydialog", param, out result);
        }

        private void Button_Click_Modal_WithParamWithResult(object sender, RoutedEventArgs eventArgs)
        {
            MyParam param = new MyParam() { SomeText = "Hello modal World", SomeValue = 5000 };
            MyResult result;

            dialogService.ShowModalDialog("funkydialog", param, out result);

            if (result != null)
            {
                MessageBox.Show($"IDialog modal result:\n\n{result.SomeText}", "Info");
            }
        }

        private void Button_Click_NonModal_NoParamNoResultNoHandlers(object sender, RoutedEventArgs eventArgs)
        {
            // Here we do not use compiler inference because it is more work to type in
            // the signature of the lambda, instead just write types for generic method.
            // It is up to you. C# compiler inference is great, but has his limitations.

            dialogService.ShowNonModalDialog<MyParam, MyResult>("funkydialog", null, null);
        }

        private void Button_Click_NonModal_WithParamWithResultWithHandlers(object sender, RoutedEventArgs eventArgs)
        {
            dialogService.ShowNonModalDialog("funkydialog",
                new MyParam()
                {
                    SomeText = "Hello nonmodal World",
                    SomeValue = 9000
                },
                (MyResult result) =>
                {
                    MessageBox.Show($"IDialog nonmodal result:\n\n{result.SomeText}", "Info");
                });
        }

        private void Button_Click_NonModal_Close(object sender, RoutedEventArgs e)
        {
            // Since we need no param/result here, we have to specify the types.
            // Maybe it is a little bit weird, but it was the best compromise to
            // design a generic dialog service user-friendly for most parts of it.

            dialogService.CloseNonModalDialog<MyParam, MyResult>("funkydialog");
        }
    }
}
