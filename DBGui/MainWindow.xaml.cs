using System.Windows;
using DB;
using DBGui.Models;
using WinForms = System.Windows.Forms;

namespace DBGui
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
        }

        private async void ImportMenu_Click( object sender, RoutedEventArgs e )
        {
            var dialog = new WinForms.FolderBrowserDialog();
            if ( dialog.ShowDialog() == WinForms.DialogResult.OK )
            {
                await Database.ImportFromDirectoryAsync( dialog.SelectedPath );
            }
        }

        private void PersonNameInput_Executed( string text )
        {
            DoAsync( async () => { PeopleView.Items = await PersonInfo.SearchByNameAsync( text ); } );
        }

        private void PersonWhereInput_Executed( string text )
        {
            DoAsync( async () => { PeopleView.Items = await PersonInfo.SearchAsync( text ); } );
        }
    }
}