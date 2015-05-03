using System.Windows;
namespace DBGui
{
    public sealed partial class App
    {
        public App()
        {
            DispatcherUnhandledException += ( sender, args ) =>
            {
                MessageBox.Show( args.Exception.ToString(), "Error" );
                args.Handled = true;
            };
        }
    }
}