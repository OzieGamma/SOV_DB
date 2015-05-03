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

        private void ImportMenu_Click( object sender, RoutedEventArgs e )
        {
            var dialog = new WinForms.FolderBrowserDialog();
            if ( dialog.ShowDialog() == WinForms.DialogResult.OK )
            {
                DoAsync( () => Database.ImportFromDirectoryAsync( dialog.SelectedPath ) );
            }
        }

        private void PersonNameInput_Executed( string text )
        {
            DoAsync( async () => PeopleView.ItemsSource = await PersonInfo.SearchByNameAsync( text ) );
        }

        private void PersonWhereInput_Executed( string text )
        {
            DoAsync( async () => PeopleView.ItemsSource = await PersonInfo.SearchAsync( text ) );
        }


        private void ProductionTitleInput_Executed( string text )
        {
            DoAsync( async () => ProductionsView.ItemsSource = await ProductionInfo.SearchByTitleAsync( text ) );
        }

        private void ProductionWhereInput_Executed( string text )
        {
            DoAsync( async () => ProductionsView.ItemsSource = await ProductionInfo.SearchAsync( text ) );
        }


        private void CharacterNameInput_Executed( string text )
        {
            DoAsync( async () => CharactersView.ItemsSource = await CharacterInfo.SearchByNameAsync( text ) );
        }

        private void CharacterWhereInput_Executed( string text )
        {
            DoAsync( async () => CharactersView.ItemsSource = await CharacterInfo.SearchAsync( text ) );
        }


        private void RawInput_Executed( string text )
        {
            DoAsync( async () => RawQueryResultsView.ItemsSource = ( await Database.ExecuteQueryAsync( text ) ).DefaultView );
        }
    }
}