using System;
using System.Windows;
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

            new ProductionWindow( 2102036 ).Show();
        }

        private void PersonInfo_Click( object sender, EventArgs e )
        {
            var info = (PersonInfo) ( (FrameworkElement) sender ).DataContext;
            if ( info != null )
            {
                new PersonWindow( info.Id ).Show();
            }
        }

        private void ProductionInfo_Click( object sender, EventArgs e )
        {
            var info = (ProductionInfo) ( (FrameworkElement) sender ).DataContext;
            if ( info != null )
            {
                new ProductionWindow( info.Id ).Show();
            }
        }

        private void CharacterInfo_Click( object sender, EventArgs e )
        {
            var info = (CharacterInfo) ( (FrameworkElement) sender ).DataContext;
            if ( info != null )
            {
                new CharacterWindow( info.Id ).Show();
            }
        }
    }
}