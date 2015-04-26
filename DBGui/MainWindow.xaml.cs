using System;
using System.Windows;
using DB;
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
                await DatabaseImport.ImportFromDirectoryAsync( dialog.SelectedPath );
            }
        }

        private async void PeopleSearchButton_Click( object sender, RoutedEventArgs e )
        {
            var table = await Database.ExecuteQueryAsync(
@"SELECT Id, FirstName, LastName, Gender, Trivia, Quotes, BirthDate, DeathDate, BirthName, ShortBio, SpouseInfo, Height
  FROM Person 
  WHERE FirstName LIKE '%" + PeopleSearchBox.Text + "%' OR LastName LIKE '%" + PeopleSearchBox.Text + "%';" );
            PeopleView.ItemsSource = table.DefaultView;

        }

        private async void RawQueryButton_Click( object sender, RoutedEventArgs e )
        {
            try
            {
                var table = await Database.ExecuteQueryAsync( RawQueryBox.Text );
                RawQueryResultsGrid.ItemsSource = table.DefaultView;
            }
            catch ( Exception ex )
            {
                MessageBox.Show( ex.StackTrace );
            }
        }
    }
}