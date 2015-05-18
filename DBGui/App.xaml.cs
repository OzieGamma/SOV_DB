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

            new ProductionWindow( 1 ).Show();
        }

        private void PersonInfo_MouseDoubleClick( object sender, MouseButtonEventArgs e )
        {
            var info = (PersonInfo) ( (ContentControl) sender ).Content;
            if ( info != null )
            {
                new PersonWindow( info.Id ).Show();
            }
        }

        private void ProductionInfo_MouseDoubleClick( object sender, MouseButtonEventArgs e )
        {
            var info = (ProductionInfo) ( (ContentControl) sender ).Content;
            if ( info != null )
            {
                new ProductionWindow( info.Id ).Show();
            }
        }

        private void CharacterInfo_MouseDoubleClick( object sender, MouseButtonEventArgs e )
        {
            var info = (CharacterInfo) ( (ContentControl) sender ).Content;
            if ( info != null )
            {
                new CharacterWindow( info.Id ).Show();
            }
        }
    }
}