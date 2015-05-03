namespace DBGui
{
    public sealed partial class App
    {
        public App()
        {
#if !DEBUG
            DispatcherUnhandledException += ( sender, args ) =>
            {
                System.Windows.MessageBox.Show( args.Exception.ToString(), "Error" );
                args.Handled = true;
            };
#endif

            new PersonWindow( 22378 ).Show();
        }
    }
}