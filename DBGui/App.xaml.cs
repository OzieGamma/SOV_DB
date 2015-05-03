using System.Windows.Controls;
using System.Windows.Input;
using DBGui.Models;

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

        private void PersonInfo_MouseDoubleClick( object sender, MouseButtonEventArgs e )
        {
            var info = (PersonInfo) ( (ListViewItem) sender ).DataContext;
            new PersonWindow( info.Id ).Show();
        }

        private void ProductionInfo_MouseDoubleClick( object sender, MouseButtonEventArgs e )
        {
            var info = (ProductionInfo) ( (ListViewItem) sender ).DataContext;
            new ProductionWindow( info.Id ).Show();
        }

        private void CharacterInfo_MouseDoubleClick( object sender, MouseButtonEventArgs e )
        {
            var info = (CharacterInfo) ( (ListViewItem) sender ).DataContext;
            new CharacterWindow( info.Id ).Show();
        }
    }
}